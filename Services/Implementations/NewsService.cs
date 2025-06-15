using ApiAggregator.Models;
using ApiAggregator.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ApiAggregator.Services.Implementations
{
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;

        public NewsService(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _config = config;
            _cache = cache;
        }

        public async Task<List<NewsArticle>> GetNewsAsync(string category = "general")
        {
            var cacheKey = $"news-{category.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out List<NewsArticle> cached))
                return cached;

            var apiKey = _config["NewsApi:ApiKey"];
            var url = $"{_config["NewsApi:BaseUrl"]}/top-headlines?country=us&category={category}&apiKey={apiKey}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var articlesJson = doc.RootElement.GetProperty("articles");

                var articles = new List<NewsArticle>();

                foreach (var item in articlesJson.EnumerateArray())
                {
                    var article = new NewsArticle
                    {
                        Title = item.GetProperty("title").GetString() ?? "",
                        Description = item.GetProperty("description").GetString() ?? "",
                        Url = item.GetProperty("url").GetString() ?? "",
                        Source = item.GetProperty("source").GetProperty("name").GetString() ?? "",
                        PublishedAt = item.GetProperty("publishedAt").GetDateTime()
                    };
                    articles.Add(article);
                }

                _cache.Set(cacheKey, articles, TimeSpan.FromMinutes(5)); // Cache για 5 λεπτά
                return articles;
            }
            catch
            {
                // fallback
                return new List<NewsArticle>();
            }
        }
    }

}
