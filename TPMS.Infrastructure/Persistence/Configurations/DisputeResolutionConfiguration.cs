using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations;


    public class DisputeResolutionConfiguration : IEntityTypeConfiguration<DisputeResolution>
    {
        public void Configure(EntityTypeBuilder<DisputeResolution> builder)
        {
            builder.ToTable("DisputeResolutions");

            builder.HasKey(x => x.DisputeResolutionId);

            builder.Property(x => x.ResolutionType)
                .IsRequired();

            builder.Property(x => x.ResolutionSummary)
                .IsRequired();

            builder.Property(x => x.ResolvedAt)
                .IsRequired();

            builder.HasOne(x => x.Dispute)
                .WithOne()
                .HasForeignKey<DisputeResolution>(x => x.DisputeId)
                .OnDelete(DeleteBehavior.Cascade);

            //-- One Dispute → One Resolution
            builder.HasOne(x => x.Dispute)
                .WithOne(d => d.Resolution)
                .HasForeignKey<DisputeResolution>(x => x.DisputeId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
