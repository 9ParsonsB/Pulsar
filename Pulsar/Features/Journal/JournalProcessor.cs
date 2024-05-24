namespace Pulsar.Features.Journal;

using Observatory.Framework;
using Observatory.Framework.Files.Journal;

public class JournalProcessor(
    ILogger<JournalProcessor> logger,
    IJournalService journalService,
    IEventHubContext hub) : IJournalProcessor, IHostedService, IDisposable
{
    private CancellationTokenSource tokenSource = new();
    
    readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowOutOfOrderMetadataProperties = true,
        Converters = { new JournalInvalidDoubleConverter(), new JournalInvalidFloatConverter() },
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
    };

    public async Task HandleFileInner(string filePath, CancellationToken token = new())
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

                newJournals.Add(journal);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Error deserializing journal file: '{File}', line: {Line}", filePath, line);
            }

            //return ValueTask.CompletedTask;
        }


        if (newJournals.Any())
        {
            await hub.Clients.All.JournalUpdated(newJournals);
        }
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
            while (!tokenSource.Token.IsCancellationRequested)
            {
                if (journalService.TryDequeue(out var file))
                {
                    await HandleFileInner(file, tokenSource.Token);
                }
                else
                {
                    await Task.Delay(1000);
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

public interface IJournalProcessor
{
}