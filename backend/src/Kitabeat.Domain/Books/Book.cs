namespace Kitabeat.Domain.Books;

public class Book
{
    private Book() { }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = default!;
    public string Author { get; private set; } = default!;
    public string Description { get; private set; } = default!;

    public Book(string title, string author, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title.Trim();
        Author = author?.Trim() ?? string.Empty;
        Description = description ?? string.Empty;
        Id = Guid.NewGuid();
    }
}
