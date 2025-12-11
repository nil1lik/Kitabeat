using Kitabeat.Application.BookEmotions;
using Kitabeat.Application.BookEmotions.Dtos;
using Kitabeat.Application.Books.Dtos;
using Kitabeat.Application.Sentiment;
using Kitabeat.Domain.Books;

namespace Kitabeat.Application.Books;

/// <summary>
/// Service responsible for analyzing emotions in books using sentiment analysis.
/// </summary>
public sealed class BookEmotionAnalyzerService : IBookEmotionAnalyzerService
{
    private readonly IBookRepository _bookRepository;
    private readonly ISentimentClient _sentimentClient;
    private readonly IBookEmotionService _bookEmotionService;

    public BookEmotionAnalyzerService(
        IBookRepository bookRepository,
        ISentimentClient sentimentClient,
        IBookEmotionService bookEmotionService)
    {
        _bookRepository = bookRepository;
        _sentimentClient = sentimentClient;
        _bookEmotionService = bookEmotionService;
    }

    public async Task<BookEmotionAnalysisResultDto?> AnalyzeBookEmotionAsync(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);

        if (book is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(book.Description))
        {
            return CreateEmptyResult(book);
        }

        var sentimentResult = await _sentimentClient.AnalyzeAsync(book.Description, cancellationToken);

        var (primaryLabel, primaryScore) = DeterminePrimaryEmotion(sentimentResult);

        var emotionDtos = await SaveBookEmotionsAsync(book.Id, sentimentResult, cancellationToken);

        return new BookEmotionAnalysisResultDto(
            book.Id,
            book.Title,
            book.Author,
            primaryLabel,
            primaryScore,
            emotionDtos);
    }

    private static BookEmotionAnalysisResultDto CreateEmptyResult(Book book) =>
        new(
            book.Id,
            book.Title,
            book.Author,
            "neutral",
            0.0,
            Array.Empty<BookEmotionDto>());

    private static (string Label, double Score) DeterminePrimaryEmotion(SentimentResult result)
    {
        if (result.Emotions.Count == 0)
        {
            return ("neutral", 0.0);
        }

        // Try to find primary from non-neutral emotions first
        var nonNeutralPrimary = result.Emotions
            .Where(e => !string.Equals(e.Label, "neutral", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.Score)
            .FirstOrDefault();

        if (nonNeutralPrimary is not null)
        {
            return (nonNeutralPrimary.Label, nonNeutralPrimary.Score);
        }

        // Fallback to highest scoring emotion
        var fallbackPrimary = result.Emotions
            .OrderByDescending(e => e.Score)
            .First();

        return (fallbackPrimary.Label, fallbackPrimary.Score);
    }

    private async Task<IReadOnlyList<BookEmotionDto>> SaveBookEmotionsAsync(
        Guid bookId,
        SentimentResult result,
        CancellationToken cancellationToken)
    {
        var createDtos = result.Emotions
            .Select(e => new BookEmotionCreateDto(bookId, e.Label, e.Score))
            .ToList();

        return await _bookEmotionService.AddRangeAsync(createDtos, cancellationToken);
    }
}
