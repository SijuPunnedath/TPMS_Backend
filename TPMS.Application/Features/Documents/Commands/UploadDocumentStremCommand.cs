using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Commands;

public class UploadDocumentStremCommand : IRequest<DocumentDto>
{
    public UploadDocumentStremDto Document { get; }

    public UploadDocumentStremCommand(UploadDocumentStremDto document)
    {
          Document = document;  
    }
}