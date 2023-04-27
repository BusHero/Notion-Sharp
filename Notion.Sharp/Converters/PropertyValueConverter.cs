using System;
using System.Text.Json;
using Notion.Converters.Utils;
using Notion.experiment;
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
            "id" => Parser.String.Updater((string? id, PropertyValue propertyValue1) => propertyValue1 with { Id = id }),
            "type" => Parser.String!.Updater<string, PropertyValue>(),
            "title" => Parser.ParseType<RichText[]>().Updater((RichText[]? content, PropertyValue propertyValue2) => propertyValue2.Copy<Title>() with { Content = content }),
            "rich_text" => Parser.ParseType<RichText[]>().Updater((RichText[]? content, PropertyValue propertyValue3) => propertyValue3.Copy<PropertyValue.Text>() with { Content = content }),
            "number" => Parser.OptionalDouble.Updater((double? value, PropertyValue propertyValue4) => propertyValue4.Copy<Number>() with { Value = value }),
            "select" => Parser.ParseType<Option>().Updater((Option? option, PropertyValue propertyValue5) => propertyValue5.Copy<Select>() with { Option = option }),
            "multi_select" => Parser.ParseType<Option[]>().Updater((Option[]? options1, PropertyValue propertyValue6) => propertyValue6.Copy<MultiSelect>() with { Options = options1 }),
            "date" => (Parser.NullToken.Updater((Void _, PropertyValue propertyValue7) => propertyValue7.Copy<Date>())).Or(
                Parser.ParseObject(name => name switch
                {
                    "start" => Parser.OptionalDateTime.Updater((DateTime? start, Date date) => date with { Start = start }),
                    "end" => Parser.OptionalDateTime.Updater((DateTime? end, Date date) => date with { End = end }),
                    "time_zone" => Parser.OptionalString.Updater((string? timeZone, Date date) => date with {TimeZone = timeZone}),
                    _ => Parser.FailUpdate<Date>($"Unknown key property_value.date.{name}")
                }, (PropertyValue propertyValue8) => propertyValue8.Copy<Date>())),
            "people" => Parser.ParseType<User[]>().Updater((User[]? users, PropertyValue propertyValue9) => propertyValue9.Copy<People>() with { Value = users }),
            "files" => Parser.ParseType<File[]>().Updater((File[]? files, PropertyValue propertyValue10) => propertyValue10.Copy<Files>() with { Value = files }),
            "checkbox" => Parser.Bool.Updater((bool @checked, PropertyValue propertyValue11) => propertyValue11.Copy<Checkbox>() with { Checked = @checked }),
            "url" => Parser.OptionalUri.Updater((Uri? link, PropertyValue propertyValue12) => propertyValue12.Copy<Url>() with { Link = link }),
            "email" => Parser.OptionalString.Updater((string? email, PropertyValue propertyValue13) => propertyValue13.Copy<Email>() with { Value = email }),
            "phone_number" => Parser.OptionalString.Updater((string? phoneNumber, PropertyValue propertyValue14) => propertyValue14.Copy<PhoneNumber>() with { Value = phoneNumber }),
            "formula" => Parser.ParseObject(name => name switch
            {
                "type" => Parser.String.Updater<string?, Formula>(),
                "string" => Parser.OptionalString.Updater((string? value, Formula formula) => formula.Copy<StringFormula>() with { Value = value }),
                "number" => Parser.OptionalDecimal.Updater((decimal? value, Formula formula) => formula.Copy<NumberFormula>() with { Value = value }),
                "boolean" => Parser.OptionalBool.Updater((bool? value, Formula formula) => formula.Copy<BooleanFormula>() with { Value = value }),
                "date" => Parser.ParseType<Date1>().Updater((Date1? date, Formula formula) => formula.Copy<DateFormula>() with { Value = date }),
                _ => Parser.FailUpdate<Formula>($"Unknown key property_value.formula.{name}")
            }, (PropertyValue propertyValue15) => propertyValue15.Copy<Formula>()),
            "relation" => Parser.ParseType<PageReference[]>().Updater((PageReference[]? pages, PropertyValue propertyValue16) => propertyValue16.Copy<Relation>() with { Pages = pages }),
            "rollup" => Parser.ParseObject(name => name switch
            {
                "type" => Parser.String!.Updater<string, Rollup>(),
                "number" => Parser.OptionalDecimal.Updater((decimal? value, Rollup rollup) => rollup.Copy<NumberRollup>() with { Value = value }),
                "date" => Parser.OptionalDateTime.Updater((DateTime? value, Rollup rollup) => rollup.Copy<DateRollup>() with { Value = value }),
                "array" => Parser.ParseType<PropertyValue[]>().Updater((PropertyValue[]? value, Rollup rollup) => rollup.Copy<ArrayRollup>() with { Value = value }),
                "function" => Parser.String.Updater((string? function, Rollup rollup) => rollup with { Function = function }),
                _ => Parser.FailUpdate<Rollup>($"Unknown key property_value.rollup.{name}")
            }, (PropertyValue propertyValue17) => propertyValue17.Copy<Rollup>()),
            "status" => Parser.ParseType<Property.Status.Option>().Updater((Property.Status.Option? option, PropertyValue value) => value.Copy<Status>() with{ Value = option }),
            "created_time" => Parser.DateTime.Updater((DateTime createdTime, PropertyValue propertyValue18) => propertyValue18.Copy<CreatedTime>() with { Value = createdTime }),
            "created_by" => Parser.ParseType<User>().Updater((User? createdBy, PropertyValue propertyValue19) => propertyValue19.Copy<PropertyValue.CreatedBy>() with { Value = createdBy }),
            "last_edited_time" => Parser.DateTime.Updater((DateTime lastEditedTime, PropertyValue propertyValue20) => propertyValue20.Copy<LastEditedTime>() with { Value = lastEditedTime }),
            "last_edited_by" => Parser.ParseType<User>().Updater((User? lastEditedBy, PropertyValue propertyValue21) => propertyValue21.Copy<PropertyValue.LastEditedBy>() with { Value = lastEditedBy }),
            "has_more" => Parser.Bool.Updater<bool, PropertyValue>(),
            _ => Parser.FailUpdate<PropertyValue>($"Unknown key property_value.'{propertyName}'")
        }).Parse(ref reader, options);
    }
}
