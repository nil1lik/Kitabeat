namespace Kitabeat.Application.Sentiment;

public interface ISentimentClient
{
    Task<SentimentResult> AnalyzeAsync(string text, CancellationToken cancellationToken = default);
}
