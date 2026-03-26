using Entities.Concrate;

namespace BlogProject.Models;

public class BlogDetailViewModel
{
    public BlogPost BlogPost { get; set; }
    public List<Comment> Comments { get; set; }
    public Comment NewComment { get; set; }
    public Dictionary<int, int> LikeCounts { get; set; } = new();
    public List<int> LikedCommentIds { get; set; } = new();
}