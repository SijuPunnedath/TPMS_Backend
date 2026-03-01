namespace TPMS.Domain.Enums;

public enum PropertyStatus
{
    Draft = 0,
    Vacant=1,          // No outbound tenant
    Occupied =2,        // Has active outbound lease
    UnderMaintenance=3,
    Blocked=4,
    Reserved=5
   
    
}