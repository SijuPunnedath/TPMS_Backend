using MediatR;

namespace TPMS.Application.Features.Settings.Commands;

public record DeleteCompanySettingsCommand(int CompanyID) : IRequest<bool>;