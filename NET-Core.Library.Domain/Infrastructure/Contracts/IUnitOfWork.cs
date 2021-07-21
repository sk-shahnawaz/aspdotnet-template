using System.Threading;
using System.Threading.Tasks;

namespace NET.Core.Library.Domain.Infrastructure.Contracts
{
    public interface IUnitOfWork
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        void Dispose();

        IAppDbContext Context { get; }
    }
}