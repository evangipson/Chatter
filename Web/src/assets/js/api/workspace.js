/** A collection of workspace API endpoints. */
export const WorkspaceAPI = {
    /**
     * Imports an archive to create a workspace.
     * @param {File} file The archive containing workspace files.
     * @returns An object returning a `conversationId` and `workspaceId`.
     */
    import: async (file) => {
        const res = await fetch("/workspaces/import", {method: "POST", body: new FormData().append("file", file)});
        return await res.json();
    },

    /**
     * Gets an entire workspace as a list of directories and files.
     * @param {string} workspaceId The workspace identifier.
     * @returns An object containing all the files in a workspace.
     */
    get: async (workspaceId) => {
        const res = await fetch(`/workspaces/${workspaceId}/tree`);
        return res.json();
    },

    /**
     * Opens a single file for a workspace.
     * @param {string} workspaceId The workspace identifier.
     * @param {string} path The path of the file to open.
     * @returns The file from a workspace.
     */
    open: async (workspaceId, path) => {
        const res = await fetch(`/workspaces/${workspaceId}/file?path=${encodeURIComponent(path)}`);
        return res.json();
    },

    /**
     * Saves a file to a workspace.
     * @param {string} workspaceId The workspace identifier.
     * @param {string} path The path of the file to save.
     * @param {string} content The content of the file to save.
     * @returns `response.ok` when successful.
     */
    save: async (workspaceId, path, content) => {
        await fetch(`/workspaces/${workspaceId}/file`, {method: "PUT", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ path, content })});
    },
};