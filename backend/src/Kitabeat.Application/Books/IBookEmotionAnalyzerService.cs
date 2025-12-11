using Kitabeat.Application.Books.Dtos;

namespace Kitabeat.Application.Books;

/// <summary>
/// Service for analyzing emotions in books.
/// </summary>
public interface IBookEmotionAnalyzerService
{
    /// <summary>
    /// Analyzes the emotion of a book based on its description.
    /// </summary>
    Task<BookEmotionAnalysisResultDto?> AnalyzeBookEmotionAsync(Guid bookId, CancellationToken cancellationToken = default);
}
