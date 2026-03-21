using Entities.Concrate;

namespace BlogProject.Models;

public class HomeEditorPickViewModel
{
    public List<BlogPost> TodaySliderPosts { get; set; }
    public CategoryWithLatestBlog Category1 { get; set; }
    public CategoryWithLatestBlog Category2 { get; set; }
    public CategoryWithLatestBlog Category3 { get; set; }
    public CategoryWithLatestBlog? Category4 { get; set; }
}

public class CategoryWithLatestBlog
{
    public string CategoryName { get; set; }
    public string CategorySlug { get; set; }
    public List<BlogPost> Posts { get; set; }
}