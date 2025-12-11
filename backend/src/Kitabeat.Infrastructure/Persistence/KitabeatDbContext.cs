using Kitabeat.Domain.Books;
using Kitabeat.Domain.BookEmotions;
using Microsoft.EntityFrameworkCore;

namespace Kitabeat.Infrastructure.Persistence;

public class KitabeatDbContext : DbContext
{
    public KitabeatDbContext(DbContextOptions<KitabeatDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookEmotion> BookEmotions => Set<BookEmotion>();

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

            b.HasMany(x => x.Emotions)
    .WithOne(e => e.Book)
    .HasForeignKey(e => e.BookId)
    .OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<BookEmotion>(e =>
{
    e.ToTable("BookEmotions");
    e.HasKey(x => x.Id);

    e.Property(x => x.Label)
        .IsRequired()
        .HasMaxLength(128);

    e.Property(x => x.Score)
        .HasPrecision(5, 4);

    e.HasIndex(x => new { x.BookId, x.Label })
        .IsUnique();
});
        base.OnModelCreating(modelBuilder);
    }
}
