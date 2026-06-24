using System.IO.Compression;
using System.Net.Mime;
using API.Extensions;
using Application.Repositories;
using Application.Workspaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

/// <summary>
/// A <see langword="static"/> collection of workspace endpoints.
/// </summary>
public static class WorkspaceEndpoints
{
    extension(WebApplication app)
    {
        /// <summary>
        /// Maps all workspace endpoints.
        /// </summary>
        /// <returns>The mapped <see cref="WebApplication"/>.</returns>
        internal WebApplication MapWorkspaceEndpoints()
        {
            var routeGroup = app.MapGroup("/workspaces")
                .WithTags("Workspaces");

            routeGroup.MapPost("/import", async ([FromForm] IFormFile file, [FromServices] IWorkspaceService workspaceService, [FromServices] IConversationRepository repo) =>
            {
                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest("No file uploaded.");
                }

                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var workspace = await workspaceService.CreateAsync(fileName);
                var zipPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");
                await using (var stream = File.Create(zipPath))
                {
                    await file.CopyToAsync(stream);
                }

                ZipFile.ExtractToDirectory(zipPath, workspace.RootPath, overwriteFiles: true);
                File.Delete(zipPath);
                var conversation = await repo.CreateAsync(workspace.Id);
                await repo.TouchAsync(conversation.Id);
                return Results.Ok(new {workspaceId = workspace.Id, conversationId = conversation.Id});
            }).Produces<object>(StatusCodes.Status200OK, MediaTypeNames.Application.Json).DisableAntiforgery();

            routeGroup.MapGet("/{id:guid}/tree", async (Guid id, [FromServices] IWorkspaceRepository repo) =>
            {
                var workspace = await repo.GetByIdAsync(id);
                return workspace is null
                    ? Results.NotFound()
                    : Results.Ok(workspace.RootPath.BuildTree());
            }).Produces<object?>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            routeGroup.MapGet("/{id:guid}/file", async (Guid id, [FromQuery] string path, [FromServices] IWorkspaceRepository repo) =>
            {
                var workspace = await repo.GetByIdAsync(id);
                if (workspace == null)
                {
                    return Results.NotFound();
                }

                var fullPath = Path.Combine(workspace.RootPath, path);
                if (!File.Exists(fullPath))
                {
                    return Results.NotFound();
                }

                var content = await File.ReadAllTextAsync(fullPath);
                return Results.Ok(new { path, content });
            }).Produces<object>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            routeGroup.MapPut("/{id:guid}/file", async (Guid id, [FromBody] CreateFileRequest request, IWorkspaceRepository repo) =>
            {
                var workspace = await repo.GetByIdAsync(id);
                if (workspace == null)
                {
                    return Results.NotFound();
                }

                var fullPath = Path.Combine(workspace.RootPath, request.Path);
                var directory = Path.GetDirectoryName(fullPath);
                if (directory != null)
                {
                    Directory.CreateDirectory(directory);
                }

                await File.WriteAllTextAsync(fullPath, request.Content);
                return Results.Ok();
            }).Produces<IResult>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            // TODO: retire this endpoint when workspaces no longer need debugging
            routeGroup.MapGet("/{id}/debug", async ([FromRoute] Guid id, [FromServices] IWorkspaceRepository repo) =>
            {
                var workspace = await repo.GetByIdAsync(id);
                return workspace is null
                    ? Results.NotFound()
                    : Results.Ok(new {workspace.Id, workspace.RootPath, Files = Directory.GetFiles(workspace.RootPath, "*", SearchOption.AllDirectories)});
            }).Produces<object>(StatusCodes.Status200OK, MediaTypeNames.Application.Json);

            return app;
        }
    }
}
