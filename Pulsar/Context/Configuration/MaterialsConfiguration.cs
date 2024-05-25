namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

public class MaterialsConfiguration : IEntityTypeConfiguration<Materials>
{
    public void Configure(EntityTypeBuilder<Materials> builder)
    {
        builder.HasKey(x => x.Timestamp);
        builder.OwnsMany(x => x.Raw, b =>
        {
            b.ToJson();
        });
        builder.OwnsMany(x => x.Encoded, b =>
        {
            b.ToJson();
        });
        builder.OwnsMany(x => x.Manufactured, b =>
        {
            b.ToJson();
        });
    }
}