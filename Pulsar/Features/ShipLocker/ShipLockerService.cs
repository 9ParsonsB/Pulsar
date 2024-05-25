namespace Pulsar.Features.ShipLocker;

using Observatory.Framework.Files.Journal.Odyssey;

public interface IShipLockerService : IJournalHandler<ShipLockerMaterials>;

public class ShipLockerService(
    ILogger<ShipLockerService> logger, 
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub)
    : IShipLockerService
{
    public string FileName => FileHandlerService.ShipLockerFileName;

    public async Task<ShipLockerMaterials> Get()
    {
        var shipLockerFile = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(shipLockerFile))
        {
            return new ShipLockerMaterials();
        }

        await using var file = File.Open(shipLockerFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var shipLocker = await JsonSerializer.DeserializeAsync<ShipLockerMaterials>(file);
        if (shipLocker != null) return shipLocker;

        logger.LogWarning("Failed to deserialize ship locker file {ShipLockerFile}", shipLockerFile);
        return new ShipLockerMaterials();
    }

    public async Task HandleFile(string filePath, CancellationToken token = new())
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return;
        }

        var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var shipLocker = await JsonSerializer.DeserializeAsync<ShipLockerMaterials>(file, cancellationToken: token);

        if (shipLocker == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }

        await hub.Clients.All.ShipLockerUpdated(shipLocker);
    }
}