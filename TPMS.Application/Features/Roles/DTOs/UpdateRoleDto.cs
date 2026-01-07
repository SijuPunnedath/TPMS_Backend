namespace TPMS.Application.Features.Roles.DTOs;

public class UpdateRoleDto
{
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}