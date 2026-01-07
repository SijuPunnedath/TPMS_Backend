using System.Collections.Generic;
using System.Threading.Tasks;
using TPMS.Application.Features.Lookups.DTOs;

namespace TPMS.Application.Features.Lookups.Services;

public interface ILookupCacheService
{
    Task<List<LandlordLookupDto>> GetLandlordsAsync();
    Task<List<TenantLookupDto>> GetTenantsAsync();
    Task<List<OwnerTypeLookupDto>> GetOwnerTypesAsync();
    
    //Task<List<PropertyLookupDto>> GetPropertiesAsync();
    //Task<List<RoleLookupDto>> GetRolesAsync();
    //Task<List<PermissionLookupDto>> GetPermissionsAsync();
    //Task<List<DocumentTypeLookupDto>> GetDocumentTypesAsync();
    //Task<List<PenaltyPolicyLookupDto>> GetPenaltyPoliciesAsync();

    //Task RefreshAllAsync();

    // Refresh after create/update/delete
    Task RefreshLandlordsAsync();
    Task RefreshTenantsAsync();
    Task RefreshOwnerTypesAsync();
}