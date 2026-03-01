//namespace TPMS.Application.Features.DocumentSequences.Handlers;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.DocumentSequences.Commands;
using TPMS.Application.Features.DocumentSequences.Services;

namespace TPMS.Application.Features.DocumentSequences.Handlers;

public class GenerateDocumentNumberHandler 
    : IRequestHandler<GenerateDocumentNumberCommand, string>
{
    private readonly IDocumentNumberService _documentNumberService;

    public GenerateDocumentNumberHandler(
        IDocumentNumberService documentNumberService)
    {
        _documentNumberService = documentNumberService;
    }

    public async Task<string> Handle(
        GenerateDocumentNumberCommand request,
        CancellationToken cancellationToken)
    {
        return await _documentNumberService.GenerateAsync(
            request.ModuleName,
            cancellationToken);
    }
}
