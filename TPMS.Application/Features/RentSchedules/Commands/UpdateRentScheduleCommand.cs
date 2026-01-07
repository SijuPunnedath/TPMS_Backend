using MediatR;
using TPMS.Application.Features.RentSchedules.DTOs;

namespace TPMS.Application.Features.RentSchedules.Commands;

public class UpdateRentScheduleCommand : IRequest<bool>
{
    public RentScheduleDtoCrud RentSchedule { get; set; }
    public UpdateRentScheduleCommand(RentScheduleDtoCrud rentSchedule)
    {
        RentSchedule = rentSchedule;
    }
}