public class ApiPerformanceStats
{
    public string ApiName { get; set; } = default!;
    public int TotalRequests { get; set; }
    public double AverageResponseTimeMs { get; set; }
    public string PerformanceBucket => AverageResponseTimeMs switch
    {
        < 100 => "Fast",
        < 200 => "Average",
        _ => "Slow"
    };
}
