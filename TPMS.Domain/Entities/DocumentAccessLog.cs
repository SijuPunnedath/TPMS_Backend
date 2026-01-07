using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class DocumentAccessLog
    {
        public int LogID { get; set; }                    // PK

        public int DocumentID { get; set; }               // FK → Document
        public string? AccessedBy { get; set; }           // Username / UserID of accessor
        public DateTime AccessedAt { get; set; }          // UTC timestamp

        public string AccessType { get; set; } = "View";  // e.g., View, Download, Upload, Delete
        public string? IPAddress { get; set; }            // Optional: user’s IP for traceability
        public string? Device { get; set; }               // Optional: browser, mobile, API client

        public string? Notes { get; set; }                // Extra info like reason or remarks

        // Navigation
        public virtual Document? Document { get; set; }
    }
}
