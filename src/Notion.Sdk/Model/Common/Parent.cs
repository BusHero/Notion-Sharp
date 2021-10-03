using System;

namespace Notion.Model
{
    public record Parent
    {
        public record Workspace : Parent { }

        public record Page : Parent
        {
            public Guid Id { get; set; }
        }

        public record Database : Parent
        {
            public Guid Id { get; set; }
        }
    }
}