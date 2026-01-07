namespace TPMS.Application.Features.Users.DTOs;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // plain text (will be hashed)
    public int RoleID { get; set; }
}