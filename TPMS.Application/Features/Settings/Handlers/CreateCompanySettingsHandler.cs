using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Settings.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Settings.Handlers;

public class CreateCompanySettingsHandler : IRequestHandler<CreateCompanySettingsCommand, int>
{
    private readonly TPMSDBContext _db;
    public CreateCompanySettingsHandler(TPMSDBContext db) => _db = db;

    public async Task<int> Handle(CreateCompanySettingsCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CompanySettings;

        var entity = new CompanySetting
        {
            CompanyName = dto.CompanyName,
            RegistrationNumber = dto.RegistrationNumber,
            TaxID = dto.TaxID,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            PostalCode = dto.PostalCode,
            Email = dto.Email,
            Phone1 = dto.Phone1,
            Phone2 = dto.Phone2,
            Website = dto.Website,
            LogoUrl = dto.LogoUrl,
            Currency = dto.Currency,
            TimeZone = dto.TimeZone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.CompanySettings.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.CompanyID;
    }   
}