﻿using System.Collections.Immutable;

namespace Observatory.Framework.Files.Journal.Other;

public class WingJoin : JournalBase
{
    public override string Event => "WingJoin";
    public ICollection<string> Others { get; init; }
}