using Microsoft.EntityFrameworkCore;

using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.Infrastructure;

/// <summary>
/// Implementor of IUnitOfWork abstraction.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly IAppDbContext _dbContext = null!;

    public UnitOfWork(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Returns underlying Database context.
    /// </summary>
    public IAppDbContext Context { get => _dbContext; }

    /// <summary>
    /// Releases the allocated resources for this type.
    /// </summary>
    public void Dispose()
    {
        if (_dbContext is DbContext dbContext)
            dbContext.Dispose();
    }

    /// <summary>
    /// Saves changes into Database.
    /// </summary>
    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    /// <summary>
    /// Saves changes into Database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled</param>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
