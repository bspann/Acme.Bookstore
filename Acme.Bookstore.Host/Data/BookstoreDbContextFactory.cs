using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Acme.Bookstore.Data;

public class BookstoreDbContextFactory : IDesignTimeDbContextFactory<BookstoreDbContext>
{
    public BookstoreDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<BookstoreDbContext>()
        .UseSqlServer(configuration.GetConnectionString("Default"));

        return new BookstoreDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
