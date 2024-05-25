using Microsoft.EntityFrameworkCore;
using Observatory.Framework.Files.Journal.Startup;
using Observatory.Framework.Files.Journal.StationServices;

namespace Pulsar.Features.Journal;

using Observatory.Framework.Files.Journal;

public interface IJournalService : IJournalHandler<List<JournalBase>>
{
    Task<List<JournalBase>> GetLastStartupEvents();
}

public class JournalService(
    ILogger<JournalService> logger,
    IJournalStore store,
    PulsarContext context
) : IJournalService
{
    public string FileName => FileHandlerService.JournalLogFileName;

    public Task HandleFile(string filePath, CancellationToken token = new())
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return Task.CompletedTask;
        }

        store.EnqueueFile(filePath);
        return Task.CompletedTask;
    }
    
    
    
        
    // Start of game events/order:
    /** Commander
     *  Materials
        Rank
        Progress
        Reputation
        EngineerProgress
        LoadGame
        --Some time later--
        Statistics
        -- Game Events (e.g. FSSSignalDiscovered) --
        Location
        Powerplay
        ShipLocker
        Missions
        Loadout
        Cargo
     */
    
    // StartupEvents:
    // Commander
    // Materials
    // Rank
    // Progress
    // Reputation
    // EngineerProgress
    // LoadGame

    // StateEvents:
    // Location
    // Powerplay
    // Music
    // ShipLocker
    // Missions
    // Loadout
    // Cargo
    public async Task<List<JournalBase>> GetLastStartupEvents()
    {
        //TODO: add other state events
        var commanderTask = context.Commander.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var materialsTask = context.Materials.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var rankTask = context.Rank.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var progressTask = context.Progress.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var reputationTask = context.Reputation.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var engineerProgressTask = context.EngineerProgress.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var loadGameTask = context.LoadGames.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var statisticsTask = context.Statistics.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();

        await Task.WhenAll(
            commanderTask,
            materialsTask,
            rankTask,
            progressTask,
            reputationTask,
            engineerProgressTask,
            loadGameTask,
            statisticsTask);

        var commander = await commanderTask;
        var materials = await materialsTask;
        var rank = await rankTask;
        var progress = await progressTask;
        var reputation = await reputationTask;
        var engineerProgress = await engineerProgressTask;
        var loadGame = await loadGameTask;
        var statistics = await statisticsTask;

        // if any null, return empty list
        if (materials == null || rank == null || progress == null || reputation == null || engineerProgress == null ||
            loadGame == null || statistics == null || commander == null)
        {
            return [];
        }

        // dont check the time of statistics as it may occur a few moments after
        if (commander.Timestamp != materials.Timestamp ||
            materials.Timestamp != rank.Timestamp ||
            rank.Timestamp != progress.Timestamp ||
            progress.Timestamp != reputation.Timestamp ||
            reputation.Timestamp != engineerProgress.Timestamp ||
            engineerProgress.Timestamp != loadGame.Timestamp || 
            statistics.Timestamp < materials.Timestamp)
        {
            throw new InvalidOperationException("Timestamps do not match");
        }

        return [commander, materials, rank, progress, reputation, engineerProgress, loadGame, statistics];
    }

    public async Task<List<JournalBase>> Get()
    {
        return [];
    }
}