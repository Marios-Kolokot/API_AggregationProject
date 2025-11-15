namespace API_AggregationProject.Models.Requests
{
    public class AggregationRequest
    {
        public string? Query { get; set; }
        public string? Category { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Sort { get; set; } = "date";

        public string CacheKey()
        {
            return $"{Query}-{Category}-{From}-{To}-{Sort}";
        }
    }
}
