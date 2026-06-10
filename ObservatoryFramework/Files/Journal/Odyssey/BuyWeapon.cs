namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when purchasing a new hand weapon.
/// </summary>
public class BuyWeapon : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public int Price { get; init; }
    public ulong SuitModuleID { get; init; }
}