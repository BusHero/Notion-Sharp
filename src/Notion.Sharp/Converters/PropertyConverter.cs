using Notion.Model;

using Pevac;

using System;
using System.Text.Json;

using Void = Pevac.Void;

namespace Notion.Converters;

internal class PropertyConverter : MyJsonConverter<Property>
{
    public override Property Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Parser.ParseObject(propertyName => propertyName switch
        {
            "id" => Parser.String.Updater((string id, Property property) => property with { Id = id }),
            "name" => Parser.String.Updater((string name, Property property) => property with { Name = name }),
            "type" => Parser.String.Updater<string, Property>(),
            "title" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Title>()),
            "rich_text" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.RichTextProperty>()),
            "number" => Parser.ParseObject(propertyName => propertyName switch
            {
                "format" => Parser.String.Updater((string numberFormat, Property.Number number) => number with { Format = numberFormat }),
                var key => Parser.FailUpdate<Property.Number>($"Unknown key property.number.{key}")
            }, (Property property) => property.Copy<Property.Number>()),
            "select" => Parser.ParseObject(propertyName => propertyName switch
            {
                "options" => Parser.ParseType<Option[]>().Updater((Option[] options, Property.Select select) => select with { Options = options }),
                var key => Parser.FailUpdate<Property.Select>($"Unknown key property.select.{key}")
            }, (Property property) => property.Copy<Property.Select>()),
            "multi_select" => Parser.ParseObject(propertyName => propertyName switch
            {
                "options" => Parser.ParseType<Option[]>().Updater((Option[] options, Property.MultiSelect multiSelect) => multiSelect with { Options = options }),
                var key => Parser.FailUpdate<Property.MultiSelect>($"Unknown key property.multi_select.'{key}'")
            }, (Property property) => property.Copy<Property.MultiSelect>()),
            "date" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Date>()),
            "people" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.People>()),
            "files" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Files>()),
            "checkbox" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Checkbox>()),
            "url" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Url>()),
            "email" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Email>()),
            "phone_number" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.PhoneNumber>()),
            "formula" => Parser.ParseObject(propertyName => propertyName switch
            {
                "expression" => Parser.OptionalString.Updater((string expression, Property.Formula Formula) => Formula with { Expression = expression }),
                var key => Parser.FailUpdate<Property.Formula>($"Unknown key property.formula.{key}")
            }, (Property property) => property.Copy<Property.Formula>()),
            "relation" => Parser.ParseObject(propertyName => propertyName switch
            {
                "database_id" => Parser.Guid.Updater((Guid databaseId, Property.Relation relation) => relation with { DatabaseId = databaseId }),
                "synced_property_name" => Parser.String.Updater((string syncedPropertyName, Property.Relation relation) => relation with { SyncedPropertyName = syncedPropertyName }),
                "synced_property_id" => Parser.String.Updater((string syncedPropertyId, Property.Relation relation) => relation with { SyncedPropertyId = syncedPropertyId }),
                var key => Parser.FailUpdate<Property.Relation>($"Unknown key property.relation.{key}")
            }, (Property property) => property.Copy<Property.Relation>()),
            "rollup" => Parser.ParseObject(propertyName => propertyName switch
            {
                "rollup_property_name" => Parser.String.Updater((string rollupPropertyName, Property.Rollup rollup) => rollup with { RollupPropertyName = rollupPropertyName }),
                "relation_property_name" => Parser.String.Updater((string relationPropertyName, Property.Rollup multiSelect) => multiSelect with { RelationPropertyName = relationPropertyName }),
                "rollup_property_id" => Parser.String.Updater((string rollupPropertyId, Property.Rollup multiSelect) => multiSelect with { RollupPropertyId = rollupPropertyId }),
                "relation_property_id" => Parser.String.Updater((string relationPropertyId, Property.Rollup multiSelect) => multiSelect with { RelationPropertyId = relationPropertyId }),
                "function" => Parser.String.Updater((string function, Property.Rollup multiSelect) => multiSelect with { Function = function }),
                var key => Parser.FailUpdate<Property.Rollup>($"Unknown key property.rollup.{key}")
            }, (Property property) => property.Copy<Property.Rollup>()),
            "created_time" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.CreatedTime>()),
            "created_by" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.CreatedBy>()),
            "last_edited_time" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.LastEditedTime>()),
            "last_edited_by" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.LastEditedBy>()),
            var key => Parser.FailUpdate<Property>($"Unknown key property.{key}")
        }).Parse(ref reader, options);
    }
}
