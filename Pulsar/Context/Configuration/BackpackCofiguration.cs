using JasperFx.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files;

namespace Pulsar.Context.Configuration;

public class BackpackCofiguration : IEntityTypeConfiguration<BackpackFile>
{
    public void Configure(EntityTypeBuilder<BackpackFile> builder)
    {
        builder.OwnsMany(b => b.Components, b => b.ToJson());
        builder.OwnsMany(b => b.Consumables, b => b.ToJson());
        builder.OwnsMany(b => b.Items, b => b.ToJson());
        builder.OwnsMany(b => b.Data, b => b.ToJson());
    }
}