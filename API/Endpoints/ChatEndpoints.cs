using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Application.Chat;
using Application.Repositories;
using Domain.Models;

namespace API.Endpoints;

/// <summary>
/// A <see langword="static"/> collection of chat endpoints.
/// </summary>
internal static class ChatEndpoints
{
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
                await foreach (var token in chatService.RespondAsync(request))
                {
                    await context.Response.WriteAsync(token);
                    await context.Response.Body.FlushAsync();
                }
            }).Produces<IAsyncEnumerable<string>>(StatusCodes.Status200OK, MediaTypeNames.Text.Plain).WithRequestTimeout(TimeSpan.FromMinutes(5));

            return app;
        }
    }
}
