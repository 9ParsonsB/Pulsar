using Observatory.Framework.Files.ParameterTypes;

namespace Pulsar.Context.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observatory.Framework.Files.Journal.Startup;

public class StatisticsConfiguration : IEntityTypeConfiguration<Statistics>
{
    public void Configure(EntityTypeBuilder<Statistics> builder)
    {
        builder.HasKey(x => x.Timestamp);
        builder.OwnsOne(x => x.BankAccount, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Combat, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Crime, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Smuggling, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Trading, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Mining, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Exploration, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Passengers, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.SearchAndRescue, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Crafting, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Crew, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Multicrew, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Thargoid, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.MaterialTrader, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.CQC, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.FleetCarrier, b =>
        {
            b.ToJson();
        });
        builder.OwnsOne(x => x.Exobiology, b =>
        {
            b.ToJson();
        });
    }
}
