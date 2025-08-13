using System.Linq.Expressions;

namespace PaymentService.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
        Task<T> GetOne(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task Create(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        Task Save();
        Task<int> GetLength();
    }
}
