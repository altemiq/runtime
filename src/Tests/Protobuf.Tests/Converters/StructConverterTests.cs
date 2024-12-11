namespace Altemiq.Protobuf.Converters;

public class StructConverterTests
{
    [Fact]
    public void CreateStructFromJson()
    {
        var json = """
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

        var document = System.Text.Json.JsonDocument.Parse(json);
        var @struct = StructConverter.ToStruct(document);
        @struct.Should().NotBeNull();
    }

    [Fact]
    public void CreateFromEmptyDocument()
    {
        var json = "{}";
        var document = System.Text.Json.JsonDocument.Parse(json);
        var @struct = StructConverter.ToStruct(document);
        @struct.Should().NotBeNull();
        @struct!.Fields.Should().BeEmpty();
    }

    [Fact]
    public void CreateStructFromNull()
    {
        var document = System.Text.Json.JsonDocument.Parse("null");
        var @struct = StructConverter.ToStruct(document);
        @struct.Should().BeNull();
    }

    [Fact]
    public void CreateJsonFromStruct()
    {
        var @struct = new Google.Protobuf.WellKnownTypes.Struct
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
                                {"Id", Google.Protobuf.WellKnownTypes.Value.ForNumber(1) }
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

        @struct.ToJsonDocument().Should().NotBeNull();
    }
}
