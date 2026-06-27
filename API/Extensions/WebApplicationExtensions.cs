using API.Endpoints;

namespace API.Extensions;

/// <summary>
/// A <see langword="static"/> collection of methods to extend <see cref="WebApplication"/>.
/// </summary>
internal static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        /// <summary>
        /// Maps all the application's API endpoints.
        /// </summary>
        /// <returns>The <see cref="WebApplication"/> with all API endpoints mapped.</returns>
        internal WebApplication MapEndpoints() => app
            .MapChatEndpoints()
            .MapWorkspaceEndpoints()
            .MapConversationEndpoints();
    }
}
