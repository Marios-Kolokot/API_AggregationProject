using API_AggregationProject.Models.Responses;

namespace API_AggregationProject.Services.Interfaces
{
    public interface IStatisticsService
    {
        void RecordRequest(string apiName, long responseTimeMs);
        Task<StatisticsResponse> GetStatisticsAsync();
    }
}
