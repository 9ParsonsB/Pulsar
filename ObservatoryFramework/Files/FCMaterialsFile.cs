﻿using System.Collections.Immutable;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files;

/// <summary>
/// Elite Dangerous fcmaterials.json file. Contains data about current fleet carrier bartender stock.
/// </summary>
public class FCMaterialsFile : JournalBase
{
    public override string Event => "FCMaterials";
    /// <summary>
    /// List of items in stock and in demand from the carrier bartender.
    /// </summary>
    public IReadOnlyCollection<FCMaterial> Items { get; init; }
}