using System.Text.Json;

namespace Pevac;

public static partial class Parser
{
    private static Parser<Void>? optionalBoolToken;
    private static Parser<Void>? optionalFalseToken;
    private static Parser<Void>? optionalTrueToken;
    private static Parser<Void>? optionalNumberToken;
    private static Parser<Void>? optionalStringToken;

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.String"/> 
    /// or <see cref="JsonTokenType.None"/> token.
    /// </summary>
    public static Parser<Void> OptionalStringToken => optionalStringToken ??= ParseToken(JsonTokenType.String, JsonTokenType.Null);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.Number"/> 
    /// or <see cref="JsonTokenType.Null"/> token.
    /// </summary>
    public static Parser<Void> OptionalNumberToken => optionalNumberToken ??= ParseToken(JsonTokenType.Number, JsonTokenType.Null);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.True"/> 
    /// or <see cref="JsonTokenType.Null"/> tokens.
    /// </summary>
    public static Parser<Void> OptionalTrueToken => optionalTrueToken ??= ParseToken(JsonTokenType.True, JsonTokenType.Null);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.False"/> 
    /// or <see cref="JsonTokenType.Null"/> tokens.
    /// </summary>
    public static Parser<Void> OptionalFalseToken => optionalFalseToken ??= ParseToken(JsonTokenType.False, JsonTokenType.Null);

    /// <summary>
    /// Parser for the <see cref="JsonTokenType.True"/> 
    /// or <see cref="JsonTokenType.False"/> 
    /// or <see cref="JsonTokenType.Null"/> tokens.
    /// </summary>
    public static Parser<Void> OptionalBoolToken => optionalBoolToken ??= ParseToken(JsonTokenType.True, JsonTokenType.False, JsonTokenType.Null);

}
