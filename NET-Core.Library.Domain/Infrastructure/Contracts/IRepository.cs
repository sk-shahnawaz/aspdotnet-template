using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NET.Core.Library.Domain.Infrastructure.Contracts
{
    public interface IRepository<T>
    {
        IQueryable<T> AsQueryable(bool trackEntity = true);

        T Get(Expression<Func<T, bool>> expression, bool trackEntity = true);

        T Get(Expression<Func<T, bool>> expression, bool trackEntity = true, params Expression<Func<T, object>>[] includes);

        Task<T> GetAsync(Expression<Func<T, bool>> expression, bool trackEntity, CancellationToken cancellationToken = default);

        Task<T> GetAsync(Expression<Func<T, bool>> expression, bool trackEntity, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);

        void Add(T item);

        Task AddAsync(T item, CancellationToken cancellationToken = default);

        void Update(T item);

        void Delete(T item);

        void DeleteBatch(System.Collections.Generic.IEnumerable<T> items);

        void Dispose();
    }
}