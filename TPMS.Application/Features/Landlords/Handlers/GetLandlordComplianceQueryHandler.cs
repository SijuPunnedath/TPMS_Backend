using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Landlords.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Landlords.Handlers;

public class GetLandlordComplianceQueryHandler : IRequestHandler<GetLandlordComplianceQuery, DocumentHealthDto>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetLandlordComplianceQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<DocumentHealthDto> Handle(
        GetLandlordComplianceQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetMissingDocumentsAsync((int)OwnerTypeEnums.Landlord, request.LandlordId);
    }
}