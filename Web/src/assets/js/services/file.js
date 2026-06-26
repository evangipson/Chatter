/**
 * Gets the last chunk of `data.path || data` after splitting on `separator`.
 * @param {{path: string}|string} data The `string` to chunk, or an `object` with a `path` to chunk.
 * @param {string} separator Used to split `data.path || data`, defaults to `\`.
 * @returns The last separated chunk of `data.path || data`.
 */
const getLastChunk = (data, separator = '\\') => data?.path?.split(separator)?.pop()
    || data?.split(separator)?.pop();

/** A collection of functions that make working with files easier. */
export const FileService = {
    /**
     * Gets the name of a `file`.
     * @param {{path: string}|string} file The path itself, or an `object` with a `path`.
     * @param {string} separator The path separator, defaults to `\`.
     * @returns The name of the `file`.
     */
    getFileName: (file, separator = '\\') => getLastChunk(file, separator),

    /**
     * Maps a `file` with necessary properties for it to display in the editor.
     * @param {object} file The file to prepare for the editor.
     */
    getFileForEditor: file => ({
        path: file.path || path,
        name: getLastChunk(file),
        content: file.content,
        originalContent: file.content,
        dirty: false,
        saving: false,
    }),
};