using MediatR;
using TPMS.Application.Features.DocumentCategory.DTOs;

namespace TPMS.Application.Features.DocumentCategory.Commands;

public record CreateDocumentCategoryCommand(DocumentCategoryDto Category)
    : IRequest<int>;
