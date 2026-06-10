namespace Pulsar.Features.Status;

using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal.Other;
using Observatory.Framework.Files.ParameterTypes;

public interface IStatusService : IJournalHandler<Status>;

public class StatusService(
    ILogger<StatusService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub,
    PulsarContext context,
    Overlay.IOverlayStateService overlayStateService
) : IStatusService
{
    public string FileName => FileHandlerService.StatusFileName;

    public async Task HandleFile(string filePath, CancellationToken token = new())
    {
        if (!FileHelper.ValidateFile(filePath)) return;

        await using var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        if (file.Length < 2)
        {
            logger.LogWarning("File {FilePath} is empty", filePath);
            return;
        }

        var status = await JsonSerializer.DeserializeAsync<Status>(file, cancellationToken: token);

        if (status == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }

        overlayStateService.ApplyStatus(status);
        await hub.Clients.All.StatusUpdated(status);
    }

    public async Task<Status> Get()
    {
        var statusFile = Path.Join(options.Value.JournalDirectory, FileName);

        if (FileHelper.ValidateFile(statusFile))
        {
            await using var file = File.Open(statusFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var status = await JsonSerializer.DeserializeAsync<Status>(file);
            if (status != null) return status;

            logger.LogWarning("Failed to deserialize status file {StatusFile}", statusFile);
        }

        return await BuildStatusFromJournal();
    }

    private async Task<Status> BuildStatusFromJournal()
    {
        var loadGame = await context.LoadGames
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync();
        if (loadGame == null) return new Status();

        var cargo = await context.Cargo
            .Where(x => x.Timestamp >= loadGame.Timestamp)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync();
        var reservoirReplenished = await context.Set<ReservoirReplenished>()
            .Where(x => x.Timestamp >= loadGame.Timestamp)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync();

        return new Status
        {
            Timestamp = reservoirReplenished?.Timestamp ?? cargo?.Timestamp ?? loadGame.Timestamp,
            Fuel = new FuelType
            {
                FuelMain = reservoirReplenished?.FuelMain ?? (float)loadGame.FuelLevel,
                FuelReservoir = reservoirReplenished?.FuelReservoir ?? 0
            },
            Cargo = cargo?.Count,
            Balance = loadGame.Credits
        };
    }
}
