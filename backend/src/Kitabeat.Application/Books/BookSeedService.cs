using System.Text.Json;
using Kitabeat.Application.Books.Dtos;
using Kitabeat.Domain.Books;

namespace Kitabeat.Application.Books;

/// <summary>
/// Service responsible for seeding book data from JSON.
/// </summary>
public sealed class BookSeedService : IBookSeedService
{
    private readonly IBookRepository _bookRepository;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BookSeedService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<bool> HasDataAsync(CancellationToken cancellationToken = default)
    {
        return await _bookRepository.AnyAsync(cancellationToken);
    }

    public async Task<BookSeedResult> SeedFromJsonAsync(string jsonContent, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return new BookSeedResult(false, "JSON content is empty.", 0);
        }

        var seedDtos = DeserializeBooks(jsonContent);

        if (seedDtos is null || seedDtos.Count == 0)
        {
            return new BookSeedResult(false, "No books found in JSON content.", 0);
        }

        var books = CreateBooksFromDtos(seedDtos);

        if (books.Count == 0)
        {
            return new BookSeedResult(false, "No valid books found in JSON content.", 0);
        }

        await _bookRepository.AddRangeAsync(books, cancellationToken);

        return new BookSeedResult(true, "Books seeded successfully.", books.Count);
    }

    private static List<BookSeedDto>? DeserializeBooks(string jsonContent)
    {
        try
        {
            return JsonSerializer.Deserialize<List<BookSeedDto>>(jsonContent, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static List<Book> CreateBooksFromDtos(List<BookSeedDto> dtos)
    {
        return dtos
            .Where(dto => !string.IsNullOrWhiteSpace(dto.Title))
            .Select(dto => new Book(
                dto.Title!,
                dto.Author ?? string.Empty,
                dto.Description ?? string.Empty))
            .ToList();
    }
}
