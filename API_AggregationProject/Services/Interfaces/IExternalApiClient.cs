using API_AggregationProject.Models;
using API_AggregationProject.Models.Requests;

namespace API_AggregationProject.Services.Interfaces
{
    public interface IExternalApiClient
    {
        string Name { get; }
        Task<IEnumerable<AggregatedItemDto>> FetchAsync(AggregationRequest request, CancellationToken ct);
        Task<object> FetchDataAsync(string query);
    }
}
