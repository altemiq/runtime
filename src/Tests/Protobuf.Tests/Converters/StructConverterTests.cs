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
        Assert.Equal(Struct, System.Text.Json.JsonSerializer.Deserialize<Google.Protobuf.WellKnownTypes.Struct>(Json, Options));
    }

    [Fact]
    public void DeserializeVsParseJson()
    {
        Assert.Equal(Google.Protobuf.WellKnownTypes.Struct.Parser.ParseJson(Json), System.Text.Json.JsonSerializer.Deserialize<Google.Protobuf.WellKnownTypes.Struct>(Json, Options));
    }

    [Fact]
    public void Serialize()
    {
        Assert.Equal(NormalizeJson(Json), NormalizeJson(System.Text.Json.JsonSerializer.Serialize(Struct, Options)));
    }

    [Fact]
    public void SerializeVsToString()
    {
        Assert.Equal(NormalizeJson(Struct.ToString()), NormalizeJson(System.Text.Json.JsonSerializer.Serialize(Struct, Options)));
    }

    [Fact]
    public void CreateStructFromJson()
    {
        Assert.Equal(Struct, StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse(Json)));
    }

    [Fact]
    public void CreateFromEmptyDocument()
    {
        var actual = Assert.IsType<Google.Protobuf.WellKnownTypes.Struct>(StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse("{}")));
        Assert.Empty(actual.Fields);
    }

    [Fact]
    public void CreateStructFromNull()
    {
        Assert.Null(StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse("null")));
    }

    [Fact]
    public void CreateJsonFromStruct()
    {
        Assert.NotNull(Struct.ToJsonDocument());
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