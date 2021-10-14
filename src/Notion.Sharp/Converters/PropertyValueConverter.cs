using Notion.Model;

using Pevac;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using static Notion.Model.PropertyValue;

using Void = Pevac.Void;

namespace Notion.Converters
{
    internal class PropertyValueConverter : JsonConverter<PropertyValue>
    {
        public override PropertyValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Parser.ParseObject(propertyName => propertyName switch
            {
                "id" => Parser.String.Updater((string id, PropertyValue propertyValue) => propertyValue with { Id = id }),
                "type" => Parser.String.Updater<string, PropertyValue>(),
                "title" => Parser.ParseType<RichText[]>().Updater((RichText[] content, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Title>() with { Content = content}),
                "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] content, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Text>() with { Content = content }),
                "number" => Parser.OptionalDouble.Updater((double? value, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Number>() with { Value = value }),
                "select" => Parser.ParseType<Option>().Updater((Option option, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Select>() with { Option = option }),
                "multi_select" => Parser.ParseType<Option[]>().Updater((Option[] options, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.MultiSelect>() with { Options = options }),
                "date" => (Parser.NullToken.Updater((Void _, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Date>())).Or(
                    Parser.ParseObject(propertyName => propertyName switch
                    {
                        "start" => Parser.OptionalDateTime.Updater((DateTime? start, PropertyValue.Date date) => date with { Start = start }),
                        "end" => Parser.OptionalDateTime.Updater((DateTime? end, PropertyValue.Date date) => date with { Start = end }),
                        _ => Parser.FailUpdate<PropertyValue.Date>()
                    }, (PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Date>())),
                "people" => Parser.ParseType<User[]>().Updater((User[] users, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.People>() with { Value = users }),
                "files" => Parser.ParseType<File[]>().Updater((File[] files, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Files>() with { Vaule = files }),
                "checkbox" => Parser.Bool.Updater((bool @checked, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Checkbox>() with { Checked = @checked }),
                "url" => Parser.OptionalString.Select(uri =>
                {
                    Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var @return);
                    return @return;
                }).Updater((Uri link, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Url>() with { Link = link }),
                "email" => Parser.OptionalString.Updater((string email, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Email>() with { Value = email }),
                "phone_number" => Parser.OptionalString.Updater((string phoneNumber, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.PhoneNumber>() with { Value = phoneNumber }),
                "formula" => Parser.ParseObject(propertyName => propertyName switch
                {
                    "type" => Parser.String.Updater<string, PropertyValue.Formula>(),
                    "string" => Parser.String.Updater((string value, PropertyValue.Formula formula) => formula.Copy<PropertyValue.SrtingFormula>() with { Value = value }),
                    "number" => Parser.Decimal.Updater((decimal value, PropertyValue.Formula formula) => formula.Copy<PropertyValue.NumberFormula>() with { Value = value }),
                    "boolean" => Parser.Bool.Updater((bool value, PropertyValue.Formula formula) => formula.Copy<PropertyValue.BooleanFormula>() with { Value = value }),
                    "date" => Parser.OptionalDateTime.Updater((DateTime? value, Formula formula) => formula.Copy<PropertyValue.DateFormula>() with { Value = value }),
                    _ => Parser.FailUpdate<PropertyValue.Formula>()
                }, (PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Formula>()),
                "relation" => Parser.ParseType<PageReference[]>().Updater((PageReference[] pages, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Relation>() with { Pages = pages }),
                "rollup" => Parser.ParseObject(propertyName => propertyName switch
                {
                    "type" => Parser.String.Updater<string, PropertyValue.Rollup>(),
                    "number" => Parser.OptionalDecimal.Updater((decimal? value, PropertyValue.Rollup rollup) => rollup.Copy<PropertyValue.NumberRollup>() with { Value = value }),
                    "date" => Parser.OptionalDateTime.Updater((DateTime? value, PropertyValue.Rollup rollup) => rollup.Copy<PropertyValue.DateRollup>() with { Value = value }),
                    "array" => Parser.ParseType<PropertyValue[]>().Updater((PropertyValue[] value, PropertyValue.Rollup rollup) => rollup.Copy<PropertyValue.ArrayRollup>() with { Value = value }),
                    "function" => Parser.String.Updater((string? function, PropertyValue.Rollup rollup) => rollup with { Function = function }),
                    _ => Parser.FailUpdate<Rollup>()
                }, (PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Rollup>()),
                "created_time" => Parser.DateTime.Updater((DateTime createdTime, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.CreatedTime>() with { Value = createdTime }),
                "created_by" => Parser.ParseType<User>().Updater((User createdBy, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.CreatedBy>() with { Value = createdBy }),
                "last_edited_time" => Parser.DateTime.Updater((DateTime lastEditedTime, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.LastEditedTime>() with { Value = lastEditedTime }),
                "last_edited_by" => Parser.ParseType<User>().Updater((User lastEditedBy, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.LastEditedBy>() with { Value = lastEditedBy }),
                var x => Parser.FailUpdate<PropertyValue>()
            }).Parse(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, PropertyValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
