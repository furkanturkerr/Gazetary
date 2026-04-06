using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}