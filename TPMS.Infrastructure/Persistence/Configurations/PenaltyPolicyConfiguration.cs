using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Configurations
{
    public class PenaltyPolicyConfiguration : IEntityTypeConfiguration<PenaltyPolicy>
    {
        public void Configure(EntityTypeBuilder<PenaltyPolicy> builder)
        {
          /*  builder.HasKey(x => x.PenaltyPolicyID);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasMaxLength(500);

            builder.Property(x => x.FixedAmount)
                   .HasColumnType("numeric(12,2)");

            builder.Property(x => x.PercentageOfRent)
                   .HasColumnType("numeric(5,2)"); */

            builder.HasData(
                new PenaltyPolicy
                {
                    PenaltyPolicyID = 1,
                    Name = "No penalty",
                    Description = "No late fee within grace period",
                    FixedAmount = 0,
                    PercentageOfRent = 0,
                    GracePeriodDays = 5,
                    IsActive = true
                },
                new PenaltyPolicy
                {
                    PenaltyPolicyID = 2,
                    Name = "Fixed Late Fee",
                    Description = "Flat 100 after grace period",
                    FixedAmount = 100,
                    PercentageOfRent = 0,
                    GracePeriodDays = 3,
                    IsActive = true
                },
                new PenaltyPolicy
                {
                    PenaltyPolicyID = 3,
                    Name = "Percentage Late Fee",
                    Description = "5% of monthly rent after due date",
                    FixedAmount = 0,
                    PercentageOfRent = 5,
                    GracePeriodDays = 5,
                    IsActive = true
                },
                new PenaltyPolicy
                {
                    PenaltyPolicyID = 4,
                    Name = "Daily Penalty",
                    Description = "100 per day after grace period",
                    FixedAmount = 100,
                    PercentageOfRent = 0,
                    GracePeriodDays = 2,
                    IsActive = true
                }
            );
        }
    }
}
