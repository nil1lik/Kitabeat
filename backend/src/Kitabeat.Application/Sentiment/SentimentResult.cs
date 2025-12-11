namespace Kitabeat.Application.Sentiment;

/// <summary>
/// Result from the sentiment analysis service.
/// </summary>
public sealed record SentimentResult(
    string PrimaryLabel,
    double PrimaryScore,
    IReadOnlyList<EmotionScoreItem> Emotions);

