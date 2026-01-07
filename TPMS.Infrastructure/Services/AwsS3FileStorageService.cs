using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Infrastructure.Services
{
    public class AwsS3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly IWebHostEnvironment _env;

        public AwsS3FileStorageService(IConfiguration config, IWebHostEnvironment env)
        {
            _bucketName = config["StorageSettings:Aws:BucketName"]!;
            var region = Amazon.RegionEndpoint.GetBySystemName(config["StorageSettings:Aws:Region"] ?? "ap-south-1");
            _s3Client = new AmazonS3Client(region);
            _env = env;
        }

       
        public async Task<string> SaveFileAsync(IFormFile file, string ownerType, int ownerId, CancellationToken cancellationToken)
        {
            string key = $"{ownerType.ToLower()}/{ownerId}/{file.FileName}";
            using var stream = file.OpenReadStream();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = _bucketName,
                ContentType = file.ContentType
            };

            var transfer = new TransferUtility(_s3Client);
            await transfer.UploadAsync(uploadRequest, cancellationToken);

            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }


        public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken)
        {
            try
            {
                // Extract key from URL
                var uri = new Uri(fileUrl);
                var key = uri.AbsolutePath.TrimStart('/');

                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(request, cancellationToken);
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Safe ignore — file doesn’t exist
                Console.WriteLine($"⚠️ S3 file not found during delete: {fileUrl}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"❌ Error deleting S3 file: {ex.Message}");
                throw;
            }
        }

        public async Task<byte[]> GetFileBytesAsync(string fileUrl, CancellationToken cancellationToken)
        {
            var fullPath = Path.Combine(_env.WebRootPath ?? "wwwroot", fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found at {fileUrl}");
            return await File.ReadAllBytesAsync(fullPath, cancellationToken);
        }

        // Overload for merged file path
        public async Task<string> SaveFileAsync(string localFilePath, string ownerType, int ownerId, CancellationToken cancellationToken)
        {
            string fileName = Path.GetFileName(localFilePath);
            string destinationFolder = Path.Combine(_env.ContentRootPath, "Uploads", ownerType, ownerId.ToString());
            Directory.CreateDirectory(destinationFolder);

            string destinationPath = Path.Combine(destinationFolder, fileName);
            File.Copy(localFilePath, destinationPath, overwrite: true);

            // optional: delete local temp file after copy
            await Task.CompletedTask;
            return $"/Uploads/{ownerType}/{ownerId}/{fileName}";
        }
        
        public async Task<Stream> OpenReadAsync(string relativePath, CancellationToken ct)
        {
            var root = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");

            var fullPath = Path.Combine(
                root,
                relativePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found on disk.");

            return new FileStream(
                fullPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true);
        }

    }
}
