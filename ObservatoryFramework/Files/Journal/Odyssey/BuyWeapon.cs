namespace Observatory.Framework.Files.Journal.Odyssey;

public class BuyWeapon : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public int Price { get; init; }
    public ulong SuitModuleID { get; init; }
}