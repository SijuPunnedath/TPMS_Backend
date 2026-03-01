using System.Collections.Generic;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Services;

public interface IDocumentQueryService
{
    Task<DocumentHealthDto> GetMissingDocumentsAsync(int ownerTypeId, int ownerId);
    
    Task<List<DocumentDto>> GetByOwnerAsync(int ownerTypeId, int ownerId);
    
  
}