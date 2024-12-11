namespace Altemiq.Protobuf.Converters;

using BenchmarkDotNet.Attributes;
using Google.Protobuf.WellKnownTypes;

public class StructConverterBenchmarks
{
    private static readonly System.Text.Json.JsonSerializerOptions Options = new()
    {
        Converters =
        {
            StructConverter.Instance,
            ValueConverter.Instance,
        },
    };

    public class Serialize
    {
        private readonly Struct Struct = new()
        {
            Fields =
            {
                { "Number", Value.ForNumber(1) },
                { "Double", Value.ForNumber(123.456) },
                { "False", Value.ForBool(false) },
                { "True", Value.ForBool(true) },
                { "Null", Value.ForNull() },
                { "String", Value.ForString("value") },
                {
                    "Nested",
                    Value.ForStruct(
                        new Struct
                        {
                            Fields =
                            {
                                { "Id", Value.ForNumber(1) }
                            },
                        })
                },
                {
                    "Array",
                    Value.ForList(
                        Value.ForNumber(1),
                        Value.ForNumber(2),
                        Value.ForNumber(3),
                        Value.ForNumber(4),
                        Value.ForNumber(5))

                }
            }
        };

        [Benchmark]
        public string JsonSerializer() => System.Text.Json.JsonSerializer.Serialize(Struct, Options);

        [Benchmark]
        public string StructToString() => Struct.ToString();
    }

    public class Deserialize()
    {
        private readonly string json =
            """
            {
                "Number": 1,
                "Double": 123.456,
                "False": false,
                "True": true,
                "Null": null,
                "String": "value",
                "Nested": {
                    "Id": 1
                },
                "Array": [
                    1,
                    2,
                    3,
                    4,
                    5
                ]
            }
            """;

        [Benchmark]
        public Struct? JsonSerializer() => System.Text.Json.JsonSerializer.Deserialize<Struct>(json, Options);

        [Benchmark]
        public Struct Parse() => Struct.Parser.ParseJson(json);
    }
}
