using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

namespace Pulsar.Context.Configuration;

public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
{
    public void Configure(EntityTypeBuilder<Cargo> builder)
    {
        builder.OwnsMany(c => c.Inventory, c => c.ToJson());
    }
}