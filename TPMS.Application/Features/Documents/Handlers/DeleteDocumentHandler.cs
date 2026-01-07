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
    public class DeleteDocumentHandler : IRequestHandler<DeleteDocumentCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public DeleteDocumentHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _db.Documents.FindAsync(new object[] { request.DocumentID }, cancellationToken);
            if (doc == null) return false;

            _db.Documents.Remove(doc);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
