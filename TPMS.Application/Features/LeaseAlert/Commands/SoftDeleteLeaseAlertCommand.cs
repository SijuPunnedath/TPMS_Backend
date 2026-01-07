using MediatR;
namespace TPMS.Application.Features.LeaseAlert.Commands;

public record SoftDeleteLeaseAlertCommand(int AlertID) : IRequest<bool>;