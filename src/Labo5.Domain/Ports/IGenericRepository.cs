using System.Linq.Expressions;

namespace Labo5.Domain.Ports;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task InsertAsync(TEntity entity);
    Task InsertAndSaveAsync(TEntity entity);
    void Update(TEntity entity);
    Task UpdateAndSaveAsync(TEntity entity);
    void Delete(TEntity entity);
    Task DeleteByIdAsync(int id);
    Task DeleteAndSaveAsync(TEntity entity);
}
