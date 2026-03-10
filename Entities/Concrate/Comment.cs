namespace Entities.Concrate;

public class Comment
{
    public int CommentId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsStatus { get; set; }
    
    public int BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }
    
    public string? AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public int? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; }
    public List<Comment> Replies { get; set; } = new();

    public List<CommentLike> Likes { get; set; } = new();
}