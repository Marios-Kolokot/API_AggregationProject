namespace API_AggregationProject.Models.Responses
{
    public class StatisticsResponse
    {
        public Dictionary<string, ApiStats> ApiStatistics { get; set; }

        public class ApiStats
        {
            public int TotalRequests { get; set; }
            public double AverageResponseTimeMs { get; set; }
            public int FastCount { get; set; }
            public int AverageCount { get; set; }
            public int SlowCount { get; set; }
        }
    }
}
