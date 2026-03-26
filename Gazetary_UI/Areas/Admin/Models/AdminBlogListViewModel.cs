namespace BlogProject.Areas.Admin.Models;


public class AdminBlogListViewModel
{
    public List<Entities.Concrate.BlogPost> Posts { get; set; } = new();
    public List<Entities.Concrate.Category> Categories { get; set; } = new();
    public string StatusFilter { get; set; } = "all";
    public string SearchQuery { get; set; } = string.Empty;
    public int CategoryFilter { get; set; } = 0;
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 15;
    public int TotalCount { get; set; }
    public int PublishedCount { get; set; }
    public int DraftCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}