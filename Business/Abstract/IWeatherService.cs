using Dtos;

namespace Business.Abstract;

public interface IWeatherService
{
    Task<WeatherDto> GetIstanbulWeatherAsync();
}