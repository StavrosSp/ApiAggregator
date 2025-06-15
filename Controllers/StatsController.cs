using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly ApiStatsTracker _tracker;

    public StatsController(ApiStatsTracker tracker)
    {
        _tracker = tracker;
    }

    [HttpGet]
    public IActionResult GetStats()
    {
        return Ok(_tracker.GetStats());
    }
}
