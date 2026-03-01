using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DisputeResolutionConfiguration : IEntityTypeConfiguration<DisputeResolution>
{
    public void Configure(EntityTypeBuilder<DisputeResolution> builder)
    {
        builder.ToTable("DisputeResolutions");

        builder.HasKey(x => x.DisputeResolutionId);

        builder.Property(x => x.ResolutionSummary)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.InternalNotes)
            .HasMaxLength(2000);

        builder.Property(x => x.CompensationAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CompensationCurrency)
            .HasMaxLength(10);

        builder.Property(x => x.ResolutionType)
            .HasConversion<int>();

        builder.Property(x => x.Outcome)
            .HasConversion<int>();

        builder.Property(x => x.ResolvedAt)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // 1 – 1 Relationship
        builder.HasOne(x => x.Dispute)
            .WithOne(d => d.Resolution)
            .HasForeignKey<DisputeResolution>(x => x.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.DisputeId)
            .IsUnique();

        // Users
        builder.HasOne(x => x.ResolvedByUser)
            .WithMany()
            .HasForeignKey(x => x.ResolvedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApprovedByUser)
            .WithMany()
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}