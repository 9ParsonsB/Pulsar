namespace Observatory.Framework.Files.Journal.Powerplay;

/// <summary>
///     Written when a player defects from one power to another.
/// </summary>
public class PowerplayDefect : JournalBase
{
    public string FromPower { get; init; }
    public string ToPower { get; init; }
}