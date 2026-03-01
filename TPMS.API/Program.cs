
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using MediatR;
using TPMS.API.Common;
using TPMS.API.Middleware;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Common.Services;
using TPMS.Application.Features.Auth.Handlers;
using TPMS.Application.Features.Documents.Handlers;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.DocumentSequences.Services;
using TPMS.Application.Features.Leases.Services;
using TPMS.Application.Features.Lookups.Services;
using TPMS.Application.Mappings;
using TPMS.Infrastructure.Common.DataSeed;
using TPMS.Infrastructure.Persistence;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.POCO;
using TPMS.Infrastructure.Services;


namespace TPMS.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            //-- Enable legacy timestamp behavior
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // Configuration for JWT in appsettings (see snippet below)
            var jwtKey = builder.Configuration["Jwt:Secret"];
        
            
           // Adding Cache Services
           builder.Services.AddMemoryCache();
           builder.Services.AddScoped<ILookupCacheService, LookupCacheService>();
           
            // services
            builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
           // builder.Services.AddHostedService<RentScheduleMonitorService>();


            builder.Services.AddDbContext<TPMSDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddFileStorage(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            // Add services to the container.
           builder.Services.AddScoped<IOwnerTypeCacheService, OwnerTypeCacheService>();
           builder.Services.AddScoped<IDocumentTypeCacheService, DocumentTypeCacheService>();

           builder.Services.AddScoped<IEmailService, SmtpEmailService>();
           builder.Services.AddScoped<ISmsService, TwilioSmsService>();
           builder.Services.AddScoped<ILeaseSettlementService, LeaseSettlementService>();
           builder.Services.AddScoped<IDocumentNumberService,DocumentNumberService>();
         
           //-- These services were causing server issues
          // builder.Services.AddHostedService<RentScheduleMonitorService>();
          // builder.Services.AddHostedService<LeaseAlertDispatcherService>();
          //builder.Services.AddHostedService<DocumentUploadCleanupService>();
          
           //-- Settig lease expiry automatically
          // builder.Services.AddHostedService<LeaseExpirationService>();

          //builder.Services.AddHostedService<PermissionSeederHostedService>();

           //Base url
           builder.Services.Configure<AppSettings>(
               builder.Configuration.GetSection("AppSettings"));

           builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UploadDocumentChunkHandler>());
            //builder.Services.AddSingleton<IOwnerTypeCacheService, OwnerTypeCacheService>();
            builder.Services.AddScoped<IFileStorageService, FileStorageService>();
            
            //Leaser Alert Services
          //  builder.Services.AddHostedService<LeaseAlertDailyJob>();
            builder.Services.AddScoped<ILeaseAlertService, LeaseAlertService>();
            builder.Services.AddScoped<IDocumentQueryService, DocumentQueryService>();
            builder.Services.AddScoped<IFileStorageServiceV2, FileStorageServiceV2>();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    
                });
            });
            
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters
                        .Add(new JsonStringEnumConverter());
                });;

            //Mapping SMS class
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            
            builder.Services.AddEndpointsApiExplorer();
           
           builder.Services.AddSwaggerGen(c =>
           {
               c.CustomSchemaIds(type => type.FullName);
               c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
               {
                   Title = "TPMS API",
                   Version = "v1"
               });
           });

            //-- JWT auth
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey) ?? throw new InvalidOperationException("JWT Secret is not configured");;
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
               // options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

            builder.Services.AddAuthorization();
            

            //--File Upload Options
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100_000_000; // 100 MB
            });
           
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto |
                    ForwardedHeaders.XForwardedHost;

                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            
            
            var app = builder.Build();
            
           try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TPMSDBContext>();
                    await DbInitializer.SeedAsync(context);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            } 
            
            app.UseForwardedHeaders();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TPMS API V1");
            });

         
         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseRouting();
         app.UseCors("AllowAll"); 
         app.UseAuthentication();
         app.UseAuthorization();
         app.MapControllers();

         app.Run();
        }
    }
}
