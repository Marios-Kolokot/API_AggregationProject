using API_AggregationProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_AggregationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IStatisticsService _stats;

        public StatsController(IStatisticsService stats)
        {
            _stats = stats;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stats = await _stats.GetStatisticsAsync();
            return Ok(stats);
        }


    }
}
