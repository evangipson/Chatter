import { v4 as uuidv4 } from 'uuid';

/**
 * Creates a user message to display in chat.
 * @param {string} message The user's message, defaults to an empty `string`.
 * @returns The `new` user chat message.
 */
const createUserMessage = message => ({
    id: uuidv4(),
    role: 'user',
    text: message ?? '',
});

/**
 * Creates an agent message to display in chat.
 * @param {string} message The agent's message, defaults to an empty `string`.
 * @returns The `new` agent chat message.
 */
const createAgentMessage = message => ({
    id: uuidv4(),
    role: 'agent',
    text: message ?? '',
    agent: {
        thinking: false,
        duration: null,
        tools: [],
    },
});

/** Responsible for providing the means to create chat messages. */
export const ChatFactory = { createUserMessage, createAgentMessage };