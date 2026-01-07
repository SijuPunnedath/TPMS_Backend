using MediatR;
namespace TPMS.Application.Features.LeaseAlert.Commands;

public record DeleteLeaseAlertCommand(int AlertID) : IRequest<bool>;
