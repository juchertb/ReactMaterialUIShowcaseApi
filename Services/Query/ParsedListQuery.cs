namespace ReactMaterialUIShowcaseApi.Services.Query
{
    public class ParsedListQuery
    {
        public Dictionary<string, object>? Filters { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
        public int RangeStart { get; set; }
        public int RangeEnd { get; set; }
        public List<string>? EmbededTables { get; set; }
    }
}