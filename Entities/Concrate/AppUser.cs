using Microsoft.AspNetCore.Identity;

namespace Entities.Concrate;

public class AppUser : IdentityUser
{
    public string? NameSurname { get; set; }
    public string? ImageUrl { get; set; }
    
    public List<Comment> Comments { get; set; }
}