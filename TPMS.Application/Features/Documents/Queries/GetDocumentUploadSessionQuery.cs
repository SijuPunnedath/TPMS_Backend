using System;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public record GetDocumentUploadSessionQuery(Guid SessionId)
    : IRequest<DocumentUploadSessionDto>;