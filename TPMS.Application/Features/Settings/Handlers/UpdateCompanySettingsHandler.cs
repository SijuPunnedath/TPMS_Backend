using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Settings.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Settings.Handlers;

public class UpdateCompanySettingsHandler : IRequestHandler<UpdateCompanySettingsCommand, bool>
{
    private readonly TPMSDBContext _db;
    public UpdateCompanySettingsHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(UpdateCompanySettingsCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CompanySettings;
        var entity = await _db.CompanySettings.FirstOrDefaultAsync(c => c.CompanyID == dto.CompanyID, cancellationToken);
        if (entity == null) return false;

        entity.CompanyName = dto.CompanyName;
        entity.RegistrationNumber = dto.RegistrationNumber;
        entity.TaxID = dto.TaxID;
        entity.AddressLine1 = dto.AddressLine1;
        entity.AddressLine2 = dto.AddressLine2;
        entity.City = dto.City;
        entity.State = dto.State;
        entity.Country = dto.Country;
        entity.PostalCode = dto.PostalCode;
        entity.Email = dto.Email;
        entity.Phone1 = dto.Phone1;
        entity.Phone2 = dto.Phone2;
        entity.Website = dto.Website;
        entity.LogoUrl = dto.LogoUrl;
        entity.Currency = dto.Currency;
        entity.TimeZone = dto.TimeZone;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }   
}