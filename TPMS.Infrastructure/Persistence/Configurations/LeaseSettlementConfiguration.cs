using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class LeaseSettlementConfiguration 
        : IEntityTypeConfiguration<LeaseSettlement>
    {
        public void Configure(EntityTypeBuilder<LeaseSettlement> builder)
        {
            builder.HasKey(x => x.LeaseSettlementId);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.OutstandingRent)
                .HasPrecision(18, 2);

            builder.Property(x => x.PenaltyAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.DamageCharges)
                .HasPrecision(18, 2);

            builder.Property(x => x.DepositPaid)
                .HasPrecision(18, 2);

            builder.Property(x => x.DepositAdjusted)
                .HasPrecision(18, 2);

            builder.Property(x => x.DepositRefunded)
                .HasPrecision(18, 2);

            builder.Property(x => x.BalancePayableByTenant)
                .HasPrecision(18, 2);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.HasOne(x => x.Lease)
                .WithMany(l => l.Settlements)
                .HasForeignKey(x => x.LeaseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}