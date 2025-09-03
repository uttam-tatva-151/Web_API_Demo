using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Beans
{
    /// <summary>
    /// Query options for queries that return entity type T (no projection).
    /// Encapsulates filter, include and tracking behavior.
    /// </summary>
    public class QueryOptions<T> where T : class
    {
        /// <summary>
        /// Optional filter predicate. If null, no filtering is applied (i.e. "get all").
        /// </summary>
        public Expression<Func<T, bool>>? Predicate { get; set; }

        /// <summary>
        /// Optional include function for eager loading (use the Func to build Include/ThenInclude chains).
        /// </summary>
        public Func<IQueryable<T>, IIncludableQueryable<T, object>>? Include { get; set; }

        /// <summary>
        /// Whether to use AsNoTracking for read-only queries. Default true.
        /// </summary>
        public bool AsNoTracking { get; set; } = true;
    }

    /// <summary>
    /// Query options for queries that return TResult (projection).
    /// Inherits Predicate/Include/AsNoTracking and adds a Selector.
    /// </summary>
    public class QueryOptions<T, TResult> : QueryOptions<T> where T : class
    {
        /// <summary>
        /// Projection selector. Required for projection queries (GetList/GetFirstOrDefault with TResult).
        /// </summary>
        public Expression<Func<T, TResult>>? Selector { get; set; }
    }
}