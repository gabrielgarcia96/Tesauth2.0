using teste.Domain.Models.Enums;

namespace teste.Application.DTOs;

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public Role Roles { get; set; }
}
