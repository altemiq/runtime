namespace Altemiq.Protobuf.Converters;

public class StructConverterTests
{
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

    [Fact]
    public void Deserialize()
    {
        _ = System.Text.Json.JsonSerializer.Deserialize<Google.Protobuf.WellKnownTypes.Struct>(Json, Options).Should().BeEquivalentTo(Struct);
    }

    [Fact]
    public void DeserializeVsParseJson()
    {
        _ = System.Text.Json.JsonSerializer.Deserialize<Google.Protobuf.WellKnownTypes.Struct>(Json, Options).Should().BeEquivalentTo(Google.Protobuf.WellKnownTypes.Struct.Parser.ParseJson(Json));
    }

    [Fact]
    public void Serialize()
    {
        _ = NormalizeJson(System.Text.Json.JsonSerializer.Serialize(Struct, Options)).Should().BeEquivalentTo(NormalizeJson(Json));
    }

    [Fact]
    public void SerializeVsToString()
    {
        _ = NormalizeJson(System.Text.Json.JsonSerializer.Serialize(Struct, Options)).Should().BeEquivalentTo(NormalizeJson(Struct.ToString()));
    }

    [Fact]
    public void CreateStructFromJson()
    {
        _ = StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse(Json)).Should().BeEquivalentTo(Struct);
    }

    [Fact]
    public void CreateFromEmptyDocument()
    {
        _ = StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse("{}"))?.Fields.Should().BeEmpty();
    }

    [Fact]
    public void CreateStructFromNull()
    {
        _ = StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse("null")).Should().BeNull();
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
