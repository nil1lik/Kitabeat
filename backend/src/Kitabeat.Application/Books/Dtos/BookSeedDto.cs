namespace Kitabeat.Application.Books.Dtos;

/// <summary>
/// DTO for seeding books from JSON file.
/// </summary>
public sealed record BookSeedDto(
    string Title,
    string? Author = null,
    string? Description = null);

