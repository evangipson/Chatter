using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Application.Repositories;
using Domain.Models;

namespace API.Endpoints;

/// <summary>
/// A <see langword="static"/> collection of conversation endpoints.
/// </summary>
internal static class ConversationEndpoints
{
    extension(WebApplication app)
    {
        /// <summary>
        /// Maps all conversation endpoints.
        /// </summary>
        /// <returns>The mapped <see cref="WebApplication"/>.</returns>
        internal WebApplication MapConversationEndpoints()
        {
            var routeGroup = app.MapGroup("/conversations")
                .WithTags("Conversations");

            routeGroup.MapGet("/{id:guid}", ([FromRoute] string id, [FromServices] IConversationRepository repo) => repo.GetByWorkspaceId(Guid.Parse(id)))
                .Produces<List<ConversationEntity>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            routeGroup.MapPost("/", ([FromBody] CreateChatRequest request, [FromServices] IConversationRepository repo) => repo.CreateAsync(Guid.Parse(request.Id)))
                .Produces<ConversationEntity>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            routeGroup.MapDelete("/{conversationId:guid}", (Guid conversationId, [FromServices] IConversationRepository repo) => repo.DeleteAsync(conversationId).OkAsync())
                .Produces<IResult>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            return app;
        }
    }

    public sealed class CreateChatRequest
    {
        public string Id { get; set; } = string.Empty;
    }
}
