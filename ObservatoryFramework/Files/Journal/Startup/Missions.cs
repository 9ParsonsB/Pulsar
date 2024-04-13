﻿using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Startup;

public class Missions : JournalBase
{
    public ImmutableList<Mission> Active { get; init; }
    public ImmutableList<Mission> Failed { get; init; }
    public ImmutableList<Mission> Complete { get; init; }
}