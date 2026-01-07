using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public class ViewDocumentQuery  : IRequest<ViewDocumentResult>
{
    public int DocumentID { get; set; }
}