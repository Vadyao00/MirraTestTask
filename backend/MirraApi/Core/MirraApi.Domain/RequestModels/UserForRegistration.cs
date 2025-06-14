using System.ComponentModel.DataAnnotations;

namespace MirraApi.Models.RequestModels;

public record UserForRegistration
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; init; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }
}