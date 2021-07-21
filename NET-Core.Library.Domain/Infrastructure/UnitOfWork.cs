using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.Infrastructure
{
    /// <summary>
    /// Implementor of IUnitOfWork abstration.
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IAppDbContext _dbContext = null;

        public UnitOfWork(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns underlying Database context.
        /// </summary>
        public IAppDbContext Context { get => _dbContext; }

        /// <summary>
        /// Releases the allocated resources for this type
        /// </summary>
        public void Dispose()
        {
            if (_dbContext != null && _dbContext is DbContext)
                (_dbContext as DbContext).Dispose();
        }

        /// <summary>
        /// Saves changes into Database.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Save changes into Database (Asynchronously).
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be cancelled</param>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}