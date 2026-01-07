using MediatR;
using TPMS.Application.Features.RentSchedules.DTOs;

namespace TPMS.Application.Features.RentSchedules.Commands;

public class CreateRentScheduleCommand : IRequest<int>
{
    public RentScheduleDtoCrud RentSchedule { get; set; }
    public CreateRentScheduleCommand(RentScheduleDtoCrud rentSchedule)
    {
        RentSchedule = rentSchedule;
    }
}