import { useEffect, useState } from "react";
import { ConversationAPI } from "../api/conversation";
import { GlobalWorkspaceId } from '../constants/workspace';

export function useConversation(workspaceId = GlobalWorkspaceId) {
    const [conversationId, setConversationId] = useState(null);

    useEffect(() => {
        const init = async () => {
            // if there is already a conversation, set it as "active" and leave
            const existing = await ConversationAPI.get(workspaceId);
            if (!!existing && existing.length > 0) {
                setConversationId(existing[0].id);
                return;
            }

            // make a new conversation if there isn't one already
            const newConversation = await ConversationAPI.create(workspaceId);
            setConversationId(newConversation.id);
        }
        init();
    }, [workspaceId]);

    return { conversationId, setConversationId };
}