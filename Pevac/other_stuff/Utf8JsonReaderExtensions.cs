namespace Pevac;

using System;
using System.Text.Json;

/// <summary>
/// Provides some extenssion methods for the <see cref="Utf8JsonReader"/> class.
/// </summary>
public static class Utf8JsonReaderExtensions
{
    /// <inheritdoc cref="Utf8JsonReader.TryGetDateTime(out DateTime)"/>
    public static bool TryGetNullableDateTime(this ref Utf8JsonReader reader, out DateTime? value)
    {
        switch (reader.GetString())
        {
            case null:
                value = default;
                return true;
            case var str:
                switch (DateTime.TryParse(str, out DateTime dateTime))
                {
                    case true:
                        value = dateTime;
                        return true;
                    case false:
                        value = default;
                        return false;
                }
        }
    }

    /// <inheritdoc cref="Utf8JsonReader.TryGetGuid(out Guid)"/>
    public static bool TryGetNullableGuid(this ref Utf8JsonReader reader, out Guid? value)
    {
        switch (reader.GetString())
        {
            case null:
                value = default;
                return true;
            case var str:
                switch (Guid.TryParse(str, out var guid))
                {
                    case true:
                        value = guid;
                        return true;
                    case false:
                        value = default;
                        return false;
                }
        }
    }

    /// <inheritdoc cref="Uri.TryCreate(string?, UriKind, out Uri?)"/>
    public static bool TryGetUri(this ref Utf8JsonReader reader, UriKind uriKind, out Uri? value)
    {
        var uriAsString = reader.GetString();
        return Uri.TryCreate(uriAsString, uriKind, out value);
    }
}
