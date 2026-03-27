using System.Text.Json;
using Business.Abstract;
using Dtos;
using Microsoft.Extensions.Caching.Memory;

public class WeatherManager : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "istanbul_weather";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    public WeatherManager(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<WeatherDto> GetIstanbulWeatherAsync()
    {
        if (_cache.TryGetValue(CacheKey, out WeatherDto? cachedWeather) && cachedWeather is not null)
        {
            return cachedWeather;
        }

        var url = "https://api.open-meteo.com/v1/forecast?latitude=41.01&longitude=28.97&current=temperature_2m&timezone=Europe%2FIstanbul";

        var response = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(response);

        var temperature = doc.RootElement
            .GetProperty("current")
            .GetProperty("temperature_2m")
            .GetDecimal();

        var result = new WeatherDto
        {
            City = "Istanbul",
            Temperature = temperature
        };

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration
        };

        _cache.Set(CacheKey, result, cacheOptions);

        return result;
    }
}