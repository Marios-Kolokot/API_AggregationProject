using API_AggregationProject.Models.Responses;
using API_AggregationProject.Services.Interfaces;
using System.Collections.Concurrent;
using static API_AggregationProject.Models.Responses.StatisticsResponse;

namespace API_AggregationProject.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ConcurrentDictionary<string, List<long>> _apiStats = new();
        public Task<StatisticsResponse> GetStatisticsAsync()
        {
            var stats = _apiStats.ToDictionary(
               x => x.Key,
               x =>
               {
                   var values = x.Value;
                   return new ApiStats
                   {
                       TotalRequests = values.Count,
                       AverageResponseTimeMs = values.Average(),
                       FastCount = values.Count(v => v < 100),
                       AverageCount = values.Count(v => v >= 100 && v < 200),
                       SlowCount = values.Count(v => v >= 200)
                   };
               });

            return Task.FromResult(new StatisticsResponse { ApiStatistics = stats });
        }

        public void RecordRequest(string apiName, long responseTimeMs)
        {
            _apiStats.AddOrUpdate(apiName,
               _ => new List<long> { responseTimeMs },
               (_, list) => { list.Add(responseTimeMs); return list; });
        }
    }
}
