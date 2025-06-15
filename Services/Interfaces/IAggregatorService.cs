using ApiAggregator.Models;

namespace ApiAggregator.Services.Interfaces
{
    public interface IAggregatorService
    {
        Task<AggregatedResponse> GetAggregatedDataAsync(string? category = null);

    }
}
