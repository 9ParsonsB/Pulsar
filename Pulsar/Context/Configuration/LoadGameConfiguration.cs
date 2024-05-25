namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

public class LoadGameConfiguration : IEntityTypeConfiguration<LoadGame>
{
    public void Configure(EntityTypeBuilder<LoadGame> builder)
    {
        builder.HasKey(j => j.Timestamp);
    }
}