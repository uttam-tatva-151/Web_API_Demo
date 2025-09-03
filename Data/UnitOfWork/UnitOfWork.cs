using Data.BaseRepository;
using Data.Entity;

namespace Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IGenericRepository<Car> Cars { get; }
    public IGenericRepository<Manufacturer> Manufacturers { get; }
    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Cars = new GenericRepository<Car>(_context);
        Manufacturers = new GenericRepository<Manufacturer>(_context);
    }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
