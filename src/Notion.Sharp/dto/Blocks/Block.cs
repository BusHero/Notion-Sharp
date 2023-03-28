using System;
using System.Text.Json.Serialization;

namespace Notion.Model;

public record LastEditedBy
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

public record CreatedBy
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

public record Block
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedTime { get; init; }
    public DateTimeOffset LastEditedTime { get; init; }
    public bool HasChildren { get; init; }
    public bool Archived { get; init; }
    
    public Parent? Parent { get; init; }
    
    public LastEditedBy? LastEditedBy { get; init; }
    
    public CreatedBy? CreatedBy { get; init; }
    
    public T Copy<T>() where T : Block, new() => new()
    {
        Id = Id,
        CreatedTime = CreatedTime,
        LastEditedTime = LastEditedTime,
        HasChildren = HasChildren,
        Parent = Parent,
        LastEditedBy = LastEditedBy
    };

    public static T Copy<T>(Block block) where T : Block, new() => block.Copy<T>();

    #region SubTypes
    
    public record Paragraph : Block
    {
        public RichText[] Text { get; set; }
        public string Color { get; set; }
    }

    public record Heading1 : Block
    {
        public RichText[] Text { get; set; }
        public string Color { get; set; }
        
        public bool IsToggable { get; set; }
    }

    public record Heading2 : Block
    {
        public RichText[] Text { get; set; }
    }

    public record Heading3 : Block
    {
        public RichText[] Text { get; set; }
    }

    public record NumberedListItem : Block
    {
        public RichText[] Text { get; set; }
        public Block[] Children { get; set; }
    }

    public record BulletedListItem : Block
    {
        public RichText[] Text { get; set; }
        public Block[] Children { get; set; }
    }

    public record ToDo : Block
    {
        public RichText[] Text { get; set; }
        public bool Checked { get; set; }
        public Block[] Children { get; set; }
    }

    public record Toggle : Block
    {
        public RichText[] Text { get; set; }
        public bool Checked { get; set; }
        public Block[] Children { get; set; }
    }

    public record Code : Block
    {
        public class LanguageType
        {
            public static LanguageType Abap { get; } = new LanguageType("abap");
            public static LanguageType Arduino { get; } = new LanguageType("arduino");
            public static LanguageType Bash { get; } = new LanguageType("bash");
            public static LanguageType Basic { get; } = new LanguageType("basic");
            public static LanguageType C { get; } = new LanguageType("c");
            public static LanguageType Clojure { get; } = new LanguageType("clojure");
            public static LanguageType Coffeescript { get; } = new LanguageType("coffeescript");
            public static LanguageType Cpp { get; } = new LanguageType("c++");
            public static LanguageType CSharp { get; } = new LanguageType("c#");
            public static LanguageType Css { get; } = new LanguageType("css");
            public static LanguageType Dart { get; } = new LanguageType("dart");
            public static LanguageType Diff { get; } = new LanguageType("diff");
            public static LanguageType Docker { get; } = new LanguageType("docker");
            public static LanguageType Elixir { get; } = new LanguageType("elixir");
            public static LanguageType Elm { get; } = new LanguageType("elm");
            public static LanguageType Erlang { get; } = new LanguageType("erlang");
            public static LanguageType Flow { get; } = new LanguageType("flow");
            public static LanguageType Fortran { get; } = new LanguageType("fortran");
            public static LanguageType FSharp { get; } = new LanguageType("f#");
            public static LanguageType Gherkin { get; } = new LanguageType("gherkin");
            public static LanguageType Glsl { get; } = new LanguageType("glsl");
            public static LanguageType Go { get; } = new LanguageType("go");
            public static LanguageType Graphql { get; } = new LanguageType("graphql");
            public static LanguageType Groovy { get; } = new LanguageType("groovy");
            public static LanguageType Haskell { get; } = new LanguageType("haskell");
            public static LanguageType Html { get; } = new LanguageType("html");
            public static LanguageType Java { get; } = new LanguageType("java");
            public static LanguageType JavaSsript { get; } = new LanguageType("javaSsript");
            public static LanguageType Json { get; } = new LanguageType("json");
            public static LanguageType Julia { get; } = new LanguageType("julia");
            public static LanguageType Kotlin { get; } = new LanguageType("kotlin");
            public static LanguageType Latex { get; } = new LanguageType("latex");
            public static LanguageType Less { get; } = new LanguageType("less");
            public static LanguageType Lisp { get; } = new LanguageType("lisp");
            public static LanguageType Livescript { get; } = new LanguageType("livescript");
            public static LanguageType Lua { get; } = new LanguageType("lua");
            public static LanguageType Makefile { get; } = new LanguageType("makefile");
            public static LanguageType Markdown { get; } = new LanguageType("markdown");
            public static LanguageType Markup { get; } = new LanguageType("markup");
            public static LanguageType Matlab { get; } = new LanguageType("matlab");
            public static LanguageType Mermaid { get; } = new LanguageType("mermaid");
            public static LanguageType Nix { get; } = new LanguageType("nix");
            public static LanguageType ObjectiveC { get; } = new LanguageType("objective-c");
            public static LanguageType Ocaml { get; } = new LanguageType("ocaml");
            public static LanguageType Pascal { get; } = new LanguageType("pascal");
            public static LanguageType Perl { get; } = new LanguageType("perl");
            public static LanguageType Php { get; } = new LanguageType("php");
            public static LanguageType PlainText { get; } = new LanguageType("plain text");
            public static LanguageType Powershell { get; } = new LanguageType("powershell");
            public static LanguageType Prolog { get; } = new LanguageType("prolog");
            public static LanguageType Protobuf { get; } = new LanguageType("protobuf");
            public static LanguageType Python { get; } = new LanguageType("python");
            public static LanguageType R { get; } = new LanguageType("r");
            public static LanguageType Reason { get; } = new LanguageType("reason");
            public static LanguageType Ruby { get; } = new LanguageType("ruby");
            public static LanguageType Rust { get; } = new LanguageType("rust");
            public static LanguageType Sass { get; } = new LanguageType("sass");
            public static LanguageType Scala { get; } = new LanguageType("scala");
            public static LanguageType Scheme { get; } = new LanguageType("scheme");
            public static LanguageType Scss { get; } = new LanguageType("scss");
            public static LanguageType Shell { get; } = new LanguageType("shell");
            public static LanguageType Sql { get; } = new LanguageType("sql");
            public static LanguageType Swift { get; } = new LanguageType("swift");
            public static LanguageType Typescript { get; } = new LanguageType("typescript");
            public static LanguageType VbNet { get; } = new LanguageType("vb.net");
            public static LanguageType Verilog { get; } = new LanguageType("verilog");
            public static LanguageType Vhdl { get; } = new LanguageType("vhdl");
            public static LanguageType VisualBasic { get; } = new LanguageType("visual basic");
            public static LanguageType Webassembly { get; } = new LanguageType("webassembly");
            public static LanguageType Xml { get; } = new LanguageType("xml");
            public static LanguageType Yaml { get; } = new LanguageType("yaml");

            private readonly string language;

            public LanguageType(string language) => this.language = language ?? throw new ArgumentNullException(nameof(language));

            public override string ToString() => language;
        }

        public object Text { get; init; }
        public string Language { get; init; }
        
        public RichText[] Caption { get; init; }
    }

    public record ChildPage : Block
    {
        public string Title { get; set; }
    }

    public record ChildDatabase : Block
    {
        public string Title { get; set; }
    }

    public record Embed : Block
    {
        public RichText[] Caption { get; init; }
        public Uri Url { get; init; }
    }

    public record Image : Block
    {
        public File File { get; init; }
    }

    public record Video : Block
    {
        public File File { get; init; }
    }

    public record FileBlock : Block
    {
        public File File { get; init; }
    }

    public record Pdf : Block
    {
        public File File { get; init; }
    }

    public record Bookmark : Block
    {
        public RichText[] Caption { get; init; }
        public Uri Url { get; init; }
    }

    public record Callout : Block
    {
        public RichText[] Text { get; init; }
        public Emoji Icon { get; init; }
    }

    public record Quote : Block
    {
        public RichText[] Text { get; init; }
    }

    public record Equation : Block
    {
        public string Expression { get; init; }
    }

    public record Divider : Block
    {

    }

    public record TableOfContents : Block
    {

    }

    public record Unsupported : Block { }

    public record Breadcrumb : Block { }

    public record ColumnList : Block { }

    public record Column : Block { }

    public record LinkPreview : Block
    {
        public Uri Url { get; init; }
    }

    public record LinkToPage: Block
    {
        
    }

    public record DatabasePageLink : LinkToPage
    {
        public Guid DatabaseId { get; set; } 
    }

    public record PagePageLink : LinkToPage
    {
        public Guid PageId { get; set; } 
    }

    #endregion
}
