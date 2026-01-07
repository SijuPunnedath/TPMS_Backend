using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.DocumentTypes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentTypes.Handlers
{
    public class UpdateDocumentTypeHandler 
        : IRequestHandler<UpdateDocumentTypeCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public UpdateDocumentTypeHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(UpdateDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            var type = await _db.DocumentTypes
                .FirstOrDefaultAsync(t => t.DocumentTypeID == request.Dto.DocumentTypeID);

            if (type == null)
                throw new Exception("DocumentType not found");

            type.DocumentCategoryID = request.Dto.DocumentCategoryID;
            type.TypeName = request.Dto.TypeName;
            type.Description = request.Dto.Description;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
