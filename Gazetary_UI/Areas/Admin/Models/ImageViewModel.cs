using System.ComponentModel.DataAnnotations;

namespace BlogProject.Areas.Admin.Models;

public class ImageViewModel
{
    public int ImagesId { get; set; }
    
    [Required(ErrorMessage = "Lütfen bir görsel seçin")]
    public IFormFile Image { get; set; }
    public string? ImagePath { get; set; }
}