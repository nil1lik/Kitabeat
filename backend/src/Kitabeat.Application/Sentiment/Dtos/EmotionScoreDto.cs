namespace Kitabeat.Application.Sentiment.Dtos;

/// <summary>
/// Represents an emotion label with its score.
/// </summary>
public sealed record EmotionScoreDto(string Label, double Score);
