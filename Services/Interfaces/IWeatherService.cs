using ApiAggregator.Models;

namespace ApiAggregator.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherInfo?> GetWeatherAsync(string city);
    }
}
