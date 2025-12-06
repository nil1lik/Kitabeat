using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kitabeat.Infrastructure.Persistence;

public class KitabeatDbContextFactory : IDesignTimeDbContextFactory<KitabeatDbContext>
{
    public KitabeatDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<KitabeatDbContext>();

        // Design-time için sabit connection string kullanıyoruz
        var connectionString =
            "Host=localhost;Port=5432;Database=kitabeat;Username=kitabeat;Password=kitabeat_pwd";

        optionsBuilder.UseNpgsql(connectionString);

        return new KitabeatDbContext(optionsBuilder.Options);
    }
}
