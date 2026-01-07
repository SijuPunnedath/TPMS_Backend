using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class LeaseConfiguration : IEntityTypeConfiguration<Lease>
    {
        public void Configure(EntityTypeBuilder<Lease> b)
        {
            b.HasKey(x => x.LeaseID);

            b.Property(l => l.LeaseName)
                .HasMaxLength(200)
                .IsRequired();

            b.Property(x => x.Rent)
                .HasPrecision(18, 2);

            b.Property(x => x.Deposit)
                .HasPrecision(18, 2);

            b.Property(x => x.Commission)
                .HasPrecision(18, 2);

            b.Property(x => x.GuaranteedRent)
                .HasPrecision(18, 2);

            b.Property(x => x.Deductions)
                .HasPrecision(18, 2);

            b.Property(x => x.LeaseType)
                .HasMaxLength(20)
                .IsRequired();

            b.HasMany(x => x.RentSchedules)
                .WithOne(rs => rs.Lease)
                .HasForeignKey(rs => rs.LeaseID)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.LeaseAlerts)
                .WithOne(a => a.Lease)
                .HasForeignKey(a => a.LeaseID)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.DepositMaster)
                .WithOne(dm => dm.Lease)
                .HasForeignKey<DepositMaster>(dm => dm.LeaseID)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.LeaseType);
            b.HasIndex(x => x.Status);
        }
    }
}