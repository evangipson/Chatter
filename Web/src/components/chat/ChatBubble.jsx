import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { oneDark } from 'react-syntax-highlighter/dist/esm/styles/prism';

/** A collection of markdown component customizations. */
const markdownComponents = {
    /** Modifies the behavior of the `<code>` element for generated markdown components. */
    code({ node, inline, className, children, ...props }) {
        const match = /language-(\w+)/.exec(className || '');
        return !inline && match
            ? (<SyntaxHighlighter style={oneDark} language={match[1]} PreTag="div" {...props}>{String(children).replace(/\n$/, '')}</SyntaxHighlighter>)
            : (<code className={className} {...props}>{children}</code>);
    },
};

/** Returns the DOM markup for a chat bubble. */
const chatBubble = ({ role, text }) => {
    return <div className={`chatter__chat-row ${role === 'user' ? 'chatter__chat-row--user' : 'chatter__chat-row--bot'}`}>
        <span className='chatter__bubble'>
            <ReactMarkdown remarkPlugins={[remarkGfm]} components={markdownComponents}>{text}</ReactMarkdown>
        </span>
    </div>;
};

/** Renders a chat bubble that displays `text` for a `role`, intended to be used for any user or bot messages. */
export const ChatBubble = React.memo(chatBubble);