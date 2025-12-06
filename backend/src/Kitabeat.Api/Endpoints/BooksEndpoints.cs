using Kitabeat.Domain.Books;
using Kitabeat.Application.Books;

namespace Kitabeat.Api.Endpoints;

public static class BooksEndpoints
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/books/search", async (
            string? query,
            IBookRepository bookRepository,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // İleride AllBooksQuery diye ayrı bir şey de yapabilirsin
                return Results.Ok(Array.Empty<Book>());
            }

            var books = await bookRepository.SearchAsync(query, 50, cancellationToken);

            return Results.Ok(books); // ileride DTO'ya mapleriz
        });

        return app;
    }
}
