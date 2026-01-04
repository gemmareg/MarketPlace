/* 
PSEUDOCÓDIGO / PLAN (detalle paso a paso)
1. Objetivo: Corregir los métodos asincrónicos/síncronos en Repository<T> para evitar:
   - Usar 'await' fuera de métodos marcados 'async'.
   - Intentar devolver 'Task' desde métodos que retornan EntityEntry<T>.
   - Errores de compilación CS4032, CS1061, CS0029.

2. Estrategia:
   - Para operaciones EF Core que devuelven una tarea/ValueTask (p. ej. FindAsync, ToListAsync, AddAsync):
     - Mantener los métodos como 'async Task' y await la llamada.
   - Para operaciones que devuelven inmediatamente un 'EntityEntry<T>' (p. ej. Update, Remove):
     - Ejecutar la llamada (sin await).
     - Devolver 'Task.CompletedTask' para cumplir la firma 'Task' y mantener la API asincrónica.
   - Asegurar consistencia y legibilidad en la clase.
   - No introducir cambios en la semántica de la API: métodos siguen indicando que la operación fue solicitada al DbContext,
     el guardado real (SaveChangesAsync) queda fuera del repositorio (según convención común).

3. Implementación concreta:
   - Modificar 'UpdateAsync' así:
       public Task UpdateAsync(T entity)
       {
           _dbSet.Update(entity);
           return Task.CompletedTask;
       }
   - Modificar 'RemoveAsync' así:
       public Task RemoveAsync(T entity)
       {
           _dbSet.Remove(entity);
           return Task.CompletedTask;
       }
   - Mantener 'AddAsync' como método async que await a '_dbSet.AddAsync(entity)'.
   - Mantener 'GetByIdAsync' y 'GetAllAsync' sin cambios funcionales.

4. Validaciones:
   - Compilar para confirmar que desaparecen CS4032, CS1061 y CS0029.
*/

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
        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
        public Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
