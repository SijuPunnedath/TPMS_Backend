using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.HasKey(x => x.DocumentTypeID);

            builder.Property(x => x.TypeName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(255);

            builder.HasOne(x => x.Category)
                .WithMany(c => c.DocumentTypes)
                .HasForeignKey(x => x.DocumentCategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                // ───────────────── Property Documents (1)
                new DocumentType { DocumentTypeID = 101, DocumentCategoryID = 1, TypeName = "Title Deed" },
                new DocumentType { DocumentTypeID = 102, DocumentCategoryID = 1, TypeName = "Property Tax Receipt" },
                new DocumentType { DocumentTypeID = 103, DocumentCategoryID = 1, TypeName = "Building Plan Approval" },
                new DocumentType { DocumentTypeID = 104, DocumentCategoryID = 1, TypeName = "Occupancy Certificate" },

                // ───────────────── Tenant Documents (2)
                new DocumentType { DocumentTypeID = 201, DocumentCategoryID = 2, TypeName = "Government ID Proof" },
                new DocumentType { DocumentTypeID = 202, DocumentCategoryID = 2, TypeName = "Address Proof" },
                new DocumentType { DocumentTypeID = 203, DocumentCategoryID = 2, TypeName = "Police Verification" },
                new DocumentType { DocumentTypeID = 204, DocumentCategoryID = 2, TypeName = "Tenant Photograph" },

                // ───────────────── Lease & Agreements (3)
                new DocumentType { DocumentTypeID = 301, DocumentCategoryID = 3, TypeName = "Lease Agreement" },
                new DocumentType { DocumentTypeID = 302, DocumentCategoryID = 3, TypeName = "Renewal Agreement" },
                new DocumentType { DocumentTypeID = 303, DocumentCategoryID = 3, TypeName = "Termination Notice" },

                // ───────────────── Financial Documents (4)
                new DocumentType { DocumentTypeID = 401, DocumentCategoryID = 4, TypeName = "Rent Receipt" },
                new DocumentType { DocumentTypeID = 402, DocumentCategoryID = 4, TypeName = "Security Deposit Receipt" },
                new DocumentType { DocumentTypeID = 403, DocumentCategoryID = 4, TypeName = "Invoice" },

                // ───────────────── Owner / Landlord Documents (5)
                new DocumentType { DocumentTypeID = 501, DocumentCategoryID = 5, TypeName = "Owner ID Proof" },
                new DocumentType { DocumentTypeID = 502, DocumentCategoryID = 5, TypeName = "Ownership Declaration" },

                // ───────────────── Maintenance Documents (6)
                new DocumentType { DocumentTypeID = 601, DocumentCategoryID = 6, TypeName = "Maintenance Request" },
                new DocumentType { DocumentTypeID = 602, DocumentCategoryID = 6, TypeName = "Work Order" },
                new DocumentType { DocumentTypeID = 603, DocumentCategoryID = 6, TypeName = "Service Completion Report" },

                // ───────────────── Legal Documents (7)
                new DocumentType { DocumentTypeID = 701, DocumentCategoryID = 7, TypeName = "Legal Notice" },
                new DocumentType { DocumentTypeID = 702, DocumentCategoryID = 7, TypeName = "Court Order" },

                // ───────────────── Other / Misc (8)
                new DocumentType { DocumentTypeID = 801, DocumentCategoryID = 8, TypeName = "General Attachment" },
                new DocumentType { DocumentTypeID = 802, DocumentCategoryID = 8, TypeName = "Supporting Document" },
                
                // ───────────────── Rental Deposit Scheme (9)
                new DocumentType { DocumentTypeID = 901, DocumentCategoryID = 9, TypeName = "Tenancy Documents" },
                new DocumentType { DocumentTypeID = 902, DocumentCategoryID = 9, TypeName = "Deposit Protection Documents" },
                new DocumentType { DocumentTypeID = 903, DocumentCategoryID = 9, TypeName = "Property Condition Reports" },
                new DocumentType { DocumentTypeID = 904, DocumentCategoryID = 9, TypeName = "Financial Records" },
                new DocumentType { DocumentTypeID = 905, DocumentCategoryID = 9, TypeName = "Compliance Certificates" },
                new DocumentType { DocumentTypeID = 906, DocumentCategoryID = 9, TypeName = "Dispute Resolution Evidence" },
                new DocumentType { DocumentTypeID = 907, DocumentCategoryID = 9, TypeName = "End Of Tenancy Documents" },
          
                // ───────────────── Dispute Documents(10)
                new DocumentType { DocumentTypeID = 1001, DocumentCategoryID = 10, TypeName = "Initiation & Notice" },
                new DocumentType { DocumentTypeID = 1002, DocumentCategoryID = 10, TypeName = "Contract & Reference Documents" },
                new DocumentType { DocumentTypeID = 1003, DocumentCategoryID = 10, TypeName = "Evidence & Supporting Material" },
                
                new DocumentType { DocumentTypeID = 1004, DocumentCategoryID = 10, TypeName = "Communication Records" },
                new DocumentType { DocumentTypeID = 1005, DocumentCategoryID = 10, TypeName = "ELegal Proceedings" },
                new DocumentType { DocumentTypeID = 1006, DocumentCategoryID = 10, TypeName = "Financial & Claims" },
                new DocumentType { DocumentTypeID = 1007, DocumentCategoryID = 10, TypeName = "Resolution & Closure" }
                
                );
        }
    }
}