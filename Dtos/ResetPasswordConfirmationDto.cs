using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class ResetPasswordConfirmationDto
{
    [Required(ErrorMessage = "Doğrulama kodu zorunludur.")]
    [Range(100000, 999999, ErrorMessage = "Geçerli bir 6 haneli kod giriniz.")]
    public int Code { get; set; }
}