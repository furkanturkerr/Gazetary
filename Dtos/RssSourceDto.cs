namespace Dtos;

public class RssSourceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Url { get; set; }

    public RssSourceDto(int id, string name, string category, string url)
    {
        Id = id;
        Name = name;
        Category = category;
        Url = url;
    }
}