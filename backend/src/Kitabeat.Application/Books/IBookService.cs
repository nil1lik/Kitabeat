using Kitabeat.Application.Books.Dtos;

namespace Kitabeat.Application.Books;

public interface IBookService
{
    Task<BookDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BookDto>> SearchAsync(
        string query,
        int take = 20,
        CancellationToken cancellationToken = default);
}
