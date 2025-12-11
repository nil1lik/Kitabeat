using Kitabeat.Domain.BookEmotions;

namespace Kitabeat.Domain.Books;

public class Book
{
    private readonly List<BookEmotion> _emotions;

    private Book()
    {
        _emotions = new List<BookEmotion>();
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = default!;
    public string Author { get; private set; } = default!;
    public string Description { get; private set; } = default!;

    public IReadOnlyCollection<BookEmotion> Emotions => _emotions.AsReadOnly();

    public Book(string title, string author, string description)
    {
        Title = title.Trim();
        Author = author.Trim();
        Description = description.Trim();
        Id = Guid.NewGuid();
        _emotions = new List<BookEmotion>();
    }

    public void SetEmotions(IEnumerable<BookEmotion> emotions)
    {
        _emotions.Clear();
        _emotions.AddRange(emotions);
    }
}
