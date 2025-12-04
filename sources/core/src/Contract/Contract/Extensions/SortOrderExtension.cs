using System.Linq.Expressions;
using Contract.Enumerations;

namespace Contract.Extensions;
public static class SortOrderExtension
{
    public static SortOrder ConvertStringToSortOrder(string? sortOrder)
        => !string.IsNullOrEmpty(sortOrder)
            ? sortOrder.Trim().ToUpper().Equals("ASC") ? SortOrder.Ascending : SortOrder.Descending
            : SortOrder.Descending;

    public static IDictionary<string, SortOrder> ConvertStringToSortColumnOrder<T>(string? sortColumnOrder)
    {
        var result = new Dictionary<string, SortOrder>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(sortColumnOrder))
            return result;

        var properties = typeof(T).GetProperties()
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var item in sortColumnOrder.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!item.Contains('-'))
                throw new FormatException("Invalid format. Expected: 'Column-Order'.");

            var pair = item.Split('-', StringSplitOptions.RemoveEmptyEntries);

            if (pair.Length != 2)
                throw new FormatException("Invalid format. Expected: 'Column-Order'.");

            var column = pair[0].Trim();

            if (!properties.Contains(column))
                throw new Exception($"Property '{column}' does not exist on type {typeof(T).Name}.");

            var order = SortOrderExtension.ConvertStringToSortOrder(pair[1]);

            result[column] = order;
        }

        return result;
    }


    public static IQueryable<T> ApplyMultiColumnSorting<T>(
        IQueryable<T> source,
        IDictionary<string, SortOrder> sortColumns)
    {
        bool first = true;

        foreach (var kvp in sortColumns)
        {
            string columnName = kvp.Key;
            bool descending = kvp.Value == SortOrder.Descending;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, columnName);
            var keySelector = Expression.Lambda(property, parameter);

            string methodName = (first, descending) switch
            {
                (true, false) => "OrderBy",
                (true, true) => "OrderByDescending",
                (false, false) => "ThenBy",
                (false, true) => "ThenByDescending",
            };

            source = typeof(Queryable).GetMethods()
                .Single(m => m.Name == methodName
                          && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { source, keySelector }) as IQueryable<T>;

            first = false;
        }

        return source;
    }
}
