using TPMS.Application.Features.LeaseAlert.DTOs;

namespace TPMS.Application.Features.LeaseAlert.Commands;
using MediatR;
public class UpdateLeaseAlertCommand : IRequest<bool>
{
    public LeaseAlertDtoCrud LeaseAlert { get; set; }
    public UpdateLeaseAlertCommand(LeaseAlertDtoCrud alert) => LeaseAlert = alert;
}