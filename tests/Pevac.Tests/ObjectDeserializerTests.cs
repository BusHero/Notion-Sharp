using Xunit;
using FluentAssertions;
using System.Text;
using System.Text.Json;

namespace Pevac.Tests;

public class ObjectDeserializerTests
{
    private record Foo() { }

    private record Bar() : Foo
    {
        public int BarValue { get; set; }
    }

    private record Baz() : Foo
    {
        public string BazValue { get; set; }
    }

    private record Data(string Foo, string Bar)
    {
        public Data(): this(default, default) { }

        public string Foo { get; set; } = Foo;
        public string Bar { get; set; } = Bar;
    }

    private record SubType(string Foo, string Bar, string Baz) : Data(Foo, Bar)
    {
        public string Baz { get; set; } = Baz;

        public static SubType Copy(Data data) => new(data.Foo, data.Bar, "");
    }

    private Parser<Foo> FooParser { get; } = Parser.ParseObject(propertyName => propertyName switch
    {
        "type" => Parser.String.Updater<string, Foo>(),
        "bar" => Parser.ParseObject(propertyName => propertyName switch
        {
            "bar_value" => Parser.Int32.Updater((int data, Bar bar) => bar.BarValue = data),
            _ => Parser.FailUpdate<Bar>()
        }, (Foo parent) => new Bar()),
        "baz" => Parser.ParseObject(propertyName => propertyName switch
        {
            "baz_value" => Parser.String.Updater((string data, Baz baz) => baz.BazValue = data),
            _ => Parser.FailUpdate<Baz>()
        }, (Foo _) => new Baz()),
        _ => Parser.FailUpdate<Foo>()
    });

    [Fact]
    public void DeserializeObject()
    {
        var json = @"
            {
                ""foo"": ""foo"",
                ""bar"": ""bar""
            }";

        var bytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes);
        Parser<Data> parser = Parser.ParseObject(propertyName => propertyName switch
        {
            "foo" => Parser.String.Updater((string foo, Data data) => data with { Foo = foo }),
            "bar" => Parser.String.Updater((string bar, Data data) => data.Bar = bar),
            _ => Parser.FailUpdate<Data>()
        }, new Data("", ""));

        Parser.Parse(parser, ref reader, default).Should().Be(new Data("foo", "bar"));
    }

    [Fact]
    public void DeserializeStartedObject()
    {
        var json = @"
            {
                ""foo"": ""foo"",
                ""bar"": ""bar""
            }";

        var bytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes);
        reader.Read();
        Parser<Data> parser = Parser.ParseObject(propertyName => propertyName switch
        {
            "foo" => Parser.String.Updater((string foo, Data data) => data with { Foo = foo }),
            "bar" => Parser.String.Updater((string bar, Data data) => data.Bar = bar),
            _ => Parser.FailUpdate<Data>()
        }, new Data("", ""));

        Parser.Parse(parser, ref reader, default).Should().Be(new Data("foo", "bar"));
    }



    [Fact]
    public void DeserializeBar()
    {
        var json = @"
            {
                ""type"": ""bar"",
                ""bar"": {
                    ""bar_value"": 1
                }
            }";

        var bytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes);
        reader.Read();

        Parser.Parse(FooParser, ref reader, default).Should().BeEquivalentTo(new Bar { BarValue = 1 });
    }

    [Fact]
    public void DeserializeBaz()
    {
        var json = @"
            {
                ""type"": ""bar"",
                ""baz"": {
                    ""baz_value"": ""1""
                }
            }";

        var bytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes);
        reader.Read();
        Parser.Parse(FooParser, ref reader, default).Should().BeEquivalentTo(new Baz { BazValue = "1" });
    }

    [Fact]
    public void Deserializer_Succeds()
    {
        
        var json = @"
            {
                ""foo"": ""foo"",
                ""bar"": ""baz""
            }";
        var bytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes);
        reader.Read();
        var data = Parser.ParseObject(propertyName => propertyName switch
        {
            "foo" => Parser.String.Updater((string type, Data data) => data with { Foo = type}),
            "bar" => Parser.String.Updater<Data>(),
            _ => Parser.FailUpdate<Data>()
        }, new Data("default-value", "default-value")).Parse(ref reader, default).Should().BeEquivalentTo(new Data("foo", "default-value"));
    }
}
