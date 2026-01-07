using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Infrastructure.Services
{
    public interface IFileStorageService
    {
        // Fro Regular Uploads
        Task<string> SaveFileAsync(IFormFile file, string ownerType, int ownerId, CancellationToken cancellationToken);
       
        // For chunked uploads (merged local file)
        Task<string> SaveFileAsync(string localFilePath, string ownerType, int ownerId, CancellationToken cancellationToken);

        Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken);
       
        Task<byte[]> GetFileBytesAsync(string fileUrl, CancellationToken cancellationToken);
        
        Task<Stream> OpenReadAsync(string relativePath, CancellationToken ct);
    }
}
