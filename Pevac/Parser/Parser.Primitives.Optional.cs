using System;
using System.Text.Json;

namespace Pevac;

public static partial class Parser
{
    private static Parser<DateTime?>? optionalDateTime;
    private static Parser<Uri?>? optionalUri;
    private static Parser<Guid?>? optionalGuid;
    private static Parser<string?>? optionalString;
    private static Parser<bool?>? optionalBool;
    private static Parser<double?>? optionalDouble;
    private static Parser<decimal?>? optionalDecimal;

    /// <summary>
    /// Parse an optional <see cref="string"/> value.
    /// </summary>
    public static Parser<string?> OptionalString => optionalString ??= OptionalStringToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) =>
        {
            return Result.Success(reader.GetString());
        });

    /// <summary>
    /// Parse a nullable <see cref="bool"/> value.
    /// </summary>
    public static Parser<bool?> OptionalBool => optionalBool ??= (NullToken.Return(default(bool?))).Or(Bool.Select(@bool => @bool as bool?));

    /// <summary>
    /// Parse an optional <see cref="System.DateTime"/> value.
    /// </summary>
    public static Parser<DateTime?> OptionalDateTime => optionalDateTime ??= (NullToken.Return(default(DateTime?))).Or(DateTime.Select(dateTime => dateTime as DateTime?));

    /// <summary>
    /// Parse an optional <see cref="System.Uri"/> value.
    /// </summary>
    public static Parser<System.Uri?> OptionalUri => optionalUri ??= (NullToken.Return(default(Uri))).Or(Uri);

    /// <summary>
    /// Parse a nullable <see cref="Guid"/> value.
    /// </summary>
    public static Parser<Guid?> OptionalGuid => optionalGuid ??= (NullToken.Return(default(Guid?)))
        .Or(Guid.Select(value => value as Guid?));

    /// <summary>
    /// 
    /// </summary>
    public static Parser<double?> OptionalDouble => optionalDouble ??= (NullToken.Return(default(double?)))
        .Or(Double.Select(value => value as double?));

    /// <summary>
    /// 
    /// </summary>
    public static Parser<decimal?> OptionalDecimal => optionalDecimal ??= (NullToken.Return(default(decimal?)))
        .Or(Decimal.Select(value => value as decimal?));


}
