using System.Diagnostics;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiStatsTracker _tracker;

    public RequestTimingMiddleware(RequestDelegate next, ApiStatsTracker tracker)
    {
        _next = next;
        _tracker = tracker;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await _next(context);
        stopwatch.Stop();

        if (context.Items.TryGetValue("ApiName", out var apiName))
        {
            _tracker.Track(apiName.ToString()!, stopwatch.ElapsedMilliseconds);
        }
    }
}
