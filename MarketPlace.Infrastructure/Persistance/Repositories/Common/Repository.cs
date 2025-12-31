using MarketPlace.Application.Abstractions.Repositories.Common;
using MarketPlace.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Persistance.Repositories.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MarketPlaceDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(MarketPlaceDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public async Task<IReadOnlyList<T>> GetAllAsync()  => await _dbSet.ToListAsync();
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Remove(T entity) => _dbSet.Remove(entity);
    }
}
