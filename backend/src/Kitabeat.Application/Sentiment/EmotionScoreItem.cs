namespace Kitabeat.Application.Sentiment;

/// <summary>
/// Represents a single emotion label with its confidence score.
/// </summary>
public sealed record EmotionScoreItem(string Label, double Score);
