namespace Pulsar.Features.NavRoute;

using Observatory.Framework.Files;

public interface INavRouteService :  IJournalHandler<NavRouteFile>;

public class NavRouteService(
    IOptions<PulsarConfiguration> options, 
    ILogger<NavRouteService> logger, 
    IEventHubContext hub) 
    : INavRouteService
{
    public async Task<NavRouteFile> Get()
    {
        var navRouteFile = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(navRouteFile))
        {
            return new NavRouteFile();
        }

        await using var file = File.Open(navRouteFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var shipLocker = await JsonSerializer.DeserializeAsync<NavRouteFile>(file);
        if (shipLocker != null) return shipLocker;

        logger.LogWarning("Failed to deserialize nav route file {ShipLockerFile}", navRouteFile);
        return new NavRouteFile();
    }

    public async Task HandleFile(string path, CancellationToken token = new ())
    {
        if (!FileHelper.ValidateFile(path))
        {
            return;
        }

        var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var navRoute = await JsonSerializer.DeserializeAsync<NavRouteFile>(file, cancellationToken: token);

        if (navRoute == null)
        {
            logger.LogWarning("Failed to deserialize nav route {FilePath}", file);
            return;
        }

        await hub.Clients.All.NavRouteUpdated(navRoute);
    }

    public string FileName => FileHandlerService.NavRouteFileName;
}