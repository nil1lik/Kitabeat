using Kitabeat.Domain.Books;

namespace Kitabeat.Domain.BookEmotions;

public class BookEmotion
{
    private BookEmotion() { }

    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public string Label { get; private set; } = default!;
    public double Score { get; private set; }
    public Book Book { get; private set; } = default!;

    public BookEmotion(Guid bookId, string label, double score)
    {
        Id = Guid.NewGuid();
        BookId = bookId;
        Label = label;
        Score = score;
    }
}