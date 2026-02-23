using Entities.Concrate;

namespace BlogProject.Models;

public class BlogDetailViewModel
{
    public List<Comment> Comments { get; set; }
    public BlogPost BlogPost { get; set; }
    public Comment NewComment { get; set; }
}