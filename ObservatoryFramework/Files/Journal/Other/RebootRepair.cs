﻿using System.Collections.Immutable;

namespace Observatory.Framework.Files.Journal.Other;

public class RebootRepair : JournalBase
{
    public override string Event => "RebootRepair";
    public IList<string> Modules { get; init; }
}