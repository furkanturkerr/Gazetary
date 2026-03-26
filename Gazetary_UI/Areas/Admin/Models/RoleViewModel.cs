using System.ComponentModel.DataAnnotations;

namespace BlogProject.Areas.Admin.Models;

public class RoleViewModel
{
    [Required(ErrorMessage = "Lütfen rol adını girin.")]
    public string Name { get; set; }
}