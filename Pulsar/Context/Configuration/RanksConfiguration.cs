namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

public class RanksConfiguration : IEntityTypeConfiguration<Rank>
{
    public void Configure(EntityTypeBuilder<Rank> builder)
    {
        builder.Ignore(x => x.AdditionalProperties);
        builder.HasKey(x => x.Timestamp);
    }
}