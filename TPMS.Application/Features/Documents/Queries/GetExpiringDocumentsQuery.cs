using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public class GetExpiringDocumentsQuery : IRequest<List<ExpiringDocumentDto>>
{
    public int DaysAhead { get; }

    public GetExpiringDocumentsQuery(int daysAhead)
    {
        DaysAhead = daysAhead;
    }
}