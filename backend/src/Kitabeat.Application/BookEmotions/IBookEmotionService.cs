using Kitabeat.Application.BookEmotions.Dtos;

namespace Kitabeat.Application.BookEmotions;

public interface IBookEmotionService
{
    Task<BookEmotionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BookEmotionDto> AddAsync(BookEmotionCreateDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookEmotionDto>> AddRangeAsync(IEnumerable<BookEmotionCreateDto> dtos, CancellationToken cancellationToken = default);
}