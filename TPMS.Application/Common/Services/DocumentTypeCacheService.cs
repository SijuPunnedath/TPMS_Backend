using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Common.Services
{
    public class DocumentTypeCacheService : IDocumentTypeCacheService
    {
        // Cache lookup by name → (TypeID, CategoryID, IsActive)
        private readonly Dictionary<string, (int TypeId, int CategoryId, bool IsActive)> _byName = new();

        // Cache lookup by ID → (TypeName, CategoryID, CategoryName, IsActive)
        private readonly Dictionary<int, (string TypeName, int CategoryId, string CategoryName, bool IsActive)> _byId = new();

        public DocumentTypeCacheService(TPMSDBContext db)
        {
            var types = db.DocumentTypes
                .Select(dt => new
                {
                    dt.DocumentTypeID,
                    dt.TypeName,
                    dt.DocumentCategoryID,
                    CategoryName = dt.Category.CategoryName,
                    dt.IsActive
                })
                .ToList();

            foreach (var type in types)
            {
                if (!string.IsNullOrWhiteSpace(type.TypeName))
                {
                    string key = type.TypeName.ToLower();

                    _byName[key] = (type.DocumentTypeID, type.DocumentCategoryID, type.IsActive);

                    _byId[type.DocumentTypeID] = (type.TypeName, type.DocumentCategoryID, type.CategoryName, type.IsActive);
                }
            }
        }

        // ---------------------------
        // Get DocumentTypeID by name
        // ---------------------------
        public int GetDocumentTypeId(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException("Document type name cannot be empty.");

            if (_byName.TryGetValue(typeName.ToLower(), out var data))
                return data.TypeId;

            throw new KeyNotFoundException($"DocumentType '{typeName}' not found in cache.");
        }

        // ---------------------------
        // Get DocumentTypeName by ID
        // ---------------------------
        public string GetDocumentTypeName(int id)
        {
            if (_byId.TryGetValue(id, out var data))
                return data.TypeName;

            throw new KeyNotFoundException($"DocumentType ID '{id}' not found in cache.");
        }

        // ---------------------------
        // Get CategoryID by TypeName
        // ---------------------------
        public int GetCategoryIdByTypeName(string typeName)
        {
            if (_byName.TryGetValue(typeName.ToLower(), out var data))
                return data.CategoryId;

            throw new KeyNotFoundException($"Category for DocumentType '{typeName}' not found.");
        }

        // ---------------------------
        // Get CategoryName by TypeID
        // ---------------------------
        public string GetCategoryNameByTypeId(int typeId)
        {
            if (_byId.TryGetValue(typeId, out var data))
                return data.CategoryName;

            throw new KeyNotFoundException($"CategoryName for DocumentType ID '{typeId}' not found.");
        }

        // ---------------------------
        // Validate if TypeID exists
        // ---------------------------
        public bool IsValidType(int id) => _byId.ContainsKey(id);

        // ---------------------------
        // Validate active type
        // ---------------------------
        public bool IsActiveType(int id)
        {
            return _byId.TryGetValue(id, out var data) && data.IsActive;
        }
    }
}
