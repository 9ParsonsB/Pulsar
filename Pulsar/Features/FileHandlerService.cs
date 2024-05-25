namespace Pulsar.Features;

using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Odyssey;

public interface IFileHandler
{
    Task HandleFile(string path, CancellationToken token = new());
}

public interface IFileHandlerService : IFileHandler;

public class FileHandlerService(
    ILogger<FileHandlerService> logger,
    IServiceProvider serviceProvider) : IFileHandlerService
{
    public static readonly string MarketFileName = "Market.json";
    public static readonly string StatusFileName = "Status.json";
    public static readonly string OutfittingFileName = "Outfitting.json";
    public static readonly string ShipyardFileName = "Shipyard.json";
    public static readonly string ModulesFileName = "Modules.json";
    public static readonly string JournalLogFileNameRegEx = @"Journal\.\d\d\d\d-\d\d-\d\dT\d+\.\d\d\.log";
    public static readonly string JournalLogFileName = "Journal.*.log";
    public static readonly string JournalLogFileNameStart = "Journal.";
    public static readonly string JournalLogFileNameEnd = ".log";
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
        JournalLogFileNameStart,
        RouteFileName,
        CargoFileName,
        BackpackFileName,
        ModulesInfoFileName,
        ShipLockerFileName,
        NavRouteFileName
    ];

    private readonly Dictionary<string, Type> Handlers = new()
    {
        { StatusFileName, typeof(IJournalHandler<Observatory.Framework.Files.Status>) },
        { ModulesInfoFileName, typeof(IJournalHandler<ModuleInfoFile>) },
        { ShipLockerFileName, typeof(IJournalHandler<ShipLockerMaterials>) },
        { ShipyardFileName, typeof(IJournalHandler<ShipyardFile>) },
        { MarketFileName, typeof(IJournalHandler<MarketFile>) },
        { NavRouteFileName, typeof(IJournalHandler<NavRouteFile>) },
        { CargoFileName, typeof(IJournalHandler<CargoFile>) },
        { BackpackFileName, typeof(IJournalHandler<BackpackFile>) },
        { RouteFileName, typeof(IJournalHandler<NavRouteFile>) },
        { OutfittingFileName, typeof(IJournalHandler<OutfittingFile>) },
        { JournalLogFileNameStart, typeof(IJournalHandler<List<JournalBase>>) }
    };
    

    public async Task HandleFile(string path, CancellationToken token = new())
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

        if (!Handlers.TryGetValue(match, out var type))
        {
            logger.LogWarning("File {FileName} was not handled", fileName);
            return;
        }
        
        if (serviceProvider.GetRequiredService(type) is not IJournalHandler handler)
        {
            logger.LogWarning("Handler for {FileName} is not available", fileName);
            return;
        }
            
        logger.LogInformation("Handling file {FileName} with Type {Type}", fileName, handler.GetType().ToString());
        await handler.HandleFile(path, token);
    }
}