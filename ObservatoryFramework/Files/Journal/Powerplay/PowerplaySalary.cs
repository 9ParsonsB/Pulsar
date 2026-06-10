namespace Observatory.Framework.Files.Journal.Powerplay;

/// <summary>
///     Written when receiving salary payment from a power.
/// </summary>
public class PowerplaySalary : PowerplayJoin
{
    public int Amount { get; init; }
}