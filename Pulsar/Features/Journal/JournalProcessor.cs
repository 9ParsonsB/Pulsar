namespace Pulsar.Features.Journal;

using Observatory.Framework;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Travel;
using Observatory.Framework.Files.Journal.Startup;
using Observatory.Framework.Files.Journal.StationServices;
using Observatory.Framework.Files.Journal.Exploration;

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

            JournalBase? journal = null;
            try
            {
                journal = JsonSerializer.Deserialize<JournalBase>(new ReadOnlySpan<byte>(line.ToArray()), options);
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
                    case Missions missions when context.Missions.Any(m => m.Timestamp == missions.Timestamp):
                        continue;
                    case Missions missions:
                        await context.Missions.AddAsync(missions, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Location location when context.Locations.Any(l => l.Timestamp == location.Timestamp):
                        continue;
                    case Location location:
                        await context.Locations.AddAsync(location, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Loadout loadout when context.Loadout.Any(l => l.Timestamp == loadout.Timestamp):
                        continue;
                    case SupercruiseExit supercruiseExit when context.SupercruiseExit.Any(s => s.Timestamp == supercruiseExit.Timestamp):
                        continue;
                    case SupercruiseExit supercruiseExit:
                        await context.SupercruiseExit.AddAsync(supercruiseExit, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case SupercruiseEntry supercruiseEntry when context.SupercruiseEntry.Any(s => s.Timestamp == supercruiseEntry.Timestamp):
                        continue;
                    case SupercruiseEntry supercruiseEntry:
                        await context.SupercruiseEntry.AddAsync(supercruiseEntry, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Liftoff liftoff when context.Liftoff.Any(l => l.Timestamp == liftoff.Timestamp):
                        continue;
                    case Liftoff liftoff:
                        await context.Liftoff.AddAsync(liftoff, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Touchdown touchdown when context.Touchdown.Any(t => t.Timestamp == touchdown.Timestamp):
                        continue;
                    case Touchdown touchdown:
                        await context.Touchdown.AddAsync(touchdown, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case FSDJump fsdJump when context.FSDJump.Any(f => f.Timestamp == fsdJump.Timestamp):
                        continue;
                    case FSDJump fsdJump:
                        await context.FSDJump.AddAsync(fsdJump, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Docked docked when context.Docked.Any(d => d.Timestamp == docked.Timestamp):
                        continue;
                    case Docked docked:
                        await context.Docked.AddAsync(docked, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Undocked undocked when context.Undocked.Any(u => u.Timestamp == undocked.Timestamp):
                        continue;
                    case Undocked undocked:
                        await context.Undocked.AddAsync(undocked, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Scan scan when context.Scans.Any(s => s.Timestamp == scan.Timestamp && s.BodyID == scan.BodyID):
                        continue;
                    case Scan scan:
                        await context.Scans.AddAsync(scan, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case FSSDiscoveryScan fssDiscoveryScan when context.FSSDiscoveryScans.Any(f => f.Timestamp == fssDiscoveryScan.Timestamp):
                        continue;
                    case FSSDiscoveryScan fssDiscoveryScan:
                        await context.FSSDiscoveryScans.AddAsync(fssDiscoveryScan, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case FSSBodySignals fssBodySignals when context.FSSBodySignals.Any(f => f.Timestamp == fssBodySignals.Timestamp && f.BodyID == fssBodySignals.BodyID):
                        continue;
                    case FSSBodySignals fssBodySignals:
                        await context.FSSBodySignals.AddAsync(fssBodySignals, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case SAASignalsFound saaSignalsFound when context.SAASignalsFound.Any(s => s.Timestamp == saaSignalsFound.Timestamp && s.BodyID == saaSignalsFound.BodyID):
                        continue;
                    case SAASignalsFound saaSignalsFound:
                        await context.SAASignalsFound.AddAsync(saaSignalsFound, token);
                        await context.SaveChangesAsync(token);
                        break;
                    case Loadout loadout:
                        await context.Loadout.AddAsync(loadout, token);
                        await context.SaveChangesAsync(token);
                        break;
                }

                newJournals.Add(journal);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Error deserializing journal file: '{File}', line: {Line}", filePath, line);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Error updating database with journal file: '{File}', line: {Line}", filePath, line);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing journal file: '{File}', line# {LineNumber}, line: {Line}, type?: {Type}",
                    filePath, index, Encoding.UTF8.GetString(line.ToArray()), journal?.GetType());
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
                        var journals = await HandleFileInner(file, tokenSource.Token);
                        handled.AddRange(await AugmentJournals(journals));
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

    private async Task<List<JournalBase>> AugmentJournals(List<JournalBase> journals)
    {
        var augmented = new List<JournalBase>();
        foreach (var journal in journals)
        {
            if (journal is SAASignalsFound signals)
            {
                var scan = await context.Scans
                    .Where(s => s.SystemAddress == signals.SystemAddress && s.BodyID == signals.BodyID)
                    .OrderByDescending(s => s.Timestamp)
                    .FirstOrDefaultAsync();
                
                if (scan != null)
                {
                    // For now we just add the body name to the event if it's missing (though it shouldn't be)
                    // In a more advanced implementation we might add more data.
                }
            }
            augmented.Add(journal);
        }
        return augmented;
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