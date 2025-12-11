namespace Kitabeat.Application.Books;

/// <summary>
/// Service for seeding initial book data.
/// </summary>
public interface IBookSeedService
{
    /// <summary>
    /// Seeds books from JSON file if database is empty.
    /// </summary>
    Task<BookSeedResult> SeedFromJsonAsync(string jsonContent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if books table has any data.
    /// </summary>
    Task<bool> HasDataAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of book seeding operation.
/// </summary>
public sealed record BookSeedResult(bool Success, string Message, int Count);
