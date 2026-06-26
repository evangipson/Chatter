export const getLanguage = path => {
    if (!path) return 'plaintext';
    if (path.endsWith('.md')) return 'markdown';
    if (path.endsWith('.dll')) return 'library';
    if (path.endsWith('.cs')) return 'csharp';
    if (path.endsWith('.h') || path.endsWith('.c')) return 'c';
    if (path.endsWith('.hpp') || path.endsWith('.cpp')) return 'cpp';
    if (path.endsWith('.css')) return 'css';
    if (path.endsWith('.rs')) return 'rust';
    if (path.endsWith('.js')) return 'javascript';
    if (path.endsWith('.ts')) return 'typescript';
    if (path.endsWith('.jsx') || path.endsWith('.tsx')) return 'typescript';
    if (path.endsWith('.json')) return 'json';
    if (path.endsWith('.glsl')) return 'glsl';
    if (path.endsWith('.lua')) return 'lua';
    if (path.endsWith('.ps1')) return 'powershell';
    return 'plaintext';
};