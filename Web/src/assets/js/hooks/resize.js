import { useEffect, useState } from 'react';

/**
 * A hook that provides all resize functionality for a resizable pane.
 * @param {boolean} isHorizontal A flag that, when `true`, denotes a horizontal pane layout.
 * @param {number} initialSize The initial size of the resizable pane, defaults to `300`.
 * @param {number} min The minimum size of the resiable pane, defaults to `150`.
 * @param {number} max The maximum size of the resiable pane, defaults to `600`.
 * @param {string} storageKey The key for `localStorage` to persist pane sizes.
 */
export const useResize = (isHorizontal, initialSize = 300, min = 150, max = 600, storageKey) => {
    let frame = -1;
    let ignored = false;
    const [size, setSize] = useState(() => {
        if (!storageKey) {
            return initialSize;
        }
        const saved = localStorage.getItem(storageKey);
        return saved ? Number(saved) : initialSize;
    });

    /* set up the local storage persistence hook each time the size updates. */
    useEffect(() => !!storageKey && localStorage.setItem(storageKey, size), [size, storageKey]);

    /**
     * Updates the exported `size` for the resizable pane in a performant way.
     * @param {PointerEvent} pointerEvent The broadcasted event as a result of a pointer move.
     */
    const onMouseMove = pointerEvent => {
        const newSize = isHorizontal ? pointerEvent.clientX : pointerEvent.clientY;
        const clampedNewSize = Math.max(min, Math.min(max, newSize));
        cancelAnimationFrame(frame);
        frame = requestAnimationFrame(() => setSize(clampedNewSize));
    };

    /** Resets the cursor on mousue up and clears any latent user selections. */
    const stopResize = () => {
        document.body.style.userSelect = '';
        document.body.style.cursor = '';
        window.removeEventListener('mousemove', onMouseMove);
        window.removeEventListener('mouseup', stopResize);
    };

    return {
        /** The current size of the resizable pane. */
        size: size,

        /**
         * Sets up necessary window event listeners and sets the correct cursor for a resizable pane.
         * @param {PointerEvent} pointerEvent The broadcasted event as a result of a pointer being pressed.
         */
        beginResize: pointerEvent => {
            pointerEvent.preventDefault();
            document.body.style.userSelect = 'none';
            document.body.style.cursor = isHorizontal ? 'col-resize' : 'row-resize';
            window.addEventListener('mousemove', onMouseMove);
            window.addEventListener('mouseup', stopResize);
        },
    };
};