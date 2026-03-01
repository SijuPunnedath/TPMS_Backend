using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Landlords.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Landlords.Handlers;

public class GetLandlordDocumentsQueryHandler : IRequestHandler<GetLandlordDocumentsQuery, List<DocumentDto>>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetLandlordDocumentsQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<List<DocumentDto>> Handle(
        GetLandlordDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetByOwnerAsync((int)OwnerTypeEnums.Landlord, request.LandlordId);
    }
    
}