using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.DocumentTypes.DTOs;

namespace TPMS.Application.Features.DocumentTypes.Queries;

public record GetDocumentTypesByCategoryQuery(int CategoryID)
    : IRequest<List<DocumentTypeDto>>;
