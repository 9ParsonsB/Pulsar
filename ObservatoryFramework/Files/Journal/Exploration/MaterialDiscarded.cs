namespace Observatory.Framework.Files.Journal.Exploration;

/// <summary>
/// Event generated when discarding held material resources.
/// </summary>
public class MaterialDiscarded : MaterialCollected
{
    public override string Event => "MaterialDiscarded";
}