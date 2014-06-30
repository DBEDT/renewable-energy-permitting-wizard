namespace HawaiiDBEDT.Web.utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;

    public static class SortExpressionBuilder<T>
    {
        private static IDictionary<SortDirection, ISortExpression> directions =
                new Dictionary<SortDirection, ISortExpression>
        {
            { SortDirection.Ascending, new OrderByAscendingSortExpression() },
            { SortDirection.Descending, new OrderByDescendingSortExpression() }
        };

        interface ISortExpression
        {
            Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression();
        }

        class OrderByAscendingSortExpression : ISortExpression
        {
            public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
            {
                return (c, f) => c.OrderBy(f);
            }
        }

        class OrderByDescendingSortExpression : ISortExpression
        {
            public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
            {
                return (c, f) => c.OrderByDescending(f);
            }
        }

        public static Func<IEnumerable<T>, Func<T, object>,
          IEnumerable<T>> CreateExpression(SortDirection direction)
        {
            return directions[direction].GetExpression();
        }
    }
}