using Observatory.Framework.Files.Journal.StationServices;

namespace Pulsar.Features.Journal;

using Observatory.Framework;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Startup;

public class JournalProcessor(
    ILogger<JournalProcessor> logger,
    IJournalStore journalStore,
    PulsarContext context,
    IEventHubContext hub) : IHostedService, IDisposable
{
    private CancellationTokenSource tokenSource = new();

    readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowOutOfOrderMetadataProperties = true,
        Converters = { new JournalInvalidDoubleConverter(), new JournalInvalidFloatConverter() },
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
    };

    public async Task<List<JournalBase>> HandleFileInner(string filePath, CancellationToken token = new())
    {
        logger.LogInformation("Processing journal file: '{File}'", filePath);
        var file = await File.ReadAllBytesAsync(filePath, token);
        var lines = file.Split(Encoding.UTF8.GetBytes(Environment.NewLine)).ToList();
        var newJournals = new List<JournalBase>();
        //await Parallel.ForEachAsync(lines, new ParallelOptions() { MaxDegreeOfParallelism = 32, TaskScheduler = TaskScheduler.Default, CancellationToken = token}, (line, _) =>
        for (var index = 0; index < lines.Count; index++)
        {
            var line = lines[index];
            if (line.Count == 0)
            {
//                return ValueTask.CompletedTask;
                continue;
            }

            if (line.Contains("\"RotationPeriod\":inf"u8.ToArray()))
            {
                var newLine = line.Replace("\"RotationPeriod\":inf"u8, "\"RotationPeriod\":0"u8);
                line = newLine;
            }

            try
            {
                var journal = JsonSerializer.Deserialize<JournalBase>(new ReadOnlySpan<byte>(line.ToArray()), options);
                if (journal == null)
                {
                    //return ValueTask.CompletedTask;
                    continue;
                }

                switch (journal)
                {
                    case Commander commander when context.Commander.Any(c => c.Timestamp == commander.Timestamp):
                        continue;
                    case Commander commander:
                        await context.Commander.AddAsync(commander, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Materials materials when context.Materials.Any(m => m.Timestamp == materials.Timestamp):
                        continue;
                    case Materials materials:
                        await context.Materials.AddAsync(materials, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Rank rank when context.Rank.Any(r => r.Timestamp == rank.Timestamp):
                        continue;
                    case Rank rank:
                        await context.Rank.AddAsync(rank, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Progress progress when context.Progress.Any(p => p.Timestamp == progress.Timestamp):
                        continue;
                    case Progress progress:
                        await context.Progress.AddAsync(progress, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Reputation reputation when context.Reputation.Any(r => r.Timestamp == reputation.Timestamp):
                        continue;
                    case Reputation reputation:
                        await context.Reputation.AddAsync(reputation, token);
                        await context.SaveChangesAsync(token);
                        break;

                    case EngineerProgress engineerProgress
                        when context.EngineerProgress.Any(e => e.Timestamp == engineerProgress.Timestamp):
                        continue;
                    case EngineerProgress engineerProgress:
                        await context.EngineerProgress.AddAsync(engineerProgress, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case LoadGame loadGame when context.LoadGames.Any(l => l.Timestamp == loadGame.Timestamp):
                        continue;
                    case LoadGame loadGame:
                        await context.LoadGames.AddAsync(loadGame, token);
                        await context.SaveChangesAsync(token);
                        break;
                    
              
                    case Statistics statistics when context.Statistics.Any(s => s.Timestamp == statistics.Timestamp):
                        continue;
                    case Statistics statistics:
                        await context.Statistics.AddAsync(statistics, token);
                        await context.SaveChangesAsync(token);
                        break;
                    
                }

                newJournals.Add(journal);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Error deserializing journal file: '{File}', line: {Line}", filePath, line);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing journal file: '{File}', line# {LineNumber}, line: {Line}",
                    filePath, index, Encoding.UTF8.GetString(line.ToArray()));
            }

            //return ValueTask.CompletedTask;
        }

        return newJournals;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        tokenSource.Dispose();
        tokenSource = new();
        ProcessQueue();
        return Task.CompletedTask;
    }

    private void ProcessQueue()
    {
        Task.Run(async () =>
        {
            var token = tokenSource.Token;
            var handled = new List<JournalBase>();
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (journalStore.TryDequeue(out var file))
                    {
                        handled.AddRange(await HandleFileInner(file, tokenSource.Token));
                    }
                    else if (handled.Count > 0)
                    {
                        //get last loadgame
                        var lastLoadGame = context.LoadGames.OrderByDescending(l => l.Timestamp).FirstOrDefault();
                        // only emit journals since last loadgame
                        if (lastLoadGame != null)
                        {
                            handled = handled.Where(j => j.Timestamp > lastLoadGame.Timestamp).ToList();
                        }

                        await hub.Clients.All.JournalUpdated(handled);
                        handled.Clear();
                    }
                    else
                    {
                        await Task.Delay(1000, token);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing journal queue");
                }
            }
        }, tokenSource.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        tokenSource?.Cancel();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        tokenSource?.Dispose();
    }
}