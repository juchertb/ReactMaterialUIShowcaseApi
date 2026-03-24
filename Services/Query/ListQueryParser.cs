using System.Text.Json;

namespace ReactMaterialUIShowcaseApi.Services.Query
{
    public static class ListQueryParser
    {
        public static ParsedListQuery Parse(ListQueryParameters p)
        {
            var parsed = new ParsedListQuery();

            // Parse sort
            if (!string.IsNullOrWhiteSpace(p.Sort))
            {
                try
                {
                    var sort = JsonSerializer.Deserialize<string[]>(p.Sort);
                    parsed.SortField = sort?[0];
                    parsed.SortOrder = sort?[1];
                }
                catch { }
            }

            // Parse range
            if (!string.IsNullOrWhiteSpace(p.Range))
            {
                try
                {
                    var range = JsonSerializer.Deserialize<int[]>(p.Range);
                    parsed.RangeStart = range?[0] ?? 0;
                    parsed.RangeEnd = range?[1] ?? 24;
                }
                catch
                {
                    parsed.RangeStart = 0;
                    parsed.RangeEnd = 24;
                }
            }
            else
            {
                parsed.RangeStart = 0;
                parsed.RangeEnd = 24;
            }

            // Parse filter
            if (!string.IsNullOrWhiteSpace(p.Filter))
            {
                try
                {
                    parsed.Filters = JsonSerializer.Deserialize<Dictionary<string, object>>(p.Filter);
                }
                catch
                {
                    parsed.Filters = new Dictionary<string, object>();
                }
            }

            if (!string.IsNullOrWhiteSpace(p.Embed))
            {
                try
                {
                    parsed.EmbededTables = JsonSerializer.Deserialize<List<string>>(p.Embed);
                }
                catch { }
            }

            return parsed;
        }
    }

}