namespace API.Extensions;

/// <summary>
/// A <see langword="static"/> collection of <see cref="string"/> extensions.
/// </summary>
internal static class StringExtensions
{
    extension(string str)
    {
        /// <summary>
        /// A recursive function to build a tree of <see cref="object"/> from a directory <see cref="string"/>.
        /// </summary>
        /// <returns>A tree of <see cref="object"/> that represents a <see cref="string"/> directory.</returns>
        internal object BuildTree()
        {
            DirectoryInfo directoryInfo = new(str);
            return new
            {
                name = directoryInfo.Name,
                type = "folder",
                children = directoryInfo.GetDirectories()
                    .Select(d => BuildTree(d.FullName))
                    .Concat(directoryInfo.GetFiles().Select(f => new { name = f.Name, type = "file", path = f.FullName }))
            };
        }
    }
}
