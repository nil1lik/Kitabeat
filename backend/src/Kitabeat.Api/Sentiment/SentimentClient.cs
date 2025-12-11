using Kitabeat.Application.Sentiment;

namespace Kitabeat.Api.Sentiment;

/// <summary>
/// HTTP client for communicating with the sentiment analysis service.
/// </summary>
public sealed class SentimentClient : ISentimentClient
{
    private readonly HttpClient _httpClient;

    public SentimentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SentimentResult> AnalyzeAsync(string text, CancellationToken cancellationToken = default)
    {
        var request = new SentimentApiRequest(text);

        using var response = await _httpClient.PostAsJsonAsync("/analyze", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content
            .ReadFromJsonAsync<SentimentApiResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Empty response from sentiment service.");

        return MapToResult(apiResponse);
    }

    private static SentimentResult MapToResult(SentimentApiResponse apiResponse)
    {
        var emotions = apiResponse.Emotions
            .Select(e => new EmotionScoreItem(e.Label.ToLowerInvariant(), e.Score))
            .ToList();

        var (primaryLabel, primaryScore) = DeterminePrimaryEmotion(emotions);

        return new SentimentResult(primaryLabel, primaryScore, emotions);
    }

    private static (string Label, double Score) DeterminePrimaryEmotion(IReadOnlyList<EmotionScoreItem> emotions)
    {
        if (emotions.Count == 0)
        {
            return ("neutral", 0.0);
        }

        // Prefer non-neutral emotions
        var nonNeutralPrimary = emotions
            .Where(e => !string.Equals(e.Label, "neutral", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.Score)
            .FirstOrDefault();

        if (nonNeutralPrimary is not null)
        {
            return (nonNeutralPrimary.Label, nonNeutralPrimary.Score);
        }

        // Fallback to highest scoring emotion
        var fallbackPrimary = emotions
            .OrderByDescending(e => e.Score)
            .First();

        return (fallbackPrimary.Label, fallbackPrimary.Score);
    }
}