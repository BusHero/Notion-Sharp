using System;

namespace Notion.Model
{
    public record Option
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public Color Color { get; init; }
    }
}