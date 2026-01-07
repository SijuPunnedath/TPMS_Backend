using System;

namespace TPMS.Application.Features.RentSchedules.DTOs;

public class RentScheduleDtoCrud
{
    public int ScheduleID { get; set; }
    public int LeaseID { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidDate { get; set; }
    public decimal? Penalty { get; set; }
}