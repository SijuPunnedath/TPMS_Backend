using MediatR;

namespace TPMS.Application.Features.DocumentTypes.Commands;
public record SoftDeleteDocumentTypeCommand(int DocumentTypeID)
    : IRequest<bool>;