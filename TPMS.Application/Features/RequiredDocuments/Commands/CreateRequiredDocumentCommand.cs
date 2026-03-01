using MediatR;

namespace TPMS.Application.Features.RequiredDocuments.Commands;

public class CreateRequiredDocumentCommand : IRequest<int>
{
    public int OwnerTypeID { get; set; }
    public int DocumentTypeID { get; set; }
    public bool IsMandatory { get; set; } = true;
}
