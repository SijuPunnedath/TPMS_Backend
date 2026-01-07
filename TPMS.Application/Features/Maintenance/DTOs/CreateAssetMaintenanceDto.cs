using System;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Maintenance.DTOs;

public class CreateAssetMaintenanceDto
{
    public int AssetId { get; set; }
    public MaintenanceType MaintenanceType { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string Description { get; set; }
    public decimal? Cost { get; set; }
    public DateTime? NextDueDate { get; set; }
}