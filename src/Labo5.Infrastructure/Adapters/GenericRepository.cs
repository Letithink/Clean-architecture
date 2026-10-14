using System.Linq.Expressions;
using Labo5.Domain.Ports;
using Labo5.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Labo5.Infrastructure.Adapters;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly Lab05Context _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(Lab05Context context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var include in includes)
            query = query.Include(include);
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public async Task InsertAndSaveAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task UpdateAndSaveAsync(TEntity entity)
    {
        Update(entity);
        await _context.SaveChangesAsync();
    }

    public void Delete(TEntity entity) => _dbSet.Remove(entity);

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null) _dbSet.Remove(entity);
    }

    public async Task DeleteAndSaveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
