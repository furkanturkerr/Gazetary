using System.Globalization;
using System.Xml.Linq;
using Business.Abstract;
using Dtos;
using Microsoft.Extensions.Caching.Memory;

public class ExchangeRateManager : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "tcmb_rates";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public ExchangeRateManager(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<ExchangeRateDto> GetRatesAsync()
    {
        if (_cache.TryGetValue(CacheKey, out ExchangeRateDto? cachedRates) && cachedRates is not null)
        {
            return cachedRates;
        }

        var xml = await _httpClient.GetStringAsync("https://www.tcmb.gov.tr/kurlar/today.xml");
        var doc = XDocument.Parse(xml);

        var result = new ExchangeRateDto
        {
            USD = GetCurrency(doc, "USD"),
            EUR = GetCurrency(doc, "EUR")
        };

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration
        };

        _cache.Set(CacheKey, result, cacheOptions);

        return result;
    }

    private decimal GetCurrency(XDocument doc, string code)
    {
        var currency = doc.Descendants("Currency")
            .FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == code);

        var value = currency?.Element("ForexSelling")?.Value;

        if (string.IsNullOrWhiteSpace(value))
            return 0;

        value = value.Replace(",", ".");

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
            return parsedValue;

        return 0;
    }
}