﻿using System.Collections.Immutable;

namespace Observatory.Framework.Files.Journal.Exploration;

/// <summary>
/// Event generated when selling exploration data. Historically also written for multi-selling, but used only for single system sales in current live game client.
/// </summary>
public class SellExplorationData : JournalBase
{
    public override string Event => "SellExplorationData";
    /// <summary>
    /// List of systems for which data was sold.
    /// </summary>
    public ICollection<string> Systems { get; init; }
    /// <summary>
    /// List of first discovered bodies.
    /// </summary>
    public ICollection<string> Discovered { get; init; }
    /// <summary>
    /// Base value of sold data.
    /// </summary>
    public long BaseValue { get; init; }
    /// <summary>
    /// Extra amount from bonuses.
    /// </summary>
    public long Bonus { get; init; }
    /// <summary>
    /// Total amount made from selling data.
    /// </summary>
    public long TotalEarnings { get; init; }
}