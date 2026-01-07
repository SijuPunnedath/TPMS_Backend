using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Addresses.Commands;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Addresses.Queries;

namespace TPMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IMediator _mediator;
    public AddressesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllAddressesQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetAddressByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("owner/{ownerTypeId:int}/{ownerId:int}")]
    public async Task<IActionResult> GetByOwner(int ownerTypeId, int ownerId)
    {
        var result = await _mediator.Send(new GetAddressesByOwnerQuery(ownerTypeId, ownerId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddressDto dto)
    {
        var id = await _mediator.Send(new CreateAddressCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id }, new { AddressID = id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AddressDto dto)
    {
        dto.AddressID = id;
        var result = await _mediator.Send(new UpdateAddressCommand(dto));
        if (!result) return NotFound();
        return Ok(new { Message = "Address updated successfully" });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteAddressCommand(id));
        return result ? Ok(new { Message = "Address deleted" }) : NotFound();
    }

}