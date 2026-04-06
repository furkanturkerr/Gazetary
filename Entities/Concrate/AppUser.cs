using Microsoft.AspNetCore.Identity;

namespace Entities.Concrate;

public class AppUser : IdentityUser
{
    public string? NameSurname { get; set; }
    public string? ImageUrl { get; set; }
    public int? MailCode { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime? PasswordResetRequestedAt { get; set; }
    public List<Comment> Comments { get; set; }
}