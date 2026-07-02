import '../../assets/css/agent.css';

/** Renders the status of an agent, renders nothing if `agent` is falsy. */
export default function AgentStatus({agent}) {
    if (!agent) {
        return;
    }

    return (
        <div className='chatter__chat-message--agent'>
            <p>{agent.text}</p>
            {agent.tools?.map(tool => (<span key={tool.id}>{tool.name} ({tool.status})</span>))}
        </div>
    );
}