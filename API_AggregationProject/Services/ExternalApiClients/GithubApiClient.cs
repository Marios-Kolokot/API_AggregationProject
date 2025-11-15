using API_AggregationProject.Models;
using API_AggregationProject.Models.Requests;
using API_AggregationProject.Services.Interfaces;
using System.Diagnostics;
using System.Text.Json;

namespace API_AggregationProject.Services.ExternalApiClients
{
    public class GithubApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IStatisticsService _stats;
        public string Name => "GitHub";

        public GithubApiClient(HttpClient httpClient, IStatisticsService stats)
        {
            _httpClient = httpClient;
            _stats = stats;
        }

        public Task<IEnumerable<AggregatedItemDto>> FetchAsync(AggregationRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<object> FetchDataAsync(string query)
        {
            var sw = Stopwatch.StartNew();
            var url = $"https://api.github.com/search/repositories?q={query}";
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpApp");
            var response = await _httpClient.GetStringAsync(url);
            sw.Stop();

            _stats.RecordRequest(Name, sw.ElapsedMilliseconds);
            return JsonSerializer.Deserialize<object>(response);
        }
    }
}
