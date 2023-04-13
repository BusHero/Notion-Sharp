using Notion.Model;
using Pevac;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Void = Pevac.Void;

namespace Notion.Converters;

internal class PropertyConverter : MyJsonConverter<Property>
{
    public override Property Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        Parser.ParseObject(propertyName => propertyName switch
        {
            "id" => Parser.String.Updater((string? id, Property property) => property with { Id = id }),
            "name" => Parser.String.Updater((string? name, Property property) => property with { Name = name }),
            "type" => Parser.String.Updater<string?, Property>(),
            "title" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Title>()),
            "rich_text" => Parser.EmptyObject.Updater((Void _, Property property) =>
                property.Copy<Property.RichTextProperty>()),
            "number" => Parser.ParseObject(propertyName1 =>
            {
                var func = propertyName1 switch
                {
                    "format" => Parser.String.Updater((string? numberFormat, Property.Number number) =>
                        number with { Format = numberFormat }),
                    _ => Parser.FailUpdate<Property.Number>($"Unknown key property.number.{propertyName1}")
                };
                return func;
            }, (Property property) => property.Copy<Property.Number>()),
            "select" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "options" => Parser.ParseType<Option[]>().Updater((Option[]? options1, Property.Select select) =>
                    select with { Options = options1 }),
                _ => Parser.FailUpdate<Property.Select>($"Unknown key property.select.{propertyName1}")
            }, (Property property) => property.Copy<Property.Select>()),
            "multi_select" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "options" => Parser.ParseType<Option[]>()
                    .Updater((Option[]? options1, Property.MultiSelect multiSelect) =>
                        multiSelect with { Options = options1 }),
                _ => Parser.FailUpdate<Property.MultiSelect>($"Unknown key property.multi_select.'{propertyName1}'")
            }, (Property property) => property.Copy<Property.MultiSelect>()),
            "date" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Date>()),
            "people" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.People>()),
            "files" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Files>()),
            "checkbox" => Parser.EmptyObject.Updater((Void _, Property property) =>
                property.Copy<Property.Checkbox>()),
            "url" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Url>()),
            "email" => Parser.EmptyObject.Updater((Void _, Property property) => property.Copy<Property.Email>()),
            "phone_number" => Parser.EmptyObject.Updater((Void _, Property property) =>
                property.Copy<Property.PhoneNumber>()),
            "formula" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "expression" => Parser.OptionalString.Updater((string? expression, Property.Formula formula) =>
                    formula with { Expression = expression }),
                _ => Parser.FailUpdate<Property.Formula>($"Unknown key property.formula.{propertyName1}")
            }, (Property property) => property.Copy<Property.Formula>()),
            "relation" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "database_id" => Parser.Guid.Updater((Guid databaseId, Property.Relation relation) =>
                    relation with { DatabaseId = databaseId }),
                "type" => Parser.String.Updater<string?, Property.Relation>(),
                "dual_property" => Parser.ParseObject(propertyName2 => propertyName2 switch
                {
                    "synced_property_name" => Parser.String.Updater((string? syncedPropertyName, Property.Relation.DualRelation dualRelation) => dualRelation with{SyncedPropertyName = syncedPropertyName}),
                    "synced_property_id" => Parser.String.Updater((string? syncedPropertyId, Property.Relation.DualRelation dualRelation) => dualRelation with { SyncedPropertyId = syncedPropertyId}),
                    _ => Parser.FailUpdate<Property.Relation.DualRelation>($"Unknown key property.relation.dual_property.{propertyName2}"),
                }, (Property.Relation relation) => relation.Copy<Property.Relation.DualRelation>()),
                _ => Parser.FailUpdate<Property.Relation>($"Unknown key property.relation.{propertyName1}")
            }, (Property property) => property.Copy<Property.Relation>()),
            "rollup" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "rollup_property_name" => Parser.String.Updater(
                    (string? rollupPropertyName, Property.Rollup rollup) =>
                        rollup with { RollupPropertyName = rollupPropertyName }),
                "relation_property_name" => Parser.String.Updater(
                    (string? relationPropertyName, Property.Rollup multiSelect) => multiSelect with
                    {
                        RelationPropertyName = relationPropertyName
                    }),
                "rollup_property_id" => Parser.String.Updater(
                    (string? rollupPropertyId, Property.Rollup multiSelect) =>
                        multiSelect with { RollupPropertyId = rollupPropertyId }),
                "relation_property_id" => Parser.String.Updater(
                    (string? relationPropertyId, Property.Rollup multiSelect) =>
                        multiSelect with { RelationPropertyId = relationPropertyId }),
                "function" => Parser.String.Updater((string? function, Property.Rollup multiSelect) =>
                    multiSelect with { Function = function }),
                _ => Parser.FailUpdate<Property.Rollup>($"Unknown key property.rollup.{propertyName1}")
            }, (Property property) => property.Copy<Property.Rollup>()),
            "created_time" => Parser.EmptyObject.Updater((Void _, Property property) =>
                property.Copy<Property.CreatedTime>()),
            "created_by" => Parser.EmptyObject.Updater((Void _, Property property) =>
                property.Copy<Property.CreatedBy>()),
            "last_edited_time" => Parser.EmptyObject.Updater((Void _, Property property) =>
                property.Copy<Property.LastEditedTime>()),
            "last_edited_by" => Parser.EmptyObject.Updater((Void _, Property property) =>
                property.Copy<Property.LastEditedBy>()),
            "status" => Parser.ParseObject(propertyName1 => propertyName1 switch
            {
                "options" => Parser.ParseType<List<Property.Status.Option>>().Updater(
                    (List<Property.Status.Option>? options1, Property.Status status) =>
                        status with { Options = options1 }),
                "groups" => Parser.ParseType<List<Property.Status.Group>>().Updater(
                    (List<Property.Status.Group>? groups, Property.Status status) =>
                        status with { Groups = groups }),
                _ => Parser.FailUpdate<Property.Status>($"$Unknown key property.status.{propertyName1}")
            }, (Property property) => property.Copy<Property.Status>()),
            _ => Parser.FailUpdate<Property>($"Unknown key property.{propertyName}")
        }).Parse(ref reader, options);
}