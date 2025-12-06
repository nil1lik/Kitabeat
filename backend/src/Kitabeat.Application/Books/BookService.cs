using Kitabeat.Domain.Books;

namespace Kitabeat.Application.Books;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IReadOnlyList<BookDto>> SearchAsync(
        string query,
        int take = 20,
        CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.SearchAsync(query, take, cancellationToken);

        return books
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description
            })
            .ToList();
    }

    public async Task SeedIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        var hasAny = await _bookRepository.AnyAsync(cancellationToken);
        if (hasAny)
            return;

        var books = new List<Book>
        {
            new Book("The Hobbit", "J.R.R. Tolkien",
                "A fantasy adventure about a hobbit who goes on an unexpected journey."),
            new Book("1984", "George Orwell",
                "A dystopian novel about surveillance and totalitarian control."),
            new Book("The Little Prince", "Antoine de Saint-Exupéry",
                "A poetic tale about loneliness, love, friendship, and loss."),
            new Book("Sapiens", "Yuval Noah Harari",
                "A brief history of humankind exploring cognition, agriculture, and unification.")
        };

        await _bookRepository.AddRangeAsync(books, cancellationToken);
    }
}
