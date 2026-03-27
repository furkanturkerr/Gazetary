using Dtos;

namespace Business.Abstract;

public interface IRssService
{
    Task<List<RssNewsDto>> GetNewsFromFeedAsync(string rssUrl);
    List<RssSourceDto> GetRssSources();
}