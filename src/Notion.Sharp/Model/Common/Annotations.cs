namespace Notion.Model
{
    public record Annotations
    {
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Strikethrough { get; set; }
        public bool Underline { get; set; }
        public bool Code { get; set; }
        public Color Color { get; set; }
    }
}