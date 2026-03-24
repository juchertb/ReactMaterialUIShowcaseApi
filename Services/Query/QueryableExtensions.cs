using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ReactMaterialUIShowcaseApi.Services.Query
{
    /// <summary>
    /// This class mimics FakeRest's filtering, sorting, paging and embedding.
    /// </summary>
    public static class QueryableExtensions
    {
        private static readonly Dictionary<string, string> EmbedMappings =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "command", "Invoice" }
            };

        public static async Task<(IEnumerable<T> Items, int Total)> ApplyListQueryAsync<T>(
            this IQueryable<T> query,
            ParsedListQuery parsed)
            where T : class
        {
            // 1. Filtering
            if (parsed.Filters != null)
            {
                foreach (var filter in parsed.Filters)
                {
                    if (filter.Key == "q")
                    {
                        query = ApplyFullTextSearch(query, filter.Value?.ToString());
                    }
                    else
                    {
                        query = ApplyFilter(query, filter.Key, filter.Value);
                    }
                }
            }

            // Count BEFORE paging
            var total = query.Count();

            // 2. Sorting
            if (!string.IsNullOrWhiteSpace(parsed.SortField))
            {
                query = ApplySorting(query, parsed.SortField, parsed.SortOrder);
            }

            // 3. Embedding
            if (parsed.EmbededTables != null)
            {
                query = ApplyEmbed(query, parsed.EmbededTables);
            }

            // 4. Paging
            int skip = parsed.RangeStart;
            int take = parsed.RangeEnd - parsed.RangeStart + 1;
            take = 300;

            var items = await query.Skip(skip).Take(take).ToListAsync();

            return (items, total);
        }

        private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, string property, object? value)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                property = property.Replace("_", "");

                // Find the property ignoring case and replace underscores from Dto (e.g., "customer_id" should match "CustomerId")
                var propertyInfo = typeof(T)
                    .GetProperties()
                    .FirstOrDefault(p =>
                        string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo == null)
                    throw new ArgumentException($"Property '{property}' not found on type {typeof(T).Name}.");

                var member = Expression.Property(parameter, propertyInfo);

                // Convert the incoming value to the correct type
                var typedValue = ConvertToPropertyType(value, propertyInfo.PropertyType);
                var constant = Expression.Constant(typedValue, propertyInfo.PropertyType);

                var body = Expression.Equal(member, constant);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                return query.Where(lambda);
            }
            catch { }
            return query;
        }

        private static object? ConvertToPropertyType(object? value, Type targetType)
        {
            if (value == null)
                return null;

            var stringValue = value.ToString();
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            // Handle Nullable<T>
            var underlying = Nullable.GetUnderlyingType(targetType);
            if (underlying != null)
            {
                targetType = underlying;
            }

            try
            {
                // Guid
                if (targetType == typeof(Guid))
                    return Guid.Parse(stringValue);

                // Enum
                if (targetType.IsEnum)
                    return Enum.Parse(targetType, stringValue, ignoreCase: true);

                // DateTime
                if (targetType == typeof(DateTime))
                    return DateTime.Parse(stringValue);

                // Boolean
                if (targetType == typeof(bool))
                    return bool.Parse(stringValue);

                // TimeSpan
                if (targetType == typeof(TimeSpan))
                    return TimeSpan.Parse(stringValue);

                // Fallback for numeric + convertible types
                return Convert.ChangeType(stringValue, targetType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Cannot convert value '{value}' to type '{targetType.Name}'.", ex);
            }
        }

        private static IQueryable<T> ApplyFullTextSearch<T>(IQueryable<T> query, string? search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search))
                    return query;

                var parameter = Expression.Parameter(typeof(T), "x");

                Expression? combined = null;

                foreach (var prop in typeof(T).GetProperties().Where(p => p.PropertyType == typeof(string)))
                {
                    var member = Expression.Property(parameter, prop);
                    var searchConst = Expression.Constant(search);

                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                    var containsCall = Expression.Call(member, containsMethod!, searchConst);

                    combined = combined == null ? containsCall : Expression.OrElse(combined, containsCall);
                }

                if (combined == null)
                    return query;

                var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                return query.Where(lambda);
            }
            catch { }
            return query;
        }

        private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string field, string? order)
        {
            // TODO: This is not working when the sort field is not availabe in the entity. FakeRest allows us to sort by a field
            // of an embedded table.
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var member = Expression.PropertyOrField(parameter, field);
                var lambda = Expression.Lambda(member, parameter);

                string method = order?.ToUpper() == "DESC" ? "OrderByDescending" : "OrderBy";

                var result = typeof(Queryable).GetMethods()
                    .First(m => m.Name == method && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), member.Type)
                    .Invoke(null, new object[] { query, lambda });

                return (IQueryable<T>)result!;
            }
            catch
            { }
            return query;
        }

        public static IQueryable<T> ApplyEmbed<T>(this IQueryable<T> query, IEnumerable<string> embededTables) where T : class
        {
            if (embededTables == null)
                return query;

            var type = typeof(T);
            var navProps = type.GetProperties()
                .Where(p =>
                    // Navigation properties are usually classes or collections
                    (typeof(IEnumerable<>).IsAssignableFrom(p.PropertyType) == false &&
                     p.PropertyType.IsClass &&
                     p.PropertyType != typeof(string))
                )
                .ToList();

            foreach (var embededTable in embededTables)
            {
                var normalized = embededTable.Replace("_", "");

                // Apply mapping
                if (EmbedMappings.TryGetValue(normalized, out var mappedName))
                    normalized = mappedName;

                var match = navProps.FirstOrDefault(p =>
                    p.Name.Equals(normalized, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                {
                    query = query.Include(match.Name);
                }
            }

            return query;
        }
    }
}