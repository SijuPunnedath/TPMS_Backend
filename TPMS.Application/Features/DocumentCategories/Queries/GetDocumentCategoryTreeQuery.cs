using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.DocumentCategory.DTOs;

namespace TPMS.Application.Features.DocumentCategories.Queries;

public record GetDocumentCategoryTreeQuery 
    : IRequest<List<DocumentCategoryTreeDto>>;
