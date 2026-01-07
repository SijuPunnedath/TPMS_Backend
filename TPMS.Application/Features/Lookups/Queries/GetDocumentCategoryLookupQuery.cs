using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Lookups.DTOs;

namespace TPMS.Application.Features.Lookups.Queries;

public class GetDocumentCategoryLookupQuery : IRequest<List<DocumentCategoryLookupDto>>
{
    
}