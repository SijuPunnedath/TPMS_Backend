using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Settings.DTOs;
using TPMS.Application.Features.Settings.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Settings.Handlers;

public class GetCompanySettingsByIdHandler : IRequestHandler<GetCompanySettingsByIdQuery, CompanySettingsDto?>
{
    private readonly TPMSDBContext _db;
    public GetCompanySettingsByIdHandler(TPMSDBContext db) => _db = db;

    public async Task<CompanySettingsDto?> Handle(GetCompanySettingsByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _db.CompanySettings.FirstOrDefaultAsync(c => c.CompanyID == request.CompanyID, cancellationToken);
        if (entity == null) return null;

        return new CompanySettingsDto
        {
            CompanyID = entity.CompanyID,
            CompanyName = entity.CompanyName,
            RegistrationNumber = entity.RegistrationNumber,
            TaxID = entity.TaxID,
            AddressLine1 = entity.AddressLine1,
            AddressLine2 = entity.AddressLine2,
            City = entity.City,
            State = entity.State,
            Country = entity.Country,
            PostalCode = entity.PostalCode,
            Email = entity.Email,
            Phone1 = entity.Phone1,
            Phone2 = entity.Phone2,
            Website = entity.Website,
            LogoUrl = entity.LogoUrl,
            Currency = entity.Currency,
            TimeZone = entity.TimeZone
        };
    }
}