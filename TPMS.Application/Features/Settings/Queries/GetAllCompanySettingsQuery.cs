using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Settings.DTOs;

namespace TPMS.Application.Features.Settings.Queries;

public record GetAllCompanySettingsQuery() : IRequest<List<CompanySettingsDto>>;