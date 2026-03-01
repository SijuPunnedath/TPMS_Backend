using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Properties.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Properties.Handlers;

public class GetPropertyDocumentsQueryHandler 
    : IRequestHandler<GetPropertyDocumentsQuery, List<DocumentDto>>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetPropertyDocumentsQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<List<DocumentDto>> Handle(
        GetPropertyDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetByOwnerAsync((int)OwnerTypeEnums.Property, request.PropertyId);
    }
}