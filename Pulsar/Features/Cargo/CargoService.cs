namespace Pulsar.Features.Cargo;

using Observatory.Framework.Files;

public interface ICargoService : IJournalHandler<CargoFile>;

public class CargoService(IOptions<PulsarConfiguration> options, ILogger<CargoService> logger, IEventHubContext hub) : ICargoService
{
    public string FileName => "Cargo.json";
    
    public async Task HandleFile(string filePath, CancellationToken token = new())
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return;
        }

        var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var moduleInfo = await JsonSerializer.DeserializeAsync<CargoFile>(file, cancellationToken: token);

        if (moduleInfo == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }

        await hub.Clients.All.CargoUpdated(moduleInfo);
    }

    public async Task<CargoFile> Get()
    {
        var cargoFile = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(cargoFile))
        {
            return new CargoFile();
        }

        await using var file = File.Open(cargoFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var cargo = await JsonSerializer.DeserializeAsync<CargoFile>(file);
        if (cargo != null) return cargo;

        logger.LogWarning("Failed to deserialize module info file {CargoFile}", cargoFile);
        return new CargoFile();
    }
}