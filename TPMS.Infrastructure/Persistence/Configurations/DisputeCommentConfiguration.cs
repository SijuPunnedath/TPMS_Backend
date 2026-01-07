using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations;

public class DisputeCommentConfiguration : IEntityTypeConfiguration<DisputeComment>
{
    public void Configure(EntityTypeBuilder<DisputeComment> builder)
    {
        builder.ToTable("DisputeComments");

        builder.HasKey(x => x.DisputeCommentId);

        builder.Property(x => x.Comment)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Dispute)
            .WithMany(d => d.Comments)
            .HasForeignKey(x => x.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CommentedByUser)
            .WithMany()
            .HasForeignKey(x => x.CommentedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
