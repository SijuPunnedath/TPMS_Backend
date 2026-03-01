using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentSequences.Services;

public class DocumentNumberService : IDocumentNumberService
{
    private readonly TPMSDBContext _context;

    public DocumentNumberService(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAsync(
        //int tenantId,
        string moduleName,
        CancellationToken cancellationToken = default)
    {
        var currentYear = DateTime.UtcNow.Year;

        var sequence = await _context.DocumentSequences
            .FirstOrDefaultAsync(x =>
                   
                    x.ModuleName.ToLower() == moduleName.ToLower() &&
                    (!x.ResetEveryYear || x.Year == currentYear),
                cancellationToken);

        if (sequence == null)
            throw new Exception($"Document sequence not configured for {moduleName}");

        sequence.CurrentNumber++;

        await _context.SaveChangesAsync(cancellationToken);

        var numberPart = sequence.CurrentNumber
            .ToString()
            .PadLeft(sequence.NumberLength, '0');

        if (sequence.ResetEveryYear)
        {
            return $"{sequence.Prefix}-{currentYear}-{numberPart}";
        }

        return $"{sequence.Prefix}-{numberPart}";
    }
} 
