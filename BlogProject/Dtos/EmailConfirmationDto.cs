using System.ComponentModel.DataAnnotations;

namespace BlogProject.Dtos;

public class EmailConfirmationDto
{
    [Required]
    [Range(100000, 999999, ErrorMessage = "Geçerli bir kod girin.")]
    public int Code { get; set; }
}