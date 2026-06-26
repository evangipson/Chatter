import React from 'react';

/** Returns the DOM markup for the C programming language logo SVG. */
const cSvg = ({color}) => {
    const iconColor = color ?? '#a9bacd';
    return <svg viewBox="0 0 128 128" className='chatter__icon'>
        <path fill={iconColor} d="M125 50c-4-32-24-50-62-50C29 0 3 24 3 64c0 39 24 64 64 64 32 0 55-19 58-50H87c-2 11-8 20-20 20-21 0-24-16-24-33 0-23 8-35 22-35 13 0 20 7 22 20z" />
    </svg>;
};

/** Renders the C programming language logo icon. */
export const CIcon = React.memo(cSvg);