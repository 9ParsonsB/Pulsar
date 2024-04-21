using Observatory.Framework.Files.Journal;

namespace Pulsar.Features;

/// <summary>
/// Interface for Handling Journal Files.
/// </summary>
public interface IJournalHandler : IFileHandler
{
    string FileName { get; }
    
    public bool ValidateFile(string filePath);
}

/// <summary>
/// Interface for Getting Journal Files.
/// Only used for Controllers
/// </summary>
public interface IJournalHandler<T> : IJournalHandler
    where T: IJournal
{
    Task<T> Get();
}

public abstract class JournalHandlerBase<T>(ILogger logger) : IJournalHandler<T>
    where T: IJournal
{
    public abstract string FileName { get; }

    public abstract Task HandleFile(string filePath);

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
            logger.LogWarning("Journal file {JournalFile} is not a {NameOfCurrentHandler} file", filePath, nameof(T));
            return false;
        }

        if (fileInfo.Length == 0)
        {
            logger.LogWarning("Journal file {JournalFile} is empty", filePath);
            return false;
        }

        return true;
    }

    public abstract Task<T> Get();
}