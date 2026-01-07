using MediatR;
namespace TPMS.Application.Features.LeaseAlert.Commands;

public record RestoreLeaseAlertCommand(int AlertID) : IRequest<bool>;