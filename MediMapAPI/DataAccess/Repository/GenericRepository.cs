using DataAccess.DbContext;
using DataAccess.Repository.IGenericRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(string? includeProperty = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var include in includeProperty.Split([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string? includeProperty = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var include in includeProperty.Split([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }
            TEntity? entity = await query.FirstOrDefaultAsync(filter);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetById(Expression<Func<TEntity, bool>> filter, string? includeProperty = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var include in includeProperty.Split([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }
            query = query.Where(filter);
            return await query.ToListAsync();
        }
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
