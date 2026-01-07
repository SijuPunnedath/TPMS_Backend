using System;

namespace TPMS.Domain.Entities
{
    public class LeaseAlert
    {

        public int AlertID { get; set; }

        // Foreign Key
        public int LeaseID { get; set; }
        public virtual Lease Lease { get; set; } = null!;

        // Type of alert (Reminder, Overdue, LeaseExpiry, Custom, etc.)
        public string AlertType { get; set; } = "Reminder";

        // The date/time when the alert is *scheduled* to trigger
        public DateTime AlertDate { get; set; }

        // The date/time when the alert was actually *sent* (if applicable)
        public DateTime? SentAt { get; set; }

        // "Pending", "Sent", "Failed", "Resolved"
        public string Status { get; set; } = "Pending";

        // Optional message template or note for this alert
        public string? Message { get; set; }

        // Optional: track delivery channels � e.g. Email, SMS, Dashboard
        public string? DeliveryMethod { get; set; }

        // If the system retries sending alerts, log attempt counts
        public int RetryCount { get; set; } = 0;

        // Soft delete flag (optional, if alerts can be archived)
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        
        

    }
}
