namespace Dtos;

public class RssAdminPageDto
{
    public List<RssSourceDto> Sources { get; set; } = new();
    public List<RssNewsDto> News { get; set; } = new();
    public int SelectedSourceId { get; set; }
    public string SelectedSourceName { get; set; }
    public string SelectedSourceCategory { get; set; }
}