using Observatory.Framework.Files;

namespace Pulsar.Features.Shipyard;

public interface IShipyardService : IJournalHandler<ShipyardFile>;

public class ShipyardService(ILogger<ShipyardService> logger, IOptions<PulsarConfiguration> options,
    IEventHubContext hub) : IShipyardService
{
    public string FileName => FileHandlerService.ShipyardFileName;
    public async Task<ShipyardFile> Get()   
    {
        var shipyardFile = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(shipyardFile))
        {
            return new ShipyardFile();
        }

        await using var file = File.Open(shipyardFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var shipyard = await JsonSerializer.DeserializeAsync<ShipyardFile>(file);
        if (shipyard != null) return shipyard;

        logger.LogWarning("Failed to deserialize shipyard file {ShipyardFile}", shipyardFile);
        return new ShipyardFile();
    }

    public async Task HandleFile(string path, CancellationToken token = new())
    {
        if (!FileHelper.ValidateFile(path))
        {
            return;
        }

        var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var shipyard = await JsonSerializer.DeserializeAsync<ShipyardFile>(file, cancellationToken: token);

        if (shipyard == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", path);
            return;
        }

        await hub.Clients.All.ShipyardUpdated(shipyard);
    }

}