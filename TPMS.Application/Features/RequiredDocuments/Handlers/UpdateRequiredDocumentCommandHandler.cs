using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RequiredDocuments.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RequiredDocuments.Handlers;

public class UpdateRequiredDocumentCommandHandler 
    : IRequestHandler<UpdateRequiredDocumentCommand>
{
    private readonly TPMSDBContext _context;

    public UpdateRequiredDocumentCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateRequiredDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RequiredDocuments
            .FirstOrDefaultAsync(x => x.RequiredDocumentID == request.RequiredDocumentID, cancellationToken);

        if (entity == null)
            throw new Exception("RequiredDocument not found.");

        entity.IsMandatory = request.IsMandatory;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

