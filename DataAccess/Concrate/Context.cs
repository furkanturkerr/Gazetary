using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrate;

public class Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1995;Database=BlogProje;User Id=sa;Password=Furkan12*;TrustServerCertificate=True;");
    }
    
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Category> Categories { get; set; }
}