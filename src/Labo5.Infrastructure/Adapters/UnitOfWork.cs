using System.Collections;
using Labo5.Domain.Ports;
using Labo5.Infrastructure.Data;

namespace Labo5.Infrastructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly Lab05Context _context;

    public UnitOfWork(Lab05Context context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    public Task<int> Complete() => _context.SaveChangesAsync();

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;
        if (_repositories!.ContainsKey(type))
            return (IGenericRepository<TEntity>)_repositories[type]!;

        var instance = Activator.CreateInstance(
            typeof(GenericRepository<>).MakeGenericType(typeof(TEntity)), _context);
        _repositories.Add(type, instance!);
        return (IGenericRepository<TEntity>)instance!;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
