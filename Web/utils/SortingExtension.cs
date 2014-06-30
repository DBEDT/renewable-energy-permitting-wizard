namespace HawaiiDBEDT.Web.utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.UI.WebControls;

    public static class SortingExtension
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> collection,
       string columnName, SortDirection direction)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");   // x

            Expression property;
            if (!columnName.Contains("."))
                property = Expression.Property(param, columnName);       // x.ColumnName

            else //has sub-properties
            {
                string[] subProperties = columnName.Split('.');
                property = subProperties.Aggregate<string, Expression>(param, (result, subPropertyName) => result = Expression.Property((Expression) result, (string) subPropertyName)); // x.First.Second.Third and so on
            }

            Func<T, object> lambda = Expression.Lambda<Func<T, object>>(   // x => x.ColumnName
                            Expression.Convert(property, typeof(object)),
                            param)
                    .Compile();

            Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> expression = SortExpressionBuilder<T>.CreateExpression(direction);

            IEnumerable<T> sorted = expression(collection, lambda);
            return sorted;
        }
    }
}