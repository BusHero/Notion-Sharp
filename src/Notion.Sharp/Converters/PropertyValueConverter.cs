using System;
using System.Text.Json;
using Notion.Model;
using Pevac;
using static Notion.Model.PropertyValue;
using Void = Pevac.Void;

namespace Notion.Converters;

internal class PropertyValueConverter : MyJsonConverter<PropertyValue>
{
    public override PropertyValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Parser.ParseObject(propertyName => propertyName switch
        {
            "id" => Parser.String.Updater((string id, PropertyValue propertyValue) => propertyValue with { Id = id }),
            "type" => Parser.String.Updater<string, PropertyValue>(),
            "title" => Parser.ParseType<RichText[]>().Updater((RichText[] content, PropertyValue propertyValue) => propertyValue.Copy<Title>() with { Content = content }),
            "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[] content, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.Text>() with { Content = content }),
            "number" => Parser.OptionalDouble.Updater((double? value, PropertyValue propertyValue) => propertyValue.Copy<Number>() with { Value = value }),
            "select" => Parser.ParseType<Option>().Updater((Option option, PropertyValue propertyValue) => propertyValue.Copy<Select>() with { Option = option }),
            "multi_select" => Parser.ParseType<Option[]>().Updater((Option[] options1, PropertyValue propertyValue) => propertyValue.Copy<MultiSelect>() with { Options = options1 }),
            "date" => (Parser.NullToken.Updater((Void _, PropertyValue propertyValue) => propertyValue.Copy<Date>())).Or(
                Parser.ParseObject(name => name switch
                {
                    "start" => Parser.OptionalDateTime.Updater((DateTime? start, Date date) => date with { Start = start }),
                    "end" => Parser.OptionalDateTime.Updater((DateTime? end, Date date) => date with { Start = end }),
                    "time_zone" => Parser.OptionalString.Updater((string? timeZone, Date date) => date with {TimeZone = timeZone}),
                    _ => Parser.FailUpdate<Date>($"Unknown key property_value.date.{name}")
                }, (PropertyValue propertyValue) => propertyValue.Copy<Date>())),
            "people" => Parser.ParseType<User[]>().Updater((User[] users, PropertyValue propertyValue) => propertyValue.Copy<People>() with { Value = users }),
            "files" => Parser.ParseType<File[]>().Updater((File[] files, PropertyValue propertyValue) => propertyValue.Copy<Files>() with { Value = files }),
            "checkbox" => Parser.Bool.Updater((bool @checked, PropertyValue propertyValue) => propertyValue.Copy<Checkbox>() with { Checked = @checked }),
            "url" => Parser.OptionalUri.Updater((Uri link, PropertyValue propertyValue) => propertyValue.Copy<Url>() with { Link = link }),
            "email" => Parser.OptionalString.Updater((string email, PropertyValue propertyValue) => propertyValue.Copy<Email>() with { Value = email }),
            "phone_number" => Parser.OptionalString.Updater((string phoneNumber, PropertyValue propertyValue) => propertyValue.Copy<PhoneNumber>() with { Value = phoneNumber }),
            "formula" => Parser.ParseObject(name => name switch
            {
                "type" => Parser.String.Updater<string, Formula>(),
                "string" => Parser.OptionalString.Updater((string value, Formula formula) => formula.Copy<SrtingFormula>() with { Value = value }),
                "number" => Parser.OptionalDecimal.Updater((decimal? value, Formula formula) => formula.Copy<NumberFormula>() with { Value = value }),
                "boolean" => Parser.OptionalBool.Updater((bool? value, Formula formula) => formula.Copy<BooleanFormula>() with { Value = value }),
                "date" => Parser.ParseType<Date1>().Updater((Date1 date, Formula formula) => formula.Copy<DateFormula>() with { Value = date }),
                _ => Parser.FailUpdate<Formula>($"Unknown key property_value.formula.{name}")
            }, (PropertyValue propertyValue) => propertyValue.Copy<Formula>()),
            "relation" => Parser.ParseType<PageReference[]>().Updater((PageReference[] pages, PropertyValue propertyValue) => propertyValue.Copy<Relation>() with { Pages = pages }),
            "rollup" => Parser.ParseObject(name => name switch
            {
                "type" => Parser.String!.Updater<string, Rollup>(),
                "number" => Parser.OptionalDecimal.Updater((decimal? value, Rollup rollup) => rollup.Copy<NumberRollup>() with { Value = value }),
                "date" => Parser.OptionalDateTime.Updater((DateTime? value, Rollup rollup) => rollup.Copy<DateRollup>() with { Value = value }),
                "array" => Parser.ParseType<PropertyValue[]>().Updater((PropertyValue[] value, Rollup rollup) => rollup.Copy<ArrayRollup>() with { Value = value }),
                "function" => Parser.String.Updater((string? function, Rollup rollup) => rollup with { Function = function }),
                _ => Parser.FailUpdate<Rollup>($"Unknown key property_value.rollup.{name}")
            }, (PropertyValue propertyValue) => propertyValue.Copy<Rollup>()),
            "created_time" => Parser.DateTime.Updater((DateTime createdTime, PropertyValue propertyValue) => propertyValue.Copy<CreatedTime>() with { Value = createdTime }),
            "created_by" => Parser.ParseType<User>().Updater((User createdBy, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.CreatedBy>() with { Value = createdBy }),
            "last_edited_time" => Parser.DateTime.Updater((DateTime lastEditedTime, PropertyValue propertyValue) => propertyValue.Copy<LastEditedTime>() with { Value = lastEditedTime }),
            "last_edited_by" => Parser.ParseType<User>().Updater((User lastEditedBy, PropertyValue propertyValue) => propertyValue.Copy<PropertyValue.LastEditedBy>() with { Value = lastEditedBy }),
            _ => Parser.FailUpdate<PropertyValue>($"Unknown key property_value.'{propertyName}'")
        }).Parse(ref reader, options);
    }
}
