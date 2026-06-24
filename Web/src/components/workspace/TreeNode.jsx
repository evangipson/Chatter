/** Renders a recursive tree node. */
export default function TreeNode({ node, onOpen }) {
    if (node.type === "file") {
        return (
            <div onClick={() => onOpen(node.path)}>
                📄 {node.name}
            </div>
        );
    }
    return (
        <div>
            📁 {node.name}
            <div style={{ paddingLeft: 12 }}>
                {node.children?.map(c => (<TreeNode key={c.name} node={c} onOpen={onOpen} />))}
            </div>
        </div>
    );
}