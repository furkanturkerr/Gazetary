namespace Entities.Concrate;

public class CommentLike
{
    public int CommentLikeId { get; set; }
    
    public int CommentId { get; set; }
    public Comment Comment { get; set; }
    
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}