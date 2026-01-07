using MediatR;

namespace TPMS.Application.Features.RentSchedules.Commands;

public record DeleteRentScheduleCommand(int ScheduleID) : IRequest<bool>;