namespace Altemiq.Protobuf.Converters;

using TUnit.Assertions.AssertConditions.Throws;

public class StructConverterTests
{
    private static readonly string Json =
        $$"""
        {
          "Number": 1,
          "{{nameof(Double)}}": 123.456,
          "{{bool.FalseString}}": false,
          "{{bool.TrueString}}": true,
          "Null": null,
          "{{nameof(String)}}": "value",
          "Nested": {
            "Id": 1
          },
          "{{nameof(Array)}}": [
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
            { nameof(Double), Google.Protobuf.WellKnownTypes.Value.ForNumber(123.456) },
            { bool.FalseString, Google.Protobuf.WellKnownTypes.Value.ForBool(false) },
            { bool.TrueString, Google.Protobuf.WellKnownTypes.Value.ForBool(true) },
            { "Null", Google.Protobuf.WellKnownTypes.Value.ForNull() },
            { nameof(String), Google.Protobuf.WellKnownTypes.Value.ForString("value") },
            {
                "Nested",
                Google.Protobuf.WellKnownTypes.Value.ForStruct(
                    new()
                    {
                        Fields =
                        {
                            { "Id", Google.Protobuf.WellKnownTypes.Value.ForNumber(1) }
                        },
                    })
            },
            {
                nameof(Array),
                Google.Protobuf.WellKnownTypes.Value.ForList(
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(1),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(2),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(3),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(4),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(5))
            }
        }
    };

    [Test]
    public async Task Deserialize()
    {
        await Assert.That(System.Text.Json.JsonSerializer.Deserialize<Google.Protobuf.WellKnownTypes.Struct>(Json, Options)).IsEqualTo(Struct);
    }

    [Test]
    public async Task DeserializeVsParseJson()
    {
        await Assert.That(System.Text.Json.JsonSerializer.Deserialize<Google.Protobuf.WellKnownTypes.Struct>(Json, Options)).IsEqualTo(Google.Protobuf.WellKnownTypes.Struct.Parser.ParseJson(Json));
    }

    [Test]
    public async Task Serialize()
    {
        await Assert.That(NormalizeJson(System.Text.Json.JsonSerializer.Serialize(Struct, Options))).IsEqualTo(NormalizeJson(Json));
    }

    [Test]
    public async Task SerializeVsToString()
    {
        await Assert.That(NormalizeJson(System.Text.Json.JsonSerializer.Serialize(Struct, Options))).IsEqualTo(NormalizeJson(Struct.ToString()));
    }

    [Test]
    public async Task CreateStructFromJson()
    {
        await Assert.That(StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse(Json))).IsEqualTo(Struct);
    }

    [Test]
    public async Task CreateFromEmptyDocument()
    {
        await Assert.That(StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse("{}")))
            .IsTypeOf<Google.Protobuf.WellKnownTypes.Struct>()
            .And.Satisfies(a => (IEnumerable<KeyValuePair<string, Google.Protobuf.WellKnownTypes.Value>>)a.Fields, fields => fields.IsEmpty());
    }

    [Test]
    public async Task CreateStructFromNull()
    {
        await Assert.That(StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse("null"))).IsNull();
    }

    [Test]
    public async Task CreateJsonFromStruct()
    {
        await Assert.That(Struct.ToJsonDocument()).IsNotNull();
    }

    [Test]
    public async Task FailToConvertValue()
    {
        await Assert.That(() => StructConverter.ToStruct(System.Text.Json.JsonDocument.Parse(bool.TrueString))).Throws<System.Text.Json.JsonException>();
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