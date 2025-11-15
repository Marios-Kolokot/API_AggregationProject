using API_AggregationProject.Models.Responses;
using API_AggregationProject.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace API_AggregationProject.Services
{
    public class ApiAggregatorService : IApiAggregatorService
    {
        private readonly IEnumerable<IExternalApiClient> _apiClients;
        private readonly IMemoryCache _cache;

        public ApiAggregatorService(IEnumerable<IExternalApiClient> apiClients, IMemoryCache cache)
        {
            _apiClients = apiClients;
            _cache = cache;
        }
        public async Task<AggregatedResponse> GetAggregatedDataAsync(string query, string sortBy, string filterBy)
        {
            var tasks = _apiClients.Select(c => c.FetchDataAsync(query));
            var results = await Task.WhenAll(tasks);

            return new AggregatedResponse
            {
                Sources = _apiClients.Select(c => c.Name).ToList(),
                Data = results.ToList()
            };
        }


    }
}
