namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

public class MissionsConfiguration : IEntityTypeConfiguration<Missions>
{
    public void Configure(EntityTypeBuilder<Missions> builder)
    {
        builder.OwnsMany(b => b.Active, b => { });
        builder.OwnsMany(b => b.Complete, b => { });
        builder.OwnsMany(b => b.Failed, b => { });
    }
}