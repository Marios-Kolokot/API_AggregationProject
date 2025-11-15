using API_AggregationProject.Config;
using API_AggregationProject.Models;
using API_AggregationProject.Models.Requests;
using API_AggregationProject.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Text.Json;

namespace API_AggregationProject.Services.ExternalApiClients
{
    public class NewsApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IStatisticsService _stats;
        private readonly IMemoryCache _cache;
        private readonly NewsApiConfig _config;
        public string Name => "NewsAPI";

        private const string ApiKey = "YOUR_NEWS_API_KEY";

        public NewsApiClient(HttpClient httpClient, IStatisticsService stats, IMemoryCache cache, NewsApiConfig config)
        {
            _httpClient = httpClient;
            _stats = stats;
            _cache = cache;
            _config = config;
        }

        public Task<IEnumerable<AggregatedItemDto>> FetchAsync(AggregationRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<object> FetchDataAsync(string query)
        {
            //var sw = Stopwatch.StartNew();
            //var url = $"https://newsapi.org/v2/everything?q={query}&apiKey={ApiKey}";
            //var response = await _httpClient.GetStringAsync(url);
            //sw.Stop();

            //_stats.RecordRequest(Name, sw.ElapsedMilliseconds);
            //return JsonSerializer.Deserialize<object>(response);

            var cacheKey = $"{Name}:{query}";

            if (_cache.TryGetValue(cacheKey, out object cached))
                return cached;

            if (string.IsNullOrWhiteSpace(_config.ApiKey))
                return new { source = Name, error = "Missing API key" };

            var sw = Stopwatch.StartNew();

            try
            {
                var url = $"https://newsapi.org/v2/everything?q={query}&language=en&apiKey={_config.ApiKey}";

                var json = await _httpClient.GetStringAsync(url);

                sw.Stop();
                _stats.RecordRequest(Name, sw.ElapsedMilliseconds);

                var obj = JsonSerializer.Deserialize<object>(json);
                _cache.Set(cacheKey, obj, TimeSpan.FromSeconds(30));

                return obj!;
            }
            catch (Exception)
            {
                sw.Stop();
                _stats.RecordRequest(Name, sw.ElapsedMilliseconds);

                return new { source = Name, error = "Failed to fetch news" };
            }


        }
    }
}
