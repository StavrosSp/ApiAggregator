using ApiAggregator.Models;
using ApiAggregator.Services.Interfaces;

namespace ApiAggregator.Services.Implementations
{
    public class AggregatorService : IAggregatorService
    {
        private readonly IWeatherService _weather;
        private readonly INewsService _news;
        private readonly IGitHubService _gitHub;

        public AggregatorService(IWeatherService weather, INewsService news, IGitHubService gitHub)
        {
            _weather = weather;
            _news = news;
            _gitHub = gitHub;
        }

        public async Task<AggregatedResponse> GetAggregatedDataAsync(string? category = null)
        {
            var weatherTask = _weather.GetWeatherAsync("Athens");
            var newsTask = _news.GetNewsAsync(category ?? "general");
            var githubTask = _gitHub.GetRepositoriesAsync("dotnet");

            await Task.WhenAll(weatherTask, newsTask, githubTask);

            return new AggregatedResponse
            {
                Weather = weatherTask.Result,
                News = newsTask.Result,
                GitHubRepos = githubTask.Result
            };
        }
    }

}
