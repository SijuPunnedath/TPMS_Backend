using System.Collections.Generic;

namespace TPMS.Application.Features.OwnerTypes.DTOs;

public class OwnerTreeDto
{
    public int OwnerTypeID { get; set; }
    public string OwnerTypeName { get; set; } = string.Empty;
    public List<OwnerNodeDto> Owners { get; set; } = new();
}