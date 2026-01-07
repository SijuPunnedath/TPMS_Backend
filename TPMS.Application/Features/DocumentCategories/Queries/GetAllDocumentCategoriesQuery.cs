using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.DocumentCategory.DTOs;

namespace TPMS.Application.Features.DocumentCategories.Queries;

public class GetAllDocumentCategoriesQuery : IRequest<List<DocumentCategoryDto>>
{
    public bool IncludeInactive { get; set; } = false;
}