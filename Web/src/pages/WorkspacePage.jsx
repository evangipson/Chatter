import { useState } from "react";
import { useParams } from "react-router-dom";
import FileExplorer from '../components/workspace/FileExplorer';
import { WorkspaceAPI } from "../assets/js/api/workspace";
import '../assets/css/workspace.css';

/** Renders a page for a workspace. */
export default function Workspace() {
    const [openFile, setOpenFile] = useState(null);
    const [content, setContent] = useState("");
    const {id} = useParams();

    const handleOpen = async (path) => {
        const file = await WorkspaceAPI.open(id, path);
        setOpenFile(file.path);
        setContent(file.content);
    };

    const handleSave = async () => {
        if (!openFile) {
            return;
        }
        await WorkspaceAPI.save(id, openFile, content);
    };

    if (!id) {
        return <div><p>No workspace selected</p></div>;
    }

    return (
        <div className='chatter__workspace'>
            <h1>Workspace {id}</h1>
            <div className='chatter__workspace-file-area'>
                <div className='chatter__file-explorer'>
                    <FileExplorer workspaceId={id} onOpenFile={handleOpen}/>
                </div>
                <div style={{ flex: 1 }}>
                    <textarea style={{ width: "100%", height: "90%" }} value={content} onChange={(e) => setContent(e.target.value)}/>
                    <button onClick={handleSave}>Save</button>
                </div>
            </div>
        </div>
    );
}