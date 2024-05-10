namespace Pulsar.Features.ModulesInfo;

using Observatory.Framework.Files;

public interface IModulesInfoService : IJournalHandler<ModuleInfoFile>;

public class ModulesInfoService(
    ILogger<ModulesInfoService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub) : IModulesInfoService
{
    public string FileName => FileHandlerService.ModulesInfoFileName;

    public async Task HandleFile(string filePath)
    {
        if (!FileHelper.ValidateFile(filePath))
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

        if (!FileHelper.ValidateFile(moduleInfoFile))
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
