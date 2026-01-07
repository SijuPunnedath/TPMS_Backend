using TPMS.Application.Features.LeaseAlert.DTOs;
using MediatR;
namespace TPMS.Application.Features.LeaseAlert.Queries;

public record GetLeaseAlertByIdQuery(int AlertID) : IRequest<LeaseAlertDtoCrud?>;