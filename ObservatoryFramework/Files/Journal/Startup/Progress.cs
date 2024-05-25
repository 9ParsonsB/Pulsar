namespace Observatory.Framework.Files.Journal.Startup;

public class Progress : JournalBase
{
    public override string Event => "Progress";
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int Combat { get; init; }
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int Trade { get; init; }
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int Explore { get; init; }
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int CQC { get; init; }
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int Soldier { get; init; }
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int Exobiologist { get; init; }
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int Empire { get; init; }
    /// <summary>
    /// percent progress towards next rank
    /// </summary>
    public int Federation { get; init; }
}