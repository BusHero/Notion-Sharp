using System;

namespace Notion.Model
{
    public record PropertyValue
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public T Copy<T>() where T : PropertyValue, new() => new()
        {
            Id = Id,
            Name = Name
        };

        public static T Copy<T>(PropertyValue property) where T : PropertyValue, new() => property.Copy<T>();

        public record Select : PropertyValue
        {
            public Option Option { get; set; }
        }

        public record MultiSelect : PropertyValue
        {
            public Option[] Options { get; set; }
        }

        public record Title : PropertyValue
        {
            public RichText[] Content { get; init; }
        }

        public record Number : PropertyValue
        {
            public double Value { get; init; }
        }

        public record Text : PropertyValue
        {
            public RichText[] Content { get; set; }
        }

        public record Checkbox : PropertyValue
        {
            public bool Checked { get; set; }
        }

        public record Url : PropertyValue
        {
            public Uri Link { get; init; }
        }

        public record Date : PropertyValue
        {
            public DateTimeOffset? Start { get; init; }
            public DateTimeOffset? End { get; init; }
        }

        public record PageReference(Guid id);

        public record Formula : PropertyValue
        {
            public record Srting : Formula
            {
                public string Value { get; init; }
            }

            public record Number : Formula
            {
                public decimal? Value { get; init; }
            }

            public record Boolean : Formula
            {
                public bool? Value { get; init; }
            }

            public record Date : Formula
            {
                public DateTime? Value { get; init; }
            }
        }

        public record Relation : PropertyValue
        {
            public PageReference[] Pages { get; init; }
        }

        public record Rollup : PropertyValue
        {
            public record Number : Rollup
            {
                public decimal Value { get; init; }
            }

            public record Date : Rollup
            {
                public DateTime? Value { get; init; }
            }

            public record Array : Rollup
            {
                public PropertyValue[] Value { get; init; }
            }
        }

        public record Email : PropertyValue
        {
            public string Value { get; init; }
        }

        public record People : PropertyValue
        {
            public User[] Value { get; init; }
        }

        public record Files : PropertyValue
        {
            public File[] Vaule { get; init; }
        }

        public record PhoneNumber : PropertyValue
        {
            public string Value { get; init; }
        }

        public record CreatedTime : PropertyValue
        {
            public DateTime Value { get; init; }
        }

        public record LastEditedTime : PropertyValue
        {
            public DateTime Value { get; init; }
        }

        public record CreatedBy : PropertyValue
        {
            public User Value { get; init; }
        }

        public record LastEditedBy : PropertyValue
        {
            public User Value { get; init; }
        }
    }
}