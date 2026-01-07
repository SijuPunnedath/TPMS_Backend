using MediatR;
using TPMS.Application.Features.RentSchedules.DTOs;

namespace TPMS.Application.Features.RentSchedules.Queries;

public record GetRentScheduleByIdQuery(int ScheduleID) : IRequest<RentScheduleDtoCrud?>;
