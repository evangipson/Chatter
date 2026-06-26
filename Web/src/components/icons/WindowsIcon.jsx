import React from 'react';

/** Returns the DOM markup for the Windows logo SVG. */
const windowsSvg = ({color}) => {
    const iconColor = color ?? '#0078d4';
    return <svg viewBox="0 0 128 128" className='chatter__icon'>
        <path fill={iconColor} d="M67.328 67.331h60.669V128H67.328zm-67.325 0h60.669V128H.003zM67.328 0h60.669v60.669H67.328zM.003 0h60.669v60.669H.003z" />
    </svg>;
};

/** Renders the Windows logo as an icon. */
export const WindowsIcon = React.memo(windowsSvg);