import { useRef, useState } from 'react';
import { PaperclipIcon } from '../icons/PaperclipIcon';
import { FilePlusIcon } from '../icons/FilePlusIcon';
import { PlusIcon } from '../icons/PlusIcon';
import '../../assets/css/actions.css';

/** Renders the chat actions component. */
export default function ChatActions({ onImageUpload }) {
    const fileInputRef = useRef(null);
    const [checked, setChecked] = useState(false);    

    const openFileExplorer = () => {
        fileInputRef.current?.click();
    };

    return (
        <div className='chatter__actions'>
            <input className='chatter__input chatter__input--upload' type='file' accept='image/*' ref={fileInputRef} onChange={e => {onImageUpload(e);setChecked(false);}} />
            <div className='chatter__options' onClick={() => setChecked(!checked)}>
                <input className='chatter__options--checkbox' type='checkbox' defaultChecked={checked} />
                <PlusIcon />
            </div>
            <div className='chatter__options-tooltip'>
                <FilePlusIcon />
                <p>Actions</p>
            </div>
            <div className='chatter__actions-tooltip'>
                <div className='chatter__actions-list'>
                    <div className='chatter__action' onClick={openFileExplorer}>
                        <PaperclipIcon />
                        <p>Upload image</p>
                    </div>
                </div>
            </div>
        </div>
    );
}