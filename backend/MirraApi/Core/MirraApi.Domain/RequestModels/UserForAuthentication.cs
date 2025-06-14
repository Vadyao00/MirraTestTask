using System.ComponentModel.DataAnnotations;

namespace MirraApi.Models.RequestModels;

public record UserForAuthentication
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Password name is required")]
    public string? Password { get; init; }
}