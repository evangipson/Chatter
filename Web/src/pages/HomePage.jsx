import { useState, useEffect, useCallback } from 'react';
import { ConversationAPI } from '../assets/js/api/conversation';
import { GlobalWorkspaceId } from '../assets/js/constants/workspace';
import { useConversation } from '../assets/js/hooks/conversation';
import Sidebar from '../components/sidebar/Sidebar';
import ChatWindow from '../components/chat/ChatWindow';
import '../assets/css/app.css';

/** The home page of the application. */
export default function Home() {
    const {conversationId, setConversationId} = useConversation(GlobalWorkspaceId);
    const [conversations, setConversations] = useState({});

    /**
     * Refreshes the conversations using the server.
     * @returns The refreshed conversations.
     */
    const refreshConversations = useCallback(async () => {
        const serverConversations = await ConversationAPI.get(GlobalWorkspaceId);
        setConversations(prev => ({...prev, [GlobalWorkspaceId]: serverConversations}));
        return serverConversations;
    }, []);
    
    /**
     * Selects a conversation using `id`.
     * @param {string} id The conversation identifier.
     */
    const selectConversation = async (id) => {
        const activeConversation = conversations[GlobalWorkspaceId]?.find(c => c.id === conversationId);
        activeConversation?.title === 'New Chat' && await refreshConversations(GlobalWorkspaceId);
        setConversationId(id);
    };

    /** Creates a new conversation. */
    const newConversation = async () => {
        const created = await ConversationAPI.create(GlobalWorkspaceId);
        await refreshConversations();
        setConversationId(created.id);
    };

    /**
     * Deletes a conversation using `id`.
     * @param {string} id The conversation identifier.
     */
    const deleteConversation = async (id) => {
        await ConversationAPI.delete(id);
        const updated = await refreshConversations();
        conversationId === id && setConversationId(updated.length > 0 ? updated[0].id : null);
    };

    /** Selects the global workspace, then load global conversations into the sidebar. */
    useEffect(() => {
        let ignore = true;
        const setGlobalConversations = async () => {
            // prevent running this function twice on development environments
            if (!ignore) {
                return;
            }
            
            // if there are conversations for the global workspace, set the active conversation and exit
            const serverConversations = await refreshConversations();
            if (serverConversations.length > 0) {
                setConversationId(serverConversations[0].id);
                return;
            }

            // if there aren't any conversations for the global workspace, make one
            const created = await ConversationAPI.create(GlobalWorkspaceId);
            await refreshConversations();
            setConversationId(created.id);
        };
        setGlobalConversations();
        return () => {ignore = false};
    }, []);

    return (
        <div className='chatter__app'>
            <Sidebar
                conversations={conversations}
                activeConversationId={conversationId}
                onSelectConversation={selectConversation}
                onNewConversation={newConversation}
                onDeleteConversation={deleteConversation}
            />
            <div className='chatter__chat-area'>
                {conversationId && (<ChatWindow conversationId={conversationId} onConversationUpdated={async () => await refreshConversations()} />)}
            </div>
        </div>
    );
}