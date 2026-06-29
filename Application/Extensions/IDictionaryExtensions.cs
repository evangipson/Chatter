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
            Console.WriteLine($"attempting to get the \"{key}\" value from \"args\"...");
            foreach(var kv in kvp ?? new Dictionary<string, object?>())
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            // get the "args" element, early return empty string if there isn't one
            if(kvp?.TryGetValue("args", out var arguments) != true)
            {
                Console.WriteLine("no \"args\" element found, returning empty string.");
                return string.Empty;
            }

            // get the string representation of the value in "args", early return empty string if it is null or whitespace
            var jsonArguments = arguments?.ToString();
            if (string.IsNullOrWhiteSpace(jsonArguments))
            {
                Console.WriteLine("null or whitespace \"args\" element found, returning empty string.");
                return string.Empty;
            }

            // get the named value from "args", early return empty string if it cannot be found
            var deserializedArguments = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonArguments, JsonConstants.DefaultSerializerOptions);
            if (deserializedArguments?.TryGetValue(key, out var value) != true || string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine($"no \"{key}\" element found inside of \"args\", returning empty string.");
                return string.Empty;
            }

            Console.WriteLine($"found \"{value}\" from \"{key}\" element inside of \"args\".");
            return value;
        }
    }
}
