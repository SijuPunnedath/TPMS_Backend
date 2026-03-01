using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentSequences.DTOs;
using TPMS.Application.Features.DocumentSequences.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentSequences.Handlers;

public class GetDocumentSequencesQueryHandler 
    : IRequestHandler<GetDocumentSequencesQuery, List<DocumentSequenceDto>>
{
    private readonly TPMSDBContext _context;

    public GetDocumentSequencesQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<List<DocumentSequenceDto>> Handle(
        GetDocumentSequencesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.DocumentSequences
            .AsNoTracking()
            .Select(x => new DocumentSequenceDto
            {
                Id = x.Id,
                ModuleName = x.ModuleName,
                Prefix = x.Prefix,
                CurrentNumber = x.CurrentNumber,
                NumberLength = x.NumberLength,
                ResetEveryYear = x.ResetEveryYear,
                Year = x.Year
            })
            .ToListAsync(cancellationToken);
    }
}