using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Repositories.IRepositories;
using System.Linq.Expressions;

namespace PaymentService.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;
        public GenericRepository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }
        public async Task Update(T entity)
        {
            dbSet.Update(entity);
            await Save();
        }
        public async Task<T> GetOne(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.AsNoTracking();
                query = query.Where(filter);
            }

            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip((pageSize) * (pageNumber - 1)).Take(pageSize);
            }


            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();

        }
        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }

        public async Task<int> GetLength()
        {
            var length = dbSet.Count();
            return length;
        }
    }
}
