export const getLanguage = path => {
    if (!path) return 'plaintext';
    if (path.endsWith('.cs')) return 'csharp';
    if (path.endsWith('.js')) return 'javascript';
    if (path.endsWith('.ts')) return 'typescript';
    if (path.endsWith('.tsx')) return 'typescript';
    if (path.endsWith('.json')) return 'json';
    if (path.endsWith('.lua')) return 'lua';
    return 'plaintext';
}

export const getLanguageIcon = path => {
    if (!path) return '📄';
    if (path.endsWith('.cs')) return '⚙️';
    if (path.endsWith('.js')) return '🟨';
    if (path.endsWith('.ts')) return '🔷';
    if (path.endsWith('.tsx')) return '⚛️';
    if (path.endsWith('.json')) return '{}';
    if (path.endsWith('.lua')) return '🌙';
    return '📄';
}