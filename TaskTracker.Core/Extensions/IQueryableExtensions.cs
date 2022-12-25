using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;


namespace TaskTracker.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> propertySelector, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return source;

            var isNotNullExpression = Expression.NotEqual(propertySelector.Body,
                                                          Expression.Constant(null));

            var searchTermExpression = Expression.Constant(searchTerm.Trim().ToLower());
            var toLowerProportyValueExpression = Expression.Call(propertySelector.Body, typeof(string).GetMethod("ToLower", new Type[] { }));
            var checkContainsExpression = Expression.Call(toLowerProportyValueExpression,
                                                          typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                                                          searchTermExpression);

            var notNullAndContainsExpression = Expression.AndAlso(isNotNullExpression, checkContainsExpression);

            var methodCallExpression = Expression.Call(typeof(Queryable),
                                                       "Where",
                                                       new Type[] { source.ElementType },
                                                       source.Expression,
                                                       Expression.Lambda<Func<T, bool>>(notNullAndContainsExpression, propertySelector.Parameters));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string orderByQueryString)
        {
            if (!source.Any())
                return source;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return source;

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return source.OrderBy(orderQuery);
        }
    }
}
