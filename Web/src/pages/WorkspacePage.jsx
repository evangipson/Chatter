import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { WorkspaceAPI } from "../assets/js/api/workspace";
import { useWorkspace } from '../assets/js/hooks/workspace';
import EditorArea from '../components/editor/EditorArea';
import FileExplorer from '../components/workspace/FileExplorer';
import '../assets/css/workspace.css';

/** Renders a page for a workspace. */
export default function Workspace() {
    const [collapsedFolders, setCollapsedFolders] = useState(() => ({}));
    const {id} = useParams();
    const workspace = useWorkspace(id);

    const toggleFolder = path => {
        setCollapsedFolders(prev => {
            const currentlyCollapsed = prev[path] ?? true;
            return {...prev, [path]: !currentlyCollapsed};
        });
    };

    if (!id) {
        return (<div><p>No workspace selected</p></div>);
    }

    return (
        <div className='chatter__workspace'>
            <div className='chatter__file-explorer'>
                <FileExplorer workspaceId={id} onOpenFile={workspace.openFile} collapsedFolders={collapsedFolders} onToggleFolder={toggleFolder} />
            </div>
            <div className='chatter__workspace-editor'>
                <EditorArea workspace={workspace} />
            </div>
        </div>
    );
}