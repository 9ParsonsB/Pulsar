namespace Pulsar.Features.Journal;

using Observatory.Framework;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Exploration;
using Observatory.Framework.Files.Journal.Odyssey;
using Observatory.Framework.Files.Journal.Other;
using Observatory.Framework.Files.Journal.Startup;
using Observatory.Framework.Files.Journal.StationServices;
using Observatory.Framework.Files.Journal.Travel;
using System.Linq.Expressions;

public class JournalProcessor(
    ILogger<JournalProcessor> logger,
    IJournalStore journalStore,
    IServiceScopeFactory scopeFactory,
    IEventHubContext hub) : IHostedService, IDisposable
{
    private readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowOutOfOrderMetadataProperties = true,
        Converters = { new JournalInvalidDoubleConverter(), new JournalInvalidFloatConverter() },
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
    };

    private Task? processingTask;

    private CancellationTokenSource tokenSource = new();

    public void Dispose()
    {
        tokenSource?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        tokenSource.Dispose();
        tokenSource = new CancellationTokenSource();
        processingTask = ProcessQueueAsync(tokenSource.Token);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        tokenSource?.Cancel();

        if (processingTask == null)
            return;

        try
        {
            await processingTask.WaitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
    }

    public async Task<List<JournalBase>> HandleFileInner(
        string filePath,
        PulsarContext? context = null,
        CancellationToken token = new())
    {
        if (context == null)
        {
            using var scope = scopeFactory.CreateScope();
            var newContext = scope.ServiceProvider.GetRequiredService<PulsarContext>();
            return await HandleFileInner(filePath, newContext, token);
        }

        logger.LogInformation("Processing journal file: '{File}'", filePath);
        var newJournals = new List<JournalBase>();
        var entitiesToAdd = new List<object>();

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        context.ChangeTracker.AutoDetectChangesEnabled = false;
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        var batch = new List<JournalBase>();
        while (await reader.ReadLineAsync(token) is { } lineStr)
        {
            if (string.IsNullOrWhiteSpace(lineStr))
                continue;

            if (lineStr.Contains("\"RotationPeriod\":inf"))
                lineStr = lineStr.Replace("\"RotationPeriod\":inf", "\"RotationPeriod\":0");

            try
            {
                var journal = JsonSerializer.Deserialize<JournalBase>(lineStr, options);
                if (journal != null)
                {
                    batch.Add(journal);
                    newJournals.Add(journal);
                }
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Error deserializing journal file: '{File}', line: {Line}", filePath, lineStr);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing journal file: '{File}', line: {Line}", filePath, lineStr);
            }
        }

        if (batch.Count == 0) return newJournals;

        // Optimized batch processing
        var groups = batch.GroupBy(j => j.GetType());
        foreach (var group in groups)
        {
            var type = group.Key;
            var items = group.ToList();

            if (type == typeof(Commander))
                await FilterAndAdd(items, context.Commander, c => c.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Materials))
                await FilterAndAdd<Materials, DateTimeOffset>(items, context.Materials, m => m.Timestamp, entitiesToAdd,
                    token);
            else if (type == typeof(Rank))
                await FilterAndAdd(items, context.Rank, r => r.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Progress))
                await FilterAndAdd(items, context.Progress, p => p.Timestamp, entitiesToAdd,
                    token);
            else if (type == typeof(Reputation))
                await FilterAndAdd(items, context.Reputation, r => r.Timestamp, entitiesToAdd, token);
            else if (type == typeof(EngineerProgress))
                await FilterAndAdd<EngineerProgress, DateTimeOffset>(items, context.EngineerProgress, e => e.Timestamp,
                    entitiesToAdd, token);
            else if (type == typeof(LoadGame))
                await FilterAndAdd(items, context.LoadGames, l => l.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Statistics))
                await FilterAndAdd(items, context.Statistics, s => s.Timestamp,
                    entitiesToAdd, token);
            else if (type == typeof(Missions))
                await FilterAndAdd(items, context.Missions, m => m.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Location))
                await FilterAndAdd<Location, DateTimeOffset>(items, context.Locations, l => l.Timestamp, entitiesToAdd,
                    token);
            else if (type == typeof(SupercruiseExit))
                await FilterAndAdd(items, context.SupercruiseExit, s => s.Timestamp, entitiesToAdd, token);
            else if (type == typeof(SupercruiseEntry))
                await FilterAndAdd(items, context.SupercruiseEntry, s => s.Timestamp,
                    entitiesToAdd, token);
            else if (type == typeof(Liftoff))
                await FilterAndAdd(items, context.Liftoff, l => l.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Touchdown))
                await FilterAndAdd<Touchdown, DateTimeOffset>(items, context.Touchdown, t => t.Timestamp, entitiesToAdd,
                    token);
            else if (type == typeof(FSDJump))
                await FilterAndAdd(items, context.FSDJump, f => f.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Docked))
                await FilterAndAdd(items, context.Docked, d => d.Timestamp, entitiesToAdd,
                    token);
            else if (type == typeof(Undocked))
                await FilterAndAdd(items, context.Undocked, u => u.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Scan))
                await FilterAndAdd<Scan, (DateTimeOffset, int)>(items, context.Scans,
                    s => new ValueTuple<DateTimeOffset, int>(s.Timestamp, s.BodyID), entitiesToAdd, token);
            else if (type == typeof(FSSDiscoveryScan))
                await FilterAndAdd(items, context.FSSDiscoveryScans, f => f.Timestamp, entitiesToAdd, token);
            else if (type == typeof(FSSBodySignals))
                await FilterAndAdd(items, context.FSSBodySignals,
                    f => new ValueTuple<DateTimeOffset, int>(f.Timestamp, f.BodyID), entitiesToAdd, token);
            else if (type == typeof(SAASignalsFound))
                await FilterAndAdd(items, context.SAASignalsFound,
                    s => new ValueTuple<DateTimeOffset, int>(s.Timestamp, s.BodyID), entitiesToAdd, token);
            else if (type == typeof(Loadout))
                await FilterAndAdd(items, context.Loadout, l => l.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Cargo))
                await FilterAndAdd(items, context.Cargo, c => c.Timestamp, entitiesToAdd, token);
            else if (type == typeof(Powerplay))
                await FilterAndAdd(items, context.PowerPlay, p => p.Timestamp, entitiesToAdd, token);
            else if (type == typeof(ShipLockerMaterials))
                await FilterAndAdd<ShipLockerMaterials, DateTimeOffset>(items, context.ShipLocker, s => s.Timestamp,
                    entitiesToAdd, token);
            else if (type == typeof(ReservoirReplenished))
                await FilterAndAdd(items, context.ReservoirReplenished,
                    r => r.Timestamp, entitiesToAdd, token);
        }

        if (entitiesToAdd.Count > 0)
            try
            {
                context.ChangeTracker.AutoDetectChangesEnabled = true;
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                context.ChangeTracker.Clear();
                context.AddRange(entitiesToAdd);
                await context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving context for file '{File}'", filePath);
                throw;
            }

        return newJournals;
    }

    private async Task FilterAndAdd<T, TKey>(
        List<JournalBase> items,
        DbSet<T> dbSet,
        Expression<Func<T, TKey>> keySelector,
        List<object> entitiesToAdd,
        CancellationToken token) where T : JournalBase
    {
        var typedItems = items.Cast<T>().ToList();
        var minTimestamp = typedItems.Min(i => i.Timestamp);
        var maxTimestamp = typedItems.Max(i => i.Timestamp);

        var existingKeys = await dbSet
            .Where(e => e.Timestamp >= minTimestamp && e.Timestamp <= maxTimestamp)
            .Select(keySelector)
            .ToListAsync(token);

        var existingSet = new HashSet<TKey>(existingKeys);
        var addedInBatch = new HashSet<TKey>();

        var keyFunc = keySelector.Compile();

        foreach (var item in typedItems)
        {
            var key = keyFunc(item);
            if (!existingSet.Contains(key) && addedInBatch.Add(key)) entitiesToAdd.Add(item);
        }
    }

    private async Task ProcessQueueAsync(CancellationToken token)
    {
        var handled = new List<JournalBase>();
        while (!token.IsCancellationRequested)
            try
            {
                if (journalStore.TryDequeue(out var file))
                {
                    using var scope = scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<PulsarContext>();
                    var journals = await HandleFileInner(file, context, token);
                    handled.AddRange(await AugmentJournals(journals, context));
                }
                else if (handled.Count > 0)
                {
                    using var scope = scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<PulsarContext>();
                    var lastLoadGame = context.LoadGames.OrderByDescending(l => l.Timestamp).FirstOrDefault();
                    if (lastLoadGame != null)
                        handled = handled.Where(j => j.Timestamp > lastLoadGame.Timestamp).ToList();

                    await hub.Clients.All.JournalUpdated(handled);
                    handled.Clear();
                }
                else
                {
                    await Task.Delay(1000, token);
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing journal queue");
            }
    }

    private async Task<List<JournalBase>> AugmentJournals(List<JournalBase> journals, PulsarContext context)
    {
        var signals = journals.OfType<SAASignalsFound>().ToList();
        if (signals.Count == 0) return journals;

        var systemAddresses = signals.Select(s => s.SystemAddress).Distinct().ToList();
        var bodyIds = signals.Select(s => s.BodyID).Distinct().ToList();

        // Batch fetch all potentially relevant scans
        var scans = await context.Scans
            .Where(s => systemAddresses.Contains(s.SystemAddress) && bodyIds.Contains(s.BodyID))
            .OrderByDescending(s => s.Timestamp)
            .ToListAsync();

        foreach (var signal in signals)
        {
            var scan = scans
                .Where(s => s.SystemAddress == signal.SystemAddress && s.BodyID == signal.BodyID)
                .OrderByDescending(s => s.Timestamp)
                .FirstOrDefault();

            if (scan != null)
            {
                // Logic can be added here if needed in the future
            }
        }

        return journals;
    }
}