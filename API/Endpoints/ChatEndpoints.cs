using System.Net.Mime;
using System.Text.Json;
using Application.Chat;
using Application.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

/// <summary>
/// A <see langword="static"/> collection of chat endpoints.
/// </summary>
internal static class ChatEndpoints
{
    private const string _ndJsonContentType = "application/x-ndjson";

    extension(WebApplication app)
    {
        /// <summary>
        /// Maps all chat endpoints.
        /// </summary>
        /// <returns>The mapped <see cref="WebApplication"/>.</returns>
        internal WebApplication MapChatEndpoints()
        {
            var routeGroup = app.MapGroup("/chat")
                .WithTags("Chat");

            routeGroup.MapGet("/history/{conversationId:guid}", async (Guid conversationId, HttpContext context, [FromServices] IConversationRepository repo) =>
            {
                var messages = await repo.GetMessagesAsync(conversationId);
                return messages.Select(m => new { role = m.Role, text = m.Text });
            }).Produces<IEnumerable<object>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            routeGroup.MapPost("/respond", async (HttpContext context, [FromBody] ChatRequest request, [FromServices] IChatService chatService) =>
            {
                context.Response.ContentType = _ndJsonContentType;
                await foreach (var agentEvent in chatService.RespondAsync(request))
                {
                    var response = JsonSerializer.Serialize(agentEvent, agentEvent.GetType(), JsonSerializerOptions.Web);
                    await context.Response.WriteAsync(response);
                    await context.Response.WriteAsync("\n");
                    await context.Response.Body.FlushAsync();
                }
            }).Produces<IAsyncEnumerable<string>>(StatusCodes.Status200OK, _ndJsonContentType).WithRequestTimeout(TimeSpan.FromMinutes(10));

            return app;
        }
    }
}
