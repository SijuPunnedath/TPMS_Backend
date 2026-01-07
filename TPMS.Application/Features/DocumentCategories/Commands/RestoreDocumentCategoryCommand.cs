using MediatR;

namespace TPMS.Application.Features.DocumentCategory.Commands;

public record RestoreDocumentCategoryCommand(int CategoryID) : IRequest<bool>;
