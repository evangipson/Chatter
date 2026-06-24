namespace API.Extensions;

/// <summary>
/// A <see langword="static"/> collection of <see cref="Task"/> extensions.
/// </summary>
internal static class TaskExtensions
{
    extension(Task task)
    {
        /// <summary>
        /// Runs a <see cref="Task"/> and returns an HTTP result.
        /// </summary>
        /// <returns>Returns <c>200 OK</c> if the <see cref="Task"/> completed successfully, <c>500 INTERNAL SERVER ERROR</c> otherwise.</returns>
        internal async Task<IResult> OkAsync()
        {
            try { await task; }
            catch { return Results.InternalServerError(); }
            return Results.Ok();
        }
    }
}
