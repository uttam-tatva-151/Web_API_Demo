using System.Linq.Expressions;
using Core.Beans;
using Microsoft.EntityFrameworkCore;

namespace Data.BaseRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // Fetch single by PK
        public virtual async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            T? entity = await _dbSet.FindAsync([id], cancellationToken);
            return entity;
        }

        // Unified Get: use filter == null to get all
        public virtual async Task<IEnumerable<T>> GetListAsync(
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) query = query.Where(filter);
            return await query.ToListAsync(cancellationToken);
        }

        // Add / AddRange / Update / UpdateRange (tracked; do not save here)
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _dbSet.AddAsync(entity, cancellationToken);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => await _dbSet.AddRangeAsync(entities, cancellationToken);

        public virtual void Update(T entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _dbSet.UpdateRange(entities);
        }

        // Projection: FirstOrDefault with QueryOptions<T, TResult>
        public virtual async Task<TResult?> GetFirstOrDefaultAsync<TResult>(
            QueryOptions<T, TResult> options,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(options);
            if (options.Selector == null) throw new ArgumentNullException(nameof(options.Selector), "Selector is required for projection queries.");

            IQueryable<T> query = _dbSet;

            if (options.Include != null) query = options.Include(query);
            if (options.AsNoTracking) query = query.AsNoTracking();
            if (options.Predicate != null) query = query.Where(options.Predicate);

            return await query.Select(options.Selector).FirstOrDefaultAsync(cancellationToken);
        }

        // Non-projection: FirstOrDefault returning entity T
        public virtual async Task<T?> GetFirstOrDefaultAsync(
            QueryOptions<T> options,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(options);

            IQueryable<T> query = _dbSet;

            if (options.Include != null) query = options.Include(query);
            if (options.AsNoTracking) query = query.AsNoTracking();
            if (options.Predicate != null) query = query.Where(options.Predicate);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        // Projection: List with selector
        public virtual async Task<List<TResult>> GetListAsync<TResult>(
            QueryOptions<T, TResult> options,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(options);
            if (options.Selector == null) throw new ArgumentNullException(nameof(options.Selector), "Selector is required for projection queries.");

            IQueryable<T> query = _dbSet;

            if (options.Include != null) query = options.Include(query);
            if (options.AsNoTracking) query = query.AsNoTracking();
            if (options.Predicate != null) query = query.Where(options.Predicate);

            return await query.Select(options.Selector).ToListAsync(cancellationToken);
        }

        // Non-projection: List of entities
        public virtual async Task<List<T>> GetListAsync(
            QueryOptions<T> options,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(options);

            IQueryable<T> query = _dbSet;

            if (options.Include != null) query = options.Include(query);
            if (options.AsNoTracking) query = query.AsNoTracking();
            if (options.Predicate != null) query = query.Where(options.Predicate);

            return await query.ToListAsync(cancellationToken);
        }

        // Utility methods
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                return await _dbSet.CountAsync(predicate, cancellationToken);
            return await _dbSet.CountAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(predicate, cancellationToken);
    }
}