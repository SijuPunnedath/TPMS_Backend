using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class DocumentSequenceConfiguration 
        : IEntityTypeConfiguration<DocumentSequence>
    {
        public void Configure(EntityTypeBuilder<DocumentSequence> builder)
        {
            builder.ToTable("DocumentSequences");

            // Primary Key
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Tenant Isolation
           // builder.Property(x => x.TenantId)
             //   .IsRequired();

            // Module Name
            builder.Property(x => x.ModuleName)
                .IsRequired()
                .HasMaxLength(50);

            // Prefix
            builder.Property(x => x.Prefix)
                .IsRequired()
                .HasMaxLength(20);

            // Current Number
            builder.Property(x => x.CurrentNumber)
                .IsRequired();

            // Number Length
            builder.Property(x => x.NumberLength)
                .IsRequired()
                .HasDefaultValue(5);

            // Reset Flag
            builder.Property(x => x.ResetEveryYear)
                .IsRequired();

            // Year (nullable if not resetting)
            builder.Property(x => x.Year)
                .IsRequired(false);

            // Concurrency Control
           // builder.Property(x => x.RowVersion)
              //  .IsRowVersion();

            //  builder.UseXminAsConcurrencyToken();

            // Unique Index (per Tenant + Module + Year)
            //builder.HasIndex(x => new { x.TenantId, x.ModuleName, x.Year })
               // .IsUnique();

            // Optional performance index
           // builder.HasIndex(x => new { x.TenantId, x.ModuleName });
        }
    }
}