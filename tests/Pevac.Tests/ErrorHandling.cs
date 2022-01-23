using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace Pevac.Tests;

record Model(string Foo, string Bar);

public class ErrorHandling
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ErrorHandling(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void UnknownProperty_Fails()
    {
        const string bar = "{\"foo\": \"foo\", \"bar\": \"bar\", \"baz\": 123}";
        var foo = Parser.ParseObject(property => property switch
        {
            "foo" => Parser.ParseString("foo").Updater((string foo, Model model) => model with { Foo = foo }),
            "bar" => Parser.ParseString("bar").Updater((string bar1, Model model) => model with { Bar = bar1 }),
            var key => Parser.FailUpdate<Model>($"Unknown Key: {key}")
        }, new Model("", ""));
        
        var act = () =>
        {
            var reader = bar.ToUtf8JsonReader();
            return foo.Parse(ref reader, default);
        };
        
        act.Should()
            .Throw<ParseException>()
            .WithMessage("Unknown Key: baz");
    }
    
}

public static class StringUtils
{
    public static Utf8JsonReader ToUtf8JsonReader(this string json, int skip = 0)
    {
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        Utf8JsonReader reader = new(jsonBytes);
        if (skip <= 0) return reader;
        for (; skip != 0; skip--)
            _ = reader.Read();
        return reader;
    }
}