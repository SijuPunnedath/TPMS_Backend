using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Addresses.DTOs
{
    public class AddressDto
    {
        public int AddressID { get; set; }
        public int OwnerTypeID { get; set; }
        public int OwnerID { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }

        public bool IsPrimary { get; set; } = true;
    }
}
