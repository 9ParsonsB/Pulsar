using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

namespace Pulsar.Context.Configuration;

public class MissionsConfiguration : IEntityTypeConfiguration<Missions>
{
    public void Configure(EntityTypeBuilder<Missions> builder)
    {
        builder.OwnsMany(b => b.Active, b =>
        {
            b.HasKey(m => m.MissionID);
        });
        builder.OwnsMany(b => b.Complete, b =>
        {
            b.HasKey(m => m.MissionID);
        });
        builder.OwnsMany(b => b.Failed, b =>
        {
            b.HasKey(m => m.MissionID);
        });
    }
}