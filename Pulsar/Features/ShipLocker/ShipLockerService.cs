namespace Pulsar.Features.ShipLocker;

using Observatory.Framework.Files.Journal.Odyssey;

public interface IShipLockerService : IJournalHandler<ShipLockerMaterials>;

public class ShipLockerService(ILogger<ShipLockerService> logger)
    : IShipLockerService
{
    public string FileName => FileHandlerService.ShipLockerFileName;

    public bool ValidateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            logger.LogWarning("Journal file {JournalFile} does not exist", filePath);
            return false;
        }

        var fileInfo = new FileInfo(filePath);

        if (!string.Equals(fileInfo.Name, FileName, StringComparison.InvariantCultureIgnoreCase))
        {
            logger.LogWarning("Journal file {name} is not valid");
            return false;
        }

        if (fileInfo.Length == 0)
        {
            logger.LogWarning("Journal file {name} is empty", filePath);
            return false;
        }

        return true;
    }

    public Task<ShipLockerMaterials> Get()
    {
        throw new NotImplementedException();
    }

    public Task HandleFile(string filePath)
    {
        throw new NotImplementedException();
    }
}