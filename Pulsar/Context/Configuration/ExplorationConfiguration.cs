namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Exploration;

public class ScanConfiguration : IEntityTypeConfiguration<Scan>
{
    public void Configure(EntityTypeBuilder<Scan> builder)
    {
        builder.OwnsMany(s => s.AtmosphereComposition, b => b.ToJson());
        builder.OwnsMany(s => s.Materials, b => b.ToJson());
        builder.OwnsOne(s => s.Composition, b => b.ToJson());
        builder.OwnsMany(s => s.Rings, b => b.ToJson());
        builder.OwnsMany(s => s.Parents, b => b.ToJson());
        builder.Ignore(s => s.Parent);
    }
}

public class ScanBaryCentreConfiguration : IEntityTypeConfiguration<ScanBaryCentre>
{
    public void Configure(EntityTypeBuilder<ScanBaryCentre> builder)
    {
        builder.HasKey(s => new { s.Timestamp, s.SystemAddress, s.BodyID });
    }
}

public class FSSBodySignalsConfiguration : IEntityTypeConfiguration<FSSBodySignals>
{
    public void Configure(EntityTypeBuilder<FSSBodySignals> builder)
    {
        builder.OwnsMany(s => s.Signals, b => b.ToJson());
        builder.OwnsMany(s => s.Genuses, b => b.ToJson());
    }
}

public class SAASignalsFoundConfiguration : IEntityTypeConfiguration<SAASignalsFound>
{
    public void Configure(EntityTypeBuilder<SAASignalsFound> builder)
    {
        builder.OwnsMany(s => s.Signals, b => b.ToJson());
        builder.OwnsMany(s => s.Genuses, b => b.ToJson());
    }
}