import { CIcon } from './CIcon';
import { CPlusPlusIcon } from './CPlusPlusIcon';
import { CSharpIcon } from './CSharpIcon';
import { CssIcon } from './CssIcon';
import { FileIcon } from './FileIcon';
import { JavaScriptIcon } from './JavaScriptIcon';
import { JsonIcon } from './JsonIcon';
import { LuaIcon } from './LuaIcon';
import { OpenGLIcon } from './OpenGLIcon';
import { PowerShellIcon } from './PowerShellIcon';
import { ReactIcon } from './ReactIcon';
import { RustIcon } from './RustIcon';
import { TypeScriptIcon } from './TypeScriptIcon';
import { WindowsIcon } from './windowsIcon';

/** Renders an icon for a programming language based on `path`. */
export default function LanguageIcon({path}) {
    if (!path) return (<FileIcon />);
    if (path.endsWith('.dll')) return (<WindowsIcon color="#cbcbcb" />);
    if (path.endsWith('.cs')) return (<CSharpIcon color="#ac6bb2" />);
    if (path.endsWith('.css')) return (<CssIcon />);
    if (path.endsWith('.h')) return (<CIcon color="#adadad" />);
    if (path.endsWith('.c')) return (<CIcon color="#7cb3a7" />);
    if (path.endsWith('.hpp')) return (<CPlusPlusIcon color="#adadad" />);
    if (path.endsWith('.cpp')) return (<CPlusPlusIcon />);
    if (path.endsWith('.rs')) return (<RustIcon />);
    if (path.endsWith('.js')) return (<JavaScriptIcon />);
    if (path.endsWith('.ts')) return (<TypeScriptIcon />);
    if (path.endsWith('.jsx')) return (<ReactIcon color='#287987' />);
    if (path.endsWith('.tsx')) return (<ReactIcon color='#24418a' />);
    if (path.endsWith('.json')) return (<JsonIcon />);
    if (path.endsWith('.glsl')) return (<OpenGLIcon />);
    if (path.endsWith('.lua')) return (<LuaIcon color='#5a83d5' />);
    if (path.endsWith('.ps1')) return (<PowerShellIcon color="#7abde1" />);
    return (<FileIcon />);
}