using MediatR;
using TPMS.Application.Features.Settings.DTOs;

namespace TPMS.Application.Features.Settings.Commands;

public class UpdateCompanySettingsCommand : IRequest<bool>
{
    public CompanySettingsDto CompanySettings { get; set; }
    public UpdateCompanySettingsCommand(CompanySettingsDto companySettings)
    {
        CompanySettings = companySettings;
    }
}

