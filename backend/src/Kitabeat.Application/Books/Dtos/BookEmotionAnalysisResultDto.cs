using Kitabeat.Application.BookEmotions.Dtos;

namespace Kitabeat.Application.Books.Dtos;

/// <summary>
/// Result DTO for book emotion analysis endpoint.
/// </summary>
public sealed record BookEmotionAnalysisResultDto(
    Guid BookId,
    string Title,
    string Author,
    string PrimaryLabel,
    double PrimaryScore,
    IReadOnlyList<BookEmotionDto> Emotions);
