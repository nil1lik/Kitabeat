namespace Kitabeat.Application.Books;

public interface IBookService
{
    Task<IReadOnlyList<BookDto>> SearchAsync(
        string query,
        int take = 20,
        CancellationToken cancellationToken = default);

    Task SeedIfEmptyAsync(CancellationToken cancellationToken = default);
}
