using System.Linq.Expressions;

namespace Kitabeat.Domain.Books;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Book>> SearchAsync(
        string query,
        int take = 20,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<Book> books, CancellationToken cancellationToken = default);

    // İleride fazladan metotlar ekleyebilirsin (pagination vs.)
}
