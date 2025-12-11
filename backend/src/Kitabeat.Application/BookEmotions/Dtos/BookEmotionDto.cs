namespace Kitabeat.Application.BookEmotions.Dtos;

/// <summary>
/// Represents a book emotion entity as a DTO.
/// </summary>
public sealed record BookEmotionDto(
    Guid Id,
    Guid BookId,
    string Label,
    double Score);