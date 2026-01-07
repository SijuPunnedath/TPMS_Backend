using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPMS.Infrastructure.Interfaces;
using TPMS.Infrastructure.Persistence;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly TPMSDBContext _db;
        private readonly DbSet<T> _set;

        public EfRepository(TPMSDBContext db)
        {
            _db = db;
            _set = _db.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _set.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<IEnumerable<T>> ListAsync(int skip, int take)
        {
            return await _set.AsNoTracking().Skip(skip).Take(take).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
