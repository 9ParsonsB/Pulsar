﻿using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class TransferMicroResources : JournalBase
{
    public override string Event => "TransferMicroResources";
    public List<MicroTransfer> Transfers { get; init; }
}