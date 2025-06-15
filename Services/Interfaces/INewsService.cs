using ApiAggregator.Models;

namespace ApiAggregator.Services.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsArticle>> GetNewsAsync(string category = "general");
    }

}
