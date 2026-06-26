import { useEffect, useState } from 'react';
import '../../assets/css/panes.css';

/** The rendering used to "split" two components where one is resizable. */
export default function SplitPane({children, direction = 'horizontal', initialSize = 300, min = 150, max = 600, storageKey, className}) {
    const [size, setSize] = useState(() => {
        if (!storageKey) {
            return initialSize;
        }
        const saved = localStorage.getItem(storageKey);
        return saved ? Number(saved) : initialSize;
    });

    useEffect(() => !!storageKey && localStorage.setItem(storageKey, size), [size, storageKey]);

    function beginResize(e) {
        e.preventDefault();
        document.body.style.userSelect = "none";
        document.body.style.cursor = direction === "horizontal" ? "col-resize" : "row-resize";
        function onMouseMove(event) {
            const newSize = direction === "horizontal" ? event.clientX : event.clientY;
            setSize(Math.max(min, Math.min(max, newSize)));
        }
        function stopResize() {
            document.body.style.userSelect = "";
            document.body.style.cursor = "";
            window.removeEventListener("mousemove", onMouseMove);
            window.removeEventListener("mouseup", stopResize);
        }
        window.addEventListener("mousemove", onMouseMove);
        window.addEventListener("mouseup", stopResize);
    }

    const [first, second] = children;
    const isHorizontal = direction === "horizontal";
    return (
        <div className={`chatter__split-pane${!!className?.length ? ` ${className}` : ''}`} style={{flexDirection: isHorizontal ? "row" : "column"}}>
            <div className='chatter__split-pane-resizable' style={{minWidth: isHorizontal ? size : "100%", minHeight: !isHorizontal ? size : "100%"}}>
                {first}
            </div>
            <div className={`chatter__resizer${!!isHorizontal ? ' chatter__resizer--horizontal' : ''}`} onMouseDown={beginResize} />
            <div className='chatter__split-pane-priority'>
                {second}
            </div>
        </div>
    );
}