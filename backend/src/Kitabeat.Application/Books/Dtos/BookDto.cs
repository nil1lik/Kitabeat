namespace Kitabeat.Application.Books.Dtos;

/// <summary>
/// Represents a book entity as a DTO.
/// </summary>
public sealed record BookDto(
    Guid Id,
    string Title,
    string Author,
    string Description);

