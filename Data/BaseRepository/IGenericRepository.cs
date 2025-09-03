using System.Linq.Expressions;
using Core.Beans;

namespace Data.BaseRepository
{
    /// <summary>
    /// Generic repository interface for standard data operations.
    /// Implementations should forward CancellationToken into EF Core async calls.
    /// QueryOptions<T> / QueryOptions<T, TResult> are used for advanced queries (filter/include/selector/asNoTracking).
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        // --- Fetching single/multiple entities ---
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Simple unified list fetch. Pass filter == null to return all items.
        /// </summary>
        Task<IEnumerable<T>> GetListAsync(
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default);

        // --- Adding / updating (tracked; do not save here) ---
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity, CancellationToken cancellationToken = default);
        void UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        // --- Advanced queries with projection, includes, and tracking options ---
        Task<TResult?> GetFirstOrDefaultAsync<TResult>(
            QueryOptions<T, TResult> options,
            CancellationToken cancellationToken = default);

        Task<T?> GetFirstOrDefaultAsync(
            QueryOptions<T> options,
            CancellationToken cancellationToken = default);

        Task<List<TResult>> GetListAsync<TResult>(
            QueryOptions<T, TResult> options,
            CancellationToken cancellationToken = default);

        Task<List<T>> GetListAsync(
            QueryOptions<T> options,
            CancellationToken cancellationToken = default);

        // --- Utility methods ---
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}