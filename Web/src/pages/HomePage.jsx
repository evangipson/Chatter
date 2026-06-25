
import { useState, useEffect, useCallback } from 'react';
import { ConversationAPI } from '../assets/js/api/conversation';
import Sidebar from '../components/sidebar/Sidebar';
import ChatWindow from '../components/chat/ChatWindow';
import general from '../assets/json/general.json';
import '../assets/css/app.css';

/** The home page of the application. */
export default function Home() {
    const [activeBot, setActiveBot] = useState(null);
    const [activeConversationId, setActiveConversationId] = useState(null);
    const [conversations, setConversations] = useState({});

    /**
     * Refreshes the conversations using the server.
     * @param {string} botId The bot identifier.
     * @returns The refreshed conversations.
     */
    const refreshConversations = useCallback(async (botId) => {
        const serverConversations = await ConversationAPI.get(botId);
        setConversations(prev => ({...prev, [botId]: serverConversations}));
        return serverConversations;
    }, []);
    
    /**
     * Selects a conversation using `id`.
     * @param {string} id The conversation identifier.
     */
    const selectConversation = async (id) => {
        const activeConversation = conversations[activeBot.id]?.find(c => c.id === activeConversationId);
        activeConversation?.title === 'New Chat' && await refreshConversations(activeBot.id);
        setActiveConversationId(id);
    };

    /** Creates a new conversation. */
    const newConversation = async () => {
        const created = await ConversationAPI.create(activeBot.id);
        await refreshConversations(activeBot.id);
        setActiveConversationId(created.id);
    };

    /**
     * Deletes a conversation using `id`.
     * @param {string} id The conversation identifier.
     */
    const deleteConversation = async (id) => {
        await ConversationAPI.delete(id);
        const updated = await refreshConversations(activeBot.id);
        activeConversationId === id && setActiveConversationId(updated.length > 0 ? updated[0].id : null);
    };

    // select the "general" bot when the sidebar loads
    useEffect(() => {
        let ignore = true;
        const setBotConversations = async (bot) => {
            // prevent running this function twice on development environments
            if (!ignore) {
                return;
            }

            // set the active bot
            setActiveBot(bot);
            
            // if there are conversations for this bot, set the active conversation and exit
            const serverConversations = await refreshConversations(bot.id);
            if (serverConversations.length > 0) {
                setActiveConversationId(serverConversations[0].id);
                return;
            }

            // if there aren't any conversations for this bot, make one
            const created = await ConversationAPI.create(bot.id);
            await refreshConversations(bot.id);
            setActiveConversationId(created.id);
        };

        setBotConversations(general);
        // prevent race conditions
        return () => {ignore = false};
    }, []);

    return (
        <div className='chatter__app'>
            <Sidebar
                bot={activeBot}
                conversations={conversations}
                activeConversationId={activeConversationId}
                onSelectConversation={selectConversation}
                onNewConversation={newConversation}
                onDeleteConversation={deleteConversation}
            />
            <div className='chatter__chat-area'>
                {activeBot && activeConversationId &&
                    <ChatWindow
                        bot={activeBot}
                        conversationId={activeConversationId}
                        onConversationUpdated={async () => await refreshConversations(activeBot.id)}
                    />
                }
            </div>
        </div>
    );
}