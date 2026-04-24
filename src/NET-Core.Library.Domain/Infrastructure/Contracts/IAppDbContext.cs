namespace NET.Core.Library.Domain.Infrastructure.Contracts;

public interface IAppDbContext
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}