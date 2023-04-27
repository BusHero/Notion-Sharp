using System;
using System.Text.Json;
using Notion.Converters.Utils;
using Notion.Model;
using Pevac;
using static Notion.Model.User;
using Void = Pevac.Void;

namespace Notion.Converters;

internal class UserConverter : MyJsonConverter<User>
{
    public override User Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Parser.ParseObject(property => property switch
        {
            "object" => Parser.ParseString("user")!.Updater<string, User>(),
            "id" => Parser.Guid.Updater((Guid id, User user) => user.Id = id),
            "type" => Parser.String!.Updater<string, User>(),
            "name" => Parser.String.Updater((string? name, User user) => user.Name = name),
            "avatar_url" => Parser.OptionalUri.Updater((Uri? avatarUrl, User user) => user.AvatarUrl = avatarUrl),
            "person" => Parser.ParseObject(propertyName => propertyName switch
            {
                "email" => Parser.String.Updater((string? email, Person person) => person.Email = email),
                _ => Parser.FailUpdate<Person>($"Unknown key user.person.{propertyName}")
            }, (User user) => user.Cast<Person>()),
            "bot" => Ignored.Updater((Void _, User user) => user.Cast<Bot>()),
            _ => Parser.FailUpdate<User>($"Unknown key user.{property}")
        }).Parse(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteEndObject();
    }

    private static Parser<Void>? ignored;

    public static Parser<Void> Ignored => ignored ??= (ref Utf8JsonReader reader, JsonSerializerOptions? _) =>
    {
        return reader.TrySkip() switch
        {
            true => Result.Success(Void.Default),
            false => Result.Failure<Void>("Cannot skip")
        };
    };
}
