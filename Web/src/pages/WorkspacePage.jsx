import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { ConversationAPI } from '../assets/js/api/conversation';
import { WorkspaceAPI } from "../assets/js/api/workspace";
import { useWorkspace } from '../assets/js/hooks/workspace';
import { useConversation } from '../assets/js/hooks/conversation';
import ChatWindow from '../components/chat/ChatWindow';
import EditorArea from '../components/editor/EditorArea';
import FileExplorer from '../components/workspace/FileExplorer';
import SplitPane from '../components/panes/SplitPane';
import '../assets/css/workspace.css';

/** Renders a page for a workspace. */
export default function Workspace() {
    const {id} = useParams();
    const [collapsedFolders, setCollapsedFolders] = useState(() => ({}));
    const {conversationId, setConversationId} = useConversation(id);
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
        <SplitPane className='chatter__workspace' initialSize={180} min={120} max={400} storageKey='workspace-sidebar'>
            <div className='chatter__file-explorer'>
                <FileExplorer workspaceId={id} onOpenFile={workspace.openFile} collapsedFolders={collapsedFolders} onToggleFolder={toggleFolder} />
            </div>
            <SplitPane className='chatter__editor-pane' direction='vertical' resizeSecond={true} storageKey='editor-chat' initialSize={92} min={92} max={9999}>
                <div className='chatter__workspace-editor'>
                    <EditorArea workspace={workspace} />
                </div>
                {conversationId && (<ChatWindow workspaceId={id} conversationId={conversationId} />)}
            </SplitPane>
        </SplitPane>
    );
}