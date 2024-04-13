﻿using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class Synthesis : JournalBase
{
    public string Name { get; init; }

    [JsonConverter(typeof(MaterialConverter))]
    public ImmutableList<Material> Materials { get; init; }
}