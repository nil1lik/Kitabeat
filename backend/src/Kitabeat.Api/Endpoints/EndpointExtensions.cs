namespace Kitabeat.Api.Endpoints;

/// <summary>
/// Extension methods for registering all API endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Maps all API endpoints.
    /// </summary>
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapBookEndpoints();
        app.MapBookEmotionEndpoints();
        
        return app;
    }
}
