using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrate;

public class Context : IdentityDbContext<AppUser, AppRole, string>{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Image> Images { get; set; }
    
}