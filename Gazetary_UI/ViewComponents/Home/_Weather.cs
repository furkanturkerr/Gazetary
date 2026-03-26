using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _Weather : ViewComponent
{
    private readonly IWeatherService _weatherService;

    public _Weather(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var value = await _weatherService.GetIstanbulWeatherAsync();
        return View(value);
    }
}