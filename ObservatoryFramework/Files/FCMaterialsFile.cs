namespace Observatory.Framework.Files;

using Journal;
using ParameterTypes;

/// <summary>
///     Elite Dangerous fcmaterials.json file. Contains data about current fleet carrier bartender stock.
/// </summary>
public class FCMaterialsFile : JournalBase
{
    /// <summary>
    ///     List of items in stock and in demand from the carrier bartender.
    /// </summary>
    public List<FCMaterial> Items { get; init; }
}