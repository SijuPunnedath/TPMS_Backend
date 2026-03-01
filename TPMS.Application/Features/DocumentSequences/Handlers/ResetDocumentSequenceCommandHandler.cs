using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Exceptions;
using TPMS.Application.Features.DocumentSequences.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentSequences.Handlers;

public class ResetDocumentSequenceCommandHandler 
    : IRequestHandler<ResetDocumentSequenceCommand, Unit>
{
    private readonly TPMSDBContext _context;

    public ResetDocumentSequenceCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        ResetDocumentSequenceCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentSequences
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(DocumentSequence), request.Id);

        entity.CurrentNumber = 0;

        if (entity.ResetEveryYear)
            entity.Year = DateTime.UtcNow.Year;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
