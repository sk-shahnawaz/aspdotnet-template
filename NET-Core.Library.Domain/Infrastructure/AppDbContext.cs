using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.Infrastructure;

/// <summary>
/// Application's Database Context type.
/// </summary>
public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext()
    {
        // This constructor is included so as to provide the capability to initialize the sub-class AppSqlServerDbContext/AppPostgreSQLDbContext without any parameters,
        // (i.e. Default Constructor), it is required when the project "NET-Core.Console.DB.SqlServer"/"NET-Core.Console.DB.PostgreSQL" is executed stand-alone for performing DB migrations.
    }

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
        : base(dbContextOptions)
    { }

    protected internal static DbContextOptions<AppDbContext> ChangeOptionsType(DbContextOptions options)
    {
        // Reference: https://entityframeworkcore.com/knowledge-base/41829229/how-do-i-implement-dbcontext-inheritance-for-multiple-databases-in-ef7----net-core
        // Accessed on: 2021-09-28

        // Need to pass the core options extensions (injected via DI by runtime) from sub-class constructor to base class constructor
        // in order to make EF Core work
        CoreOptionsExtension coreOptionsExtension = (CoreOptionsExtension)options.Extensions.FirstOrDefault(e => e is CoreOptionsExtension);
        if (coreOptionsExtension != null)
        {
            return (DbContextOptions<AppDbContext>)new DbContextOptions<AppDbContext>().WithExtension(coreOptionsExtension);
        }
        else
        {
            return new DbContextOptions<AppDbContext>();
        }
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
}