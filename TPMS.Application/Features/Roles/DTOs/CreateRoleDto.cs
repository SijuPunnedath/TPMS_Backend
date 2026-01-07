namespace TPMS.Application.Features.Roles.DTOs;

public class CreateRoleDto
{
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
}