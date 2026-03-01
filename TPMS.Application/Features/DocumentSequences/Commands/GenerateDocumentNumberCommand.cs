using MediatR;

namespace TPMS.Application.Features.DocumentSequences.Commands;

public record GenerateDocumentNumberCommand(
    string ModuleName
) : IRequest<string>;