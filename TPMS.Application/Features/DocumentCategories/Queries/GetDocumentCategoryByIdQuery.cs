using MediatR;
using TPMS.Application.Features.DocumentCategory.DTOs;

namespace TPMS.Application.Features.DocumentCategories.Queries;

public class GetDocumentCategoryByIdQuery : IRequest<DocumentCategoryDto?>
{
    public int CategoryID { get; set; }
    public GetDocumentCategoryByIdQuery(int id) => CategoryID = id; 
}