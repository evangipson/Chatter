import { useEffect } from 'react';

/**
 * Automatically resizes a textarea to fit its contents.
 * @param {React.RefObject<HTMLTextAreaElement>} textareaRef The react reference to the `<textarea>`.
 * @param {string} value The current value of the `<textarea>`.
 * @param {number} maxHeight The maximum height of the `<textarea>`, defaults to `Infinity`.
 */
export function useAutosizeTextArea(textareaRef, value, maxHeight = Infinity) {
    useEffect(() => {
        const textarea = textareaRef.current;
        if (!textarea) {
            return;
        }

        textarea.style.height = '0px';
        textarea.style.height = `${Math.min(textarea.scrollHeight, maxHeight)}px`;
    }, [textareaRef, value]);
}