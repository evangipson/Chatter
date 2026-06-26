namespace API.Extensions;

/// <summary>
/// A <see langword="static"/> collection of methods for building directory trees.
/// </summary>
internal static class TreeBuilder
{
    /// <summary>
    /// A recursive function to build a tree of <see cref="object"/> from a directory <see cref="string"/>.
    /// </summary>
    /// <param name="currentPath">The current path of the directory tree.</param>
    /// <param name="rootPath">The root path of the directory tree.</param>
    /// <param name="rootName">The name of the root folder, only provided for the first call.</param>
    /// <returns>A tree of <see cref="object"/> that represents a <see cref="string"/> directory.</returns>
    public static object BuildTree(string currentPath, string rootPath, string? rootName = null)
    {
        DirectoryInfo directoryInfo = new(currentPath);
        return new
        {
            name = rootName ?? directoryInfo.Name,
            type = "folder",
            path = Path.GetRelativePath(rootPath, currentPath),
            children = directoryInfo.GetDirectories()
                .Select(d => BuildTree(d.FullName, rootPath))
                .Concat(directoryInfo.GetFiles().Select(f => new
                {
                    name = f.Name,
                    type = "file",
                    path = Path.GetRelativePath(rootPath, f.FullName)
                }))
        };
    }
}