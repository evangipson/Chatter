using System.Text.Json;

namespace Domain.Constants;

/// <summary>
/// A <see langword="static"/> collection of constant JSON values.
/// </summary>
public static class JsonConstants
{
    /// <summary>
    /// The default JSON serializer options which allow trailing commas, case-insensitive naming, and camel case properties.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
