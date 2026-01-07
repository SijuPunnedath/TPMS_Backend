using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.RentSchedules.DTOs;

namespace TPMS.Application.Features.RentSchedules.Queries;

public record GetRentSchedulesByLeaseIdQuery(int LeaseID) : IRequest<List<RentScheduleDtoCrud>>;