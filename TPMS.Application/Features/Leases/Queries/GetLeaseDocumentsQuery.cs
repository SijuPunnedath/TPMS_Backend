using MediatR;
using System.Collections.Generic;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Leases.Queries;

public class GetLeaseDocumentsQuery : IRequest<List<DocumentDto>>
{
    public int LeaseId { get; }

    public GetLeaseDocumentsQuery(int leaseId)
    {
        LeaseId = leaseId;
    }
}