using Kitabeat.Application.BookEmotions;
using Kitabeat.Application.BookEmotions.Dtos;

namespace Kitabeat.Api.Endpoints;

/// <summary>
/// Endpoints for book emotion operations.
/// </summary>
public static class BookEmotionEndpoints
{
    public static IEndpointRouteBuilder MapBookEmotionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books/emotions")
            .WithTags("BookEmotions");

        group.MapGet("/{id:guid}", GetBookEmotion)
            .WithName("GetBookEmotion")
            .WithDescription("Get a book emotion by ID");

        group.MapPost("/", CreateBookEmotion)
            .WithName("CreateBookEmotion")
            .WithDescription("Create a new book emotion");

        group.MapPost("/batch", CreateBookEmotionRange)
            .WithName("CreateBookEmotionRange")
            .WithDescription("Create multiple book emotions");

        return app;
    }

    private static async Task<IResult> GetBookEmotion(
        Guid id,
        IBookEmotionService bookEmotionService,
        CancellationToken cancellationToken)
    {
        var bookEmotion = await bookEmotionService.GetByIdAsync(id, cancellationToken);

        if (bookEmotion is null)
        {
            return Results.NotFound(new { Message = "Book emotion not found." });
        }

        return Results.Ok(bookEmotion);
    }

    private static async Task<IResult> CreateBookEmotion(
        BookEmotionCreateDto dto,
        IBookEmotionService bookEmotionService,
        CancellationToken cancellationToken)
    {
        var bookEmotion = await bookEmotionService.AddAsync(dto, cancellationToken);
        return Results.Created($"/api/books/emotions/{bookEmotion.Id}", bookEmotion);
    }

    private static async Task<IResult> CreateBookEmotionRange(
        IEnumerable<BookEmotionCreateDto> dtos,
        IBookEmotionService bookEmotionService,
        CancellationToken cancellationToken)
    {
        var bookEmotions = await bookEmotionService.AddRangeAsync(dtos, cancellationToken);
        return Results.Ok(bookEmotions);
    }
}
