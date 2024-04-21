namespace Pulsar.Features.ModulesInfo;

using Observatory.Framework.Files;

public interface IModulesInfoService : IJournalHandler<ModuleInfoFile>;

public class ModulesInfoService : IModulesInfoService
{
    public string FileName => FileHandlerService.ModulesInfoFileName;
    public Task HandleFile(string filePath)
    {
        throw new NotImplementedException();
    }
    
    public bool ValidateFile(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task<ModuleInfoFile> Get()
    {
        throw new NotImplementedException();
    }
}