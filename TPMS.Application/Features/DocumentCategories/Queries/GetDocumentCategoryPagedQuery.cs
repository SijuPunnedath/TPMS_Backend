using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.DocumentCategory.DTOs;

namespace TPMS.Application.Features.DocumentCategories.Queries;

public class GetDocumentCategoryPagedQuery : IRequest<PagedResult<DocumentCategoryDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
}