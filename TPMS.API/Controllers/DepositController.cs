using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Deposit.Commands;
using TPMS.Application.Features.Deposit.Queries;

[ApiController]
[Route("api/[controller]")]
public class DepositController : ControllerBase
{
    private readonly IMediator _mediator;
    public DepositController(IMediator mediator) => _mediator = mediator;

    // Create deposit master
    [HttpPost("master")]
    public async Task<IActionResult> CreateMaster(CreateDepositMasterCommand command)
        => Ok(await _mediator.Send(command));

    // Update deposit master
    [HttpPut("master")]
    public async Task<IActionResult> UpdateMaster(UpdateDepositMasterCommand command)
        => Ok(await _mediator.Send(command));

    // Delete deposit master
    [HttpDelete("master/{id}")]
    public async Task<IActionResult> DeleteMaster(int id)
        => Ok(await _mediator.Send(new DeleteDepositMasterCommand(id)));

    // Add transaction
    [HttpPost("transaction")]
    public async Task<IActionResult> AddTransaction(AddDepositTransactionCommand command)
        => Ok(await _mediator.Send(command));

    // Get deposit master by lease ID
    [HttpGet("master/lease/{leaseId}")]
    public async Task<IActionResult> GetByLease(int leaseId)
        => Ok(await _mediator.Send(new GetDepositMasterByLeaseIdQuery(leaseId)));

    // Get deposit transactions
    [HttpGet("transactions/{masterId}")]
    public async Task<IActionResult> GetTransactions(int masterId)
        => Ok(await _mediator.Send(new GetDepositTransactionsQuery(masterId)));
}