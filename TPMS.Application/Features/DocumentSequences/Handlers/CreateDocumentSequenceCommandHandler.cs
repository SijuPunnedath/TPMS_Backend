using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.DocumentSequences.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentSequences.Handlers;

public class CreateDocumentSequenceCommandHandler 
    : IRequestHandler<CreateDocumentSequenceCommand, int>
{
    private readonly TPMSDBContext _context;

    public CreateDocumentSequenceCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        CreateDocumentSequenceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {

        
        var sequence = new DocumentSequence
        {
           // TenantId = request.TenantId,
            ModuleName = request.ModuleName.ToUpper(),
            Prefix = request.Prefix.ToUpper(),
            CurrentNumber = 0,
            NumberLength = request.NumberLength,
            ResetEveryYear = request.ResetEveryYear,
            Year = request.ResetEveryYear ? DateTime.UtcNow.Year : null
        };

        _context.DocumentSequences.Add(sequence);
        await _context.SaveChangesAsync(cancellationToken);

        return sequence.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
