using ApiAggregator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregationController : ControllerBase
    {
        private readonly IAggregatorService _aggregatorService;

        public AggregationController(IAggregatorService aggregatorService)
        {
            _aggregatorService = aggregatorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAggregatedData([FromQuery] string? category = null)
        {
            var result = await _aggregatorService.GetAggregatedDataAsync(category);
            return Ok(result);
        }
    }
}