using Kitabeat.Domain.Books;
using Kitabeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kitabeat.Infrastructure.Books;

public class EfBookRepository : IBookRepository
{
    private readonly KitabeatDbContext _dbContext;

    public EfBookRepository(KitabeatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> SearchAsync(
     string query,
     int take = 20,
     CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Array.Empty<Book>();

        query = query.Trim().ToLowerInvariant();

        return await _dbContext.Books
            .AsNoTracking()
            .Where(b =>
                (!string.IsNullOrEmpty(b.Title) && b.Title.ToLower().Contains(query)) ||
                (!string.IsNullOrEmpty(b.Author) && b.Author.ToLower().Contains(query)))
            .OrderBy(b => b.Title)
            .Take(take)
            .ToListAsync(cancellationToken);
    }


    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books.AnyAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Book> books, CancellationToken cancellationToken = default)
    {
        await _dbContext.Books.AddRangeAsync(books, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _dbContext.Books.Update(book);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
