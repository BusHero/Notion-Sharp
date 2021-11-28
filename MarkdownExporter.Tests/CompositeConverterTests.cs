using MarkdownExporter.Converters;

using System.Collections.Generic;
using System.Text.Json;

namespace MarkdownExporter.Tests;

public class CompositeConverterTests
{
    [Fact]
    public void Foo()
    {
        var first = Converter.ToConverter<int>((_, _) => Option.None<List<string>>());
        var second = Converter.ToConverter<int>((_, _) => Option.None<List<string>>());
        var third= Converter.ToConverter<int>((x, _) => Lists.Of(x.ToString()).ToOption());

        var converter = new CompositeConverter(
            first, second, third);

        var expectedResult = Lists.Of("1").ToOption().Select(x => JsonSerializer.Serialize(x));

        var actualResult = converter.Convert(1, default).Select(x => JsonSerializer.Serialize(x));

        Assert.Equal(expectedResult, actualResult, new OptionComparer<string>());
    }
}
