import { useEffect, useState } from 'react';
import { WorkspaceAPI } from "../../assets/js/api/workspace";
import TreeNode from './TreeNode';

/** Renders a file explorer, filled with directories and files. */
export default function FileExplorer({workspaceId, onOpenFile}) {
    const [tree, setTree] = useState(null);

    useEffect(() => {
        if (!workspaceId) {
            return;
        }

        let ignore = false;
        async function getWorkspace() {
            const workspaceResponse = await WorkspaceAPI.get(workspaceId);
            !ignore && setTree(workspaceResponse);
        };

        getWorkspace();
        // prevent race conditions
        return () => {ignore = true};
    }, [workspaceId]);

    return (
        <div style={{ fontFamily: "monospace" }}>
            {!!tree
                ? (<TreeNode node={tree} onOpen={onOpenFile} />)
                : (<div>Loading files...</div>)}
        </div>
    );
}