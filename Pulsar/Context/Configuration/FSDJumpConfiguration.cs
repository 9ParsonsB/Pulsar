namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Travel;

public class FSDJumpConfiguration : IEntityTypeConfiguration<FSDJump>
{
    public void Configure(EntityTypeBuilder<FSDJump> builder)
    {
        builder.OwnsOne(l => l.StarPos, b => b.ToJson());
        builder.OwnsMany(l => l.Conflicts, b =>
        {
            b.OwnsOne(c => c.FirstFaction);
            b.OwnsOne(c => c.SecondFaction);
            b.ToJson();
        });
        builder.OwnsOne(l => l.SystemFaction, b =>
        {
            b.OwnsMany(f => f.ActiveStates);
            b.OwnsMany(f => f.RecoveringStates);
            b.OwnsMany(f => f.PendingStates);
            b.ToJson();
        });
        builder.OwnsMany(l => l.Factions, b =>
        {
            b.OwnsMany(f => f.ActiveStates);
            b.OwnsMany(f => f.RecoveringStates);
            b.OwnsMany(f => f.PendingStates);
            b.ToJson();
        });
        builder.OwnsOne(l => l.ThargoidWar, tb => tb.ToJson());
        builder.PrimitiveCollection(l => l.Powers).IsRequired(false);
        builder.Property(l => l.Powers).HasDefaultValue(new List<string>());
    }
}