using System.Collections.Concurrent;

public class ApiStatsTracker
{
    private readonly ConcurrentDictionary<string, List<long>> _stats = new();

    public void Track(string apiName, long responseTimeMs)
    {
        _stats.AddOrUpdate(apiName,
            _ => new List<long> { responseTimeMs },
            (_, list) => { lock (list) list.Add(responseTimeMs); return list; });
    }

    public IEnumerable<ApiPerformanceStats> GetStats()
    {
        return _stats.Select(pair =>
        {
            var times = pair.Value;
            double avg = 0;
            lock (times) avg = times.Average();
            return new ApiPerformanceStats
            {
                ApiName = pair.Key,
                TotalRequests = times.Count,
                AverageResponseTimeMs = avg
            };
        });
    }
}

