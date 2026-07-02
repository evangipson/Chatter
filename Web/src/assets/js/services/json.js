/**
 * Handles reading an NDJSON stream and handling the parsed JSON events.
 * @param {Response} response The response containing the NDJSON stream as the body.
 * @param {(object) => void} onSuccess The callback to run when NDJSON is successfully parsed.
 */
export const readNdJsonStream = async (response, onSuccess) => {
    // read the response body
    const reader = response.body.getReader();
    const decoder = new TextDecoder();

    // parse the response body while it's streaming
    let buffer = '';
    while (true) {
        // read the chunk, early exit if the stream has ended
        const { value, done } = await reader.read();
        if (done) {
            break;
        }

        // split the chunk on new line characters
        buffer += decoder.decode(value, { stream: true });
        const lines = buffer.split('\n');
        buffer = lines.pop() ?? '';

        // handle success or failure for each line
        for (const line of lines) {
            if (!line.trim()) {
                continue;
            }
            try {
                onSuccess(JSON.parse(line));
            } catch (err) {
                console.warn('bad NDJSON:', line, err);
            }
        }
    }
    
    // if there is no final line after EOF, early exit
    buffer += decoder.decode();
    if (!buffer.trim()) {
        return;
    }

    // handle the last chunk of NDJSON
    try {
        onSuccess(JSON.parse(buffer));
    } catch (err) {
        console.warn('bad EOF NDJSON:', line, err);
    }
};