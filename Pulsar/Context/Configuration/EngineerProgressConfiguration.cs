namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.StationServices;

public class EngineerProgressConfiguration : IEntityTypeConfiguration<EngineerProgress>
{
    public void Configure(EntityTypeBuilder<EngineerProgress> builder)
    {
        builder.HasKey(x => x.Timestamp);
        builder.OwnsMany(x => x.Engineers, b =>
        {
            b.ToJson();
        });
    }
}