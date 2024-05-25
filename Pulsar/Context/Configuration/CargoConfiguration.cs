namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
{
    public void Configure(EntityTypeBuilder<Cargo> builder)
    {
        builder.OwnsMany(c => c.Inventory, c => c.ToJson());
    }
}