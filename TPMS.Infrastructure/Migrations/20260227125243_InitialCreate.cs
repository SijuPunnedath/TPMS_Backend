using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerTypeID = table.Column<int>(type: "integer", nullable: false),
                    OwnerID = table.Column<int>(type: "integer", nullable: false),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Phone1 = table.Column<string>(type: "text", nullable: true),
                    Phone2 = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressID);
                });

            migrationBuilder.CreateTable(
                name: "AssetCategories",
                columns: table => new
                {
                    AssetCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDepreciable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DefaultUsefulLifeMonths = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    RequiresComplianceCheck = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategories", x => x.AssetCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "CompanySettings",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: true),
                    TaxID = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Phone1 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Phone2 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySettings", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCategories",
                columns: table => new
                {
                    DocumentCategoryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCategories", x => x.DocumentCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentSequences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Prefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CurrentNumber = table.Column<int>(type: "integer", nullable: false),
                    NumberLength = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                    ResetEveryYear = table.Column<bool>(type: "boolean", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentUploadSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    OwnerTypeID = table.Column<int>(type: "integer", nullable: false),
                    OwnerID = table.Column<int>(type: "integer", nullable: false),
                    DocType = table.Column<string>(type: "text", nullable: true),
                    DocumentTypeID = table.Column<int>(type: "integer", nullable: true),
                    DocumentCategoryID = table.Column<int>(type: "integer", nullable: true),
                    TotalChunks = table.Column<int>(type: "integer", nullable: false),
                    UploadedChunks = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    UploadedBy = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUploadSessions", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "KYCTypes",
                columns: table => new
                {
                    KYCTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KYCTypes", x => x.KYCTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Landlords",
                columns: table => new
                {
                    LandlordID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LandlordNumber = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landlords", x => x.LandlordID);
                });

            migrationBuilder.CreateTable(
                name: "LeaseAlertRules",
                columns: table => new
                {
                    RuleID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AlertType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TriggerDays = table.Column<int>(type: "integer", nullable: false),
                    LeaseType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PaymentFrequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DeliveryMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MessageTemplate = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RunTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseAlertRules", x => x.RuleID);
                });

            migrationBuilder.CreateTable(
                name: "OwnerTypes",
                columns: table => new
                {
                    OwnerTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerTypes", x => x.OwnerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "PartyKYCs",
                columns: table => new
                {
                    PartyKYCID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerTypeID = table.Column<int>(type: "integer", nullable: false),
                    OwnerID = table.Column<int>(type: "integer", nullable: false),
                    KYCTypeID = table.Column<int>(type: "integer", nullable: false),
                    DocumentID = table.Column<int>(type: "integer", nullable: true),
                    KYCNumber = table.Column<string>(type: "text", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    VerifiedBy = table.Column<int>(type: "integer", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyKYCs", x => x.PartyKYCID);
                });

            migrationBuilder.CreateTable(
                name: "PenaltyPolicies",
                columns: table => new
                {
                    PenaltyPolicyID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FixedAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    PercentageOfRent = table.Column<decimal>(type: "numeric", nullable: true),
                    GracePeriodDays = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyPolicies", x => x.PenaltyPolicyID);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermissionName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Module = table.Column<string>(type: "text", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantID);
                });

            migrationBuilder.CreateTable(
                name: "AssetSubCategories",
                columns: table => new
                {
                    AssetSubCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetSubCategories", x => x.AssetSubCategoryId);
                    table.ForeignKey(
                        name: "FK_AssetSubCategories_AssetCategories_AssetCategoryId",
                        column: x => x.AssetCategoryId,
                        principalTable: "AssetCategories",
                        principalColumn: "AssetCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocumentTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentCategoryID = table.Column<int>(type: "integer", nullable: false),
                    TypeName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeID);
                    table.ForeignKey(
                        name: "FK_DocumentTypes_DocumentCategories_DocumentCategoryID",
                        column: x => x.DocumentCategoryID,
                        principalTable: "DocumentCategories",
                        principalColumn: "DocumentCategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PropertyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SerialNo = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<string>(type: "text", nullable: true),
                    LandlordID = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ActiveInboundLeaseId = table.Column<int>(type: "integer", nullable: true),
                    ActiveOutboundLeaseId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.PropertyID);
                    table.ForeignKey(
                        name: "FK_Properties_Landlords_LandlordID",
                        column: x => x.LandlordID,
                        principalTable: "Landlords",
                        principalColumn: "LandlordID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RolePermissionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    PermissionID = table.Column<int>(type: "integer", nullable: false),
                    IsAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    RoleID1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.RolePermissionID);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permissions",
                        principalColumn: "PermissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleID1",
                        column: x => x.RoleID1,
                        principalTable: "Roles",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyId = table.Column<int>(type: "integer", nullable: false),
                    PropertyUnitId = table.Column<int>(type: "integer", nullable: true),
                    AssetCategoryId = table.Column<int>(type: "integer", nullable: false),
                    AssetSubCategoryId = table.Column<int>(type: "integer", nullable: true),
                    AssetName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AssetTag = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    InstalledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WarrantyExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastServiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextServiceDue = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PurchaseValue = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_Assets_AssetCategories_AssetCategoryId",
                        column: x => x.AssetCategoryId,
                        principalTable: "AssetCategories",
                        principalColumn: "AssetCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_AssetSubCategories_AssetSubCategoryId",
                        column: x => x.AssetSubCategoryId,
                        principalTable: "AssetSubCategories",
                        principalColumn: "AssetSubCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OwnerTypeID = table.Column<int>(type: "integer", nullable: false),
                    OwnerID = table.Column<int>(type: "integer", nullable: false),
                    DocumentTypeID = table.Column<int>(type: "integer", nullable: false),
                    DocType = table.Column<string>(type: "text", nullable: true),
                    DocumentCategoryID = table.Column<int>(type: "integer", nullable: false),
                    DocumentCategoryName = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: true),
                    UploadedBy = table.Column<int>(type: "integer", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    PreviousDocumentID = table.Column<int>(type: "integer", nullable: true),
                    DocumentNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_DocumentTypeID",
                        column: x => x.DocumentTypeID,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_OwnerTypes_OwnerTypeID",
                        column: x => x.OwnerTypeID,
                        principalTable: "OwnerTypes",
                        principalColumn: "OwnerTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequiredDocuments",
                columns: table => new
                {
                    RequiredDocumentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerTypeID = table.Column<int>(type: "integer", nullable: false),
                    DocumentTypeID = table.Column<int>(type: "integer", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredDocuments", x => x.RequiredDocumentID);
                    table.ForeignKey(
                        name: "FK_RequiredDocuments_DocumentTypes_DocumentTypeID",
                        column: x => x.DocumentTypeID,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequiredDocuments_OwnerTypes_OwnerTypeID",
                        column: x => x.OwnerTypeID,
                        principalTable: "OwnerTypes",
                        principalColumn: "OwnerTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    LeaseID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaseNumber = table.Column<string>(type: "text", nullable: false),
                    LeaseName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PropertyID = table.Column<int>(type: "integer", nullable: false),
                    TenantID = table.Column<int>(type: "integer", nullable: true),
                    LandlordID = table.Column<int>(type: "integer", nullable: true),
                    ParentLeaseID = table.Column<int>(type: "integer", nullable: true),
                    LeaseType = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateMovedIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Rent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Deposit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Commission = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    GuaranteedRent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    PaymentFrequency = table.Column<string>(type: "text", nullable: false),
                    PenaltyPolicyID = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    EndReason = table.Column<string>(type: "text", nullable: true),
                    Deductions = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DeductionReason = table.Column<string>(type: "text", nullable: true),
                    DisputeNotes = table.Column<string>(type: "text", nullable: true),
                    OverdueAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    OverdueReason = table.Column<string>(type: "text", nullable: true),
                    LeaseNotes = table.Column<string>(type: "text", nullable: true),
                    IsTerminated = table.Column<bool>(type: "boolean", nullable: false),
                    TerminatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.LeaseID);
                    table.ForeignKey(
                        name: "FK_Leases_Landlords_LandlordID",
                        column: x => x.LandlordID,
                        principalTable: "Landlords",
                        principalColumn: "LandlordID");
                    table.ForeignKey(
                        name: "FK_Leases_PenaltyPolicies_PenaltyPolicyID",
                        column: x => x.PenaltyPolicyID,
                        principalTable: "PenaltyPolicies",
                        principalColumn: "PenaltyPolicyID");
                    table.ForeignKey(
                        name: "FK_Leases_Properties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_Tenants_TenantID",
                        column: x => x.TenantID,
                        principalTable: "Tenants",
                        principalColumn: "TenantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Disputes",
                columns: table => new
                {
                    DisputeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisputeNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RaisedByUserId = table.Column<int>(type: "integer", nullable: false),
                    RaisedBy = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReferenceType = table.Column<int>(type: "integer", nullable: false),
                    ReferenceId = table.Column<int>(type: "integer", nullable: true),
                    RaisedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedToUserId = table.Column<int>(type: "integer", nullable: true),
                    IsEscalated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disputes", x => x.DisputeId);
                    table.ForeignKey(
                        name: "FK_Disputes_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disputes_Users_RaisedByUserId",
                        column: x => x.RaisedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    TokenID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.TokenID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserRoleID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserRoleID);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetMaintenances",
                columns: table => new
                {
                    AssetMaintenanceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<int>(type: "integer", nullable: false),
                    MaintenanceType = table.Column<int>(type: "integer", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: true),
                    NextDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMaintenances", x => x.AssetMaintenanceId);
                    table.ForeignKey(
                        name: "FK_AssetMaintenances_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAccessLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentID = table.Column<int>(type: "integer", nullable: false),
                    AccessedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccessType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IPAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    Device = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAccessLogs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_DocumentAccessLogs_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositMasters",
                columns: table => new
                {
                    DepositMasterID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaseID = table.Column<int>(type: "integer", nullable: false),
                    ExpectedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositMasters", x => x.DepositMasterID);
                    table.ForeignKey(
                        name: "FK_DepositMasters_Leases_LeaseID",
                        column: x => x.LeaseID,
                        principalTable: "Leases",
                        principalColumn: "LeaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaseAlerts",
                columns: table => new
                {
                    AlertID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaseID = table.Column<int>(type: "integer", nullable: false),
                    AlertType = table.Column<string>(type: "text", nullable: false),
                    AlertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    DeliveryMethod = table.Column<string>(type: "text", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseAlerts", x => x.AlertID);
                    table.ForeignKey(
                        name: "FK_LeaseAlerts_Leases_LeaseID",
                        column: x => x.LeaseID,
                        principalTable: "Leases",
                        principalColumn: "LeaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaseRenewals",
                columns: table => new
                {
                    LeaseRenewalID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaseID = table.Column<int>(type: "integer", nullable: false),
                    OldEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NewStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NewEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OldRent = table.Column<decimal>(type: "numeric", nullable: false),
                    NewRent = table.Column<decimal>(type: "numeric", nullable: false),
                    OldDeposit = table.Column<decimal>(type: "numeric", nullable: true),
                    NewDeposit = table.Column<decimal>(type: "numeric", nullable: true),
                    AdditionalDeposit = table.Column<decimal>(type: "numeric", nullable: true),
                    RenewalReason = table.Column<string>(type: "text", nullable: false),
                    RenewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RenewedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseRenewals", x => x.LeaseRenewalID);
                    table.ForeignKey(
                        name: "FK_LeaseRenewals_Leases_LeaseID",
                        column: x => x.LeaseID,
                        principalTable: "Leases",
                        principalColumn: "LeaseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaseSettlements",
                columns: table => new
                {
                    LeaseSettlementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaseId = table.Column<int>(type: "integer", nullable: false),
                    SettlementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    OutstandingRent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DamageCharges = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositAdjusted = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositRefunded = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BalancePayableByTenant = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    SettledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SettledBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseSettlements", x => x.LeaseSettlementId);
                    table.ForeignKey(
                        name: "FK_LeaseSettlements_Leases_LeaseId",
                        column: x => x.LeaseId,
                        principalTable: "Leases",
                        principalColumn: "LeaseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaseTerminations",
                columns: table => new
                {
                    LeaseTerminationID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaseID = table.Column<int>(type: "integer", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TerminationType = table.Column<string>(type: "text", nullable: false),
                    TerminationReason = table.Column<string>(type: "text", nullable: false),
                    OutstandingRent = table.Column<decimal>(type: "numeric", nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    DamageCharges = table.Column<decimal>(type: "numeric", nullable: false),
                    DepositAdjusted = table.Column<decimal>(type: "numeric", nullable: false),
                    DepositRefunded = table.Column<decimal>(type: "numeric", nullable: false),
                    SettlementStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    SettledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SettledBy = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseTerminations", x => x.LeaseTerminationID);
                    table.ForeignKey(
                        name: "FK_LeaseTerminations_Leases_LeaseID",
                        column: x => x.LeaseID,
                        principalTable: "Leases",
                        principalColumn: "LeaseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentSchedules",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaseID = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Penalty = table.Column<decimal>(type: "numeric", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentSchedules", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK_RentSchedules_Leases_LeaseID",
                        column: x => x.LeaseID,
                        principalTable: "Leases",
                        principalColumn: "LeaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisputeAttachments",
                columns: table => new
                {
                    DisputeAttachmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisputeId = table.Column<int>(type: "integer", nullable: false),
                    DocumentId = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeAttachments", x => x.DisputeAttachmentId);
                    table.ForeignKey(
                        name: "FK_DisputeAttachments_Disputes_DisputeId",
                        column: x => x.DisputeId,
                        principalTable: "Disputes",
                        principalColumn: "DisputeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisputeComments",
                columns: table => new
                {
                    DisputeCommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisputeId = table.Column<int>(type: "integer", nullable: false),
                    CommentedByUserId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    IsInternal = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeComments", x => x.DisputeCommentId);
                    table.ForeignKey(
                        name: "FK_DisputeComments_Disputes_DisputeId",
                        column: x => x.DisputeId,
                        principalTable: "Disputes",
                        principalColumn: "DisputeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisputeComments_Users_CommentedByUserId",
                        column: x => x.CommentedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DisputeResolutions",
                columns: table => new
                {
                    DisputeResolutionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisputeId = table.Column<int>(type: "integer", nullable: false),
                    ResolutionType = table.Column<int>(type: "integer", nullable: false),
                    ResolutionSummary = table.Column<string>(type: "text", nullable: false),
                    AdjustedAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    AdjustedInvoiceId = table.Column<int>(type: "integer", nullable: true),
                    ResolvedByUserId = table.Column<int>(type: "integer", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeResolutions", x => x.DisputeResolutionId);
                    table.ForeignKey(
                        name: "FK_DisputeResolutions_Disputes_DisputeId",
                        column: x => x.DisputeId,
                        principalTable: "Disputes",
                        principalColumn: "DisputeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositTransactions",
                columns: table => new
                {
                    DepositTransactionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepositMasterID = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositTransactions", x => x.DepositTransactionID);
                    table.ForeignKey(
                        name: "FK_DepositTransactions_DepositMasters_DepositMasterID",
                        column: x => x.DepositMasterID,
                        principalTable: "DepositMasters",
                        principalColumn: "DepositMasterID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AssetCategories",
                columns: new[] { "AssetCategoryId", "CategoryName", "Code", "DefaultUsefulLifeMonths", "IsActive", "RequiresComplianceCheck" },
                values: new object[] { 1, "Safety & Security", "SEC", 0, true, true });

            migrationBuilder.InsertData(
                table: "AssetCategories",
                columns: new[] { "AssetCategoryId", "CategoryName", "Code", "DefaultUsefulLifeMonths", "IsActive", "IsDepreciable" },
                values: new object[,]
                {
                    { 2, "Plumbing & Sanitary", "PLUMB", 120, true, true },
                    { 3, "Mechanical", "MECH", 120, true, true }
                });

            migrationBuilder.InsertData(
                table: "AssetCategories",
                columns: new[] { "AssetCategoryId", "CategoryName", "Code", "DefaultUsefulLifeMonths", "IsActive", "IsDepreciable", "RequiresComplianceCheck" },
                values: new object[,]
                {
                    { 4, "Structural", "STRUCT", 300, true, true, true },
                    { 5, "Utility & Metering", "UTIL", 120, true, true, true },
                    { 6, "Electrical", "ELEC", 120, true, true, true }
                });

            migrationBuilder.InsertData(
                table: "AssetCategories",
                columns: new[] { "AssetCategoryId", "CategoryName", "Code", "DefaultUsefulLifeMonths", "IsActive", "IsDepreciable" },
                values: new object[,]
                {
                    { 7, "Furniture", "FURN", 60, true, true },
                    { 8, "Appliances", "APPL", 60, true, true },
                    { 9, "Kitchen", "KITCH", 60, true, true }
                });

            migrationBuilder.InsertData(
                table: "AssetCategories",
                columns: new[] { "AssetCategoryId", "CategoryName", "Code", "DefaultUsefulLifeMonths", "IsActive", "RequiresComplianceCheck" },
                values: new object[] { 10, "Documents & Inventory", "DOCINV", 0, true, true });

            migrationBuilder.InsertData(
                table: "DocumentCategories",
                columns: new[] { "DocumentCategoryID", "CategoryName", "CreatedDate", "Description", "IsActive", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Property Documents", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Tenant Documents", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Lease & Agreements", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Financial Documents", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Owner/Landlord Documents", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Maintenance Documents", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Legal Documents", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Other / Misc.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Rental Deposit Scheme.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Dispute Documents.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "DocumentSequences",
                columns: new[] { "Id", "CurrentNumber", "ModuleName", "NumberLength", "Prefix", "ResetEveryYear", "Year" },
                values: new object[,]
                {
                    { 1, 0, "LEASE", 5, "LSE", true, 2026 },
                    { 2, 0, "PROPERTY", 5, "PRO", false, null },
                    { 3, 0, "TENANT", 5, "TEN", false, null },
                    { 4, 0, "INVOICE", 6, "INV", true, 2026 },
                    { 5, 0, "LANDLORD", 6, "LLD", true, 2026 }
                });

            migrationBuilder.InsertData(
                table: "LeaseAlertRules",
                columns: new[] { "RuleID", "AlertType", "CreatedAt", "DeliveryMethod", "IsActive", "LeaseType", "MessageTemplate", "PaymentFrequency", "RuleCode", "RunTime", "TriggerDays", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "LeaseExpiry", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Email", true, "Outbound", "Lease expires in {Days} days", "Monthly", "LEASE_EXP_30", new TimeSpan(0, 2, 0, 0, 0), 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "LeaseExpiry", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dashboard", true, "Outbound", "Lease expires soon", "Monthly", "LEASE_EXP_7", new TimeSpan(0, 2, 0, 0, 0), 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "RentDue", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Email", true, "Outbound", "Rent due today", "Monthly", "RENT_DUE", new TimeSpan(0, 2, 0, 0, 0), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "RentOverdue", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SMS", true, "Outbound", "Rent overdue by {Days} days", "Monthly", "RENT_OVERDUE", new TimeSpan(0, 2, 0, 0, 0), -3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "OwnerTypes",
                columns: new[] { "OwnerTypeID", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Owner type Property", true, "Property", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Owner type Landlord", true, "Landlord", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Owner type Tenant", true, "Tenant", null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lease of the company ", true, "Lease", null, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vendor of the company", true, "Vendor", null, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "General owner type. This can be useful to add general documents", true, "General", null, null }
                });

            migrationBuilder.InsertData(
                table: "PenaltyPolicies",
                columns: new[] { "PenaltyPolicyID", "Description", "FixedAmount", "GracePeriodDays", "IsActive", "Name", "PercentageOfRent" },
                values: new object[,]
                {
                    { 1, "No late fee within grace period", 0m, 5, true, "No penalty", 0m },
                    { 2, "Flat 100 after grace period", 100m, 3, true, "Fixed Late Fee", 0m },
                    { 3, "5% of monthly rent after due date", 0m, 5, true, "Percentage Late Fee", 5m },
                    { 4, "100 per day after grace period", 100m, 2, true, "Daily Penalty", 0m }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionID", "Description", "IsSystem", "Module", "PermissionName" },
                values: new object[,]
                {
                    { 1001, "Create property", true, "Property", "PROPERTY_CREATE" },
                    { 1002, "View property", true, "Property", "PROPERTY_VIEW" },
                    { 1003, "Update property", true, "Property", "PROPERTY_UPDATE" },
                    { 1004, "Delete property", true, "Property", "PROPERTY_DELETE" },
                    { 1005, "Archive property", true, "Property", "PROPERTY_ARCHIVE" },
                    { 1101, "Create tenant", true, "Tenant", "TENANT_CREATE" },
                    { 1102, "View tenant", true, "Tenant", "TENANT_VIEW" },
                    { 1103, "Update tenant", true, "Tenant", "TENANT_UPDATE" },
                    { 1104, "Delete tenant", true, "Tenant", "TENANT_DELETE" },
                    { 1105, "Verify tenant KYC", true, "Tenant", "TENANT_VERIFY_KYC" },
                    { 1201, "Create lease", true, "Lease", "LEASE_CREATE" },
                    { 1202, "View lease", true, "Lease", "LEASE_VIEW" },
                    { 1203, "Update lease", true, "Lease", "LEASE_UPDATE" },
                    { 1204, "Approve lease", true, "Lease", "LEASE_APPROVE" },
                    { 1205, "Renew lease", true, "Lease", "LEASE_RENEW" },
                    { 1206, "Terminate lease", true, "Lease", "LEASE_TERMINATE" },
                    { 1301, "Record payment", true, "Payment", "PAYMENT_RECORD_CREATE" },
                    { 1302, "View payment", true, "Payment", "PAYMENT_VIEW" },
                    { 1303, "Edit payment", true, "Payment", "PAYMENT_EDIT" },
                    { 1304, "Refund payment", true, "Payment", "PAYMENT_REFUND" },
                    { 1401, "Collect deposit", true, "Deposit", "DEPOSIT_COLLECT" },
                    { 1402, "View deposit", true, "Deposit", "DEPOSIT_VIEW" },
                    { 1403, "Adjust deposit", true, "Deposit", "DEPOSIT_ADJUST" },
                    { 1404, "Refund deposit", true, "Deposit", "DEPOSIT_REFUND" },
                    { 1499, "For testing the permission operations", false, "Deposit", "Test Permission" },
                    { 1501, "Upload document", true, "Document", "DOCUMENT_UPLOAD" },
                    { 1502, "View document", true, "Document", "DOCUMENT_VIEW" },
                    { 1503, "Download document", true, "Document", "DOCUMENT_DOWNLOAD" },
                    { 1504, "Delete document", true, "Document", "DOCUMENT_DELETE" },
                    { 1505, "Verify document", true, "Document", "DOCUMENT_VERIFY" },
                    { 1510, "Permission to add document category", true, "Document", "DOCUMENT_CATEGORY_ADD" },
                    { 1511, "Permission to view document category", true, "Document", "DOCUMENT_CATEGORY_VIEW" },
                    { 1512, "Permission to edit document category", true, "Document", "DOCUMENT_CATEGORY_EDIT" },
                    { 1513, "Permission to delete document category", true, "Document", "DOCUMENT_CATEGORY_DELETE" },
                    { 1520, "Permission to add document type", true, "Document", "DOCUMENT_TYPE_ADD" },
                    { 1521, "Permission to view document types", true, "Document", "DOCUMENT_TYPE_VIEW" },
                    { 1522, "Permission to edit document type", true, "Document", "DOCUMENT_TYPE_EDIT" },
                    { 1523, "Permission to delete document type", true, "Document", "DOCUMENT_TYPE_DELETE" },
                    { 1601, "Create dispute", true, "Dispute", "DISPUTE_CREATE" },
                    { 1602, "View dispute", true, "Dispute", "DISPUTE_VIEW" },
                    { 1603, "Assign dispute", true, "Dispute", "DISPUTE_ASSIGN" },
                    { 1604, "Resolve dispute", true, "Dispute", "DISPUTE_RESOLVE" },
                    { 1605, "Close dispute", true, "Dispute", "DISPUTE_CLOSE" },
                    { 1701, "Create complaint", true, "Maintenance", "COMPLAINT_CREATE" },
                    { 1702, "View complaint", true, "Maintenance", "COMPLAINT_VIEW" },
                    { 1703, "Assign complaint", true, "Maintenance", "COMPLAINT_ASSIGN" },
                    { 1704, "Update complaint status", true, "Maintenance", "COMPLAINT_UPDATE_STATUS" },
                    { 1705, "Close complaint", true, "Maintenance", "COMPLAINT_CLOSE" },
                    { 1801, "Create user", true, "User", "USER_CREATE" },
                    { 1802, "View user", true, "User", "USER_VIEW" },
                    { 1803, "Update user", true, "User", "USER_UPDATE" },
                    { 1804, "Delete user", true, "User", "USER_DELETE" },
                    { 1805, "Assign role to user", true, "User", "USER_ASSIGN_ROLE" },
                    { 1901, "Create role", true, "Role", "ROLE_CREATE" },
                    { 1902, "View role", true, "Role", "ROLE_VIEW" },
                    { 1903, "Update role", true, "Role", "ROLE_UPDATE" },
                    { 1904, "Delete role", true, "Role", "ROLE_DELETE" },
                    { 1905, "Assign permission to role", true, "Role", "ROLE_ASSIGN_PERMISSION" },
                    { 2001, "View reports", true, "Report", "REPORT_VIEW" },
                    { 2002, "Generate reports", true, "Report", "REPORT_GENERATE" },
                    { 2003, "Export reports", true, "Report", "REPORT_EXPORT" },
                    { 2101, "Create landlord", true, "Landlord", "LANDLORD_CREATE" },
                    { 2102, "View landlord", true, "Landlord", "LANDLORD_VIEW" },
                    { 2103, "Update landlord", true, "Landlord", "LANDLORD_UPDATE" },
                    { 2104, "Delete landlord", true, "Landlord", "LANDLORD_DELETE" },
                    { 2201, "Permission to add asset category", true, "Asset Category", "ASSET_CATEGORY_ADD" },
                    { 2202, "Permission to view asset category", true, "Asset Category", "ASSET_CATEGORY_VIEW" },
                    { 2203, "Permission to edit asset category", true, "Asset Category", "ASSET_CATEGORY_EDIT" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleID", "CreatedAt", "Description", "IsActive", "RoleName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System administrator with full access", true, "Admin" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Owner of properties", true, "Property Owner" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manages properties for owners", true, "Property Manager" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tenant or lessee", true, "Tenant" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Service or maintenance vendor", true, "Vendor" }
                });

            migrationBuilder.InsertData(
                table: "AssetSubCategories",
                columns: new[] { "AssetSubCategoryId", "AssetCategoryId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1001, 1, true, "Fire Extinguishers" },
                    { 1002, 1, true, "Fire Alarm System" },
                    { 1003, 1, true, "Smoke Detectors" },
                    { 1004, 1, true, "CCTV Cameras" },
                    { 1005, 1, true, "Access Control System" },
                    { 1101, 2, true, "Water Supply Pipes" },
                    { 1102, 2, true, "Drainage / Sewer Lines" },
                    { 1103, 2, true, "Toilets & WC Units" },
                    { 1104, 2, true, "Wash Basins" },
                    { 1105, 2, true, "Water Tanks" },
                    { 1201, 3, true, "Elevators / Lifts" },
                    { 1202, 3, true, "HVAC Systems" },
                    { 1203, 3, true, "Ventilation Systems" },
                    { 1204, 3, true, "Generators" },
                    { 1301, 4, true, "Foundation" },
                    { 1302, 4, true, "Columns & Beams" },
                    { 1303, 4, true, "Roof Structure" },
                    { 1401, 5, true, "Electricity Meter" },
                    { 1402, 5, true, "Water Meter" },
                    { 1403, 5, true, "Gas Meter" },
                    { 1501, 6, true, "Internal Wiring" },
                    { 1502, 6, true, "Distribution Boards" },
                    { 1503, 6, true, "Lighting Fixtures" },
                    { 1601, 7, true, "Chairs" },
                    { 1602, 7, true, "Tables" },
                    { 1603, 7, true, "Wardrobes / Cupboards" },
                    { 1701, 8, true, "Air Conditioner" },
                    { 1702, 8, true, "Refrigerator" },
                    { 1703, 8, true, "Washing Machine" },
                    { 1801, 9, true, "Kitchen Cabinets" },
                    { 1802, 9, true, "Kitchen Sink" },
                    { 1803, 9, true, "Gas Stove" },
                    { 1901, 10, true, "Property Documents" },
                    { 1902, 10, true, "Lease Agreements" }
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocumentTypeID", "Description", "DocumentCategoryID", "IsActive", "TypeName" },
                values: new object[,]
                {
                    { 101, null, 1, true, "Title Deed" },
                    { 102, null, 1, true, "Property Tax Receipt" },
                    { 103, null, 1, true, "Building Plan Approval" },
                    { 104, null, 1, true, "Occupancy Certificate" },
                    { 201, null, 2, true, "Government ID Proof" },
                    { 202, null, 2, true, "Address Proof" },
                    { 203, null, 2, true, "Police Verification" },
                    { 204, null, 2, true, "Tenant Photograph" },
                    { 301, null, 3, true, "Lease Agreement" },
                    { 302, null, 3, true, "Renewal Agreement" },
                    { 303, null, 3, true, "Termination Notice" },
                    { 401, null, 4, true, "Rent Receipt" },
                    { 402, null, 4, true, "Security Deposit Receipt" },
                    { 403, null, 4, true, "Invoice" },
                    { 501, null, 5, true, "Owner ID Proof" },
                    { 502, null, 5, true, "Ownership Declaration" },
                    { 601, null, 6, true, "Maintenance Request" },
                    { 602, null, 6, true, "Work Order" },
                    { 603, null, 6, true, "Service Completion Report" },
                    { 701, null, 7, true, "Legal Notice" },
                    { 702, null, 7, true, "Court Order" },
                    { 801, null, 8, true, "General Attachment" },
                    { 802, null, 8, true, "Supporting Document" },
                    { 901, null, 9, true, "Tenancy Documents" },
                    { 902, null, 9, true, "Deposit Protection Documents" },
                    { 903, null, 9, true, "Property Condition Reports" },
                    { 904, null, 9, true, "Financial Records" },
                    { 905, null, 9, true, "Compliance Certificates" },
                    { 906, null, 9, true, "Dispute Resolution Evidence" },
                    { 907, null, 9, true, "End Of Tenancy Documents" },
                    { 1001, null, 10, true, "Initiation & Notice" },
                    { 1002, null, 10, true, "Contract & Reference Documents" },
                    { 1003, null, 10, true, "Evidence & Supporting Material" },
                    { 1004, null, 10, true, "Communication Records" },
                    { 1005, null, 10, true, "ELegal Proceedings" },
                    { 1006, null, 10, true, "Financial & Claims" },
                    { 1007, null, 10, true, "Resolution & Closure" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RolePermissionID", "IsAllowed", "PermissionID", "RoleID", "RoleID1" },
                values: new object[,]
                {
                    { 1, true, 1001, 1, null },
                    { 2, true, 1002, 1, null },
                    { 3, true, 1003, 1, null },
                    { 4, true, 1004, 1, null },
                    { 5, true, 1005, 1, null },
                    { 6, true, 1101, 1, null },
                    { 7, true, 1102, 1, null },
                    { 8, true, 1103, 1, null },
                    { 9, true, 1104, 1, null },
                    { 10, true, 1105, 1, null },
                    { 11, true, 1201, 1, null },
                    { 12, true, 1202, 1, null },
                    { 13, true, 1203, 1, null },
                    { 14, true, 1204, 1, null },
                    { 15, true, 1205, 1, null },
                    { 16, true, 1206, 1, null },
                    { 17, true, 1301, 1, null },
                    { 18, true, 1302, 1, null },
                    { 19, true, 1303, 1, null },
                    { 20, true, 1304, 1, null },
                    { 21, true, 1401, 1, null },
                    { 22, true, 1402, 1, null },
                    { 23, true, 1403, 1, null },
                    { 24, true, 1404, 1, null },
                    { 25, true, 1499, 1, null },
                    { 26, true, 1501, 1, null },
                    { 27, true, 1502, 1, null },
                    { 28, true, 1503, 1, null },
                    { 29, true, 1504, 1, null },
                    { 30, true, 1505, 1, null },
                    { 31, true, 1510, 1, null },
                    { 32, true, 1511, 1, null },
                    { 33, true, 1512, 1, null },
                    { 34, true, 1513, 1, null },
                    { 35, true, 1520, 1, null },
                    { 36, true, 1521, 1, null },
                    { 37, true, 1522, 1, null },
                    { 38, true, 1523, 1, null },
                    { 39, true, 1601, 1, null },
                    { 40, true, 1602, 1, null },
                    { 41, true, 1603, 1, null },
                    { 42, true, 1604, 1, null },
                    { 43, true, 1605, 1, null },
                    { 44, true, 1701, 1, null },
                    { 45, true, 1702, 1, null },
                    { 46, true, 1703, 1, null },
                    { 47, true, 1704, 1, null },
                    { 48, true, 1705, 1, null },
                    { 49, true, 1801, 1, null },
                    { 50, true, 1802, 1, null },
                    { 51, true, 1803, 1, null },
                    { 52, true, 1804, 1, null },
                    { 53, true, 1805, 1, null },
                    { 54, true, 1901, 1, null },
                    { 55, true, 1902, 1, null },
                    { 56, true, 1903, 1, null },
                    { 57, true, 1904, 1, null },
                    { 58, true, 1905, 1, null },
                    { 59, true, 2001, 1, null },
                    { 60, true, 2002, 1, null },
                    { 61, true, 2003, 1, null },
                    { 62, true, 2101, 1, null },
                    { 63, true, 2102, 1, null },
                    { 64, true, 2103, 1, null },
                    { 65, true, 2104, 1, null },
                    { 66, true, 2201, 1, null },
                    { 67, true, 2202, 1, null },
                    { 68, true, 2203, 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_OwnerTypeID_OwnerID",
                table: "Addresses",
                columns: new[] { "OwnerTypeID", "OwnerID" },
                unique: true,
                filter: "\"IsPrimary\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCategories_Code",
                table: "AssetCategories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_AssetId",
                table: "AssetMaintenances",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetCategoryId",
                table: "Assets",
                column: "AssetCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetSubCategoryId",
                table: "Assets",
                column: "AssetSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSubCategories_AssetCategoryId",
                table: "AssetSubCategories",
                column: "AssetCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositMasters_LeaseID",
                table: "DepositMasters",
                column: "LeaseID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_DepositMasterID",
                table: "DepositTransactions",
                column: "DepositMasterID");

            migrationBuilder.CreateIndex(
                name: "IX_DisputeAttachments_DisputeId_DocumentId",
                table: "DisputeAttachments",
                columns: new[] { "DisputeId", "DocumentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisputeComments_CommentedByUserId",
                table: "DisputeComments",
                column: "CommentedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DisputeComments_DisputeId",
                table: "DisputeComments",
                column: "DisputeId");

            migrationBuilder.CreateIndex(
                name: "IX_DisputeResolutions_DisputeId",
                table: "DisputeResolutions",
                column: "DisputeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_AssignedToUserId",
                table: "Disputes",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_DisputeNumber",
                table: "Disputes",
                column: "DisputeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_RaisedByUserId",
                table: "Disputes",
                column: "RaisedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessLogs_DocumentID",
                table: "DocumentAccessLogs",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentNumber",
                table: "Documents",
                column: "DocumentNumber",
                unique: true,
                filter: "\"DocumentNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeID",
                table: "Documents",
                column: "DocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_OwnerTypeID",
                table: "Documents",
                column: "OwnerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_DocumentCategoryID",
                table: "DocumentTypes",
                column: "DocumentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Landlords_LandlordNumber",
                table: "Landlords",
                column: "LandlordNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAlertRules_IsActive_AlertType",
                table: "LeaseAlertRules",
                columns: new[] { "IsActive", "AlertType" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAlertRules_RuleCode",
                table: "LeaseAlertRules",
                column: "RuleCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAlerts_LeaseID",
                table: "LeaseAlerts",
                column: "LeaseID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseRenewals_LeaseID",
                table: "LeaseRenewals",
                column: "LeaseID");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_LandlordID",
                table: "Leases",
                column: "LandlordID");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_LeaseNumber",
                table: "Leases",
                column: "LeaseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leases_LeaseType",
                table: "Leases",
                column: "LeaseType");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_PenaltyPolicyID",
                table: "Leases",
                column: "PenaltyPolicyID");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_PropertyID",
                table: "Leases",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_Status",
                table: "Leases",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_TenantID",
                table: "Leases",
                column: "TenantID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseSettlements_LeaseId",
                table: "LeaseSettlements",
                column: "LeaseId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseTerminations_LeaseID",
                table: "LeaseTerminations",
                column: "LeaseID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionName",
                table: "Permissions",
                column: "PermissionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_LandlordID",
                table: "Properties",
                column: "LandlordID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyName",
                table: "Properties",
                column: "PropertyName");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyNumber",
                table: "Properties",
                column: "PropertyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserID",
                table: "RefreshTokens",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RentSchedules_LeaseID",
                table: "RentSchedules",
                column: "LeaseID");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredDocuments_DocumentTypeID",
                table: "RequiredDocuments",
                column: "DocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredDocuments_OwnerTypeID",
                table: "RequiredDocuments",
                column: "OwnerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredDocuments_OwnerTypeID_DocumentTypeID",
                table: "RequiredDocuments",
                columns: new[] { "OwnerTypeID", "DocumentTypeID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionID",
                table: "RolePermissions",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleID_PermissionID",
                table: "RolePermissions",
                columns: new[] { "RoleID", "PermissionID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleID1",
                table: "RolePermissions",
                column: "RoleID1");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_TenantNumber",
                table: "Tenants",
                column: "TenantNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleID",
                table: "UserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserID",
                table: "UserRoles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AssetMaintenances");

            migrationBuilder.DropTable(
                name: "CompanySettings");

            migrationBuilder.DropTable(
                name: "DepositTransactions");

            migrationBuilder.DropTable(
                name: "DisputeAttachments");

            migrationBuilder.DropTable(
                name: "DisputeComments");

            migrationBuilder.DropTable(
                name: "DisputeResolutions");

            migrationBuilder.DropTable(
                name: "DocumentAccessLogs");

            migrationBuilder.DropTable(
                name: "DocumentSequences");

            migrationBuilder.DropTable(
                name: "DocumentUploadSessions");

            migrationBuilder.DropTable(
                name: "KYCTypes");

            migrationBuilder.DropTable(
                name: "LeaseAlertRules");

            migrationBuilder.DropTable(
                name: "LeaseAlerts");

            migrationBuilder.DropTable(
                name: "LeaseRenewals");

            migrationBuilder.DropTable(
                name: "LeaseSettlements");

            migrationBuilder.DropTable(
                name: "LeaseTerminations");

            migrationBuilder.DropTable(
                name: "PartyKYCs");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RentSchedules");

            migrationBuilder.DropTable(
                name: "RequiredDocuments");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "DepositMasters");

            migrationBuilder.DropTable(
                name: "Disputes");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "AssetSubCategories");

            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "OwnerTypes");

            migrationBuilder.DropTable(
                name: "AssetCategories");

            migrationBuilder.DropTable(
                name: "PenaltyPolicies");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "DocumentCategories");

            migrationBuilder.DropTable(
                name: "Landlords");
        }
    }
}
