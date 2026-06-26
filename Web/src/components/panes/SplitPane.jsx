import React, { useEffect, useState } from 'react';
import { useResize } from '../../assets/js/hooks/resize';
import '../../assets/css/panes.css';

/** The base class name for the `<SplitPane />` component. */
const baseClass = 'chatter__split-pane';

/**
 * Gets the size for a resizable pane based on `isHorizontal` and `size`.
 * @param {boolean} isHorizontal A flag that, when `true`, denotes the resizable pane is horizontal.
 * @param {number} size The current size of the resizable pane.
 */
const getResizablePaneSize = (isHorizontal, size) => ({minWidth: isHorizontal ? size : '100%', minHeight: !isHorizontal ? size : '100%'});

/** The rendering used to split two components into a resiable side and static side, also includes a resizer. */
export default function SplitPane({children, direction = 'horizontal', initialSize = 300, min = 150, max = 600, storageKey, className}) {
    const isHorizontal = direction === 'horizontal';
    const {size, beginResize} = useResize(isHorizontal, initialSize, min, max, storageKey);
    const wrapperClass = [baseClass, !!isHorizontal && `${baseClass}--horizontal`, !!className?.length && className].filter(x => x).join(' ');
    const panes = React.Children.toArray(children);

    return (
        <div className={wrapperClass}>
            <div className='chatter__split-pane-resizable' style={getResizablePaneSize(isHorizontal, size)}>{panes[0]}</div>
            <div className='chatter__resizer' onMouseDown={beginResize} />
            <div className='chatter__split-pane-priority'>{panes[1]}</div>
        </div>
    );
}