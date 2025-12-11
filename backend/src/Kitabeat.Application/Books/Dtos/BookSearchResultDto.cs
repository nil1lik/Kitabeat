namespace Kitabeat.Application.Books.Dtos;

/// <summary>
/// Represents a book search result with optional emotion data.
/// </summary>
public sealed record BookSearchResultDto(
    Guid Id,
    string Title,
    string Author,
    string Description,
    string? EmotionLabel = null,
    double? EmotionScore = null);
