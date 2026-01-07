using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Common.Services
{
    public class OwnerTypeCacheService : IOwnerTypeCacheService
    {
        private  Dictionary<string, int> _ownerTypeIds = new();
        private readonly Dictionary<int, string> _byId;
        private readonly TPMSDBContext _db;

        public OwnerTypeCacheService(TPMSDBContext db)
        {
            _db = db;

            //-- Load IDs once on startup
            RefreshCache();
        }

        public int GetOwnerTypeId(string typeName)
        {
            if (_ownerTypeIds.TryGetValue(typeName, out int id))
                return id;

            throw new KeyNotFoundException($"OwnerType '{typeName}' not found in cache.");
        }
        
        public string GetOwnerTypeName(int id)
        {
            if (_byId.TryGetValue(id, out string? name))
                return name;

            throw new KeyNotFoundException($"OwnerType ID '{id}' not found.");
        }

        public void RefreshCache()
        {
            _ownerTypeIds = _db.OwnerTypes
                .Where(o => o.IsActive && o.Name != null)
                .ToDictionary(o => o.Name!, o => o.OwnerTypeID);
        }
    }
}
