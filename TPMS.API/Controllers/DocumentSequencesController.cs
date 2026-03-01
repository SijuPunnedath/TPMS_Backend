using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.DocumentSequences.Commands;
using TPMS.Application.Features.DocumentSequences.Queries;

namespace TPMS.API.Controllers;

[ApiController]
[Route("api/document-sequences")]
public class DocumentSequencesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentSequencesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Generate next document number for module
    /// Example: api/documentsequences/generate/Lease
    /// </summary>
    [HttpPost("generate/{moduleName}")]
    public async Task<IActionResult> Generate(string moduleName)
    {
        var result = await _mediator.Send(
            new GenerateDocumentNumberCommand(moduleName));

        return Ok(result);
    } 
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateDocumentSequenceCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut]
    public async Task<IActionResult> Update(UpdateDocumentSequenceCommand command)
        => Ok(await _mediator.Send(command));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteDocumentSequenceCommand { Id = id }));

    [HttpPost("{id}/reset")]
    public async Task<IActionResult> Reset(int id)
        => Ok(await _mediator.Send(new ResetDocumentSequenceCommand { Id = id }));

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetDocumentSequencesQuery());
        return Ok(result);
    }
}
