namespace Pulsar.Features.Journal;

using Observatory.Framework.Files.Journal;

public interface IJournalService : IJournalHandler<List<JournalBase>>
{
    /// <summary>
    /// Gets the Latest of the following (start of game) events:
    /// Commander
    /// Materials
    /// Rank
    /// Progress
    /// Reputation
    /// EngineerProgress
    /// LoadGame
    /// Statistics
    /// </summary>
    /// <returns></returns>
    Task<List<JournalBase>> GetLastStartupEvents();

    /// <summary>
    /// Get the Latest of the following events: 
    /// <p>
    /// Location<br/>
    /// Powerplay<br/>
    /// Music<br/>
    /// ShipLocker<br/>
    /// Missions<br/>
    /// Loadout</p>
    /// <p>When there are none of an event since the last game start, no event will be given.</p>
    /// </summary>
    /// <returns></returns>
    Task<List<JournalBase>> GetLatestState();
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
    // -- ...
    // Statistics
    public async Task<List<JournalBase>> GetLastStartupEvents()
    {
        var commander = await context.Commander.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var materials = await context.Materials.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var rank = await context.Rank.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var progress = await context.Progress.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var reputation = await context.Reputation.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var engineerProgress = await context.EngineerProgress.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var loadGame = await context.LoadGames.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var statistics = await context.Statistics.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();


        // if any null, return empty list
        if (materials == null || rank == null || progress == null || reputation == null || engineerProgress == null ||
            loadGame == null || statistics == null || commander == null)
        {
            return [];
        }

        // dont check the time of statistics as it may occur a few moments after
        if (commander.Timestamp > materials.Timestamp ||
            commander.Timestamp > materials.Timestamp ||
            commander.Timestamp > rank.Timestamp ||
            commander.Timestamp > progress.Timestamp ||
            commander.Timestamp > reputation.Timestamp ||
            commander.Timestamp > engineerProgress.Timestamp ||
            commander.Timestamp > loadGame.Timestamp ||
            commander.Timestamp > statistics.Timestamp)
        {
            throw new InvalidOperationException("Timestamps are invalid");
        }

        return [commander, materials, rank, progress, reputation, engineerProgress, loadGame, statistics];
    }

    /// <summary>
    /// StateEvents:
    /// Location
    /// Powerplay
    /// Music
    /// ShipLocker
    /// Missions
    /// Loadout
    /// Cargo
    /// </summary>
    /// <returns></returns>
    public async Task<List<JournalBase>> GetLatestState()
    {
        // dont get anything before the last command timestamp
        var commander = await context.Commander.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();

        if (commander == null) return [];
        
        var location = await context.Locations
            .Where(x => x.Timestamp > commander.Timestamp)
            .OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var powerplay = await context.PowerPlay
            .Where(x => x.Timestamp > commander.Timestamp)
            .OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var shiplocker = await context.ShipLocker
            .Where(x => x.Timestamp > commander.Timestamp)
            .OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var missions = await context.Missions
            .Where(x => x.Timestamp > commander.Timestamp)
            .OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var loadout = await context.Loadout
            .Where(x => x.Timestamp > commander.Timestamp)
            .OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        var cargo = await context.Cargo
            .Where(x => x.Timestamp > commander.Timestamp)
            .OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();

        return new List<JournalBase?> { location, powerplay, shiplocker, missions, loadout, cargo }
            .Where(x => x != null).Cast<JournalBase>().ToList();
    }

    public async Task<List<JournalBase>> Get()
    {
        return [];
    }
}