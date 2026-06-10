namespace Observatory.Framework.Files.Journal.Powerplay;

/// <summary>
///     Written when receiving payment for powerplay combat.
/// </summary>
public class PowerplayVoucher : PowerplayJoin
{
    public IList<string> Systems { get; init; }
}