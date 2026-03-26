using Entities.Concrate;

namespace BlogProject.Models;

public class CategoryBlogsViewModel
{
    public BlogPost LatestPost { get; set; }
    public List<BlogPost> OtherPosts { get; set; }
}