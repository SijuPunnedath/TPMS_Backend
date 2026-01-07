using MediatR;
using System.Collections.Generic;
using TPMS.Application.Features.LeaseAlert.DTOs;

namespace TPMS.Application.Features.LeaseAlert.Queries;

public record GetPendingLeaseAlertsQuery() : IRequest<List<LeaseAlertDtoCrud>>;
