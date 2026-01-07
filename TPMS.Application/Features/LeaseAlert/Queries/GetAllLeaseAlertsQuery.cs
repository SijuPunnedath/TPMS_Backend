using MediatR;
using TPMS.Application.Features.LeaseAlert.DTOs;
using System.Collections.Generic;

namespace TPMS.Application.Features.LeaseAlert.Queries;

public record GetAllLeaseAlertsQuery() : IRequest<List<LeaseAlertDtoCrud>>;