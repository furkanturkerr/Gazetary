namespace BlogProject.Models;

public class HomeMostReadViewModel
{
    public List<Entities.Concrate.BlogPost> MostReadPosts { get; set; } = new();
    public List<CategorySummary> Categories { get; set; } = new();
}

public class CategorySummary
{
    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public int BlogCount { get; set; }
}