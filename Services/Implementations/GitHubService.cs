using ApiAggregator.Models;
using ApiAggregator.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ApiAggregator.Services.Implementations
{
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public GitHubService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<GitHubRepo>> GetRepositoriesAsync(string username)
        {
            var cacheKey = $"github-{username.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out List<GitHubRepo> cached))
                return cached;

            var url = $"https://api.github.com/users/{username}/repos";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.ParseAdd("ApiAggregatorApp"); // GitHub API requires User-Agent

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<List<JsonElement>>(json);

                var repos = data?
                    .Select(r => new GitHubRepo
                    {
                        Name = r.GetProperty("name").GetString() ?? "",
                        Description = r.GetProperty("description").GetString() ?? "",
                        Url = r.GetProperty("html_url").GetString() ?? "",
                        Stars = r.GetProperty("stargazers_count").GetInt32()
                    })
                    .ToList() ?? new();

                _cache.Set(cacheKey, repos, TimeSpan.FromMinutes(10));
                return repos;
            }
            catch
            {
                return new List<GitHubRepo>();
            }
        }
    }

}
