import { readNdJsonStream } from '../services/json';

/** A set of endpoints for interacting with chats. */
export const ChatAPI = {
    /**
     * Sends a message to the chat backend and streams events as they arrive.
     * @param {Object} request
     * @param {(event: Object) => void} onEvent
     * @returns {Promise<void>}
     */
    async respond(request, onEvent) {
        const response = await fetch('/chat/respond', {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(request)});
        if (!response.ok) {
            throw new Error(`Chat request failed (${response.status})`);
        }
        await readNdJsonStream(response, onEvent);
    },

    /**
     * Loads the message history for a conversation.
     * @param {string} conversationId
     * @returns {Promise<Object[]>}
     */
    async getHistory(conversationId) {
        const response = await fetch(`/chat/history/${conversationId}`);
        if (!response.ok) {
            throw new Error(`Unable to load history (${response.status})`);
        }
        return await response.json();
    },
};