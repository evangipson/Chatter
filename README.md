# Chatter
An artificial intelligence agent with continually improving context and sandboxed code execution.

## Features
- Text generation
- Conversational chat
- Image vision
- Multiple conversations
- Persistence layer (currently SQLite)
- Multiple workspaces
- Upload archives to create workspaces
- Sandboxed code environment

## Prerequisites
1. Entity framework
1. Ollama
1. Preferred LLM model(s)

## Getting Started
1. Go to the repository root
1. Run `dotnet ef database update --project Infrastructure --startup-project API`
1. Go to the `Web` directory
1. Run `npm run build` to populate `API/wwwroot` with the built front end assets
1. Run `dotnet run --project API -c Release` to run the backend
1. Navigate to `http://localhost:5500` in the browser