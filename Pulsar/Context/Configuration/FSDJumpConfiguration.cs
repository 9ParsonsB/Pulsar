using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Travel;

namespace Pulsar.Context.Configuration;

public class FSDJumpConfiguration : IEntityTypeConfiguration<FSDJump>
{
    public void Configure(EntityTypeBuilder<FSDJump> builder)
    {
        builder.OwnsOne(l => l.StarPos, b => b.ToJson());
        builder.OwnsMany(l => l.Conflicts, b =>
        {
            b.OwnsOne(c => c.FirstFaction, c => c.ToJson());
            b.OwnsOne(c => c.SecondFaction, c => c.ToJson());
            b.ToJson();
        });
        builder.OwnsOne(l => l.SystemFaction, b =>
        {
            b.OwnsMany(f => f.ActiveStates, fb => fb.ToJson());
            b.OwnsMany(f => f.RecoveringStates, rb => rb.ToJson());
            b.OwnsMany(f => f.PendingStates, pb => pb.ToJson());
            b.ToJson();
        });
        builder.OwnsMany(l => l.Factions, b =>
        {
            b.OwnsMany(f => f.ActiveStates, fb => fb.ToJson());
            b.OwnsMany(f => f.RecoveringStates, rb => rb.ToJson());
            b.OwnsMany(f => f.PendingStates, pb => pb.ToJson());
            b.ToJson();
        });
        builder.OwnsOne(l => l.ThargoidWar, tb => tb.ToJson());
    }
}
