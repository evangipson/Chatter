import { CSharpIcon } from './CSharpIcon';
import { CssIcon } from './CssIcon';
import { FileIcon } from './FileIcon';
import { JavaScriptIcon } from './JavaScriptIcon';
import { JsonIcon } from './JsonIcon';
import { LuaIcon } from './LuaIcon';
import { OpenGLIcon } from './OpenGLIcon';
import { ReactIcon } from './ReactIcon';
import { RustIcon } from './RustIcon';
import { TypeScriptIcon } from './TypeScriptIcon';

/** Renders an icon for a programming language based on `path`. */
export default function LanguageIcon({path}) {
    if (!path) return (<FileIcon />);
    if (path.endsWith('.cs')) return (<CSharpIcon />);
    if (path.endsWith('.css')) return (<CssIcon />);
    if (path.endsWith('.rs')) return (<RustIcon />);
    if (path.endsWith('.js')) return (<JavaScriptIcon />);
    if (path.endsWith('.ts')) return (<TypeScriptIcon />);
    if (path.endsWith('.jsx')) return (<ReactIcon color='#287987' />);
    if (path.endsWith('.tsx')) return (<ReactIcon color='#24418a' />);
    if (path.endsWith('.json')) return (<JsonIcon />);
    if (path.endsWith('.glsl')) return (<OpenGLIcon />);
    if (path.endsWith('.lua')) return (<LuaIcon color='#5a83d5' />);
    return (<FileIcon />);
}