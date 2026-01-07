using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.DocumentTypes.DTOs
{
    public class DocumentTypeDto
    {
        public int DocumentTypeID { get; set; }
        public int DocumentCategoryID { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? CategoryName { get; set; }
    }
}
