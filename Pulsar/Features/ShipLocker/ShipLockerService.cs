namespace Pulsar.Features.ShipLocker;

using Observatory.Framework.Files.Journal.Odyssey;

public interface IShipLockerService : IJournalHandler<ShipLockerMaterials>;

public class ShipLockerService(ILogger<ShipLockerService> logger)
    : JournalHandlerBase<ShipLockerMaterials>(logger), IShipLockerService
{
    public override string FileName => FileHandlerService.ShipLockerFileName;

    public override Task<ShipLockerMaterials> Get()
    {
        throw new NotImplementedException();
    }

    public override Task HandleFile(string filePath)
    {
        throw new NotImplementedException();
    }
}