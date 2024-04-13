using System.Diagnostics;
using Observatory.Framework;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;

namespace Pulsar.Utils;

class LogMonitor
{
    #region Singleton Instantiation

    public static LogMonitor GetInstance
    {
        get
        {
                return _instance.Value;
            }
    }

    private static readonly Lazy<LogMonitor> _instance = new(NewLogMonitor);

    private static LogMonitor NewLogMonitor()
    {
            return new LogMonitor();
        }

    private LogMonitor()
    {
            currentLine = new();
            journalTypes = JournalReader.PopulateEventClasses();
            InitializeWatchers(string.Empty);
            SetLogMonitorState(LogMonitorState.Idle);
        }

    #endregion

    #region Public properties
    public LogMonitorState CurrentState
    {
        get => currentState;
    }

    public Status Status { get; private set; }
        
    #endregion

    #region Public Methods

    public void Start()
    {
            if (firstStartMonitor)
            {
                // Only pre-read on first start monitor. Beyond that it's simply pause/resume.
                firstStartMonitor = false;
                PrereadJournals();
            }
            journalWatcher.EnableRaisingEvents = true;
            statusWatcher.EnableRaisingEvents = true;
            SetLogMonitorState(LogMonitorState.Realtime);
            JournalPoke();
        }

    public void Stop()
    {
            journalWatcher.EnableRaisingEvents = false;
            statusWatcher.EnableRaisingEvents = false;
            SetLogMonitorState(LogMonitorState.Idle);
        }

    public void ChangeWatchedDirectory(string path)
    {
            journalWatcher.Dispose();
            statusWatcher.Dispose();
            InitializeWatchers(path);
        }

    public bool IsMonitoring()
    {
            return currentState.HasFlag(LogMonitorState.Realtime);
        }

    // TODO(fredjk_gh): Remove?
    public bool ReadAllInProgress()
    {
            return LogMonitorStateChangedEventArgs.IsBatchRead(currentState);
        }

    public Func<IEnumerable<string>> ReadAllGenerator(out int fileCount)
    {
            // Prevent pre-reading when starting monitoring after reading all.
            firstStartMonitor = false;
            SetLogMonitorState(currentState | LogMonitorState.Batch);

            var logDirectory = GetJournalFolder();
            var files = GetJournalFilesOrdered(logDirectory);
            fileCount = files.Count();

            IEnumerable<string> ReadAllJournals()
            {
                var readErrors = new List<(Exception ex, string file, string line)>();
                foreach (var file in files)
                {
                    yield return file.Name;
                    readErrors.AddRange(
                        ProcessLines(ReadAllLines(file.FullName), file.Name));
                }

                ReportErrors(readErrors);
                SetLogMonitorState(currentState & ~LogMonitorState.Batch);
            };

            return ReadAllJournals;
        }

    public void PrereadJournals()
    {
            SetLogMonitorState(currentState | LogMonitorState.PreRead);

            var logDirectory = GetJournalFolder();
            var files = GetJournalFilesOrdered(logDirectory).ToList();

            // Read at most the last two files (in case we were launched after the game and the latest
            // journal is mostly empty) but keeping only the lines since the last FSDJump.
            List<string> lastSystemLines = new();
            List<string> lastFileLines = new();
            List<string> fileHeaderLines = new();
            var sawFSDJump = false;
            foreach (var file in files.Skip(Math.Max(files.Count - 2, 0)))
            {
                var lines = ReadAllLines(file.FullName);
                foreach (var line in lines)
                {
                    var eventType = JournalUtilities.GetEventType(line);
                    if (eventType.Equals("FSDJump") || (eventType.Equals("CarrierJump") && (line.Contains("\"Docked\":true") || line.Contains("\"OnFoot\":true"))))
                    {
                        // Reset, start collecting again.
                        lastSystemLines.Clear();
                        sawFSDJump = true;
                    }
                    else if (eventType.Equals("Fileheader"))
                    {
                        lastFileLines.Clear();
                        fileHeaderLines.Clear();
                        fileHeaderLines.Add(line);
                    }
                    else if (eventType.Equals("LoadGame") || eventType.Equals("Statistics"))
                    {
                        // A few header lines to collect.
                        fileHeaderLines.Add(line);
                    }
                    lastSystemLines.Add(line);
                    lastFileLines.Add(line);
                }
            }

            // If we didn't see a jump in the recent logs (Cmdr is stationary in a system for a while
            // ie. deep-space mining from a carrier), at very least, read from the beginning of the
            // current journal file which includes the important stuff like the last "LoadGame", etc. This
            // also helps out in cases where one forgets to hit "Start Monitor" until part-way into the
            // session (if auto-start is not enabled).
            List<string> linesToRead = lastFileLines;
            if (sawFSDJump)
            {
                // If we saw any relevant header lines, insert them as well. This ensures odyssey biologicials are properly
                // counted/presented, current Commander name is present, etc.
                if (fileHeaderLines.Count > 0)
                {
                    lastSystemLines.InsertRange(0, fileHeaderLines);
                }
                linesToRead = lastSystemLines;
            }

            ReportErrors(ProcessLines(linesToRead, "Pre-read"));
            SetLogMonitorState(currentState & ~LogMonitorState.PreRead);
        }

