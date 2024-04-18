namespace Pulsar.Features;

public interface IFileHandlerService
{
    void HandleFile(string path);
}

public class FileHandlerService(ILogger<FileHandlerService> logger, IStatusService statusService) : IFileHandlerService
{
    public static readonly string MarketFileName = "Market.json";
    public static readonly string StatusFileName = "Status.json";
    public static readonly string OutfittingFileName = "Outfitting.json";
    public static readonly string ShipyardFileName = "Shipyard.json";
    public static readonly string ModulesFileName = "Modules.json";
    public static readonly string JournalFileName = "Journal.";
    public static readonly string RouteFileName = "Route.json";
    public static readonly string CargoFileName = "Cargo.json";
    public static readonly string BackpackFileName = "Backpack.json";
    public static readonly string ModulesInfoFileName = "ModulesInfo.json";
    public static readonly string ShipLockerFileName = "ShipLocker.json";
    public static readonly string NavRouteFileName = "NavRoute.json";
    
    public static readonly string[] AllFileNames =
    [
        MarketFileName,
        StatusFileName,
        OutfittingFileName,
        ShipyardFileName,
        ModulesFileName,
        JournalFileName,
        RouteFileName,
        CargoFileName,
        BackpackFileName,
        ModulesInfoFileName,
        ShipLockerFileName,
        NavRouteFileName
    ];
    
    private readonly Dictionary<string, IJournalHandler> Handlers = new()
    {
        { StatusFileName, statusService },
    };
    
    public void HandleFile(string path)
    {
        var fileInfo = new FileInfo(path);
        var fileName = fileInfo.Name;
        
        // only scan the file if we recognize it
        var match = AllFileNames.FirstOrDefault(
            f => fileName.StartsWith(f, StringComparison.InvariantCultureIgnoreCase));
        
        if (string.IsNullOrWhiteSpace(match))
        {
            logger.LogWarning("File {FileName} is not recognized", fileName);
            return;
        }

        if (Handlers.TryGetValue(match, out var handler))
        {
            logger.LogInformation("Handling file {FileName}", fileName);
            handler.HandleFile(fileInfo.Name);
            return;
        }
        
        logger.LogInformation("File {FileName} was not handled", fileName);
    }
}
