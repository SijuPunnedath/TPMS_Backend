using MediatR;

namespace TPMS.Application.Features.DocumentTypes.Commands;

public record RestoreDocumentTypeCommand(int DocumentTypeID)
    : IRequest<bool>;


