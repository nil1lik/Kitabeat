using Kitabeat.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Kitabeat.Infrastructure.Persistence;

public class KitabeatDbContext : DbContext
{
    public KitabeatDbContext(DbContextOptions<KitabeatDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(b =>
        {
            b.ToTable("Books");
            b.HasKey(x => x.Id);

            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.Author)
                .HasMaxLength(256);

            b.Property(x => x.Description)
                .HasColumnType("text");
        });

        base.OnModelCreating(modelBuilder);
    }
}
