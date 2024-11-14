using System.Text;
using System.Linq.Dynamic.Core;

namespace Myrtus.Clarity.Core.Infrastructure.Dynamic;

public static class IQueryableDynamicFilterExtensions
{
    private static readonly string[] _orders = { "asc", "desc" };
    private static readonly string[] _logics = { "and", "or" };

    private static readonly IDictionary<string, string> _operators = new Dictionary<string, string>
    {
        { "eq", "=" },
        { "neq", "!=" },
        { "lt", "<" },
        { "lte", "<=" },
        { "gt", ">" },
        { "gte", ">=" },
        { "isnull", "== null" },
        { "isnotnull", "!= null" },
        { "startswith", "StartsWith" },
        { "endswith", "EndsWith" },
        { "contains", "Contains" },
        { "doesnotcontain", "Contains" }
    };

    private static readonly HashSet<string> _stringOperators = new()
    {
        "startswith",
        "endswith",
        "contains",
        "doesnotcontain",
        "eq"
    };

    public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQuery dynamicQuery)
    {
        if (dynamicQuery.Filter is not null)
            query = ApplyFilter(query, dynamicQuery.Filter);

        if (dynamicQuery.Sort is not null && dynamicQuery.Sort.Any())
            query = ApplySorting(query, dynamicQuery.Sort);

        return query;
    }

    private static IQueryable<T> ApplyFilter<T>(IQueryable<T> queryable, Filter filter)
    {
        var filters = GetAllFilters(filter);
        string[] values = filters.Select(f => f.Value).ToArray();
        string whereClause = BuildWhereClause(filter, filters);

        if (!string.IsNullOrEmpty(whereClause) && values.Any())
            queryable = queryable.Where(whereClause, values);

        return queryable;
    }

    private static IQueryable<T> ApplySorting<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
    {
        foreach (var item in sort)
        {
            if (string.IsNullOrEmpty(item.Field))
                throw new ArgumentException("Sort field cannot be empty.");
            if (string.IsNullOrEmpty(item.Dir) || !_orders.Contains(item.Dir))
                throw new ArgumentException($"Invalid sort direction: {item.Dir}");
        }

        if (sort.Any())
        {
            string ordering = string.Join(",", sort.Select(s => $"{s.Field} {s.Dir}"));
            return queryable.OrderBy(ordering);
        }

        return queryable;
    }

    private static IList<Filter> GetAllFilters(Filter filter)
    {
        var filters = new List<Filter>();
        CollectFilters(filter, filters);
        return filters;
    }

    private static void CollectFilters(Filter filter, IList<Filter> filters)
    {
        filters.Add(filter);

        if (filter.Filters is not null && filter.Filters.Any())
            foreach (var subFilter in filter.Filters)
                CollectFilters(subFilter, filters);
    }

    private static string BuildWhereClause(Filter filter, IList<Filter> filters)
    {
        if (string.IsNullOrEmpty(filter.Field))
            throw new ArgumentException("Filter field cannot be empty.");
        if (string.IsNullOrEmpty(filter.Operator) || !_operators.ContainsKey(filter.Operator))
            throw new ArgumentException($"Invalid filter operator: {filter.Operator}");

        int index = filters.IndexOf(filter);
        string comparison = _operators[filter.Operator];
        var whereClause = new StringBuilder();

        if (!string.IsNullOrEmpty(filter.Value))
        {
            bool isStringOperator = _stringOperators.Contains(filter.Operator);
            bool shouldBeCaseInsensitive = isStringOperator && !filter.IsCaseSensitive;

            if (filter.Operator == "doesnotcontain")
            {
                if (shouldBeCaseInsensitive)
                    whereClause.Append($"(!np({filter.Field}).ToLower().{comparison}(@{index}.ToLower()))");
                else
                    whereClause.Append($"(!np({filter.Field}).{comparison}(@{index}))");
            }
            else if (comparison is "StartsWith" or "EndsWith" or "Contains")
            {
                if (shouldBeCaseInsensitive)
                    whereClause.Append($"(np({filter.Field}).ToLower().{comparison}(@{index}.ToLower()))");
                else
                    whereClause.Append($"(np({filter.Field}).{comparison}(@{index}))");
            }
            else if (isStringOperator && shouldBeCaseInsensitive)
            {
                whereClause.Append($"np({filter.Field}).ToLower() {comparison} @{index}.ToLower()");
            }
            else
            {
                whereClause.Append($"np({filter.Field}) {comparison} @{index}");
            }
        }
        else if (filter.Operator is "isnull" or "isnotnull")
        {
            whereClause.Append($"np({filter.Field}) {comparison}");
        }

        if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
        {
            if (!_logics.Contains(filter.Logic))
                throw new ArgumentException($"Invalid filter logic: {filter.Logic}");

            string subFilters = string.Join($" {filter.Logic} ", filter.Filters.Select(f => BuildWhereClause(f, filters)));
            return $"{whereClause} {filter.Logic} ({subFilters})";
        }

        return whereClause.ToString();
    }
}