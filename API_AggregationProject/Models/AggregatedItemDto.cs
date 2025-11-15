namespace API_AggregationProject.Models
{
    public class AggregatedItemDto
    {
        public string Source { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
