using MediatR;
using TPMS.Application.Features.Settings.DTOs;

namespace TPMS.Application.Features.Settings.Queries;

public record GetCompanySettingsByIdQuery(int CompanyID) : IRequest<CompanySettingsDto?>;