    #endregion

    #region Public Events

    public event EventHandler<LogMonitorStateChangedEventArgs> LogMonitorStateChanged;

    public event EventHandler<JournalEventArgs> JournalEntry;

    public event EventHandler<JournalEventArgs> StatusUpdate;

    #endregion

    #region Private Fields

    private FileSystemWatcher? journalWatcher;
    private FileSystemWatcher? statusWatcher;
    private readonly Dictionary<string, Type> journalTypes;
    private readonly Dictionary<string, int> currentLine;
    private LogMonitorState currentState = LogMonitorState.Idle; // Change via #SetLogMonitorState
    private bool firstStartMonitor = true;
    private readonly string[] EventsWithAncillaryFile = {
            "Cargo",
            "NavRoute",
            "Market",
            "Outfitting",
            "Shipyard",
            "Backpack",
            "FCMaterials",
            "ModuleInfo",
            "ShipLocker"
        };

    #endregion

    #region Private Methods

    private void SetLogMonitorState(LogMonitorState newState)
    {
            var oldState = currentState;
            currentState = newState;
            LogMonitorStateChanged?.Invoke(this, new LogMonitorStateChangedEventArgs
            {
                PreviousState = oldState,
                NewState = newState
            }); ;

            Debug.WriteLine("LogMonitor State change: {0} -> {1}", oldState, newState);
        }

    private void InitializeWatchers(string path)
    {
            var logDirectory = GetJournalFolder();

            journalWatcher = new FileSystemWatcher(logDirectory, "Journal.*.??.log")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size |
                                NotifyFilters.FileName | NotifyFilters.CreationTime
            };
            journalWatcher.Changed += LogChangedEvent;
            journalWatcher.Created += LogCreatedEvent;

