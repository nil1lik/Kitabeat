using Kitabeat.Application.BookEmotions.Dtos;
using Kitabeat.Domain.BookEmotions;

namespace Kitabeat.Application.BookEmotions;

public sealed class BookEmotionService : IBookEmotionService
{
    private readonly IBookEmotionRepository _bookEmotionRepository;

    public BookEmotionService(IBookEmotionRepository bookEmotionRepository)
    {
        _bookEmotionRepository = bookEmotionRepository;
    }

    public async Task<BookEmotionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bookEmotion = await _bookEmotionRepository.GetByIdAsync(id, cancellationToken);

        return bookEmotion is null
            ? null
            : MapToDto(bookEmotion);
    }

    public async Task<BookEmotionDto> AddAsync(BookEmotionCreateDto dto, CancellationToken cancellationToken = default)
    {
        var bookEmotion = new BookEmotion(dto.BookId, dto.Label, dto.Score);
        await _bookEmotionRepository.AddAsync(bookEmotion, cancellationToken);

        return MapToDto(bookEmotion);
    }

    public async Task<IReadOnlyList<BookEmotionDto>> AddRangeAsync(
        IEnumerable<BookEmotionCreateDto> dtos,
        CancellationToken cancellationToken = default)
    {
        var bookEmotions = dtos
            .Select(dto => new BookEmotion(dto.BookId, dto.Label, dto.Score))
            .ToList();

        var addedBookEmotions = await _bookEmotionRepository.AddRangeAsync(bookEmotions, cancellationToken);

        return addedBookEmotions
            .Select(MapToDto)
            .ToList();
    }

    private static BookEmotionDto MapToDto(BookEmotion entity) =>
        new(entity.Id, entity.BookId, entity.Label, entity.Score);
}