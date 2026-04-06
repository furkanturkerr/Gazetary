using Business.Abstract;
using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class RssController : Controller
{
    private readonly IRssService _rssService;

    public RssController(IRssService rssService)
    {
        _rssService = rssService;
    }

    [HttpGet]
    public async Task<IActionResult> RssList(int? id)
    {
        var sources = _rssService.GetRssSources();

        var selectedSource = id.HasValue
            ? sources.FirstOrDefault(x => x.Id == id.Value)
            : sources.FirstOrDefault();

        var news = new List<RssNewsDto>();

        if (selectedSource != null)
        {
            news = await _rssService.GetNewsFromFeedAsync(selectedSource.Url);
        }

        var model = new RssAdminPageDto
        {
            Sources = sources,
            News = news,
            SelectedSourceId = selectedSource?.Id ?? 0,
            SelectedSourceName = selectedSource?.Name,
            SelectedSourceCategory = selectedSource?.Category
        };

        return View(model);
    }
}