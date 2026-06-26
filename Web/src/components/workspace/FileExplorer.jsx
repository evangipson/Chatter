import { useEffect, useState } from 'react';
import { WorkspaceAPI } from "../../assets/js/api/workspace";
import TreeNode from './TreeNode';

/** Renders a file explorer, filled with directories and files. */
export default function FileExplorer({workspaceId, collapsedFolders, onToggleFolder, onOpenFile}) {
    const [tree, setTree] = useState(null);

    useEffect(() => {
        if (!workspaceId) { return; }
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
        <div className='chatter__tree'>
            {!!tree
                ? (<TreeNode node={tree} onOpen={onOpenFile} onToggleFolder={onToggleFolder} collapsedFolders={collapsedFolders} isRoot={true} depth={1} />)
                : (<div>Loading files...</div>)}
        </div>
    );
}