using Microsoft.EntityFrameworkCore;

using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.Infrastructure
{
    /// <summary>
    /// Application's Database Context type.
    /// </summary>
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext()
        { }

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}