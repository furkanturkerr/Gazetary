using System.ComponentModel.DataAnnotations;

namespace BlogProject.Dtos;

public class RegisterDto
{
    [Required]
    public string NameSurname { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    public string? ImageUrl { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }

    [Required, Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    public string ConfirmPassword { get; set; }
}