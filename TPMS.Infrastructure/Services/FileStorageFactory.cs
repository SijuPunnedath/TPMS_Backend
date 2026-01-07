using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Infrastructure.Services
{
    public static class FileStorageFactory // Changed to static class to fix CS1106
    {
        public static void AddFileStorage(this IServiceCollection services, IConfiguration config)
        {
            var provider = config["StorageSettings:Provider"]?.ToLower() ?? "local";

            switch (provider)
            {
                case "aws":
                    services.AddScoped<IFileStorageService, AwsS3FileStorageService>();
                    break;

                case "azure":
                    services.AddScoped<IFileStorageService, AzureBlobFileStorageService>();
                    break;

                default:
                    services.AddScoped<IFileStorageService, LocalFileStorageService>();
                    break;
            }
        }
    }
}
