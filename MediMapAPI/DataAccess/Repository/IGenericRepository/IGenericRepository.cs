using System.Linq.Expressions;

namespace DataAccess.Repository.IGenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string? includeProperty = null);
        Task<IEnumerable<TEntity>> GetAllAsync(string? includeProperty = null);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetById(Expression<Func<TEntity, bool>> filter, string? includeProperty = null);
    }
}
