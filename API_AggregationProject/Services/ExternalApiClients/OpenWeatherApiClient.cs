using API_AggregationProject.Models;
using API_AggregationProject.Models.Requests;
using API_AggregationProject.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Text.Json;

namespace API_AggregationProject.Services.ExternalApiClients
{
    public class OpenWeatherApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IStatisticsService _stats;
        private readonly IMemoryCache _cache;
        public string Name => "OpenWeather";
        private const string ApiKey = "YOUR_OPENWEATHER_API_KEY";

        public OpenWeatherApiClient(HttpClient httpClient, IStatisticsService stats, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _stats = stats;
            _cache = cache;
        }
        public Task<IEnumerable<AggregatedItemDto>> FetchAsync(AggregationRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<object> FetchDataAsync(string query)
        {
            //var response = await _httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={query}&appid=YOUR_API_KEY");
            //return JsonSerializer.Deserialize<object>(response);

            //var sw = Stopwatch.StartNew();
            //var url = $"https://api.openweathermap.org/data/2.5/weather?q={query}&appid={ApiKey}&units=metric";
            //var response = await _httpClient.GetStringAsync(url);
            //sw.Stop();

            //_stats.RecordRequest(Name, sw.ElapsedMilliseconds);
            //return JsonSerializer.Deserialize<object>(response);
            var cacheKey = $"{Name}:{query}";
            // αν υπάρχει cached αποτέλεσμα, επιστρέφουμε αμέσως
            if (_cache.TryGetValue<object>(cacheKey, out var cached))
            {
                return cached!;
            }

            var sw = Stopwatch.StartNew();

            try
            {
                // Open-Meteo (no key) για demo:
                var url = $"https://api.open-meteo.com/v1/forecast?latitude=37.98&longitude=23.72&current_weather=true";
                var responseString = await _httpClient.GetStringAsync(url);

                sw.Stop();
                _stats.RecordRequest(Name, sw.ElapsedMilliseconds);

                var parsed = JsonSerializer.Deserialize<object>(responseString);

                // cache για 60s
                _cache.Set(cacheKey, parsed, TimeSpan.FromSeconds(60));

                return parsed!;
            }
            catch (Exception ex)
            {
                sw.Stop();
                // καταγραφή στατιστικού 
                _stats.RecordRequest(Name, sw.ElapsedMilliseconds);

                // 1) παίρνω από cache (fallback λογική)
                if (_cache.TryGetValue<object>(cacheKey, out var fallback))
                {
                    return fallback!;
                }

                //if (File.Exists(LocalMockFile))
                //{
                //    var local = await File.ReadAllTextAsync(LocalMockFile);
                //    return JsonSerializer.Deserialize<object>(local)!;
                //}

                // Αν Δεν έχω κάτι επιστρέφω ένα μικρό object που δηλώνει fallback
                return new { source = "fallback", message = "No weather data available" };
            }
        }
    }
}
