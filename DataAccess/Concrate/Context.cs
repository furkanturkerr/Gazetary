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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // BlogPost indexes for performance
        modelBuilder.Entity<BlogPost>()
            .HasIndex(b => b.CategoryId)
            .HasDatabaseName("IX_BlogPosts_CategoryId");

        modelBuilder.Entity<BlogPost>()
            .HasIndex(b => b.Slug)
            .HasDatabaseName("IX_BlogPosts_Slug");

        modelBuilder.Entity<BlogPost>()
            .HasIndex(b => b.CreatedDate)
            .HasDatabaseName("IX_BlogPosts_CreatedDate");

        modelBuilder.Entity<BlogPost>()
            .HasIndex(b => b.ViewCount)
            .HasDatabaseName("IX_BlogPosts_ViewCount");

        modelBuilder.Entity<BlogPost>()
            .HasIndex(b => b.Status)
            .HasDatabaseName("IX_BlogPosts_Status");

        // Category indexes
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.CategorySlug)
            .IsUnique()
            .HasDatabaseName("IX_Categories_CategorySlug");

        // Comment indexes
        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.BlogPostId)
            .HasDatabaseName("IX_Comments_BlogPostId");

        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.AppUserId)
            .HasDatabaseName("IX_Comments_AppUserId");

        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.IsStatus)
            .HasDatabaseName("IX_Comments_IsStatus");

        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.CreatedDate)
            .HasDatabaseName("IX_Comments_CreatedDate");

        // Cascade delete: Yorum silindiğinde like'ları da sil
        modelBuilder.Entity<CommentLike>()
            .HasOne(cl => cl.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        // NoAction: SQL Server'da multiple cascade path hatasını önlemek için
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.AppUser)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AppUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CommentLike>()
            .HasOne(cl => cl.AppUser)
            .WithMany()
            .HasForeignKey(cl => cl.AppUserId)
            .OnDelete(DeleteBehavior.NoAction);

        // CommentLike indexes
        modelBuilder.Entity<CommentLike>()
            .HasIndex(cl => cl.CommentId)
            .HasDatabaseName("IX_CommentLikes_CommentId");

        modelBuilder.Entity<CommentLike>()
            .HasIndex(cl => new { cl.CommentId, cl.AppUserId })
            .IsUnique()
            .HasDatabaseName("IX_CommentLikes_CommentId_AppUserId");
    }
}