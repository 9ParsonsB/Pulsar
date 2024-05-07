using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal.Odyssey;
using Pulsar.Features.ModulesInfo;
using Pulsar.Features.ShipLocker;

namespace Pulsar.Features;

public interface IFileHandler
{
    Task HandleFile(string path);
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
        JournalLogFileNameRegEx,
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
        { ShipLockerFileName, typeof(IJournalHandler<ShipLockerMaterials>) },
        { CargoFileName, typeof(IJournalHandler<CargoFile>) },
    };

    public async Task HandleFile(string path)
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

        if (Handlers.TryGetValue(match, out var type))
        {
            logger.LogInformation("Handling file {FileName} with Type {Type}", fileName);
            (serviceProvider.GetRequiredService(type) as IJournalHandler)?.HandleFile(path);
            return;
        }

        logger.LogInformation("File {FileName} was not handled", fileName);
    }
}