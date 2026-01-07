using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class SoftDeleteDocumentHandler : IRequestHandler<SoftDeleteDocumentCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public SoftDeleteDocumentHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(SoftDeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _db.Documents.FirstOrDefaultAsync(d => d.DocumentID == request.DocumentID, cancellationToken);

            if (doc == null)
                return false;

            // Soft delete
            doc.IsActive = false;
            doc.IsDeleted = true;  
            _db.Documents.Update(doc);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
