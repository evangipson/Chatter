import AgentStatus from './AgentStatus';
import { ChatBubble } from './ChatBubble';

/** Renders a message, which is comprised of a chat bubble and an agent status. */
export default function ChatMessage({role, text, agent}) {
    return (
        <>
            <ChatBubble role={role} text={text} />
            <AgentStatus agent={agent} />
        </>
    );
}