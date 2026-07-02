import { v4 as uuidv4 } from 'uuid';
import { ChatFactory } from '../factories/chat-factory';

const updateLast = (messages, updater) => {
    if (!messages.length) {
        return messages;
    }
    const updated = [...messages];
    updated[updated.length - 1] = updater(updated.at(-1));
    return updated;
};

const updateLastAgentMessage = (messages, updater) => {
    const updated = [...messages];
    for (let i = updated.length - 1; i >= 0; i--) {
        if (updated[i].agent) {
            updated[i] = updater(updated[i]);
            return updated;
        }
    }
    return updated;
};

export const MessageReducer = (messages, event) => {
    switch (event.type) {
        case 'history_loaded':
            return event.messages;
        case 'user_message':
            return [...messages, event.message];
        case 'agent_started':
            return [...messages, ChatFactory.createAgentMessage('🤖 Agent thinking...', true)];
        case 'tool_started':
            return updateLastAgentMessage(messages, message => ({
                ...message,
                agent: {
                    ...message.agent,
                    tools: [
                        ...(message.agent.tools ?? []),
                        {
                            id: uuidv4(),
                            name: event.toolName,
                            status: 'running'
                        }
                    ]
                }
            }));
        case 'tool_finished':
            return updateLastAgentMessage(messages, message => {
                const tools = [...(message.agent.tools ?? [])];
                if (tools.length) {
                    tools[tools.length - 1] = {
                        ...tools[tools.length - 1],
                        status: 'finished',
                        duration: event.duration
                    };
                }
                return {...message, agent: {...message.agent, tools}};
            });
        case 'agent_finished':
            return updateLastAgentMessage(messages, message => ({
                ...message,
                agent: {...message.agent, thinking: false, text: `🤖 Agent finished (${event.duration})`},
            }));
        case 'assistant_started':
            return [...messages, ChatFactory.createAssistantMessage()];

        case 'assistant_token':
            return updateLast(messages, message => ({
                ...message,
                text: (message.text ?? '') + event.text
            }));
        case 'assistant_finished':
            return messages;
        default:
            return messages;
    }
};