namespace Observatory.Framework.Files.Journal.Startup;

using System.Collections.Immutable;
using ParameterTypes;

public class Materials : JournalBase
{
    public override string Event => "Materials";
    public virtual List<Material> Raw { get; init; }
    public virtual List<Material> Manufactured { get; init; }
    public virtual List<Material> Encoded { get; init; }

}