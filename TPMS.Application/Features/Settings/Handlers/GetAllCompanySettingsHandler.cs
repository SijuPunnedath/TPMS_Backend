using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Settings.DTOs;
using TPMS.Application.Features.Settings.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Settings.Handlers;

public class GetAllCompanySettingsHandler : IRequestHandler<GetAllCompanySettingsQuery, List<CompanySettingsDto>>
{
    private readonly TPMSDBContext _db;
    public GetAllCompanySettingsHandler(TPMSDBContext db) => _db = db;

    public async Task<List<CompanySettingsDto>> Handle(GetAllCompanySettingsQuery request, CancellationToken cancellationToken)
    {
        return await _db.CompanySettings
            .Select(c => new CompanySettingsDto
            {
                CompanyID = c.CompanyID,
                CompanyName = c.CompanyName,
                RegistrationNumber = c.RegistrationNumber,
                TaxID = c.TaxID,
                AddressLine1 = c.AddressLine1,
                AddressLine2 = c.AddressLine2,
                City = c.City,
                State = c.State,
                Country = c.Country,
                PostalCode = c.PostalCode,
                Email = c.Email,
                Phone1 = c.Phone1,
                Phone2 = c.Phone2,
                Website = c.Website,
                LogoUrl = c.LogoUrl,
                Currency = c.Currency,
                TimeZone = c.TimeZone
            })
            .ToListAsync(cancellationToken);
    } 
}