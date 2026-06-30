using System.Text.Json;
using Domain.Constants;

namespace Application.Extensions;

/// <summary>
/// A <see langword="static"/> collection of methods to extend <see cref="IDictionary{TKey, TValue}"/>.
/// </summary>
public static class IDictionaryExtensions
{
    extension(IDictionary<string, object?>? kvp)
    {
        /// <summary>
        /// Attempts to get a value from the <c>"args"</c> element in an <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key inside of <c>"args"</c> to get the value of.</param>
        /// <returns>
        /// The <see langword="string"/> value associated to <paramref name="key"/> in <c>"args"</c>, defaults to <see cref="string.Empty"/>.
        /// </returns>
        public string TryGetJsonArg(string key)
        {
            // get the "args" element, early return empty string if there isn't one
            if(kvp?.TryGetValue("args", out var arguments) != true)
            {
                return string.Empty;
            }

            // get the string representation of the value in "args", early return empty string if it is null or whitespace
            var jsonArguments = arguments?.ToString();
            if (string.IsNullOrWhiteSpace(jsonArguments))
            {
                return string.Empty;
            }

            // get the named value from "args", early return empty string if it cannot be found
            var deserializedArguments = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonArguments, JsonConstants.DefaultSerializerOptions);
            return deserializedArguments?.TryGetValue(key, out var value) != true || string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value;
        }
    }
}
