using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RequiredDocuments.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RequiredDocuments.Handlers;

public class DeleteRequiredDocumentCommandHandler 
    : IRequestHandler<DeleteRequiredDocumentCommand>
{
    private readonly TPMSDBContext _context;

    public DeleteRequiredDocumentCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteRequiredDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RequiredDocuments
            .FirstOrDefaultAsync(x => x.RequiredDocumentID == request.Id, cancellationToken);

        if (entity == null)
            throw new Exception("RequiredDocument not found.");

        entity.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

