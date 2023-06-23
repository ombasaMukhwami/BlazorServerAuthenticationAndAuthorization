using System.ComponentModel.DataAnnotations;

namespace BlazorServerAuthenticationAndAuthorization.Shared;

public class LoginRequest
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
