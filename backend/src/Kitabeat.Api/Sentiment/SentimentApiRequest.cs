namespace Kitabeat.Api.Sentiment;

/// <summary>
/// Request payload for sentiment analysis API.
/// </summary>
internal sealed record SentimentApiRequest(string Text);
