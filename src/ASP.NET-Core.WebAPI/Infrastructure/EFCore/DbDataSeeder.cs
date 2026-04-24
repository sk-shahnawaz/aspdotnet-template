using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure;
using NET.Core.Library.Domain.Infrastructure.EFCore;

namespace ASP.NET.Core.WebAPI.Infrastructure.EFCore;

internal static class InMemoryDbDataSeeder
{
    internal static void SeedTestData(IApplicationBuilder applicationBuilder)
    {
        var appDbContext = applicationBuilder.ApplicationServices.CreateScope()
                            .ServiceProvider.GetRequiredService<AppDbContext>();

        if (appDbContext != null)
        {
            foreach (Author author in TestData.GetAuthors())
            {
                appDbContext.Authors.Add(author);
            }
            appDbContext.SaveChanges();
        }
    }
}