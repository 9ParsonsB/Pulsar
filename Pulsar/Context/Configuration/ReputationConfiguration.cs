namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

public class ReputationConfiguration : IEntityTypeConfiguration<Reputation>
{
    public void Configure(EntityTypeBuilder<Reputation> builder)
    {
        builder.HasKey(x => x.Timestamp);
    }
}