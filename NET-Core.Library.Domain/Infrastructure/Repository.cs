using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.Infrastructure;

/// <summary>
/// Implementor of IRepository<T> abstraction
/// </summary>
/// <typeparam name="T">Generic type argument, representing abstraction of IEntity</typeparam>
public sealed class Repository<T> : IRepository<T>
    where T : class
{
    private readonly AppDbContext _appDbContext;

    public Repository(IUnitOfWork unitOfWork)
    {
        _appDbContext = unitOfWork.Context as AppDbContext;
    }

    public IQueryable<T> AsQueryable(bool trackEntity = true)
    {
        var query = _appDbContext.Set<T>().AsQueryable();
        if (!trackEntity)
        {
            query = query.AsNoTracking();
        }
        return query;
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        _appDbContext.Set<T>().Add(item);
    }

    public async Task AddAsync(T item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        await _appDbContext.Set<T>().AddAsync(item, cancellationToken);
    }

    public void Delete(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        _appDbContext.Set<T>().Remove(item);
    }

    public void DeleteBatch(IEnumerable<T> items)
    {
        if (items?.Any() ?? false)
        {
            _appDbContext.Set<T>().RemoveRange(items);
        }
    }

    public T Get(Expression<Func<T, bool>> expression, bool trackEntity = true)
    {
        IQueryable<T> query = _appDbContext.Set<T>();
        if (!trackEntity)
        {
            query = query.AsNoTracking();
        }
        if (expression != null)
        {
            query = query.Where(expression);
        }
        return query.FirstOrDefault();
    }

    public T Get(Expression<Func<T, bool>> expression, bool trackEntity = true, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _appDbContext.Set<T>();
        if (!trackEntity)
        {
            query = query.AsNoTracking();
        }
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        if (expression != null)
        {
            query = query.Where(expression);
        }
        return query.FirstOrDefault();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression, bool trackEntity = true, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _appDbContext.Set<T>();
        if (!trackEntity)
        {
            query = query.AsNoTracking();
        }
        if (expression != null)
        {
            query = query.Where(expression);
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression, bool trackEntity = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _appDbContext.Set<T>();
        if (!trackEntity)
        {
            query = query.AsNoTracking();
        }
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        if (expression != null)
        {
            query = query.Where(expression);
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        _appDbContext.Set<T>().Attach(item);
        _appDbContext.Entry<T>(item).State = EntityState.Modified;
    }

    public void Dispose()
    {
        if (_appDbContext != null)
            _appDbContext.Dispose();
    }
}