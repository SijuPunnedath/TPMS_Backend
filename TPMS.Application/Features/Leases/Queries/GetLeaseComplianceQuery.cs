using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Leases.Queries;

public class GetLeaseComplianceQuery : IRequest<DocumentHealthDto>
{
    public int LeaseId { get; }

    public GetLeaseComplianceQuery(int leaseId)
    {
        LeaseId = leaseId;
    }
}