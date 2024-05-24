﻿using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Exploration;

/// <summary>
/// Event generated when discovery data for multiple systems are sold at once.
/// </summary>
public class MultiSellExplorationData : JournalBase
{
    public override string Event => "MultiSellExplorationData";
    /// <summary>
    /// List of all sold first discoveries.
    /// </summary>
    public ImmutableList<Discovered> Discovered { get; init; }
    /// <summary>
    /// Base value of total sold data.
    /// </summary>
    public long BaseValue { get; init; }
    /// <summary>
    /// Bonus value added to base amount.
    /// </summary>
    public long Bonus { get; init; }
    /// <summary>
    /// Total amount earned by CMDR for data sale.
    /// </summary>
    public long TotalEarnings { get; init; }

}