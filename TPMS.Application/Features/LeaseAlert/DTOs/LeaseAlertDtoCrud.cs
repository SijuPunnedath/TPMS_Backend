using System;
using ClosedXML.Excel;

namespace TPMS.Application.Features.LeaseAlert.DTOs;

public class LeaseAlertDtoCrud
{
    public int AlertID { get; set; }
    public int LeaseID { get; set; }
    public string AlertType { get; set; } = "Reminder";
    public DateTime AlertDate { get; set; }
    public DateTime? SentAt { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Message { get; set; }
    public string? DeliveryMethod { get; set; }
    public int RetryCount { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}