/** Renders the starting page for the editor (i.e.: when there is no active file). */
export default function StartPage({}) {
    return (
        <div className='chatter__start-page'>
            <p className='chatter__start-page-title'>Welcome to your workspace!</p>
            <p>Navigate your workspace using the file explorer on the left.</p>
            <p>Use ctrl+s to save your files.</p>
        </div>
    )
}