import { useState, useEffect } from 'react';
import { WorkspaceAPI } from '../api/workspace';
import { FileService } from '../services/file';

/**
 * A hook that provides all necessary workspace functionality and is responsible for maintaining it's state.
 * @param {string} workspaceId The identifier of the workspace to hook into.
 */
export const useWorkspace = workspaceId => {
    const [openFiles, setOpenFiles] = useState([]);
    const [activeFile, setActiveFile] = useState(null);

    /** turn on ctrl+s save functionality (look into monaco being able to do this also) */
    useEffect(() => {
        const onKeyDown = e => {
            if (!(e.ctrlKey && e.key === 's')) {
                return;
            }
            e.preventDefault();
            activeFile && saveFile(activeFile);
        }
        window.addEventListener('keydown', onKeyDown);
        return () => window.removeEventListener('keydown', onKeyDown);
    }, [activeFile, openFiles]);

    /**
     * Opens a file (using `path`) and sets it as the currenly active file.
     * @param {string} path The path of the file to open.
     */
    const openFile = async path => {
        /* if there isn't an active file, open the new active file up using `path` */
        if (!openFiles.find(f => f.path === path)) {
            const file = await WorkspaceAPI.open(workspaceId, path);
            setOpenFiles(prev => [...prev, FileService.getFileForEditor(file)]);
        }
        setActiveFile(path);
    };

    /**
     * Updates a file (using `path`) and marks it "dirty" if it's `content` has been updated.
     * @param {string} path The path of the file to update.
     * @param {string} content The current content of the file.
     */
    const updateFile = (path, content) => setOpenFiles(prev => prev.map(file => file.path !== path
        ? file
        : {...file, content, dirty: content !== file.originalContent}));

    /**
     * Saves a file (using `path`).
     * @param {string} path The path of the file to save.
     */
    const saveFile = async path => {
        const file = openFiles.find(f => f.path === path);
        if (!file) {
            return;
        }
        setOpenFiles(prev => prev.map(f => f.path === path ? { ...f, saving: true } : f));
        await WorkspaceAPI.save(workspaceId, path, file.content);
        setOpenFiles(prev => prev.map(f => f.path !== path
            ? f
            : {...f, saving: false, dirty: false, originalContent: f.content}));
    };

    /**
     * Closes a file (using `path`).
     * @param {string} path The path of the file to close.
     */
    const closeFile = path => {
        setOpenFiles(prev => {
            const filtered = prev.filter(f => f.path !== path);
            if (activeFile === path) {
                const last = filtered[filtered.length - 1];
                setActiveFile(last ? last.path : null);
            }
            return filtered;
        });
    };

    /* give the consumer all desired hook states and functions */
    return {workspaceId, openFiles, activeFile, openFile, updateFile, saveFile, closeFile, setActiveFile};
}