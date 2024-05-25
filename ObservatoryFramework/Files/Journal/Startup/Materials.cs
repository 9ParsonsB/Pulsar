namespace Observatory.Framework.Files.Journal.Startup;

using System.Collections.Immutable;
using ParameterTypes;

public class Materials : JournalBase
{
    public override string Event => "Materials";
    public virtual IReadOnlyCollection<Material> Raw { get; init; }
    public virtual IReadOnlyCollection<Material> Manufactured { get; init; }
    public virtual IReadOnlyCollection<Material> Encoded { get; init; }

}