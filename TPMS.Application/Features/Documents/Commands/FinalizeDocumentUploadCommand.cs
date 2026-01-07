using System;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Commands;

public record FinalizeDocumentUploadCommand(Guid SessionId) : IRequest<DocumentDto>;
