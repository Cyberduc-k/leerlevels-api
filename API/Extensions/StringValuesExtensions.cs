using Microsoft.Extensions.Primitives;

namespace API.Extensions;

public static class StringValuesExtensions
{
    /// <summary>
    ///      Gets an int value from a <see cref="StringValues"/> object.
    ///      Returns <paramref name="defaultValue"/> when strings is empty or null if the value is not a valid int.
    /// </summary>
    /// <param name="defaultValue">the default value</param>
    public static int? GetInt(this StringValues strings, int defaultValue = 0)
    {
        return strings.FirstOrDefault() is string s
            ? int.TryParse(s, out int i) ? i : null
            : defaultValue;
    }

    /// <summary>
    ///      Gets a bool value from a <see cref="StringValues"/> object.
    ///      Returns <see cref="false"/> when strings is empty or null if the value is not a valid bool.
    /// </summary>
    public static bool? GetBool(this StringValues strings)
    {
        return strings.FirstOrDefault() is string s
            ? bool.TryParse(s, out bool b) ? b : null
            : false;
    }
}
