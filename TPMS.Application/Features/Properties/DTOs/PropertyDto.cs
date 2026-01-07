using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;

namespace TPMS.Application.Features.Properties.DTOs
{
    public class PropertyDto
    {
        public int PropertyID { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public int? LandlordID { get; set; }
        public string? LandlordName { get; set; }

        public PropertyAddressDto Address { get; set; } = new PropertyAddressDto();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
