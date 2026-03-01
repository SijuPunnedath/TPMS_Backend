using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Landlords.DTOs;

namespace TPMS.Application.Features.Tenants.DTOs
{
    public class TenantDto
    {
        public int TenantID { get; set; }
        public string TenantNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public List<TenantAddressDto> Addresses { get; set; } = new();
       
        // public AddressDto Address { get; set; } = new AddressDto();
    }
}

