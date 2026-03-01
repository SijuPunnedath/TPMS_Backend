using MediatR;

namespace TPMS.Application.Features.RequiredDocuments.Commands;

public class UpdateRequiredDocumentCommand : IRequest
{
    public int RequiredDocumentID { get; set; }
    public bool IsMandatory { get; set; }
    public bool IsActive { get; set; }
}