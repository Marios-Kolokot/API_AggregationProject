using System.Threading.Tasks;
using API_AggregationProject.Models.Responses;

namespace API_AggregationProject.Services.Interfaces
{
    public interface IApiAggregatorService
    {
        Task<AggregatedResponse> GetAggregatedDataAsync(string query, string sortBy, string filterBy);
    }
}
