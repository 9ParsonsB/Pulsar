namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Travel;

public class DockedConfiguration : IEntityTypeConfiguration<Docked>
{
    public void Configure(EntityTypeBuilder<Docked> builder)
    {
        builder.OwnsOne(l => l.StationFaction, b =>
        {
            b.OwnsMany(f => f.ActiveStates);
            b.OwnsMany(f => f.RecoveringStates);
            b.OwnsMany(f => f.PendingStates);
            b.ToJson();
        });
        builder.OwnsMany(l => l.StationEconomies, sb => sb.ToJson());
    }
}