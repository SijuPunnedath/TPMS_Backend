using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.RequiredDocuments.DTOs;

namespace TPMS.Application.Features.RequiredDocuments.Queries;

public class GetRequiredDocumentsQuery : IRequest<List<RequiredDocumentDto>>
{
    public int? OwnerTypeID { get; set; }
    public bool? IsActive { get; set; } 
}
