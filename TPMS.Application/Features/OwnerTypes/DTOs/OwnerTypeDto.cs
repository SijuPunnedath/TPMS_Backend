namespace TPMS.Application.Features.OwnerTypes.DTOs;

public class OwnerTypeDto
{
    public int OwnerTypeID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}