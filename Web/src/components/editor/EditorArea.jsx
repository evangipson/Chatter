import { useEffect, useMemo, useState } from "react";
import { Editor } from '@monaco-editor/react';
import { WorkspaceAPI } from "../../assets/js/api/workspace";
import { getLanguage } from '../../assets/js/services/language';
import StartPage from './StartPage';
import Tabs from './Tabs';
import '../../assets/css/editor/editor.css';

/** The rendering for the code editor area. */
export default function EditorArea({workspace}) {
    const active = useMemo(() => workspace?.openFiles.find(f => f.path === workspace.activeFile), [workspace]);
    const language = useMemo(() => active ? getLanguage(active.path) : 'plaintext', [active]);
    const onEditorContextChange = value => {
        if (!active) {
            return;
        }
        onChangeFile(active.path, value);
    };

    return (
        <div className='chatter__editor-area'>
            <Tabs files={workspace.openFiles} activeFile={workspace.activeFile} workspace={workspace} />
            <div className='chatter__editor'>
                {!!active
                    ? (<Editor theme='vs-dark' language={language} value={active?.content ?? ''} onChange={value => !!active && workspace.updateFile(active.path, value || "")} />)
                    : (<StartPage />)}
            </div>
        </div>
    );
}