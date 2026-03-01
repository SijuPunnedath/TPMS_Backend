using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Properties.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Properties.Handlers;

public class GetPropertyComplianceQueryHandler 
    : IRequestHandler<GetPropertyComplianceQuery, DocumentHealthDto>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetPropertyComplianceQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<DocumentHealthDto> Handle(
        GetPropertyComplianceQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetMissingDocumentsAsync((int)OwnerTypeEnums.Property, request.PropertyId);
    }
}