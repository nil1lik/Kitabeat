namespace Kitabeat.Application.BookEmotions.Dtos;

/// <summary>
/// DTO for creating a new book emotion.
/// </summary>
public sealed record BookEmotionCreateDto(
    Guid BookId,
    string Label,
    double Score);