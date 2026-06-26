# Chatter
An artificial intelligence agent with continually improving context and sandboxed code execution.

<div style='display:flex;flex-flow:row nowrap;gap:16px;max-width:1100px;min-height:fit-content;'>
<div style='flex:1;align-self:center'>
    <h1>Chat Interface</h1>
    <p>Chatter uses <a href="https://ollama.com/">Ollama</a> to run local large language models.</p>
    <p>Any large language model can be used.</p>
</div>
<div style='display:flex;max-width:580px;border-radius:8px;overflow:hidden;'>

![A screenshot of Chatter's chat interface](./assets/img/chat-interface.png)
</div>
</div>
<div style='display:flex;flex-flow:row nowrap;gap:16px;max-width:1100px;'>
<div style='display:flex;max-width:540px;border-radius:8px;overflow:hidden;'>

![A screenshot of Chatter's code editor](./assets/img/code-editor.png)
</div>
<div style='flex:1;align-self:center'>
    <h1>Code Editor</h1>
    <p>Chatter uses the <a href="https://microsoft.github.io/monaco-editor/">Monaco Editor</a> to give that familiar editing experience out of the box.</p>
    <p>Get started by uploading an archive of a solution, or ask Chatter to build an app.</p>
</div>
</div>

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