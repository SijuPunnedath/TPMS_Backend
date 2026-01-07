using MediatR;

namespace TPMS.Application.Features.DocumentCategory.Commands;

public record SoftDeleteDocumentCategoryCommand(int CategoryID) : IRequest<bool>;