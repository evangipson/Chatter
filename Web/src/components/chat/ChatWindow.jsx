import React, { useState, useEffect, useRef, useMemo } from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { oneDark } from 'react-syntax-highlighter/dist/esm/styles/prism';
import { v4 as uuidv4 } from 'uuid';
import { WaitingChatBubble } from './WaitingChatBubble';
import { ChatBubble } from './ChatBubble';
import { ArrowIcon } from '../icons/ArrowIcon';
import { PlusIcon } from '../icons/PlusIcon';
import '../../assets/css/chat.css';
import ChatActions from './ChatActions';

export default function ChatWindow({bot, conversationId, onConversationUpdated}) {
    const [isWaitingForFirstToken, setIsWaitingForFirstToken] = useState(false);
    const [isStreaming, setIsStreaming] = useState(false);
    const [messages, setMessages] = useState([]);
    const [streamingText, setStreamingText] = useState('');
    const [input, setInput] = useState('');
    const [image, setImage] = useState(null);
    const bottomRef = useRef(null);
    const resizeRef = useRef(null);
    const streamingIndexRef = useRef(null);
    const textareaRef = useRef(null);
    const streamingRef = useRef('');
    const stableMessages = useMemo(() => messages.filter(m => m.id !== streamingIndexRef.current), [messages]);

    // fetch greeting based on the specific bot
    useEffect(() => {
        if (!conversationId) {
            return;
        }

        let ignore = false;
        async function loadHistory() {
            // set both loading states to true
            setIsWaitingForFirstToken(true);
            setIsStreaming(true);

            // clear old chat immediately
            streamingRef.current = '';
            setStreamingText('');
            setMessages([]);
            try {
                const response = await fetch(`/chat/history/${conversationId}`);
                const history = await response.json();
                !ignore && setMessages(history || []);
            } catch (err) {
                console.error(`unable to load chat history for conversation "${conversationId}": ${err}`);
            } finally {
                !ignore && setIsWaitingForFirstToken(false);
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
    useEffect(() => bottomRef.current?.scrollIntoView(), [stableMessages]);

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

    /** Handles sending a message from the user and getting the bot's response. */
    const sendMessageAndGetResponse = async () => {
        // if there is no input to send, early exit
        if (!input.trim()) {
            return;
        }

        // create a message for the user and add a placeholder message for the bot's response
        const userMessage = { id: uuidv4(), role: 'user', text: input };
        const botMessage = { id: uuidv4(), role: 'bot', text: '' };
        setMessages(prev => [...prev, userMessage, botMessage]);

        // clear out the streaming ref and set the streaming index ref to the bot's id
        streamingRef.current = '';
        streamingIndexRef.current = botMessage.id;

        // clear current input state, set both loading states to true
        setInput('');
        setIsWaitingForFirstToken(true);
        setIsStreaming(true);

        try {
            // create a request containing the user's message to send to the API
            const request = {conversationId, message: input, botId: bot.id, systemPrompt: bot.systemPrompt};

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

            // go through all tokens returned by the API
            while (true) {
                // read the next token, and if the stream has ended end the loop
                const { done, value } = await reader.read();
                if (done) {
                    break;
                }

                // get the next chunk by decoding the value from the reader
                const chunk = decoder.decode(value, {stream: true});

                // when the first token is returned, set is waiting loading state to false
                if (isWaitingForFirstToken) {
                    setIsWaitingForFirstToken(false);
                }

                // add the chunk to the streaming ref
                streamingRef.current += chunk;

                // update the messages state
                setMessages(prev => {
                    const updated = [...prev];
                    const index = updated.findIndex(m => m.id === streamingIndexRef.current);
                    if (index !== -1) {
                        updated[index] = {
                            ...updated[index],
                            text: streamingRef.current
                        };
                    }
                    return updated;
                });
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
                    {stableMessages.map(message => (<ChatBubble key={message.id} role={message.role} text={message.text} />))}
                    {isWaitingForFirstToken && (<WaitingChatBubble />)}
                    {streamingRef.current && (<ChatBubble role='bot' text={streamingRef.current} />)}
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