            statusWatcher = new FileSystemWatcher(logDirectory, "Status.json")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };
            statusWatcher.Changed += StatusUpdateEvent;
        }

    private static string GetJournalFolder()
    {
            throw new NotImplementedException();
        }

    private List<(Exception ex, string file, string line)> ProcessLines(List<string> lines, string file)
    {
            var readErrors = new List<(Exception ex, string file, string line)>();
            foreach (var line in lines)
            {
                try
                {
                    DeserializeAndInvoke(line);
                }
                catch (Exception ex)
                {
                    readErrors.Add((ex, file, line));
                }
            }
            return readErrors;
        }

    private JournalEventArgs DeserializeToEventArgs(string eventType, string line)
    {

            var eventClass = journalTypes[eventType];
            var journalRead = typeof(JournalReader).GetMethod(nameof(JournalReader.ObservatoryDeserializer));
            var journalGeneric = journalRead.MakeGenericMethod(eventClass);
            var entry = journalGeneric.Invoke(null, new object[] { line });
            return new JournalEventArgs { journalType = eventClass, journalEvent = entry };
        }

    private void DeserializeAndInvoke(string line)
    {
            var eventType = JournalUtilities.GetEventType(line);
            if (!journalTypes.ContainsKey(eventType))
            {
                eventType = "JournalBase";
            }

            var journalEvent = DeserializeToEventArgs(eventType, line);

            JournalEntry?.Invoke(this, journalEvent);

            // Files are only valid if realtime, otherwise they will be stale or empty.
            if (!currentState.HasFlag(LogMonitorState.Batch) && EventsWithAncillaryFile.Contains(eventType))
            {
                HandleAncillaryFile(eventType);
            }
        }

    private void HandleAncillaryFile(string eventType)
    {
            var filename = eventType == "ModuleInfo"
                ? "ModulesInfo.json" // Just FDev things
                : eventType + ".json";

            // I have no idea what order Elite writes these files or if they're already written
            // by the time the journal updates.
            // Brief sleep to ensure the content is updated before we read it.

            // Some files are still locked by another process after 50ms.
            // Retry every 50ms for 0.5 seconds before giving up.

            string fileContent = null;
            var retryCount = 0;

            while (fileContent == null && retryCount < 10)
            {
                Thread.Sleep(50);
                try
                {
                    using var fileStream = File.Open(journalWatcher.Path + Path.DirectorySeparatorChar + filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var reader = new StreamReader(fileStream);
                    fileContent = reader.ReadToEnd();
                    var fileObject = DeserializeToEventArgs(eventType + "File", fileContent);
                    JournalEntry?.Invoke(this, fileObject);
                }
                catch
                {
                    retryCount++;
                }
            }
        }

    private static void ReportErrors(List<(Exception ex, string file, string line)> readErrors)
    {
            if (readErrors.Any())
            {
                var errorList = readErrors.Select(error =>
                {
                    string message;
                    if (error.ex.InnerException == null)
                    {
                        message = error.ex.Message;
                    }
                    else
                    {
                        message = error.ex.InnerException.Message;
                    }
                    return ($"Error reading file {error.file}: {message}", error.line);
                });

                ErrorReporter.ShowErrorPopup($"Journal Read Error{(readErrors.Count > 1 ? "s" : "")}", errorList.ToList());

            }
        }

    private void LogChangedEvent(object source, FileSystemEventArgs eventArgs)
    {
            var fileContent = ReadAllLines(eventArgs.FullPath);

            if (!currentLine.ContainsKey(eventArgs.FullPath))
            {
                currentLine.Add(eventArgs.FullPath, fileContent.Count - 1);
            }

            foreach (var line in fileContent.Skip(currentLine[eventArgs.FullPath]))
            {
                try
                {
                    DeserializeAndInvoke(line);
                }
                catch (Exception ex)
                {
                    ReportErrors(new List<(Exception ex, string file, string line)> { (ex, eventArgs.Name ?? string.Empty, line) });
                }
            }

            currentLine[eventArgs.FullPath] = fileContent.Count;
        }

    private static List<string> ReadAllLines(string path)
    {
            var lines = new List<string>();
            try
            {
                using StreamReader file = new(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                while (!file.EndOfStream)
                {
                    lines.Add(file.ReadLine() ?? string.Empty);
                }
            }
            catch (IOException ioEx)
            {
                ReportErrors(new List<(Exception, string, string)> { (ioEx, path, "<reading all lines>") });
            }
            return lines;
        }

    private void LogCreatedEvent(object source, FileSystemEventArgs eventArgs)
    {
            currentLine.Add(eventArgs.FullPath, 0);
            LogChangedEvent(source, eventArgs);
        }

    private void StatusUpdateEvent(object source, FileSystemEventArgs eventArgs)
    {
            var handler = StatusUpdate;
            var statusLines = ReadAllLines(eventArgs.FullPath);
            if (statusLines.Count > 0)
            {
                var status = JournalReader.ObservatoryDeserializer<Status>(statusLines[0]);
                Status = status;
                handler?.Invoke(this, new JournalEventArgs { journalType = typeof(Status), journalEvent = status });
            }
        }

    /// <summary>
    /// Touches most recent journal file once every 250ms while LogMonitor is monitoring.
    /// Forces pending file writes to flush to disk and fires change events for new journal lines.
    /// </summary>
    private async void JournalPoke()
    {
            var journalFolder = GetJournalFolder();

            await Task.Run(() =>
            {
                while (IsMonitoring())
                {
                    var journals = GetJournalFilesOrdered(journalFolder);

                    if (journals.Any())
                    {
                        var fileToPoke = GetJournalFilesOrdered(journalFolder).Last();
                        using var stream = fileToPoke.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        stream.Close();
                    }
                    Thread.Sleep(250);
                }
            });
        }

    private static string GetSavedGamesPath()
    {
            throw new NotImplementedException();
        }

    private static IEnumerable<FileInfo> GetJournalFilesOrdered(string path)
    {
            var journalFolder = new DirectoryInfo(path);
            return from file in journalFolder.GetFiles("Journal.*.??.log")
                   orderby file.LastWriteTime
                   select file;
        }
    #endregion
}