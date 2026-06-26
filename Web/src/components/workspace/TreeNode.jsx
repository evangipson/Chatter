import { useMemo } from 'react';
import { getLanguage } from '../../assets/js/services/language';
import { ChevronIcon } from '../icons/ChevronIcon';
import LanguageIcon from '../icons/LanguageIcon';

/** Renders a recursive tree node. */
export default function TreeNode({node, onOpen, collapsedFolders, isRoot = false, depth, onToggleFolder}) {
    const isCollapsed = useMemo(() => isRoot ? false : (collapsedFolders?.[node.path] ?? true), [collapsedFolders]);
    if (node.type === "file") {
        return (
            <div className="chatter__tree-item chatter__tree-item--file" data-language={getLanguage(node.path)} onClick={() => onOpen(node.path)}>
                <span className='chatter__file-icon'><LanguageIcon path={node.path} /></span>
                <span>{node.name}</span>
            </div>
        );
    }

    return (
        <div className={`chatter__tree-item chatter__tree-item--folder${isCollapsed ? ' chatter__tree-item--collapsed' : ''}`}>
            <div onClick={e => {e.stopPropagation(); onToggleFolder(node.path)}}>
                <span className='chatter__tree-chevron'><ChevronIcon /></span>
                <span>{node.name}</span>
            </div>
            {!isCollapsed && (
                <div style={{paddingLeft: depth * 8}}>
                    {node.children?.map(c => (
                        <TreeNode key={c.path || c.name}
                            node={c}
                            onOpen={onOpen}
                            collapsedFolders={collapsedFolders}
                            onToggleFolder={onToggleFolder}
                            depth={depth + 1}
                        />
                    ))}
                </div>
            )}
        </div>
    );
}