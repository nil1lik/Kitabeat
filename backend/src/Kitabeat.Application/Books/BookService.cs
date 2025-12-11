using Kitabeat.Application.Books.Dtos;
using Kitabeat.Domain.Books;

namespace Kitabeat.Application.Books;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

        return book is null
            ? null
            : MapToDto(book);
    }

    public async Task<IReadOnlyList<BookDto>> SearchAsync(
        string query,
        int take = 20,
        CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.SearchAsync(query, take, cancellationToken);

        return books
            .Select(MapToDto)
            .ToList();
    }

    private static BookDto MapToDto(Book entity) =>
        new(entity.Id, entity.Title, entity.Author, entity.Description);
}
