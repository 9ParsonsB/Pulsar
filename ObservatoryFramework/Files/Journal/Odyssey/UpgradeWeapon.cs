namespace Observatory.Framework.Files.Journal.Odyssey;

public class UpgradeWeapon : JournalBase
{
    public override string Event => "UpgradeWeapon";
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public ulong SuitModuleID { get; init; }
    public int Class { get; init; }
    public int Cost { get; init; }
}