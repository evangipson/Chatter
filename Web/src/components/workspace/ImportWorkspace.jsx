import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { WorkspaceAPI } from "../../assets/js/api/workspace";

/** Renders a component to import a ZIP file of a workspace. */
export default function ImportWorkspace() {
    const [loading, setLoading] = useState(false);

    /**
     * Handles `.zip` file uploads.
     * @param {ChangeEvent} changeEvent 
     */
    const onChange = async (changeEvent) => {
        const file = changeEvent.target.files?.[0];
        if (!file) {
            return;
        }
        setLoading(true);
        try {
            // reach out to the workspace API to handle the ZIP upload
            const result = await WorkspaceAPI.import(file);

            // set in local storage so this workspace is loaded next time
            localStorage.setItem("activeWorkspace", JSON.stringify(result));

            // route the resulting workspace id to the dynamic workspace page
            useNavigate(`/workspace/${result.workspaceId}`);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <input type="file" accept=".zip" onChange={onChange} />
            {loading && <p>Importing...</p>}
        </div>
    );
}