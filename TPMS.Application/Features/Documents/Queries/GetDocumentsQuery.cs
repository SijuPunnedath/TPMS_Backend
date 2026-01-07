using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public class GetDocumentsQuery : IRequest<List<DocumentListItemDto>>
{
    public int OwnerTypeID { get; set; }
    public int OwnerID { get; set; }

    public int? DocumentCategoryID { get; set; }
    public int? DocumentTypeID { get; set; }

    public bool IncludeArchived { get; set; } = false;
}