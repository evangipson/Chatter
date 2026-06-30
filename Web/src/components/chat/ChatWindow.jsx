import React, { useState, useEffect, useRef, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { oneDark } from 'react-syntax-highlighter/dist/esm/styles/prism';
import { v4 as uuidv4 } from 'uuid';
import { GlobalWorkspaceId } from '../../assets/js/constants/workspace';
import { WaitingChatBubble } from './WaitingChatBubble';
import { ChatBubble } from './ChatBubble';
import { ArrowIcon } from '../icons/ArrowIcon';
import { PlusIcon } from '../icons/PlusIcon';
import ChatActions from './ChatActions';
import general from '../../assets/json/general';
import '../../assets/css/chat.css';

export default function ChatWindow({workspaceId = GlobalWorkspaceId, conversationId, onConversationUpdated}) {
    const [isStreaming, setIsStreaming] = useState(false);
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');
    const [image, setImage] = useState(null);
    const bottomRef = useRef(null);
    const resizeRef = useRef(null);
    const textareaRef = useRef(null);
    const navigate = useNavigate();

    // fetch greeting based on the specific workspace
    useEffect(() => {
        if (!conversationId) {
            return;
        }

        let ignore = false;
        async function loadHistory() {
            // set streaming state to true
            setIsStreaming(true);

            // clear old chat immediately
            setMessages([]);
            try {
                const response = await fetch(`/chat/history/${conversationId}`);
                const history = await response.json();
                !ignore && setMessages(history || []);
            } catch (err) {
                console.error(`unable to load chat history for conversation "${conversationId}": ${err}`);
            } finally {
                !ignore && setIsStreaming(false);
            }
        }

        loadHistory();
        // prevent race conditions
        return () => {ignore = true};
    }, [conversationId]);

    // set the reactive text area height based on the user's message length
    useEffect(() => {
        const userInputElement = textareaRef.current;
        if (!textareaRef.current) {
            return;
        }
        userInputElement.style.height = '0px';
        userInputElement.style.height = `${Math.min(userInputElement.scrollHeight, 200)}px`;
    }, [input]);

    // auto-scroll the last chat message to be the currently showing chat
    useEffect(() => bottomRef.current?.scrollIntoView(), [messages]);

    /**
     * Handles image uploads and sets the image state.
     * @param {InputEvent} inputEvent 
     */
    const handleImageUpload = inputEvent => {
        const file = inputEvent.target.files[0];
        const reader = new FileReader();
        reader.onload = () => {setImage(reader.result)};
        reader.readAsDataURL(file);
    };

    /**
     * Adds an agent `event` from the server to `messages`.
     * @param {[{}]} messages The current messages in the chat.
     * @param {{text: string, toolName: string, workspaceId: string?}} event The agent event from the server.
     */
    const addAgentEvent = (messages, event) => {
        const updated = [...messages];
        const last = updated.at(-1);
        switch (event.type) {
            case 'agent_started':
                updated[updated.length - 1] = {...last, agent: {text: '🤖 agent thinking...', thinking: true}};
                break;
            case 'tool_started':
                updated[updated.length - 1] = {...last, agent: {...last.agent, tools: [...last.agent.tools, {id: crypto.randomUUID(), name: event.toolName, status: 'running'}]}};
                break;
            case 'tool_finished':
                const tools = [...last.tools];
                tools[tools.length - 1] = {...tools.at(-1), status: 'finished', duration: event.duration};
                updated[updated.length - 1] = {...last, tools};
                break;
            case 'agent_finished':
                updated[updated.length - 1] = {...last, agent: {text: `🤖 agent finished (${event.duration} ms)}`, thinking: false}};
                break;
            case 'assistant_started':
                updated.push({id: crypto.randomUUID(), role: 'bot', text: ''});
                break;
            case 'assistant_token':
                updated[updated.length - 1] = {...last, text: last.text + event.text};
                break;
            case 'workspace_created':
                navigate(`/workspace/${event.workspaceId}`);
                break;
            case 'assistant_finished':
            default:
                break;
        }
        return updated;
    };

    /** Handles sending a message from the user and getting the agent's response. */
    const sendMessageAndGetResponse = async () => {
        // if there is no input to send, early exit
        if (!input.trim()) {
            return;
        }

        // create a message for the user and add a placeholder message for the agent's response
        const hasMessages = messages.length > 0;
        const userMessage = { id: uuidv4(), role: 'user', text: input };
        const agentMessage = { id: uuidv4(), role: 'agent', agent: {thinking: false, duration: null, tools: []}, text: ''};
        setMessages(prev => [...prev, userMessage, agentMessage]);

        // clear current input state, set streaming state to true
        setInput('');
        setIsStreaming(true);

        try {
            // create a request containing the user's message to send to the API
            const request = hasMessages
                ? {conversationId, message: input, workspaceId: workspaceId}
                : {conversationId, message: input, workspaceId: workspaceId, systemPrompt: general.systemPrompt};

            // add the image to the request if it's been filled out
            if(!!image?.length) {
                request.imageBase64 = image;
                setImage(null);
            }

            // try to reach out to the API and get a response for the user
            const response = await fetch('/chat/respond', {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(request)});

            // read the response body
            const reader = response.body.getReader();
            const decoder = new TextDecoder();

            // go through all the NDJSON returned by the API
            let buffer = '';
            while (true) {
                // read the next token, and if the stream has ended end the loop
                const { value, done } = await reader.read();

                // if the stream has ended, end the loop
                if (done) {
                    break;
                }

                // increment the buffer with the decoded value from the server
                buffer += decoder.decode(value, { stream: true });

                // add the next line to the buffer
                const lines = buffer.split('\n');
                buffer = lines.pop();
                for (const line of lines) {
                    if (!line.trim()) {
                        continue;
                    }
                    const event = JSON.parse(line);
                    // update the messages state
                    setMessages(previous => addAgentEvent(previous, event));
                }
            }

            // after all tokens are returned, set is streaming loading state to false
            setIsStreaming(false);

            // run the `onConversationUpdated` callback to update higher-order components
            if (onConversationUpdated) {
                await onConversationUpdated();
            }
        } catch (error) {
            console.error('Streaming error:', error);
        }
    };

    return (
        <div className='chatter__container'>
            {!!messages.length && 
                <div className='chatter__chat'>
                    {messages.map(message => {
                        switch (message.role) {
                            case 'user': return (<ChatBubble role='user' text={message.text} />);
                            case 'agent': return (<ChatBubble role='agent' text={message.text} />);
                            case 'assistant': return (<ChatBubble role='bot' text={message.text} />);
                        }
                    })}
                    <div ref={bottomRef} />
                </div>
            }
            <div className='chatter__composer'>
                <ChatActions onImageUpload={handleImageUpload} />
                <textarea ref={textareaRef} className='chatter__input' placeholder='Enter a message...' value={input} onChange={e => setInput(e.target.value)} rows={1} />
                <button className='chatter__send' onClick={sendMessageAndGetResponse} disabled={isStreaming || !input.trim()}>{isStreaming ? '...' : (<ArrowIcon />)}</button>
            </div>
        </div>
    );
}