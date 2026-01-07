using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TPMS.Application.Features.Lookups.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Lookups.Services;

public class LookupCacheService : ILookupCacheService
{
    private readonly TPMSDBContext _db;
    private readonly IMemoryCache _cache;

    private const string LANDLORD_CACHE = "LANDLORD_LOOKUP";
    private const string TENANT_CACHE = "TENANT_LOOKUP";
    private const string OWNER_TYPE_CACHE = "OWNER_TYPE_LOOKUP";
    
    public LookupCacheService(TPMSDBContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }
    
    private readonly MemoryCacheEntryOptions cacheOptions =
        new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
            SlidingExpiration = TimeSpan.FromHours(2)
        };
    
    public async Task<List<LandlordLookupDto>> GetLandlordsAsync()
    {
        if (!_cache.TryGetValue(LANDLORD_CACHE, out List<LandlordLookupDto>? list))
        {
            list = await LoadLandlordLookup();
            _cache.Set(LANDLORD_CACHE, list, TimeSpan.FromMinutes(10));
        }
        return list!;
    }
    
    public async Task<List<TenantLookupDto>> GetTenantsAsync()
    {
        if (!_cache.TryGetValue(TENANT_CACHE, out List<TenantLookupDto>? list))
        {
            list = await LoadTenantLookup();
            _cache.Set(TENANT_CACHE, list, TimeSpan.FromMinutes(10));
        }
        return list!;
    }
    
    public async Task<List<OwnerTypeLookupDto>> GetOwnerTypesAsync()
    {
        if (!_cache.TryGetValue(OWNER_TYPE_CACHE, out List<OwnerTypeLookupDto>? list))
        {
            list = await LoadOwnerTypeLookup();
            _cache.Set(OWNER_TYPE_CACHE, list, TimeSpan.FromMinutes(10));
        }
        return list!;
    }
    
    
    
    // ---------- REFRESH METHODS ----------
    public async Task RefreshLandlordsAsync()
    {
        var data = await LoadLandlordLookup();
        _cache.Set(LANDLORD_CACHE, data, TimeSpan.FromMinutes(10));
    }

    public async Task RefreshTenantsAsync()
    {
        var data = await LoadTenantLookup();
        _cache.Set(TENANT_CACHE, data, TimeSpan.FromMinutes(10));
    }

    public async Task RefreshOwnerTypesAsync()
    {
        var data = await LoadOwnerTypeLookup();
        _cache.Set(OWNER_TYPE_CACHE, data, TimeSpan.FromMinutes(10));
    }
    // ---------- LOAD DATA FROM DB ----------
    private async Task<List<LandlordLookupDto>> LoadLandlordLookup()
    {
        return await _db.Landlords
            .Where(x => !x.IsDeleted)
            .Select(x => new LandlordLookupDto
            {
                LandlordID = x.LandlordID,
                Name = x.Name
            }).ToListAsync();
    }

    private async Task<List<TenantLookupDto>> LoadTenantLookup()
    {
        return await _db.Tenants
            .Where(x => !x.IsDeleted)
            .Select(x => new TenantLookupDto
            {
                TenantID = x.TenantID,
                Name = x.Name
            }).ToListAsync();
    }
    
    private async Task<List<OwnerTypeLookupDto>> LoadOwnerTypeLookup()
    {
        return await _db.OwnerTypes
            .Where(x => x.IsActive)
            .Select(x => new OwnerTypeLookupDto
            {
                OwnerTypeID = x.OwnerTypeID,
                Name = x.Name
            }).ToListAsync();
    }
}