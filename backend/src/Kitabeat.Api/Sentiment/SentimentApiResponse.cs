using System.Text.Json.Serialization;

namespace Kitabeat.Api.Sentiment;

/// <summary>
/// Response from the external sentiment analysis API.
/// </summary>
internal sealed record SentimentApiResponse(
    [property: JsonPropertyName("primary_label")] string PrimaryLabel,
    [property: JsonPropertyName("primary_score")] double PrimaryScore,
    [property: JsonPropertyName("emotions")] List<SentimentApiEmotion> Emotions);

/// <summary>
/// Emotion item from the sentiment API response.
/// </summary>
internal sealed record SentimentApiEmotion(
    [property: JsonPropertyName("label")] string Label,
    [property: JsonPropertyName("score")] double Score);
