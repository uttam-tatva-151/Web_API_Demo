using Data.BaseRepository;
using Data.Entity;
namespace Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Car> Cars { get; }
    IGenericRepository<Manufacturer> Manufacturers { get; }
    Task<int> CompleteAsync();

}
