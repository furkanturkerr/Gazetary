using System.Xml.Linq;
using Business.Abstract;
using Dtos;
using Microsoft.Extensions.Caching.Memory;

public class ExchangeRateManager : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public ExchangeRateManager(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<ExchangeRateDto> GetRatesAsync()
    {
        // CACHE (çok önemli)
        if (_cache.TryGetValue("tcmb_rates", out ExchangeRateDto cached))
            return cached;

        var xml = await _httpClient.GetStringAsync("https://www.tcmb.gov.tr/kurlar/today.xml");

        var doc = XDocument.Parse(xml);

        decimal usd = GetCurrency(doc, "USD");
        decimal eur = GetCurrency(doc, "EUR");

        var result = new ExchangeRateDto
        {
            USD = usd,
            EUR = eur
        };

        _cache.Set("tcmb_rates", result, TimeSpan.FromMinutes(30));

        return result;
    }

    private decimal GetCurrency(XDocument doc, string code)
    {
        var currency = doc.Descendants("Currency")
            .FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);

        var value = currency?.Element("ForexSelling")?.Value;

        if (string.IsNullOrEmpty(value))
            return 0;

        // TR format → decimal çevir
        return decimal.Parse(value.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
    }
}