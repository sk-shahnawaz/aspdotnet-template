using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.Infrastructure
{
    /// <summary>
    /// Implementor of IRepository<T> abstraction
    /// </summary>
    /// <typeparam name="T">Generic type argument, representing abstraction of IEntity</typeparam>
    public sealed class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        private const string _argumentMissingErrorMessage = "Argument is null.";
        private readonly AppDbContext _appDbContext;

        public Repository(IUnitOfWork unitOfWork)
        {
            _appDbContext = unitOfWork.Context as AppDbContext;
        }

        public IQueryable<T> AsQueryable()
        {
            return _appDbContext.Set<T>().AsQueryable();
        }

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentException(_argumentMissingErrorMessage);

            _appDbContext.Set<T>().Add(item);
        }

        public async Task AddAsync(T item, CancellationToken cancellationToken = default)
        {
            if (item == null)
                throw new ArgumentException(_argumentMissingErrorMessage);

            await _appDbContext.Set<T>().AddAsync(item, cancellationToken);
        }

        public void Delete(T item)
        {
            if (item == null)
                throw new ArgumentException(_argumentMissingErrorMessage);

            _appDbContext.Set<T>().Remove(item);
        }

        public T Get(long id)
        {
            IQueryable<T> query = _appDbContext.Set<T>();
            return query.SingleOrDefault(o => o.Id == id);
        }

        public async Task<T> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _appDbContext.Set<T>();
            return await query.SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public void Update(T item)
        {
            if (item == null)
                throw new ArgumentException(_argumentMissingErrorMessage);

            _appDbContext.Set<T>().Attach(item);
            _appDbContext.Entry<T>(item).State = EntityState.Modified;
        }

        public void Dispose()
        {
            if (_appDbContext != null)
                _appDbContext.Dispose();
        }
    }
}