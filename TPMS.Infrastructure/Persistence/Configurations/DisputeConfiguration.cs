using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("Disputes");

        builder.HasKey(x => x.DisputeId);

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

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Category)
            .IsRequired();

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.RaisedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Assigned User (optional)
        builder.HasOne(x => x.AssignedToUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Raised By User
        builder.HasOne(x => x.RaisedByUser)
            .WithMany()
            .HasForeignKey(x => x.RaisedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Navigation collections
        builder.HasMany(x => x.Comments)
            .WithOne(c => c.Dispute)
            .HasForeignKey(c => c.DisputeId);

        builder.HasMany(x => x.Attachments)
            .WithOne(a => a.Dispute)
            .HasForeignKey(a => a.DisputeId);
    }
}
