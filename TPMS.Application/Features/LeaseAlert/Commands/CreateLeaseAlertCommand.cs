using TPMS.Application.Features.LeaseAlert.DTOs;

namespace TPMS.Application.Features.LeaseAlert.Commands;

using MediatR;
public class CreateLeaseAlertCommand : IRequest<int>
{
    public LeaseAlertDtoCrud LeaseAlert { get; set; }
    public CreateLeaseAlertCommand(LeaseAlertDtoCrud alert) => LeaseAlert = alert;
}