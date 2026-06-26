/** A collection of conversation API endpoints. */
export const ConversationAPI = {
    /**
     * Gets all conversations for a workspace.
     * @param {string} workspaceId The workspace identifier.
     * @returns A collection of all conversations for a bot.
     */
    get: async (workspaceId) => {
        const res = await fetch(`/conversations/${workspaceId}`);
        return res.json();
    },

    /**
     * Creates a conversation for a workspace.
     * @param {string} workspaceId The workspace identifier.
     * @returns The new workspace conversation.
     */
    create: async (workspaceId) => {
        const res = await fetch(`/conversations`, {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify({id: workspaceId})});
        return res.json();
    },

    /**
     * Deletes a conversation by `id`.
     * @param {string} id The identifier for the conversation to delete.
     */
    delete: async (id) => {
        await fetch(`/conversations/${id}`, {method: 'DELETE'});
    }
};