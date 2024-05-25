using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Travel;

namespace Pulsar.Context.Configuration;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.OwnsOne(l => l.StarPos, b => b.ToJson());
        builder.OwnsMany(l => l.Conflicts, b =>
        {
            b.OwnsOne(c => c.FirstFaction, c => c.ToJson());
            b.OwnsOne(c => c.SecondFaction, c => c.ToJson());
            b.ToJson();
        });
        builder.OwnsOne(l => l.StationFaction, b => b.ToJson());
        builder.OwnsOne(l => l.SystemFaction, b =>
        {
            b.OwnsMany(s => s.ActiveStates, sb => sb.ToJson());
            b.OwnsMany(s => s.RecoveringStates, rb => rb.ToJson());
            b.ToJson();
        });
        builder.OwnsMany(l => l.Factions, b =>
        {
            b.OwnsMany(f => f.ActiveStates, fb => fb.ToJson());
            b.OwnsMany(f => f.RecoveringStates, rb => rb.ToJson());
            b.ToJson();
        });
        builder.OwnsMany(l => l.StationEconomies, sb => sb.ToJson());
        builder.OwnsOne(l => l.ThargoidWar, tb => tb.ToJson());
    }
}
