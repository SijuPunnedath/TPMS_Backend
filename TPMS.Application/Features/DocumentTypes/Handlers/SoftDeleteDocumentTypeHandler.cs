using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentTypes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentTypes.Handlers;

public class SoftDeleteDocumentTypeHandler
    : IRequestHandler<SoftDeleteDocumentTypeCommand, bool>
{
    private readonly TPMSDBContext _db;

    public SoftDeleteDocumentTypeHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(SoftDeleteDocumentTypeCommand request, CancellationToken cancellationToken)
    {
        var type = await _db.DocumentTypes
            .FirstOrDefaultAsync(t => t.DocumentTypeID == request.DocumentTypeID);

        if (type == null)
            throw new Exception("DocumentType not found");

        type.IsActive = false;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
