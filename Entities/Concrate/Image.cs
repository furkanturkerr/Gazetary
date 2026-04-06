namespace Entities.Concrate;

public class Image
{
    public int ImageId { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}