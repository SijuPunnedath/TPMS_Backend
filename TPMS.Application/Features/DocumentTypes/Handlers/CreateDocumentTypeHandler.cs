using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentTypes.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentTypes.Handlers
{
    public class CreateDocumentTypeHandler 
        : IRequestHandler<CreateDocumentTypeCommand, int>
    {
        private readonly TPMSDBContext _db;

        public CreateDocumentTypeHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<int> Handle(CreateDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            // Validate category
            var category = await _db.DocumentCategories
                .FirstOrDefaultAsync(c => c.DocumentCategoryID == request.Dto.DocumentCategoryID);

            if (category == null)
                throw new Exception("Invalid DocumentCategoryID");

            var type = new DocumentType
            {
                DocumentCategoryID = request.Dto.DocumentCategoryID,
                TypeName = request.Dto.TypeName,
                Description = request.Dto.Description,
                IsActive = true
            };

            _db.DocumentTypes.Add(type);
            await _db.SaveChangesAsync(cancellationToken);

            return type.DocumentTypeID;
        }
    }

}
