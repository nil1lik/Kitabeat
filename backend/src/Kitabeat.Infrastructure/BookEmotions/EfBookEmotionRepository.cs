using Kitabeat.Domain.BookEmotions;
using Kitabeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kitabeat.Infrastructure.BookEmotions;

public class EfBookEmotionRepository : IBookEmotionRepository
{
    private readonly KitabeatDbContext _dbContext;

    public EfBookEmotionRepository(KitabeatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BookEmotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.BookEmotions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task AddAsync(BookEmotion bookEmotion, CancellationToken cancellationToken = default)
    {
        await _dbContext.BookEmotions.AddAsync(bookEmotion, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<BookEmotion>> AddRangeAsync(IEnumerable<BookEmotion> bookEmotions, CancellationToken cancellationToken = default)
    {
        var bookEmotionsList = bookEmotions.ToList();
        await _dbContext.BookEmotions.AddRangeAsync(bookEmotionsList, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return bookEmotionsList;
    }
}