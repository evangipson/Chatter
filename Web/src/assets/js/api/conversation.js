/** A collection of conversation API endpoints. */
export const ConversationAPI = {
    /**
     * Gets all conversations for bot.
     * @param {string} botId The bot identifier.
     * @returns A collection of all conversations for a bot.
     */
    get: async (botId) => {
        const res = await fetch(`/conversations?botId=${botId}`);
        return res.json();
    },

    /**
     * Creates a conversation for bot.
     * @param {string} botId The bot identifier.
     * @returns The new bot conversation.
     */
    create: async (botId) => {
        const res = await fetch(`/conversations`, {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify({botId})});
        return res.json();
    },

    /**
     * Deletes a conversation for a bot.
     * @param {string} botId The bot identifier.
     */
    delete: async (id) => {
        await fetch(`/conversations/${id}`, {method: 'DELETE'});
    }
};