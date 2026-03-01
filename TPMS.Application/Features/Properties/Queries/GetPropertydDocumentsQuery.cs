using MediatR;
using System.Collections.Generic;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Properties.Queries;

public class GetPropertyDocumentsQuery : IRequest<List<DocumentDto>>
{
    public int PropertyId { get; }

    public GetPropertyDocumentsQuery(int propertyId)
    {
        PropertyId = propertyId;
    }
}