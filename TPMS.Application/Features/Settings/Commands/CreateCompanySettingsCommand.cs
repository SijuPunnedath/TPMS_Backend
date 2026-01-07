using MediatR;
using TPMS.Application.Features.Settings.DTOs;

namespace TPMS.Application.Features.Settings.Commands;

public class CreateCompanySettingsCommand : IRequest<int>
{
    public CompanySettingsDto CompanySettings { get; set; }
    public CreateCompanySettingsCommand(CompanySettingsDto companySettings)
    {
        CompanySettings = companySettings;
    }
}