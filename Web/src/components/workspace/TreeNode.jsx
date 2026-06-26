import { useMemo } from "react";
import { getLanguageIcon } from '../../assets/js/services/language';


/** Renders a recursive tree node. */
export default function TreeNode({node, onOpen, collapsedFolders, isRoot = false, depth, onToggleFolder}) {
    const isCollapsed = useMemo(() => isRoot ? false : (collapsedFolders?.[node.path] ?? true), [collapsedFolders]);
    if (node.type === "file") {
        return (
            <div className="chatter__tree-item chatter__tree-item--file" onClick={() => onOpen(node.path)}>
                {getLanguageIcon(node.path)} {node.name}
            </div>
        );
    }

    return (
        <div className="chatter__tree-item chatter__tree-item--folder">
            <div onClick={e => {e.stopPropagation(); onToggleFolder(node.path)}}>
                {isCollapsed ? "📁" : "📂"} {node.name}
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