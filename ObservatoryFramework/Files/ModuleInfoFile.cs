namespace Observatory.Framework.Files;

using Journal;
using ParameterTypes;

/// <summary>
///     Information about current player ship equipped modules.
/// </summary>
public class ModuleInfoFile : JournalBase
{
    /// <summary>
    ///     List of all equipped modules.
    /// </summary>
    public List<Module> Modules { get; init; }
}