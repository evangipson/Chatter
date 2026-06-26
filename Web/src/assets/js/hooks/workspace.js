import { useState, useEffect } from "react";
import { WorkspaceAPI } from '../api/workspace';

export function useWorkspace(workspaceId) {
    const [openFiles, setOpenFiles] = useState([]);
    const [activeFile, setActiveFile] = useState(null);

    // save the files in a debounced fashion when editor has dirty files
    useEffect(() => {
        const dirtyFiles = openFiles.filter(f => f.dirty);
        if (!dirtyFiles.length) {
            return;
        }
        const timer = setTimeout(() => dirtyFiles.forEach(f => saveFile(f.path)), 750);
        return () => clearTimeout(timer);
    }, [openFiles]);

    // turn on ctrl+s functionality to save files
    useEffect(() => {
        function onKeyDown(e) {
            if (e.ctrlKey && e.key === "s") {
                e.preventDefault();
                if (activeFile) {
                    saveFile(activeFile);
                }
            }
        }
        window.addEventListener("keydown", onKeyDown);
        return () => window.removeEventListener("keydown", onKeyDown);
    }, [activeFile, openFiles]);

    // open file
    async function openFile(path) {
        const existing = openFiles.find(f => f.path === path);
        if (existing) {
            setActiveFile(path);
            return;
        }

        const file = await WorkspaceAPI.open(workspaceId, path);
        const newFile = {
            path: file.path || path,
            name: path.split("\\").pop(),
            content: file.content,
            originalContent: file.content,
            dirty: false,
            saving: false
        };

        setOpenFiles(prev => [...prev, newFile]);
        setActiveFile(path);
    }

    // update file content (typing in editor)
    function updateFile(path, content) {
        setOpenFiles(prev =>
            prev.map(file => {
                if (file.path !== path) return file;
                return {...file, content, dirty: content !== file.originalContent};
            })
        );
    }

    // save file
    async function saveFile(path) {
        const file = openFiles.find(f => f.path === path);
        if (!file) {
            return;
        }
        setOpenFiles(prev => prev.map(f => f.path === path ? { ...f, saving: true } : f));
        await WorkspaceAPI.save(workspaceId, path, file.content);
        setOpenFiles(prev =>
            prev.map(f => {
                if (f.path !== path) {
                    return f;
                }
                return {...f, saving: false, dirty: false,  originalContent: f.content};
            })
        );
    }

    // close file
    function closeFile(path) {
        setOpenFiles(prev => {
            const filtered = prev.filter(f => f.path !== path);
            if (activeFile === path) {
                const last = filtered[filtered.length - 1];
                setActiveFile(last ? last.path : null);
            }
            return filtered;
        });
    }

    return {
        workspaceId,
        openFiles,
        activeFile,
        openFile,
        updateFile,
        saveFile,
        closeFile,
        setActiveFile,
    };
}