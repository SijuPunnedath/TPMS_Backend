using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations;

public class DisputeAttachmentConfiguration : IEntityTypeConfiguration<DisputeAttachment>
{
    public void Configure(EntityTypeBuilder<DisputeAttachment> builder)
    {
        builder.ToTable("DisputeAttachments");

        builder.HasKey(x => x.DisputeAttachmentId);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.UploadedAt)
            .IsRequired();

        builder.HasOne(x => x.Dispute)
            .WithMany(d => d.Attachments)
            .HasForeignKey(x => x.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        // FK to Document (INT PK)
        builder.HasOne(x => x.Dispute)
            .WithMany(d => d.Attachments)
            .HasForeignKey(x => x.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.DisputeId, x.DocumentId })
            .IsUnique();
    }
}
