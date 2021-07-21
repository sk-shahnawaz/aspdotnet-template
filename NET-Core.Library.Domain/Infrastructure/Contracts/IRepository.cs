using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NET.Core.Library.Domain.Infrastructure.Contracts
{
    public interface IRepository<T>
    {
        IQueryable<T> AsQueryable();

        T Get(long id);

        Task<T> GetAsync(long id, CancellationToken cancellationToken = default);

        void Add(T item);

        Task AddAsync(T item, CancellationToken cancellationToken = default);

        void Update(T item);

        void Delete(T item);

        void Dispose();
    }
}