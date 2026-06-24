import React from 'react';

/** Returns the DOM markup for a "waiting" chat bubble. */
const waitingChatBubble = () => {
    return <div className='chatter__chat-row chatter__chat-row--bot'>
        <span className='chatter__bubble chatter__bubble--waiting'>...</span>
    </div>;
};

/** Renders a chat bubble that shows an ellipses, intended to be used when waiting for bot responses. */
export const WaitingChatBubble = React.memo(waitingChatBubble);