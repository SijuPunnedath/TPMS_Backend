using MediatR;
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
    public class UpdateDocumentHandler :IRequestHandler<UpdateDocumentCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public UpdateDocumentHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _db.Documents.FindAsync(new object[] { request.DocumentID }, cancellationToken);
            if (doc == null) return false;

            if (request.DocType != null) doc.DocType = request.DocType;
            if (request.Description != null) doc.Description = request.Description;
            if (request.Version != null) doc.Version = request.Version;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
