using System.Threading;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.Services;

using Microsoft.AspNetCore.Http;

public interface IFileStorageServiceV2
{
    Task<string> SaveFileAsync(
        IFormFile file,
        string ownerType,
        int ownerId,
        CancellationToken cancellationToken);

    Task DeleteFileAsync(
        string fileUrl,
        CancellationToken cancellationToken);
}