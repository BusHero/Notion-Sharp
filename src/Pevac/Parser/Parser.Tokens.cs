using System.Text.Json;

namespace Pevac;

public static partial class Parser
{
    private static Parser<Void>? startObjectToken;
    private static Parser<Void>? booleanToken;
    private static Parser<Void>? endObjectToken;
    private static Parser<Void>? nullToken;
    private static Parser<Void>? falseToken;
    private static Parser<Void>? trueToken;
    private static Parser<Void>? numberToken;
    private static Parser<Void>? stringToken;
    private static Parser<Void>? propertyNameToken;
    private static Parser<Void>? endArrayToken;
    private static Parser<Void>? startArrayToken;

    /// <summary>
    /// Parser for <see cref="JsonTokenType.StartObject"/> token.
    /// </summary>
    public static Parser<Void> StartObjectToken => startObjectToken ??= ParseToken(JsonTokenType.StartObject);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.EndObject"/> token.
    /// </summary>
    public static Parser<Void> EndObjectToken => endObjectToken ??= ParseToken(JsonTokenType.EndObject);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.StartArray"/> token.
    /// </summary>
    public static Parser<Void> StartArrayToken => startArrayToken ??= ParseToken(JsonTokenType.StartArray);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.EndArray"/> token.
    /// </summary>
    public static Parser<Void> EndArrayToken => endArrayToken ??= ParseToken(JsonTokenType.EndArray);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.PropertyName"/> token.
    /// </summary>
    public static Parser<Void> PropertyNameToken => propertyNameToken ??= ParseToken(JsonTokenType.PropertyName);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.String"/> token.
    /// </summary>
    public static Parser<Void> StringToken => stringToken ??= ParseToken(JsonTokenType.String);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.Number"/> token.
    /// </summary>
    public static Parser<Void> NumberToken => numberToken ??= ParseToken(JsonTokenType.Number);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.True"/> token.
    /// </summary>
    public static Parser<Void> TrueToken => trueToken ??= ParseToken(JsonTokenType.True);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.False"/> token.
    /// </summary>
    public static Parser<Void> FalseToken => falseToken ??= ParseToken(JsonTokenType.False);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.True"/> 
    /// or <see cref="JsonTokenType.False"/> tokens.
    /// </summary>
    public static Parser<Void> BooleanToken => booleanToken ??= ParseToken(JsonTokenType.True, JsonTokenType.False);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.Null"/> token.
    /// </summary>
    public static Parser<Void> NullToken => nullToken ??= ParseToken(JsonTokenType.Null);
}
