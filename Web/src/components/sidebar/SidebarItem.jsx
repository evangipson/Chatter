/** Renders an item in the sidebar. */
export default function SidebarItem({ id, text, active, onSelect, onDelete }) {
    return (
        <div key={id} className={'chatter__sidebar-item ' + (active ? 'chatter__sidebar-item--active' : '')}>
            <span className='chatter__sidebar-item-title' onClick={() => onSelect(id)}>{text}</span>
            <button className='chatter__sidebar-delete' onClick={e => onDelete(e)}>🗑</button>
        </div>
    );
}