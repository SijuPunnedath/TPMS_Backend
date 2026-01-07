using MediatR;
using TPMS.Application.Features.DocumentTypes.DTOs;

namespace TPMS.Application.Features.DocumentTypes.Queries;

public record GetDocumentTypeByIdQuery(int DocumentTypeID)
    : IRequest<DocumentTypeDto>;
