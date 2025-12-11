
namespace Kitabeat.Domain.BookEmotions;

public interface IBookEmotionRepository
{
    Task<BookEmotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(BookEmotion bookEmotion, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookEmotion>> AddRangeAsync(IEnumerable<BookEmotion> bookEmotions, CancellationToken cancellationToken = default);
}
