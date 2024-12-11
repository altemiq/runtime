namespace Altemiq.Protobuf.Converters;

public class StructConverterTests
{
    private static readonly System.Text.Json.JsonSerializerOptions Options = new()
    {
        Converters =
        {
            StructConverter.Instance,
            ValueConverter.Instance,
        },
    };

    private static readonly Google.Protobuf.WellKnownTypes.Struct Struct = new()
    {
        Fields =
        {
            { "Number", Google.Protobuf.WellKnownTypes.Value.ForNumber(1) },
            { "Double", Google.Protobuf.WellKnownTypes.Value.ForNumber(123.456) },
            { "False", Google.Protobuf.WellKnownTypes.Value.ForBool(false) },
            { "True", Google.Protobuf.WellKnownTypes.Value.ForBool(true) },
            { "Null", Google.Protobuf.WellKnownTypes.Value.ForNull() },
            { "String", Google.Protobuf.WellKnownTypes.Value.ForString("value") },
            {
                "Nested",
                Google.Protobuf.WellKnownTypes.Value.ForStruct(
                    new Google.Protobuf.WellKnownTypes.Struct
                    {
                        Fields =
                        {
                            { "Id", Google.Protobuf.WellKnownTypes.Value.ForNumber(1) }
                        },
                    })
            },
            {
                "Array",
                Google.Protobuf.WellKnownTypes.Value.ForList(
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(1),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(2),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(3),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(4),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(5))

            }
        }
    };

    private const string Json =
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

    [Fact]
    public void Deserialize()
    {
        Google.Protobuf.WellKnownTypes.Struct? @struct = System.Text.Json.JsonSerializer.Deserialize<Google.Protobuf.WellKnownTypes.Struct>(Json, Options);
        _ = @struct.Should().BeEquivalentTo(Struct);
    }

    [Fact]
    public void Serialize()
    {
        string json = System.Text.Json.JsonSerializer.Serialize(Struct, Options);
        _ = NormalizeJson(json).Should().BeEquivalentTo(NormalizeJson(Json));
    }

    [Fact]
    public void CreateStructFromJson()
    {
        System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(Json);
        Google.Protobuf.WellKnownTypes.Struct? @struct = StructConverter.ToStruct(document);
        _ = @struct.Should().NotBeNull();
    }

    [Fact]
    public void CreateFromEmptyDocument()
    {
        string json = "{}";
        System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(json);
        Google.Protobuf.WellKnownTypes.Struct? @struct = StructConverter.ToStruct(document);
        _ = @struct.Should().NotBeNull();
        _ = @struct!.Fields.Should().BeEmpty();
    }

    [Fact]
    public void CreateStructFromNull()
    {
        System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse("null");
        Google.Protobuf.WellKnownTypes.Struct? @struct = StructConverter.ToStruct(document);
        _ = @struct.Should().BeNull();
    }

    [Fact]
    public void CreateJsonFromStruct()
    {
        _ = Struct.ToJsonDocument().Should().NotBeNull();
    }

    private static string NormalizeJson(string json)
    {
        using MemoryStream memoryStream = new();
        using (System.Text.Json.Utf8JsonWriter writer = new(memoryStream))
        {
            System.Text.Json.JsonDocument.Parse(json).WriteTo(writer);
        }

        return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
    }
}
