using System;
using System.Text.Json;
using Notion.Converters.Utils;
using Notion.Model;
using Pevac;

namespace Notion.Converters;

internal class ColorConverter: MyJsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "blue" => Color.Blue,
            "blue_background" => Color.BlueBackground,
            "brown" => Color.Brown,
            "brown_background" => Color.BrownBackground,
            "default" => Color.Default,
            "gray" => Color.Gray,
            "gray_background" => Color.GrayBackground,
            "green" => Color.Green,
            "green_background" => Color.GreenBackground,
            "orange" => Color.Orange,
            "orange_background" => Color.OrangeBackground,
            "yellow" => Color.Yellow,
            "yellow_background" => Color.YellowBackground,
            "pink" => Color.Pink,
            "pink_background" => Color.PinkBackground,
            "purple" => Color.Purple,
            "purple_background" => Color.PurpleBackground,
            "red" => Color.Red,
            "red_background" => Color.RedBackground,
            _ => throw new JsonException()
        };
    }
}