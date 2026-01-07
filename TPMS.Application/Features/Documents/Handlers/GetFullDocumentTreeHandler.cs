using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers;

public class GetFullDocumentTreeHandler 
    : IRequestHandler<GetFullDocumentTreeQuery, List<DocumentTreeOwnerTypeDto>>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _ownerTypeCache;

    public GetFullDocumentTreeHandler(TPMSDBContext db, IOwnerTypeCacheService ownerTypeCache)
    {
        _db = db;
        _ownerTypeCache = ownerTypeCache;
    }

    public async Task<List<DocumentTreeOwnerTypeDto>> Handle(GetFullDocumentTreeQuery request, CancellationToken cancellationToken)
    {
        var documents = await _db.Documents
            .Where(d => !d.IsDeleted)
            .Include(d => d.DocumentType)
                .ThenInclude(dt => dt.Category)
            .ToListAsync(cancellationToken);

        var ownerTypes = await _db.OwnerTypes.ToListAsync(cancellationToken);

        List<DocumentTreeOwnerTypeDto> result = new();

        foreach (var ownerType in ownerTypes)
        {
            // Group documents by owner
            var ownerTypeDocs = documents
                .Where(d => d.OwnerTypeID == ownerType.OwnerTypeID)
                .GroupBy(d => d.OwnerID)
                .ToList();

            if (!ownerTypeDocs.Any()) continue;

            var ownerTypeDto = new DocumentTreeOwnerTypeDto
            {
                OwnerType = ownerType.Name,
                Owners = new List<DocumentTreeOwnerDto>()
            };

            foreach (var ownerGroup in ownerTypeDocs)
            {
                int ownerId = ownerGroup.Key;
                string ownerName = ResolveOwnerName(ownerType.Name, ownerId);

                var ownerDto = new DocumentTreeOwnerDto
                {
                    OwnerID = ownerId,
                    OwnerName = ownerName
                };

                // categories under this owner
                var categories = ownerGroup
                    .GroupBy(d => d.DocumentType!.DocumentCategoryID);

                foreach (var cat in categories)
                {
                    var first = cat.First();
                    var catDto = new DocumentTreeCategoryDto
                    {
                        CategoryID = first.DocumentType!.Category!.DocumentCategoryID,
                        CategoryName = first.DocumentType.Category.CategoryName
                    };

                    // document types under category
                    var types = cat.GroupBy(d => d.DocumentTypeID);

                    foreach (var t in types)
                    {
                        var tFirst = t.First();
                        var typeDto = new DocumentTreeTypeDto
                        {
                            DocumentTypeID = tFirst.DocumentTypeID,
                            TypeName = tFirst.DocumentType!.TypeName
                        };

                        // documents
                        typeDto.Documents = t.Select(d => new DocumentTreeDocumentDto
                        {
                            DocumentID = d.DocumentID,
                            FileName = d.FileName ?? "",
                            URL = d.URL ?? "",
                            Version = d.Version
                        }).ToList();

                        catDto.Types.Add(typeDto);
                    }

                    ownerDto.Categories.Add(catDto);
                }

                ownerTypeDto.Owners.Add(ownerDto);
            }

            result.Add(ownerTypeDto);
        }

        return result;
    }

    private string ResolveOwnerName(string ownerTypeName, int ownerID)
    {
        return ownerTypeName.ToLower() switch
        {
            "tenant"   => _db.Tenants.Find(ownerID)?.Name ?? "Unknown Tenant",
            "landlord" => _db.Landlords.Find(ownerID)?.Name ?? "Unknown Landlord",
            "property" => _db.Properties.Find(ownerID)?.Type ?? "Unknown Property",
            "lease"    => $"Lease #{ownerID}",
            _          => "Unknown"
        };
    }
}
