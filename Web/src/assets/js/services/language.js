export const getLanguage = path => {
    if (!path) return 'plaintext';
    if (path.endsWith('.cs')) return 'csharp';
    if (path.endsWith('.css')) return 'css';
    if (path.endsWith('.rs')) return 'rust';
    if (path.endsWith('.js')) return 'javascript';
    if (path.endsWith('.ts')) return 'typescript';
    if (path.endsWith('.tsx')) return 'typescript';
    if (path.endsWith('.json')) return 'json';
    if (path.endsWith('.glsl')) return 'glsl';
    if (path.endsWith('.lua')) return 'lua';
    return 'plaintext';
};