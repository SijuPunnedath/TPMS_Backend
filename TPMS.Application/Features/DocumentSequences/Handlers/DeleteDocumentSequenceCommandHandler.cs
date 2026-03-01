using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Exceptions;
using TPMS.Application.Features.DocumentSequences.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentSequences.Handlers;

public class DeleteDocumentSequenceCommandHandler 
    : IRequestHandler<DeleteDocumentSequenceCommand,Unit>
{
    private readonly TPMSDBContext _context;

    public DeleteDocumentSequenceCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteDocumentSequenceCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentSequences
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(DocumentSequence), request.Id);

        _context.DocumentSequences.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
