namespace Entities.Concrate;

public class Comment
{
    public int CommentId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string? Analysis { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsStatus { get; set; }
    
    public int BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }
    
    public string? AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public List<CommentLike> Likes { get; set; } = new();
}