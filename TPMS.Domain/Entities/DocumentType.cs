using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class DocumentType
    {
        
        
        public int DocumentTypeID { get; set; }
        public int DocumentCategoryID { get; set; }

        public string TypeName { get; set; } = string.Empty;  // Agreement, Invoice, KYC Form
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual DocumentCategory? Category { get; set; }

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        
       
    }
}
