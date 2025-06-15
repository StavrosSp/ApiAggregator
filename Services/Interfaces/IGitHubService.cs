using ApiAggregator.Models;

namespace ApiAggregator.Services.Interfaces
{
    public interface IGitHubService
    {
        Task<List<GitHubRepo>> GetRepositoriesAsync(string username);
    }

}
