namespace Altemiq.Protobuf.Converters;
public class ValueConverterTests
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

    public class JsonElement
    {
        private static readonly int[] intArray = [1, 2, 3, 4, 5];

        [Fact]
        public void CreateFromJson()
        {
            _ = ValueConverter.ToValue(System.Text.Json.JsonDocument.Parse(Json).RootElement).Should().NotBeNull();
        }

        [Fact]
        public void CreateFromEmpty()
        {
            _ = ValueConverter.ToValue(System.Text.Json.JsonDocument.Parse("{}").RootElement).Should().NotBeNull();
        }

        [Fact]
        public void CreateFromNull()
        {
            _ = ValueConverter.ToValue(System.Text.Json.JsonDocument.Parse("null").RootElement).KindCase.Should().Be(Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue);
        }

        [Theory]
        [MemberData(nameof(GetElementData))]
        public void CreateFromValue(Google.Protobuf.WellKnownTypes.Value value, System.Text.Json.JsonValueKind valueKind, Func<System.Text.Json.JsonElement, object?> getValue, object? expected)
        {
            System.Text.Json.JsonElement element = ValueConverter.ToJsonElement(value);
            _ = element.ValueKind.Should().Be(valueKind);
            _ = getValue(element).Should().BeEquivalentTo(expected);
        }

        public static TheoryData<Google.Protobuf.WellKnownTypes.Value, System.Text.Json.JsonValueKind, Func<System.Text.Json.JsonElement, object?>, object?> GetElementData()
        {
            return new TheoryData<Google.Protobuf.WellKnownTypes.Value, System.Text.Json.JsonValueKind, Func<System.Text.Json.JsonElement, object?>, object?>()
            {
                { Google.Protobuf.WellKnownTypes.Value.ForNumber(1), System.Text.Json.JsonValueKind.Number, (element) => element.GetUInt32(), 1 },
                { Google.Protobuf.WellKnownTypes.Value.ForNumber(123.456), System.Text.Json.JsonValueKind.Number, (element) => element.GetDouble(), 123.456 },
                { Google.Protobuf.WellKnownTypes.Value.ForBool(false), System.Text.Json.JsonValueKind.False, (element) => element.GetBoolean(), false },
                { Google.Protobuf.WellKnownTypes.Value.ForBool(true), System.Text.Json.JsonValueKind.True, (element) => element.GetBoolean(), true },
                { Google.Protobuf.WellKnownTypes.Value.ForNull(), System.Text.Json.JsonValueKind.Null, (element) => null, null },
                { Google.Protobuf.WellKnownTypes.Value.ForString("value"), System.Text.Json.JsonValueKind.String, (element) => element.GetString(), "value" },{
                    Google.Protobuf.WellKnownTypes.Value.ForList(
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(1),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(2),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(3),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(4),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(5)),
                    System.Text.Json.JsonValueKind.Array,
                    element => element.EnumerateArray().Select(e => e.GetInt32()),
                    intArray
                },
            };
        }
    }

    public class JsonNode
    {
        [Fact]
        public void CreateFromJson()
        {
            _ = ValueConverter.ToValue(System.Text.Json.Nodes.JsonNode.Parse(Json)).Should().NotBeNull();
        }

        [Fact]
        public void CreateFromEmpty()
        {
            _ = ValueConverter.ToValue(System.Text.Json.Nodes.JsonNode.Parse("{}")).Should().NotBeNull();
        }

        [Fact]
        public void CreateFromNull()
        {
            _ = ValueConverter.ToValue(System.Text.Json.Nodes.JsonNode.Parse("null")).KindCase.Should().Be(Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue);
        }

        [Theory]
        [MemberData(nameof(GetElementData))]
        public void CreateFromValue(Google.Protobuf.WellKnownTypes.Value value, System.Text.Json.JsonValueKind valueKind, Func<System.Text.Json.Nodes.JsonNode, object?> getValue, object? expected)
        {
            if (ValueConverter.ToJsonNode(value) is { } node)
            {
                _ = node.GetValueKind().Should().Be(valueKind);
                _ = getValue(node).Should().BeEquivalentTo(expected);
            }
        }

        private static readonly double[] doubleArray = [1D, 2D, 3D, 4D, 5D];

        public static TheoryData<Google.Protobuf.WellKnownTypes.Value, System.Text.Json.JsonValueKind, Func<System.Text.Json.Nodes.JsonNode, object?>, object?> GetElementData()
        {
            return new TheoryData<Google.Protobuf.WellKnownTypes.Value, System.Text.Json.JsonValueKind, Func<System.Text.Json.Nodes.JsonNode, object?>, object?>()
            {
                { Google.Protobuf.WellKnownTypes.Value.ForNumber(1), System.Text.Json.JsonValueKind.Number, (node) => (int)node.GetValue<double>(), 1 },
                { Google.Protobuf.WellKnownTypes.Value.ForNumber(123.456), System.Text.Json.JsonValueKind.Number, (node) => node.GetValue<double>(), 123.456 },
                { Google.Protobuf.WellKnownTypes.Value.ForBool(false), System.Text.Json.JsonValueKind.False, (node) => node.GetValue<bool>(), false },
                { Google.Protobuf.WellKnownTypes.Value.ForBool(true), System.Text.Json.JsonValueKind.True, (node) => node.GetValue<bool>(), true },
                { Google.Protobuf.WellKnownTypes.Value.ForNull(), System.Text.Json.JsonValueKind.Null, (node) => null, null },
                { Google.Protobuf.WellKnownTypes.Value.ForString("value"), System.Text.Json.JsonValueKind.String, (node) => node.GetValue<string>(), "value" },
                {
                    Google.Protobuf.WellKnownTypes.Value.ForList(
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(1),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(2),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(3),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(4),
                        Google.Protobuf.WellKnownTypes.Value.ForNumber(5)),
                    System.Text.Json.JsonValueKind.Array,
                    node => node.AsArray().Select(i => i!.GetValue<double>()),
                    doubleArray
                }
            };
        }
    }
}
