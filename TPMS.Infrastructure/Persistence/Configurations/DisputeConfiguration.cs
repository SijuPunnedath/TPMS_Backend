using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("Disputes");

        // =============================
        // Primary Key
        // =============================
        builder.HasKey(x => x.DisputeId);

        // =============================
        // Basic Properties
        // =============================

        builder.Property(x => x.DisputeNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.DisputeNumber)
            .IsUnique();

        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.RaisedDate)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.DueDate);

        builder.Property(x => x.ClosedAt);

        builder.Property(x => x.ReferenceId);

        builder.Property(x => x.IsEscalated)
            .HasDefaultValue(false);

        // =============================
        // Enum Configuration (Store as int)
        // =============================

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Category)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.RaisedBy)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.ReferenceType)
            .HasConversion<int>();

        // =============================
        // SaaS Multi-Tenant Isolation
        // =============================

        builder.HasIndex(x => x.TenantId);

       /* builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);*/

        // =============================
        // User Relationships
        // =============================

        builder.HasOne(x => x.RaisedByUser)
            .WithMany()
            .HasForeignKey(x => x.RaisedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AssignedToUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // =============================
        // One-to-Many Relationships
        // =============================

        builder.HasMany(x => x.Comments)
            .WithOne(c => c.Dispute)
            .HasForeignKey(c => c.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Attachments)
            .WithOne(a => a.Dispute)
            .HasForeignKey(a => a.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        // =============================
        // One-to-One Resolution
        // =============================

        builder.HasOne(x => x.Resolution)
            .WithOne(r => r.Dispute)
            .HasForeignKey<DisputeResolution>(r => r.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        // =============================
        // Performance Indexes
        // =============================

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Priority);
        builder.HasIndex(x => x.RaisedDate);
        builder.HasIndex(x => new { x.TenantId, x.Status });
        builder.HasIndex(x => new { x.TenantId, x.Priority });

        // =============================
        // Soft Delete Global Filter
        // =============================

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}