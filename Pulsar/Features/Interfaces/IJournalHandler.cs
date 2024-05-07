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
{
    Task<T> Get();
}
