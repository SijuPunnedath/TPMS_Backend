using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities;

public class AssetMaintenance
{
    public int AssetMaintenanceId { get; set; }

    public int AssetId { get; set; }
    public Asset Asset { get; set; }

    public MaintenanceType MaintenanceType { get; set; }

    public DateTime MaintenanceDate { get; set; }
    public string Description { get; set; }

    public decimal? Cost { get; set; }
    public DateTime? NextDueDate { get; set; }
}
