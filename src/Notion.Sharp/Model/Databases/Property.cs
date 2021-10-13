using System;

namespace Notion.Model
{
    public record Property
    {
        public string Id { get; init; }
        public string Name { get; init; }

        public T Copy<T>() where T : Property, new() => new()
        {
            Id = Id,
            Name = Name
        };

        public record Title : Property
        {
        }

        public record RichTextProperty : Property { }

        public record Date : Property { }

        public record People : Property { }

        public record Files : Property { }

        public record Checkbox : Property { }

        public record CreatedTime : Property { }

        public record Email : Property { }

        public record PhoneNumber : Property { }


        public record Number : Property
        {
            public string Format { get; init; }

            public enum NumberFormat
            {
                Number,
                NumberWithCommas,
                Percent,
                Dollar,
                CanadianDollar,
                EuroPound,
                Yen,
                Ruble,
                Rupee,
                Won,
                Yuan,
                Real,
                Lira,
                Rupiah,
                Franc,
                HongKongDollar,
                NewZealandDollar,
                Krona,
                NorwegianKrone,
                MexicanPeso,
                Rand,
                NewTaiwanDollar,
                DanishKrone,
                Zloty,
                Baht,
                Forint,
                Koruna,
                Shekel,
                ChileanPeso,
                PhilippinePeso,
                Dirham,
                ColombianPeso,
                Riyal,
                Ringgit,
                Leu,
            }
        }

        public record Select : Property
        {
            public Option[] Options { get; init; }
        }

        public record MultiSelect : Property
        {
            public Option[] Options { get; init; }
        }

        public record LastEditedTime : Property { }

        public record LastEditedBy : Property { }

        public record CreatedBy : Property { }

        public record Url : Property { }

        public record Rollup : Property
        {
            public const string type = "rollup";

            public string RelationPropertyName { get; init; }
            public string RelationPropertyId { get; init; }
            public string RollupPropertyName { get; init; }
            public string RollupPropertyId { get; init; }
            public string Function { get; init; }

            public enum RollupFunction
            {
                CountAll,
                CountValues,
                CountUniqueValues,
                CountEmpty,
                CountNotEmpty,
                PercentEmpty,
                PercentNotEmpty,
                Sum,
                Average,
                Median,
                Min,
                Max,
                Range,
                ShowOriginal
            }
        }

        public record Relation : Property
        {
            public const string type = "relation";

            public Guid DatabaseId { get; init; }
            public string SyncedPropertyName { get; init; }
            public string SyncedPropertyId { get; init; }
        }

        public record Formula : Property
        {
            public const string type = "formula";

            public string Expression { get; init; }
        }
    }
}