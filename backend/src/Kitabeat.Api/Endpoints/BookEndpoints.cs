using Kitabeat.Application.Books;
using Kitabeat.Application.Books.Dtos;

namespace Kitabeat.Api.Endpoints;

/// <summary>
/// Endpoints for book operations.
/// </summary>
public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books")
            .WithTags("Books");

        group.MapGet("/search", SearchBooks)
            .WithName("SearchBooks")
            .WithDescription("Search books by title or author");

        group.MapPost("/seed", SeedBooks)
            .WithName("SeedBooks")
            .WithDescription("Seed books from JSON file");

        group.MapPost("/{id:guid}/analyze-emotion", AnalyzeEmotion)
            .WithName("AnalyzeBookEmotion")
            .WithDescription("Analyze the emotional content of a book");

        return app;
    }

    private static async Task<IResult> SearchBooks(
        string? query,
        IBookService bookService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Results.Ok(Array.Empty<BookDto>());
        }

        var books = await bookService.SearchAsync(query, 50, cancellationToken);
        return Results.Ok(books);
    }

    private static async Task<IResult> SeedBooks(
        IBookSeedService seedService,
        IWebHostEnvironment env,
        CancellationToken cancellationToken)
    {
        if (await seedService.HasDataAsync(cancellationToken))
        {
            return Results.BadRequest(new { Message = "Books table already has data." });
        }

        var filePath = Path.Combine(env.ContentRootPath, "Data", "book-seed.json");

        if (!File.Exists(filePath))
        {
            return Results.NotFound(new { Message = "Seed file not found." });
        }

        var jsonContent = await File.ReadAllTextAsync(filePath, cancellationToken);
        var result = await seedService.SeedFromJsonAsync(jsonContent, cancellationToken);

        if (!result.Success)
        {
            return Results.BadRequest(new { result.Message });
        }

        return Results.Ok(new { result.Message, result.Count });
    }

    private static async Task<IResult> AnalyzeEmotion(
        Guid id,
        IBookEmotionAnalyzerService analyzerService,
        CancellationToken cancellationToken)
    {
        var result = await analyzerService.AnalyzeBookEmotionAsync(id, cancellationToken);

        if (result is null)
        {
            return Results.NotFound(new { Message = "Book not found." });
        }

        return Results.Ok(result);
    }
}
