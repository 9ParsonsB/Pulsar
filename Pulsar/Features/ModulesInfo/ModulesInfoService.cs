namespace Pulsar.Features.ModulesInfo;

using Observatory.Framework.Files;

public interface IModulesInfoService : IJournalHandler<ModuleInfoFile>;

public class ModulesInfoService(
    ILogger<ModulesInfoService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub) : IModulesInfoService
{
    public string FileName => FileHandlerService.ModulesInfoFileName;

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

    public async Task HandleFile(string filePath)
    {
        if (!ValidateFile(filePath))
        {
            return;
        }

        var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var moduleInfo = await JsonSerializer.DeserializeAsync<ModuleInfoFile>(file);

        if (moduleInfo == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }

        await hub.Clients.All.ModuleInfoUpdated(moduleInfo);
    }

    public async Task<ModuleInfoFile> Get()
    {
        var moduleInfoFile = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!ValidateFile(moduleInfoFile))
        {
            return new ModuleInfoFile();
        }

        await using var file = File.Open(moduleInfoFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var moduleInfo = await JsonSerializer.DeserializeAsync<ModuleInfoFile>(file);
        if (moduleInfo != null) return moduleInfo;

        logger.LogWarning("Failed to deserialize module info file {ModuleInfoFile}", moduleInfoFile);
        return new ModuleInfoFile();
    }
}
