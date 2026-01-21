using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Services;

public interface IDocumentQueryService
{
    Task<DocumentHealthDto> GetMissingDocumentsAsync(int ownerTypeId, int ownerId);
}