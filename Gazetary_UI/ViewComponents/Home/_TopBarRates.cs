using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _TopBarRates : ViewComponent
{
    private readonly IExchangeRateService _service;

    public _TopBarRates(IExchangeRateService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var data = await _service.GetRatesAsync();
        return View(data);
    }
}