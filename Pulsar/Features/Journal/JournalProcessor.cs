using Observatory.Framework.Files.Journal.Startup;

namespace Pulsar.Features.Journal;

using Observatory.Framework;
using Observatory.Framework.Files.Journal;

public class JournalProcessor(
    ILogger<JournalProcessor> logger,
    IJournalService journalService,
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

                if (journal is LoadGame loadGame)
                {
                    // if not existing, add
                    if (context.LoadGames.Any(l => l.Timestamp == loadGame.Timestamp))
                    {
                        //return ValueTask.CompletedTask;
                        continue;
                    }
                    await context.LoadGames.AddAsync(loadGame, token);
                    await context.SaveChangesAsync(token);
                }

                newJournals.Add(journal);
            }   
            catch (JsonException ex)
            {
                logger.LogError(ex, "Error deserializing journal file: '{File}', line: {Line}", filePath, line);
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
                    if (journalService.TryDequeue(out var file))
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
