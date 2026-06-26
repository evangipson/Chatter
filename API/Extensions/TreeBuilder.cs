namespace API.Extensions;

internal static class TreeBuilder
{
    /// <summary>
    /// A recursive function to build a tree of <see cref="object"/> from a directory <see cref="string"/>.
    /// </summary>
    /// <returns>A tree of <see cref="object"/> that represents a <see cref="string"/> directory.</returns>
    public static object BuildTree(string currentPath, string rootPath)
    {
        DirectoryInfo directoryInfo = new(currentPath);
        return new
        {
            name = directoryInfo.Name,
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