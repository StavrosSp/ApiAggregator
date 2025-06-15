namespace ApiAggregator.Models
{
    public class AggregatedResponse
    {
        public WeatherInfo? Weather { get; set; }
        public List<NewsArticle> News { get; set; } = new();
        public List<GitHubRepo> GitHubRepos { get; set; } = new();
    }

}
