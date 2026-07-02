import React, { useState, useEffect, useRef, useReducer } from 'react';
import { useNavigate } from 'react-router-dom';
import { ChatAPI } from '../../assets/js/api/chat';
import { GlobalWorkspaceId } from '../../assets/js/constants/workspace';
import { ChatFactory } from '../../assets/js/factories/chat-factory';
import { useAutosizeTextArea } from '../../assets/js/hooks/autosize-textarea';
import { useAutoScroll } from '../../assets/js/hooks/autoscroll';
import { MessageReducer } from '../../assets/js/reducers/message';
import { readNdJsonStream } from '../../assets/js/services/json';
import ChatActions from './ChatActions';
import ChatMessage from './ChatMessage';
import { ArrowIcon } from '../icons/ArrowIcon';
import general from '../../assets/json/general';
import '../../assets/css/chat.css';

/** The rendering for a chat window. */
export default function ChatWindow({workspaceId = GlobalWorkspaceId, conversationId, onConversationUpdated}) {
    const [isStreaming, setIsStreaming] = useState(false);
    const [messages, dispatch] = useReducer(MessageReducer, []);
    const [input, setInput] = useState('');
    const [image, setImage] = useState(null);
    const bottomRef = useRef(null);
    const textareaRef = useRef(null);
    const navigate = useNavigate();
    useAutosizeTextArea(textareaRef, input, 200);
    useAutoScroll(bottomRef, messages);

    /* fetch history based on the specific workspace */
    useEffect(() => {
        if (!conversationId) {
            return;
        }
        let ignore = false;
        async function loadHistory() {
            const history = await ChatAPI.getHistory(conversationId);
            dispatch({type: 'history_loaded', messages: history});
        }
        loadHistory();
        return () => {ignore = true};
    }, [conversationId]);

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
     * Handles chat events from the server.
     * @param {object} event The chat event from the server to handle.
     */
    const handleChatEvent = event => {
        if (event.type === 'workspace_created') {
            navigate(`/workspace/${event.workspaceId}`);
            return;
        }
        dispatch(event);
    };

    /** Handles sending a message from the user and getting the agent's response. */
    const sendMessageAndGetResponse = async () => {
        if (!input.trim()) {
            return;
        }
        const hasConversation = !!conversationId;
        dispatch({type: 'user_message', message: ChatFactory.createUserMessage(input)});
        setInput('');
        setIsStreaming(true);
        try {
            const request = hasConversation ? {conversationId, workspaceId, message: input} : {conversationId, workspaceId, message: input, systemPrompt: general.systemPrompt};
            if (image) {
                request.imageBase64 = image;
                setImage(null);
            }
            await ChatAPI.respond(request, handleChatEvent);
            onConversationUpdated?.();
        }
        catch (error) {
            console.error(error);
        }
        finally {
            setIsStreaming(false);
        }
    };

    return (
        <div className='chatter__container'>
            {!!messages.length && 
                <div className='chatter__chat'>
                    {messages.map(message => (<ChatMessage key={message.id} role={message.role} text={message.text} agent={message.agent} />))}
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