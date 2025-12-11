namespace Kitabeat.Application.Sentiment.Dtos;

/// <summary>
/// Result DTO for sentiment analysis containing primary emotion and all detected emotions.
/// </summary>
public sealed record SentimentAnalysisResultDto(
    string PrimaryLabel,
    double PrimaryScore,
    IReadOnlyList<EmotionScoreDto> Emotions);
