using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Commands;

public class UploadDocumentStreamV2Command : IRequest<DocumentDto>
{
    public UploadDocumentStremDto Document { get; set; } = default!;
}