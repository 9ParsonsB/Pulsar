using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

namespace Pulsar.Context.Configuration;

public class LoadoutConfiguration : IEntityTypeConfiguration<Loadout>
{
    public void Configure(EntityTypeBuilder<Loadout> builder)
    {
        builder.OwnsMany(l => l.Modules, lb =>
        {
            lb.OwnsOne(m => m.Engineering, mb =>
            {
                mb.OwnsMany(e => e.Modifiers, eb =>
                {
                    eb.OwnsOne(em => em.Value, emb => emb.ToJson());
                    eb.ToJson();
                });
                mb.ToJson();
            });
            lb.ToJson();
        });
        builder.OwnsOne(l => l.FuelCapacity, b=>b.ToJson());
    }
}