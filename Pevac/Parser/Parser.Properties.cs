using System;
using System.Text.Json;

namespace Pevac;

public static partial class Parser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="expectedPropertyName"></param>
    /// <returns></returns>
    public static Parser<string?> ParsePropertyName(string expectedPropertyName) => (ref Utf8JsonReader reader, JsonSerializerOptions? options) => PropertyName(ref reader, options) switch
    {
        Success<string> success when success.Value != expectedPropertyName => Result.Failure<string>(
            $"\"{expectedPropertyName}\" was expected but \"{success.Value}\" was instead"),
        var result => result
    };

    /// <summary>
    /// Generates a parser that will succed
    /// </summary>
    /// <param name="expectedPropertyName"></param>
    /// <returns></returns>
    public static Parser<(string, string)> ParseStringProperty(string expectedPropertyName) =>
        from propertyName in ParsePropertyName(expectedPropertyName)
        from stringValue in String
        select (propertyName, stringValue);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expectedPropertyName"></param>
    /// <param name="expectedStringValue"></param>
    /// <returns></returns>
    public static Parser<(string, string)> ParseStringProperty(string expectedPropertyName, string expectedStringValue) =>
        from propertyName in ParsePropertyName(expectedPropertyName)
        from stringValue in ParseString(expectedStringValue)
        select (propertyName, stringValue);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static Parser<T?> ParseTypeProperty<T>(string propertyName) => propertyName switch
    {
        null => throw new ArgumentNullException(nameof(propertyName)),
        not null => from _ in ParsePropertyName(propertyName)
                    from type in ParseType<T>()
                    select type,
    };
}
