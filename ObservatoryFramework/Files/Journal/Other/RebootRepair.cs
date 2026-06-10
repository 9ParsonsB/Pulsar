namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when the 'reboot repair' function is used.
/// </summary>
public class RebootRepair : JournalBase
{
    public IList<string> Modules { get; init; }
}