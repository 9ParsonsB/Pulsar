using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Exploration;

namespace Pulsar.Context.Configuration;

public class ScanConfiguration : IEntityTypeConfiguration<Scan>
{
    public void Configure(EntityTypeBuilder<Scan> builder)
    {
        builder.OwnsMany(s => s.AtmosphereComposition, b => b.ToJson());
        builder.OwnsMany(s => s.Materials, b => b.ToJson());
        builder.OwnsOne(s => s.Composition, b => b.ToJson());
        builder.OwnsMany(s => s.Rings, b => b.ToJson());
        builder.OwnsMany(s => s.Parents, b => b.ToJson());
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
