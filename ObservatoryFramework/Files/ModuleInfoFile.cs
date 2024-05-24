using System.Collections.Immutable;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files;

/// <summary>
/// Information about current player ship equipped modules.
/// </summary>
public class ModuleInfoFile : JournalBase
{
    public override string Event => "ModuleInfo";
    /// <summary>
    /// List of all equipped modules.
    /// </summary>
    public ImmutableList<Module> Modules { get; init; }
}