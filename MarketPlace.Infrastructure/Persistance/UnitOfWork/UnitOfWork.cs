using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Infrastructure.Persistance.Context;

namespace MarketPlace.Infrastructure.Persistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketPlaceDbContext _context;

        public UnitOfWork(MarketPlaceDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
