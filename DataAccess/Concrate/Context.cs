using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrate;

public class Context : IdentityDbContext<AppUser, AppRole, string>{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1995;Database=BlogProje;User Id=sa;Password=Furkan12*;TrustServerCertificate=True;");
    }
    
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    
}