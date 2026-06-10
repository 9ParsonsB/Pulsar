namespace Observatory.Framework.Files.Journal.Startup;

/// <summary>
///     Written at the start of the LoadGame process.
/// </summary>
public class Commander : JournalBase
{
    public string Name { get; init; }

    public string? FID { get; init; }
}