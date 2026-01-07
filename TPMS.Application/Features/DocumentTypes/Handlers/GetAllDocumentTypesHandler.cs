using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.DocumentTypes.DTOs;
using TPMS.Application.Features.DocumentTypes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentTypes.Handlers
{
    public class GetAllDocumentTypesHandler
        : IRequestHandler<GetAllDocumentTypesQuery, List<DocumentTypeDto>>
    {
        private readonly TPMSDBContext _db;

        public GetAllDocumentTypesHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<List<DocumentTypeDto>> Handle(GetAllDocumentTypesQuery request, CancellationToken cancellationToken)
        {
            return await _db.DocumentTypes
                .Include(t => t.Category)
                .Select(t => new DocumentTypeDto
                {
                    DocumentTypeID = t.DocumentTypeID,
                    DocumentCategoryID = t.DocumentCategoryID,
                    TypeName = t.TypeName,
                    Description = t.Description,
                    IsActive = t.IsActive,
                    CategoryName = t.Category.CategoryName
                })
                .ToListAsync(cancellationToken);
        }
    }

}
