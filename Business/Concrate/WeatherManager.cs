using System.Text.Json;
using Business.Abstract;
using Dtos;

public class WeatherManager : IWeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherDto> GetIstanbulWeatherAsync()
    {
        var url = "https://api.open-meteo.com/v1/forecast?latitude=41.01&longitude=28.97&current=temperature_2m&timezone=Europe%2FIstanbul";

        var response = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(response);

        var temperature = doc.RootElement
            .GetProperty("current")
            .GetProperty("temperature_2m")
            .GetDecimal();

        return new WeatherDto
        {
            City = "Istanbul",
            Temperature = temperature
        };
    }
}