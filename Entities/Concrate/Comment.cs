namespace Entities.Concrate;

public class Comment
{
    public int CommentId { get; set; }

    public string Name { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }

    public int BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }
}