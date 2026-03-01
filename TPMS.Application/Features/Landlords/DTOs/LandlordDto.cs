using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Landlords.DTOs
{
    public class LandlordDto
    {
        public int LandlordID { get; set; }
        
        public string LandlordNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public LandlordAddressDto LandlordAddress { get; set; } = new LandlordAddressDto();
    }
}
