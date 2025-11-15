using API_AggregationProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_AggregationProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AggregationController : ControllerBase
    {

        private readonly IApiAggregatorService _aggregatorService;

        public AggregationController(IApiAggregatorService aggregator)
        {
            _aggregatorService = aggregator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string query = "Athens", string sortBy = null, string filterBy = null)
        {
            var result = await _aggregatorService.GetAggregatedDataAsync(query, sortBy, filterBy);
            return Ok(result);
        }

        [HttpGet("aggregate")]
        public async Task<IActionResult> Aggregate([FromQuery] string q = "Mario")
        {
            var result = await _aggregatorService.GetAggregatedDataAsync(q, null, null);
            return Ok(result);
        }
    }
}
