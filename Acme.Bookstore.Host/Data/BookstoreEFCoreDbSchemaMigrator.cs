using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Acme.Bookstore.Data;

public class BookstoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public BookstoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        
        /* We intentionally resolving the BookstoreDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<BookstoreDbContext>()
            .Database
            .MigrateAsync();

    }
}
