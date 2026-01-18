namespace Entities.Concrate;

public class BlogPost
{
    public int BlogPostId { get; set; }   
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public string Slug { get; set; }   
    
    public string ImageDescription { get; set; }
    
    public string Content { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
