using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public record DownloadDocumentByIdQuery(int DocumentID)
    : IRequest<DownloadDocumentResult>;
