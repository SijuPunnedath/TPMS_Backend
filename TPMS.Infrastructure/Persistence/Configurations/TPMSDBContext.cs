using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TPMS.Domain.Entities;
using Document = TPMS.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Enums;


namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class TPMSDBContext : DbContext 
    {
        public TPMSDBContext(DbContextOptions<TPMSDBContext> options) : base(options) { } 

        public DbSet<Lease> Leases { get; set; } = null!;
        public DbSet<RentSchedule> RentSchedules { get; set; } = null!;
        public DbSet<LeaseAlert> LeaseAlerts { get; set; } = null!;
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<Landlord> Landlords { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<KYCType> KYCTypes { get; set; } = null!;
        public DbSet<PartyKYC> PartyKYCs { get; set; } = null!;
        public DbSet<OwnerType> OwnerTypes { get; set; } = null!;
        public DbSet<DocumentType> DocumentTypes { get; set; } = null!;
        public DbSet<DocumentAccessLog> DocumentAccessLogs { get; set; }
        public DbSet<DocumentUploadSession> DocumentUploadSessions { get; set; }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!; 
        public DbSet<CompanySetting> CompanySettings { get; set; } = null!;
        public DbSet<PenaltyPolicy> PenaltyPolicies { get; set; } = null!;
       
        public DbSet<DepositMaster> DepositMasters { get; set; } = null!;
        public DbSet<DepositTransaction> DepositTransactions { get; set; } = null!;
        public DbSet<DocumentCategory> DocumentCategories { get; set; } = null!;
        public DbSet<LeaseRenewal> LeaseRenewals { get; set; } = null!;
        public DbSet<LeaseTermination> LeaseTerminations { get; set; } = null!;
        
        public DbSet<AssetCategory> AssetCategories { get; set; } = null!;
        public DbSet<AssetSubCategory> AssetSubCategories { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<AssetMaintenance> AssetMaintenances { get; set; } = null!;
        
        public DbSet<Dispute> Disputes { get; set; } = null!;
        public DbSet<DisputeAttachment> DisputeAttachments { get; set; } = null!;
        public DbSet<DisputeComment> DisputeComments { get; set; } = null!;
        public DbSet<DisputeResolution> DisputeResolutions { get; set; } = null!; 
        public DbSet<RequiredDocument>  RequiredDocuments{ get; set; } = null!;
        public DbSet<LeaseAlertRule>  LeaseAlertRules { get; set; } = null!;
        public DbSet<LeaseSettlement>  LeaseSettlements { get; set; } = null!;
        public DbSet<DocumentSequence>  DocumentSequences { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(TPMSDBContext).Assembly);
            


            modelBuilder.Entity<DocumentAccessLog>(entity =>
            {
                entity.ToTable("DocumentAccessLogs");

                entity.HasKey(e => e.LogID);

                entity.Property(e => e.AccessType)
                    .HasMaxLength(50)
                   .IsRequired();

               entity.Property(e => e.AccessedBy)
                   .HasMaxLength(100);

               entity.Property(e => e.IPAddress)
                   .HasMaxLength(45);

               entity.Property(e => e.Device)
                    .HasMaxLength(100);

               entity.Property(e => e.Notes)
                   .HasMaxLength(500);

                entity.HasOne(e => e.Document)
                    .WithMany()
                    .HasForeignKey(e => e.DocumentID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

           

            modelBuilder.Entity<RentSchedule>(b =>
            {
                b.HasKey(x => x.ScheduleID);
                b.Property(x => x.Amount).HasPrecision(18,2);
            });

            modelBuilder.Entity<DocumentUploadSession>().HasKey(a => a.SessionId);
            modelBuilder.Entity<RefreshToken>().HasKey(r => r.TokenID);
            modelBuilder.Entity<LeaseAlert>().HasKey(a => a.AlertID);

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.PropertyID);

                entity.Property(p => p.PropertyName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(p => p.PropertyNumber)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(e => e.PropertyNumber)
                    .IsUnique();

                entity.HasIndex(p => p.PropertyName);

                entity.HasOne(p => p.Landlord)
                    .WithMany(l => l.Properties)
                    .HasForeignKey(p => p.LandlordID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //  modelBuilder.Entity<Property>().HasKey(p => p.PropertyID);
         /*   modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.PropertyID);
                entity.Property(p => p.PropertyName)
                    .HasMaxLength(200);
                   // .IsRequired();
                   entity.HasIndex(e=> e.PropertyNumber)
                         .IsUnique();
                entity.HasOne(p => p.Landlord)
                    .WithMany(l => l.Properties)
                    .HasForeignKey(p => p.LandlordID)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(p => p.PropertyName);
            }); */
         
         modelBuilder.Entity<Tenant>(entity =>
         {
             entity.ToTable("Tenants");

             entity.HasKey(t => t.TenantID);
   
             entity.Property(t => t.TenantNumber)
                 .HasMaxLength(50)
                 .IsRequired();

             entity.HasIndex(t => t.TenantNumber)
                 .IsUnique();

             entity.Property(t => t.Name)
                 .IsRequired()
                 .HasMaxLength(150);

             entity.HasMany(t => t.Leases)
                 .WithOne(l => l.Tenant)
                 .HasForeignKey(l => l.TenantID)
                 .OnDelete(DeleteBehavior.Restrict);
         });

         
         
        /*    modelBuilder.Entity<Tenant>(entity =>
            {
                entity.ToTable("Tenants");
                entity.HasKey(t => t.TenantID);
               
                entity.Property(t => t.TenantNumber)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasMany(t => t.Leases)
                    .WithOne(l => l.Tenant)
                    .HasForeignKey(l => l.TenantID)
                    .OnDelete(DeleteBehavior.Restrict);
            }); */
          //  modelBuilder.Entity<Landlord>().HasKey(l => l.LandlordID);

            modelBuilder.Entity<Landlord>(entity =>
                {
                    entity.ToTable("Landlords");
                    entity.HasKey(l => l.LandlordID);
                    entity.HasIndex((x => x.LandlordNumber))
                          .IsUnique();
                }

            );
                
          
            //-- Document Entity constraints
            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Documents");

                // =========================
                // Primary Key
                // =========================
                entity.HasKey(d => d.DocumentID);

                // =========================
                // Core Properties
                // =========================
                entity.Property(d => d.DocumentName)
                    .IsRequired()
                    .HasMaxLength(255);

                //  Document Number (NEW)
                entity.Property(d => d.DocumentNumber)
                    .HasMaxLength(50)        // DOC-2026-000123
                    .IsRequired(false);      // keep nullable for backward compatibility

                entity.HasIndex(e => e.DocumentNumber)
                    .IsUnique();
                // Unique constraint (only if present)
                entity.HasIndex(d => d.DocumentNumber)
                    .IsUnique()
                    .HasFilter("\"DocumentNumber\" IS NOT NULL");

                // =========================
                // Relationships
                // =========================
                entity.HasOne(d => d.DocumentType)
                    .WithMany(dt => dt.Documents)
                    .HasForeignKey(d => d.DocumentTypeID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Optional: Category shortcut (if exists)
                // entity.HasOne(d => d.DocumentCategory)
                //     .WithMany()
                //     .HasForeignKey(d => d.DocumentCategoryID)
                //     .OnDelete(DeleteBehavior.Restrict);

                // =========================
                // Indexes
                // =========================
                entity.HasIndex(d => d.DocumentTypeID);
            });

            
           // modelBuilder.Entity<Address>().HasKey(a => a.AddressID);
           modelBuilder.Entity<Address>(builder =>
           {
               builder.HasKey(a => a.AddressID);

               builder.HasIndex(a => new { a.OwnerTypeID, a.OwnerID })
                   .HasFilter("\"IsPrimary\" = true")
                   .IsUnique();
           });
            modelBuilder.Entity<KYCType>().HasKey(k => k.KYCTypeID);
            modelBuilder.Entity<PartyKYC>().HasKey(pk => pk.PartyKYCID);
            modelBuilder.Entity<OwnerType>(entity =>
            {
                entity.ToTable("OwnerTypes");
                entity.HasKey(o => o.OwnerTypeID);

                entity.Property(o => o.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(o => o.Description)
                    .HasMaxLength(250);

                entity.Property(o => o.IsActive)
                    .HasDefaultValue(true);

                entity.Property(o => o.IsDeleted)
                    .HasDefaultValue(false);

                entity.Property(o => o.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(o => o.UpdatedAt)
                    .IsRequired(false);
            });

            
            modelBuilder.Entity<CompanySetting>(entity =>
            {
                entity.ToTable("CompanySettings");
                entity.HasKey(e => e.CompanyID);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Phone1).HasMaxLength(20);
                entity.Property(e => e.Phone2).HasMaxLength(20);
                entity.Property(e => e.Currency).HasMaxLength(10);
                entity.Property(e => e.TimeZone).HasMaxLength(100);
            });
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserID);

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                // Role relationship
                entity.HasOne(u => u.Role)
                    .WithMany()
                    .HasForeignKey(u => u.RoleID)
                    .OnDelete(DeleteBehavior.Restrict);

                // RefreshToken relationship
                entity.HasMany(u => u.RefreshTokens)
                    .WithOne(r => r.User)          // reference navigation property
                    .HasForeignKey(r => r.UserID)  // reference FK property
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.RoleID);

                entity.Property(r => r.RoleName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(r => r.Description)
                    .HasMaxLength(255);

                entity.HasMany<UserRole>()
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
           
            
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(ur => ur.UserRoleID);

                // UserRole → User (many-to-one)
                entity.HasOne(ur => ur.User)
                    .WithMany()
                    .HasForeignKey(ur => ur.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                // UserRole → Role (many-to-one)
                entity.HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(ur => ur.IsActive)
                    .HasDefaultValue(true);
            });
            
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermissions");
                entity.HasKey(rp => rp.RolePermissionID);

                entity.HasOne(rp => rp.Role)
                    .WithMany()
                    .HasForeignKey(rp => rp.RoleID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Prevent duplicate Role + Permission
                entity.HasIndex(rp => new { rp.RoleID, rp.PermissionID })
                    .IsUnique();
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(p => p.PermissionID);

                entity.Property(p => p.PermissionName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(p => p.Description)
                    .HasMaxLength(255);
                entity.HasIndex(p => p.PermissionName).IsUnique();
            });

            modelBuilder.Entity<PenaltyPolicy>(entity =>
            {
                entity.ToTable("PenaltyPolicies");

                entity.HasKey(p => p.PenaltyPolicyID);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).HasMaxLength(500);
            });
            
            
            //-- Lease status COnversion
            modelBuilder.Entity<Lease>()
                .Property(l => l.Status)
                .HasConversion<string>()      //  This is the key
                .HasMaxLength(50)             // Optional but recommended
                .HasDefaultValue(LeaseStatus.Active);
            //- End Lease status conversion
            
            // -------------------------
            // DepositMaster
            // -------------------------
            modelBuilder.Entity<DepositMaster>(entity =>
            {
                entity.HasKey(d => d.DepositMasterID);

                entity.HasOne(d => d.Lease)
                    .WithOne(l => l.DepositMaster)
                    .HasForeignKey<DepositMaster>(d => d.LeaseID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(d => d.ExpectedAmount).HasColumnType("decimal(18,2)");
                entity.Property(d => d.PaidAmount).HasColumnType("decimal(18,2)");
                entity.Property(d => d.BalanceAmount).HasColumnType("decimal(18,2)");

                entity.Property(d => d.Status).HasMaxLength(50);
                entity.Property(d => d.Notes).HasMaxLength(500);
            });

            // -------------------------
            // DepositTransaction
            // -------------------------
            modelBuilder.Entity<DepositTransaction>(entity =>
            {
                entity.HasKey(t => t.DepositTransactionID);

                entity.HasOne(t => t.DepositMaster)
                    .WithMany(dm => dm.Transactions)
                    .HasForeignKey(t => t.DepositMasterID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
                entity.Property(t => t.Type).HasMaxLength(50);
                entity.Property(t => t.Notes).HasMaxLength(500);
            });
            
            modelBuilder.Entity<DocumentCategory>(entity =>
            {
                entity.ToTable("DocumentCategories");
                entity.HasKey(c => c.DocumentCategoryID);

                entity.Property(c => c.CategoryName)
                    .IsRequired()
                    .HasMaxLength(200);
            });
            
            
            
         /*   modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.ToTable("DocumentTypes");
                entity.HasKey(dt => dt.DocumentTypeID);

                entity.Property(dt => dt.TypeName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(dt => dt.Category)
                    .WithMany(c => c.DocumentTypes)
                    .HasForeignKey(dt => dt.DocumentCategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
            }); */
            
         /*   modelBuilder.Entity<Document>(entity =>
            {
                entity.HasOne(d => d.DocumentType)
                    .WithMany(dt => dt.Documents)
                    .HasForeignKey(d => d.DocumentTypeID)
                    .OnDelete(DeleteBehavior.Restrict);
            }); */

            
            modelBuilder.Entity<DocumentCategory>().HasData(
                new DocumentCategory { DocumentCategoryID = 1, CategoryName = "Property Documents" },
                new DocumentCategory { DocumentCategoryID = 2, CategoryName = "Tenant Documents" },
                new DocumentCategory { DocumentCategoryID = 3, CategoryName = "Lease & Agreements" },
                new DocumentCategory { DocumentCategoryID = 4, CategoryName = "Financial Documents" },
                new DocumentCategory { DocumentCategoryID = 5, CategoryName = "Owner/Landlord Documents" },
                new DocumentCategory { DocumentCategoryID = 6, CategoryName = "Maintenance Documents" },
                new DocumentCategory { DocumentCategoryID = 7, CategoryName = "Legal Documents" },
                new DocumentCategory { DocumentCategoryID = 8, CategoryName = "Other / Misc." },
                new DocumentCategory { DocumentCategoryID = 9, CategoryName = "Rental Deposit Scheme." },
                new DocumentCategory { DocumentCategoryID = 10, CategoryName = "Dispute Documents." }
                    
            );
            
            modelBuilder.Entity<LeaseRenewal>(entity =>
            {
                entity.HasKey(e => e.LeaseRenewalID);

                entity.HasOne(e => e.Lease)
                    .WithMany(l => l.Renewals)
                    .HasForeignKey(e => e.LeaseID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LeaseTermination>(entity =>
            {
                entity.HasKey(e => e.LeaseTerminationID);

                entity.HasOne(e => e.Lease)
                    .WithOne(l => l.Termination)
                    .HasForeignKey<LeaseTermination>(e => e.LeaseID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            
            //Maintanace 
            
        /*   // modelBuilder.Entity<AssetCategory>(entity =>
            {
                entity.HasKey(x => x.AssetCategoryId);

                entity.Property(x => x.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Code)
                    .IsRequired()
                   .HasMaxLength(20);
            }); */
            
            modelBuilder.Entity<AssetSubCategory>(entity =>
            {
                entity.HasKey(x => x.AssetSubCategoryId);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(x => x.AssetCategory)
                    .WithMany(x => x.SubCategories)
                    .HasForeignKey(x => x.AssetCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(x => x.AssetId);

                entity.Property(x => x.AssetName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.AssetTag)
                    .HasMaxLength(50);

                entity.HasOne<AssetCategory>()
                    .WithMany()
                    .HasForeignKey(x => x.AssetCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<AssetSubCategory>()
                    .WithMany()
                    .HasForeignKey(x => x.AssetSubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<AssetMaintenance>(entity =>
            {
                entity.HasKey(x => x.AssetMaintenanceId);

                entity.HasOne(x => x.Asset)
                    .WithMany(x => x.Maintenances)
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.ApplyConfiguration(new DisputeConfiguration());
            modelBuilder.ApplyConfiguration(new DisputeCommentConfiguration());
            modelBuilder.ApplyConfiguration(new DisputeAttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new DisputeResolutionConfiguration());
            
            modelBuilder.Entity<RequiredDocument>(entity =>
            {
                entity.ToTable("RequiredDocuments");

                // Primary Key
                entity.HasKey(e => e.RequiredDocumentID);

                entity.Property(e => e.RequiredDocumentID)
                    .ValueGeneratedOnAdd();

                // Required fields
                entity.Property(e => e.OwnerTypeID)
                    .IsRequired();

                entity.Property(e => e.DocumentTypeID)
                    .IsRequired();

                entity.Property(e => e.IsMandatory)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired();

                // Relationships
                entity.HasOne(e => e.OwnerType)
                    .WithMany()
                    .HasForeignKey(e => e.OwnerTypeID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DocumentType)
                    .WithMany()
                    .HasForeignKey(e => e.DocumentTypeID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint:
                // One document type can be required only once per owner type
                entity.HasIndex(e => new { e.OwnerTypeID, e.DocumentTypeID })
                    .IsUnique();

                // Performance index
                entity.HasIndex(e => e.OwnerTypeID);
            });
            
            //LeaseAlertRule
            modelBuilder.Entity<LeaseAlertRule>(entity =>
            {
                // =========================
                // Table mapping (CRITICAL)
                // =========================
                entity.ToTable("LeaseAlertRules");

                // =========================
                // Primary Key
                // =========================
                entity.HasKey(e => e.RuleID);

                entity.Property(e => e.RuleID)
                    .ValueGeneratedOnAdd();

                // =========================
                // Required Properties
                // =========================
                entity.Property(e => e.RuleCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AlertType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PaymentFrequency)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.MessageTemplate)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DeliveryMethod)
                    .IsRequired()
                    .HasMaxLength(50);

                // =========================
                // Enum Mapping
                // =========================
                
                
                entity.Property(x => x.LeaseType)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                /*entity.Property(e => e.LeaseType)
                    .HasConversion<string>()
                    .HasMaxLength(20);*/

                // =========================
                // Defaults & Auditing
                // =========================
                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .IsRequired();

                // =========================
                // Indexes (Job Performance)
                // =========================

                // Fast lookup for daily jobs
                entity.HasIndex(e => new
                {
                    e.IsActive,
                    e.AlertType
                });

                // Prevent duplicate rule codes (GLOBAL)
                entity.HasIndex(e => e.RuleCode)
                    .IsUnique();
                
                // =========================
    // Seed Data (SYSTEM RULES)
    // =========================
    entity.HasData(
        new LeaseAlertRule
        {
            RuleID = 1,
            RuleCode = "LEASE_EXP_30",
            AlertType = "LeaseExpiry",
            TriggerDays = 30,
            LeaseType = LeaseType.Outbound,
            PaymentFrequency = "Monthly",
            DeliveryMethod = "Email",
            MessageTemplate = "Lease expires in {Days} days",
            RunTime = new TimeSpan(2, 0, 0), // 02:00 AM
            IsActive = true,
            CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc),
            UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc)        },
        new LeaseAlertRule
        {
            RuleID = 2,
            RuleCode = "LEASE_EXP_7",
            AlertType = "LeaseExpiry",
            TriggerDays = 7,
            LeaseType = LeaseType.Outbound,
            PaymentFrequency = "Monthly",
            DeliveryMethod = "Dashboard",
            MessageTemplate = "Lease expires soon",
            RunTime = new TimeSpan(2, 0, 0),
            IsActive = true,
            CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc),
            UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc)
            
        },
        new LeaseAlertRule
        {
            RuleID = 3,
            RuleCode = "RENT_DUE",
            AlertType = "RentDue",
            TriggerDays = 0,
            LeaseType = LeaseType.Outbound,
            PaymentFrequency = "Monthly",
            DeliveryMethod = "Email",
            MessageTemplate = "Rent due today",
            RunTime = new TimeSpan(2, 0, 0),
            IsActive = true,
            CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc),
            UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc)

        },
        new LeaseAlertRule
        {
            RuleID = 4,
            RuleCode = "RENT_OVERDUE",
            AlertType = "RentOverdue",
            TriggerDays = -3,
            LeaseType = LeaseType.Outbound,
            PaymentFrequency = "Monthly",
            DeliveryMethod = "SMS",
            MessageTemplate = "Rent overdue by {Days} days",
            RunTime = new TimeSpan(2, 0, 0),
            IsActive = true,
            CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc),
            UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc)
        }
    );
    
    
    //-- Seed for Document Sequence
    
    modelBuilder.Entity<DocumentSequence>().HasData(
        new DocumentSequence
        {
            Id = 1,
            ModuleName = "LEASE",
            Prefix = "LSE",
            CurrentNumber = 0,
            NumberLength = 5,
            ResetEveryYear = true,
            Year = DateTime.UtcNow.Year
        },
        new DocumentSequence
        {
            Id = 2,
            ModuleName = "PROPERTY",
            Prefix = "PRO",
            CurrentNumber = 0,
            NumberLength = 5,
            ResetEveryYear = false,
            Year = null
        },
        new DocumentSequence
        {
            Id = 3,
            ModuleName = "TENANT",
            Prefix = "TEN",
            CurrentNumber = 0,
            NumberLength = 5,
            ResetEveryYear = false,
            Year = null
        },
        new DocumentSequence
        {
            Id = 4,
            ModuleName = "INVOICE",
            Prefix = "INV",
            CurrentNumber = 0,
            NumberLength = 6,
            ResetEveryYear = true,
            Year = DateTime.UtcNow.Year
        },
        new DocumentSequence
        {
            Id = 5,
            ModuleName = "LANDLORD",
            Prefix = "LLD",
            CurrentNumber = 0,
            NumberLength = 6,
            ResetEveryYear = true,
            Year = DateTime.UtcNow.Year
        },
        new DocumentSequence
        {
            Id = 6,
            ModuleName = "DOCUMENT",
            Prefix = "DOC",
            CurrentNumber = 0,
            NumberLength = 6,
            ResetEveryYear = true,
            Year = DateTime.UtcNow.Year
        }
    );
    
            });
        }
    }
}
