using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Common.Interfaces
{
    public interface IDocumentTypeCacheService
    {
        // ---------------------------
        // DocumentType ID ↔ Name
        // ---------------------------

        /// <summary>
        /// Returns the DocumentTypeID for a given TypeName.
        /// Example: "KYC Form" → 3
        /// </summary>
        int GetDocumentTypeId(string typeName);

        /// <summary>
        /// Returns the TypeName for a given DocumentTypeID.
        /// Example: 3 → "KYC Form"
        /// </summary>
        string GetDocumentTypeName(int id);


        // ---------------------------
        // Category lookups
        // ---------------------------

        /// <summary>
        /// Returns the CategoryID for a given DocumentTypeName.
        /// Example: "Agreement" → CategoryID for Agreements
        /// </summary>
        int GetCategoryIdByTypeName(string typeName);

        /// <summary>
        /// Returns the Category name for a given DocumentTypeID.
        /// Example: TypeID 4 → "KYC Documents"
        /// </summary>
        string GetCategoryNameByTypeId(int typeId);


        // ---------------------------
        // Validation
        // ---------------------------

        /// <summary>
        /// Returns TRUE if the DocumentTypeID exists in cache.
        /// </summary>
        bool IsValidType(int id);

        /// <summary>
        /// Returns TRUE only if the type is active.
        /// </summary>
        bool IsActiveType(int id);
    }
}
