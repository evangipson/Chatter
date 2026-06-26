using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using OllamaSharp;
using API.Endpoints;
using Application.Chat;
using Application.Context;
using Application.Conversation;
using Application.Repositories;
using Application.Speak;
using Application.Tool;
using Application.Workspaces;
using Domain.Constants;
using Domain.Options;
using Infrastructure.Contexts;
using Infrastructure.Repositories;

// create the builder
var builder = WebApplication.CreateBuilder(args);

// listen on port 5500 across the environment
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(5500));

// add application settings
builder.Services.AddOptions<WorkspaceSettings>();

// add necessary services to the services container
builder.Services.AddDbContext<ChatDbContext>(options => options.UseSqlite("Data Source=chat.db"))
    .AddScoped<IConversationRepository, ConversationRepository>()
    .AddScoped<IWorkspaceRepository, WorkspaceRepository>()
    .AddScoped<IWorkspaceFileSystem, WorkspaceFileSystem>()
    .AddScoped<IConversationService, ConversationService>()
    .AddScoped<IWorkspaceService, WorkspaceService>()
    .AddScoped<ISpeakingService, SpeakingService>()
    .AddScoped<IContextFactory, ContextFactory>()
    .AddScoped<IChatService, ChatService>()
    .AddScoped<ListFilesTool>()
    .AddScoped<ReadFileTool>()
    .AddScoped<WriteFileTool>()
    .AddScoped<SearchFilesTool>()
    .AddScoped<SearchTextTool>()
    .AddScoped<RunCommandTool>()
    .AddScoped<ToolRegistry>()
    .AddSingleton<IChatClient>(_ => new OllamaApiClient(new HttpClient()
    {
        BaseAddress = new Uri(LanguageModelConstants.LanguageModelAddress),
        Timeout = TimeSpan.FromMinutes(20)
    }, LanguageModelConstants.Qwen36Model))
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
await app.MapConversationEndpoints()
    .MapChatEndpoints()
    .MapWorkspaceEndpoints()
    .RunAsync();