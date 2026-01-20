namespace Entities.Concrate;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategorySlug { get; set; }
    
    public List<BlogPost> BlogPosts { get; set; }
}