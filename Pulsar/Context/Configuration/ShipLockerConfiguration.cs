namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Odyssey;

public class ShipLockerConfiguration : IEntityTypeConfiguration<ShipLockerMaterials>
{
    public void Configure(EntityTypeBuilder<ShipLockerMaterials> builder)
    {
        builder.OwnsMany(b => b.Items, b => b.ToJson());
        builder.OwnsMany(b => b.Components, b => b.ToJson());
        builder.OwnsMany(b => b.Consumables, b => b.ToJson());
        builder.OwnsMany(b => b.Data, b => b.ToJson());
    }
}