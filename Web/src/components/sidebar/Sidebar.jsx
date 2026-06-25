import { BranchIcon } from '../icons/BranchIcon';
import ImportWorkspace from '../workspace/ImportWorkspace';
import SidebarItem from './SidebarItem';
import '../../assets/css/sidebar.css';

/** Renders a sidebar containing a list of conversations. */
export default function Sidebar({ bot, conversations, activeConversationId, onSelectConversation, onNewConversation, onDeleteConversation }) {
    /**
     * Handles confirming the user wants to delete a conversation, and deleting it.
     * @param {PointerEvent} pointerEvent The event that triggered the deletion.
     * @param {string} conversation The conversation to delete.
     */
    const deleteConversation = async (pointerEvent, conversation) => {
        pointerEvent.stopPropagation();
        if (window.confirm(`Delete "${conversation.title}"?`)) {
            await onDeleteConversation(conversation.id);
        }
    };

    return (
        <div className='chatter__sidebar'>
            <div className='chatter__sidebar-section'>
                <div className='chatter__sidebar-item chatter__sidebar-item--action' onClick={onNewConversation}>
                    <BranchIcon />
                    <span className='chatter__sidebar-item--text'>New Chat</span>
                </div>
                <ImportWorkspace />
            </div>
            {bot && (
                <div className='chatter__sidebar-section'>
                    <div className='chatter__sidebar-title'>Conversations</div>
                    {(conversations[bot.id] || []).map(conversation => (
                        <SidebarItem
                            id={conversation.id}
                            text={conversation.title || 'Untitled Chat'}
                            active={activeConversationId == conversation.id}
                            onSelect={() => onSelectConversation(conversation.id)}
                            onDelete={event => deleteConversation(event, conversation)}
                        />
                    ))}
                </div>
            )}
        </div>
    );
}