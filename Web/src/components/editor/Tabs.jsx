import { CloseIcon } from '../icons/CloseIcon';
import '../../assets/css/editor/tab.css';

/** The rendering for a tab in the editor. */
export default function Tabs({files, activeFile, workspace}) {
    /**
     * Handles the click action for closing a tab.
     * @param {PointerEvent} pointerEvent The click or touch that triggered the tab close.
     * @param {object} file The file to close.
     */
    const onCloseClick = (pointerEvent, file) => {
        pointerEvent.stopPropagation();
        workspace.closeFile(file.path);
    };

    return (
        <div className='chatter__tabs'>
            {files.map(file => {
                return (<div className={`chatter__tab${file.path === activeFile ? ' chatter__tab--active' : ''}`} key={file.path}>
                    <span className='chatter__tab-name' onClick={() => workspace.setActiveFile(file.path)}>{file.dirty ? '●' : ''} {file.name}</span>
                    <span className='chatter__tab-close' onClick={event => onCloseClick(event, file)}><CloseIcon /></span>
                </div>)
            })}
        </div>
    );
}