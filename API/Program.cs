using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using API.Extensions;
using Application.Agent;
using Application.Chat;
using Application.Clients;
using Application.Context;
using Application.Conversation;
using Application.Repositories;
using Application.Speak;
using Application.Tool;
using Application.Workspaces;
using Domain.Options;
using Infrastructure.Contexts;
using Infrastructure.Repositories;

// create the builder
var builder = WebApplication.CreateBuilder(args);

// listen on port 5500 across the environment
builder.WebHost.ConfigureKestrel(x => x.ListenAnyIP(5500));

// add all appsettings.json sections as IOptions<T>
builder.Services.AddOptions<WorkspaceSettings>();

// add core application services to the services container
builder.Services.AddSingleton<IChatClient>(_ => HttpClients.ApiClient)
    .AddDbContext<ChatDbContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")))
    .AddScoped<IConversationRepository, ConversationRepository>()
    .AddScoped<IWorkspaceRepository, WorkspaceRepository>()
    .AddScoped<IWorkspaceFileSystem, WorkspaceFileSystem>()
    .AddScoped<IConversationService, ConversationService>()
    .AddScoped<IWorkspaceService, WorkspaceService>()
    .AddScoped<ISpeakingService, SpeakingService>()
    .AddScoped<IContextFactory, ContextFactory>()
    .AddScoped<IAgentRunner, AgentRunner>()
    .AddScoped<IChatService, ChatService>()
    .AddScoped<ITool, SearchFilesTool>()
    .AddScoped<ITool, SearchTextTool>()
    .AddScoped<ITool, RunCommandTool>()
    .AddScoped<ITool, WriteFileTool>()
    .AddScoped<ITool, ListFilesTool>()
    .AddScoped<ITool, ReadFileTool>()
    .AddScoped<ToolRegistry>()
    .AddOpenApi();

// build the web application
var app = builder.Build();

// serve out the built react
app.UseDefaultFiles().UseStaticFiles();
app.MapFallbackToFile("index.html");

// configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// map all the endpoints, then run the web application
await app.MapEndpoints().RunAsync();