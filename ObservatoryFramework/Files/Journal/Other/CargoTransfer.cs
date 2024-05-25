using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class CargoTransfer : JournalBase
{
    public override string Event => "CargoTransfer";
    public List<CargoTransferDetail> Transfers { get; init; }
}