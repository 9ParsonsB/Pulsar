﻿using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Startup;

public class Rank : JournalBase
{
    public override string Event => "Rank";
    public RankCombat Combat { get; init; }
    public RankTrade Trade { get; init; }
    public RankExploration Explore { get; init; }
    public RankCQC CQC { get; init; }
    public RankSoldier Soldier { get; init; }
    public RankExobiologist Exobiologist { get; init; }
    public RankEmpire Empire { get; init; }
    public RankFederation Federation { get; init; }
}