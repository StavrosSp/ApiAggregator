using ApiAggregator.Models;
using ApiAggregator.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ApiAggregator.Services.Implementations
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;

        public WeatherService(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _config = config;
            _cache = cache;
        }

        public async Task<WeatherInfo?> GetWeatherAsync(string city)
        {
            var cacheKey = $"weather-{city.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out WeatherInfo cached))
                return cached;

            var apiKey = _config["OpenWeather:ApiKey"];
            var url = $"{_config["OpenWeather:BaseUrl"]}/weather?q={city}&appid={apiKey}&units=metric";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var weather = new WeatherInfo
                {
                    City = root.GetProperty("name").GetString() ?? city,
                    Description = root.GetProperty("weather")[0].GetProperty("description").GetString() ?? "",
                    TemperatureCelsius = root.GetProperty("main").GetProperty("temp").GetDouble()
                };

                _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10)); // Cache για 10 λεπτά
                return weather;
            }
            catch
            {
                // fallback (π.χ. null ή dummy data)
                return null;
            }
        }
    }

}
