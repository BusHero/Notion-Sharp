
using System;
using System.Text.Json;

namespace Pevac;

public partial class Parser
{
    private static Parser<Void>? emptyObject;
    private static Parser<Uri?>? uri;
    private static Parser<int>? int32;
    private static Parser<long>? int64;
    private static Parser<sbyte>? sByte;
    private static Parser<float>? single;
    private static Parser<string>? propertyName;
    private static Parser<ulong>? uInt64;
    private static Parser<uint>? uInt32;
    private static Parser<ushort>? uInt16;
    private static Parser<short>? int16;
    private static Parser<double>? @double;
    private static Parser<decimal>? @decimal;
    private static Parser<DateTimeOffset>? dateTimeOffset;
    private static Parser<DateTime>? dateTime;
    private static Parser<byte[]?>? bytesFromBase64;
    private static Parser<byte>? @byte;
    private static Parser<Guid>? guid;
    private static Parser<bool>? @bool;
    private static Parser<string?>? @string;

    /// <summary>
    /// Parse a <see cref="string"/> value.
    /// </summary>
    public static Parser<string?> String => @string ??= StringToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) =>
        {
            return Result.Success(reader.GetString());
        });

    /// <summary>
    /// Parse a <see cref="bool"/> value.
    /// </summary>
    public static Parser<bool> Bool => @bool ??= BooleanToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) =>
        {
            return Result.Success(reader.GetBoolean());
        });

    /// <summary>
    /// Parse a <see cref="System.Guid"/> value.
    /// </summary>
    public static Parser<Guid> Guid => guid ??= StringToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetGuid(out var guid) switch
        {
            true => Result.Success(guid),
            false => Result.Failure<Guid>($"Cannot convert '{reader.GetString()}' to Guid")
        });

    /// <summary>
    /// Parse a <see cref="byte"/> value.
    /// </summary>
    public static Parser<byte> Byte => @byte ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetByte(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<byte>("Cannot convert the current token to a byte value")
        });

    /// <summary>
    /// Parse a bynary value codded as an array <see cref="byte"/>.
    /// </summary>
    public static Parser<byte[]?> BytesFromBase64 => bytesFromBase64 ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetBytesFromBase64(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<byte[]?>("Cannot convert the current token into a byte array")
        });

    /// <summary>
    /// Parse a <see cref="System.DateTime"/> value.
    /// </summary>
    public static Parser<DateTime> DateTime => dateTime ??= OptionalStringToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetDateTime(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<DateTime>($"Cannot convert '{reader.GetString()}' to a datetime")
        });

    /// <summary>
    /// Parse a <see cref="System.DateTimeOffset"/> value.
    /// </summary>
    public static Parser<DateTimeOffset> DateTimeOffset => dateTimeOffset ??= StringToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetDateTimeOffset(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<DateTimeOffset>($"Cannot convert '{reader.GetString()}' to DateTime")
        });

    /// <summary>
    /// Parse a <see cref="decimal"/> value.
    /// </summary>
    public static Parser<decimal> Decimal => @decimal ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetDecimal(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<decimal>($"Cannot convert the value to decimal")
        });

    /// <summary>
    /// Parse a <see cref="double"/> value.
    /// </summary>
    public static Parser<double> Double => @double ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetDouble(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<double>($"Cannot convert the value to double")
        });

    /// <summary>
    /// Parse a <see cref="short"/> value.
    /// </summary>
    public static Parser<short> Int16 => int16 ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetInt16(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<short>($"Cannot convert the value to short")
        });

    /// <summary>
    /// Parse a <see cref="int"/> value.
    /// </summary>
    public static Parser<int> Int32 => int32 ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetInt32(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<int>($"Cannot convert value to int")
        });

    /// <summary>
    /// Parse a <see cref="long"/> value.
    /// </summary>
    public static Parser<long> Int64 => int64 ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetInt64(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<long>("Cannot convert value to long")
        });

    /// <summary>
    /// Parse a <see cref="sbyte"/> value.
    /// </summary>
    public static Parser<sbyte> SByte => sByte ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetSByte(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<sbyte>("Cannot convert value to sbyte")
        });

    /// <summary>
    /// Parse a <see cref="float"/> value.
    /// </summary>
    public static Parser<float> Single => single ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetSingle(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<float>("Cannot convert value to float")
        });

    /// <summary>
    /// Parse a <see cref="ushort"/> value.
    /// </summary>
    public static Parser<ushort> UInt16 => uInt16 ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetUInt16(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<ushort>("Cannot convert value to ushort")
        });

    /// <summary>
    /// Parse a <see cref="uint"/> value.
    /// </summary>
    public static Parser<uint> UInt32 => uInt32 ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetUInt32(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<uint>("Cannot convert value to uint")
        });

    /// <summary>
    /// Parse a <see cref="ulong"/> value.
    /// </summary>
    public static Parser<ulong> UInt64 => uInt64 ??= NumberToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.TryGetUInt64(out var value) switch
        {
            true => Result.Success(value),
            false => Result.Failure<ulong>("Cannot convert value to ulong")
        });

    /// <summary>
    /// Parse a property name value.
    /// </summary>
    public static Parser<string> PropertyName => propertyName ??= PropertyNameToken
        .Then((ref Utf8JsonReader reader, JsonSerializerOptions? _) => reader.GetString() switch
        {
            null => throw new ParseException("Cannot get the string value"),
            string propertyName => Result.Success(propertyName),
        });

    /// <summary>
    /// Parse a <see cref="System.Uri"/> value.
    /// </summary>
    public static Parser<System.Uri?> Uri => uri ??= String.Select(uri =>
        {
            System.Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var @return);
            return @return;
        });
        
    /// <summary>
    /// Parses an empty object.
    /// </summary>
    public static Parser<Void> EmptyObject => emptyObject ??= StartObjectToken
        .Then(EndObjectToken);
}
