namespace Pulsar.Features;

using Microsoft.Extensions.FileProviders;
using System.Collections.Concurrent;

public class FileWatcherService(
    IOptions<PulsarConfiguration> options,
    IFileHandlerService fileHandlerService,
    ILogger<FileWatcherService> logger)
    : IHostedService, IDisposable
{
    private readonly ConcurrentDictionary<string, DateTimeOffset> FileDates = new();
    private PhysicalFileProvider watcher = null!;
    private IDisposable? watchRegistration;

    public void Dispose()
    {
        watchRegistration?.Dispose();
        watcher?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(options.Value.JournalDirectory))
            throw new Exception($"Directory {options.Value.JournalDirectory} does not exist.");

        watcher = new PhysicalFileProvider(options.Value.JournalDirectory);
        Watch(cancellationToken);

        // read the journal directory to get the initial files
        HandleFileChanged(cancellationToken);


        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        watchRegistration?.Dispose();
        watcher?.Dispose();
        return Task.CompletedTask;
    }

    private void HandleFileChanged(CancellationToken token = new())
    {
        Watch(token);
        var tasks = new List<Task>();
        try
        {
            var files = watcher.GetDirectoryContents("")
                .Where(file => !file.IsDirectory && (file.Name.EndsWith(".json") ||
                                                     (file.Name.StartsWith(FileHandlerService
                                                          .JournalLogFileNameStart) &&
                                                      file.Name.EndsWith(FileHandlerService.JournalLogFileNameEnd))))
                .OrderBy(file => file.Name)
                .ToList();

            if (!options.Value.ProcessHistoricalJournals)
            {
                var journals = files.Where(f => f.Name.StartsWith(FileHandlerService.JournalLogFileNameStart)).ToList();
                if (journals.Count > 0)
                {
                    var latestJournals = journals.TakeLast(10).ToList();
                    var journalsToSkip = journals
                        .Where(j => j.PhysicalPath != null && !latestJournals.Contains(j) &&
                                    !FileDates.ContainsKey(j.PhysicalPath)).ToList();

                    foreach (var journal in journalsToSkip)
                        if (journal.PhysicalPath != null)
                            FileDates.TryAdd(journal.PhysicalPath, journal.LastModified);
                    files.RemoveAll(journalsToSkip.Contains);
                }
            }

            foreach (var file in files)
            {
                if (file.PhysicalPath == null)
                    continue;

                logger.LogDebug("Checking File: {File}", file.PhysicalPath);

                logger.LogDebug("Has File Updated?: {File}, {LastModified}", file.PhysicalPath, file.LastModified);

                var shouldHandle = false;
                FileDates.AddOrUpdate(file.PhysicalPath, _ =>
                {
                    logger.LogDebug("New File: {File}", file.PhysicalPath);
                    shouldHandle = true;
                    return file.LastModified;
                }, (_, existing) =>
                {
                    logger.LogDebug("Existing File: {File}", file.PhysicalPath);
                    if (existing != file.LastModified)
                    {
                        logger.LogDebug("File Updated: {File}", file.PhysicalPath);
                        shouldHandle = true;
                    }

                    return file.LastModified;
                });

                if (shouldHandle) tasks.Add(fileHandlerService.HandleFile(file.PhysicalPath, token));
            }

            Task.WaitAll(tasks.ToArray(), token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling file change");
        }
    }

    private void Watch(CancellationToken token)
    {
        void Handle(object? _)
        {
            HandleFileChanged(token);
        }

        try
        {
            watchRegistration?.Dispose();
            watchRegistration = watcher.Watch("*.*").RegisterChangeCallback(Handle, null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error watching directory {Directory}", watcher.Root);
        }
    }
}