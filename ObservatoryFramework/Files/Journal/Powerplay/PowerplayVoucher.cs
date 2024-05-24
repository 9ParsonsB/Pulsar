using System.Collections.Immutable;

namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplayVoucher : PowerplayJoin
{
    public override string Event => "PowerplayVoucher";
    public ImmutableList<string> Systems { get; init; }
}