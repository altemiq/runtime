namespace Altemiq.Protobuf.Converters;
public class ValueConverterTests
{
    public class JsonElement
    {
        [Fact]
        public void CreateFromJson()
        {
            string json = """
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

            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(json);
            Google.Protobuf.WellKnownTypes.Value value = ValueConverter.ToValue(document.RootElement);
            _ = value.Should().NotBeNull();
        }

        [Fact]
        public void CreateFromEmpty()
        {
            string json = "{}";
            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(json);
            Google.Protobuf.WellKnownTypes.Value value = ValueConverter.ToValue(document.RootElement);
            _ = value.Should().NotBeNull();
        }

        [Fact]
        public void CreateFromNull()
        {
            System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse("null");
            Google.Protobuf.WellKnownTypes.Value value = ValueConverter.ToValue(document.RootElement);
            _ = value.KindCase.Should().Be(Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue);
        }

        [Theory]
        [MemberData(nameof(GetElementData))]
        public void CreateFromValue(Google.Protobuf.WellKnownTypes.Value value, System.Text.Json.JsonValueKind valueKind, Func<System.Text.Json.JsonElement, object?> getValue, object? expected)
        {
            Google.Protobuf.WellKnownTypes.Value? v = value;
            _ = v.Should().NotBeNull();
            System.Text.Json.JsonElement element = ValueConverter.ToJsonElement(v!);
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
                    new int[] { 1, 2, 3, 4, 5 }
                },
            };
        }
    }

    public class JsonNode
    {
        [Fact]
        public void CreateFromJson()
        {
            string json = """
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

            Google.Protobuf.WellKnownTypes.Value value = ValueConverter.ToValue(System.Text.Json.Nodes.JsonNode.Parse(json));
            _ = value.Should().NotBeNull();
        }

        [Fact]
        public void CreateFromEmpty()
        {
            string json = "{}";
            System.Text.Json.Nodes.JsonNode? node = System.Text.Json.Nodes.JsonNode.Parse(json);
            Google.Protobuf.WellKnownTypes.Value value = ValueConverter.ToValue(node);
            _ = value.Should().NotBeNull();
        }

        [Fact]
        public void CreateFromNull()
        {
            System.Text.Json.Nodes.JsonNode? node = System.Text.Json.Nodes.JsonNode.Parse("null");
            Google.Protobuf.WellKnownTypes.Value value = ValueConverter.ToValue(node);
            _ = value.KindCase.Should().Be(Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue);
        }

        [Theory]
        [MemberData(nameof(GetElementData))]
        public void CreateFromValue(Google.Protobuf.WellKnownTypes.Value value, System.Text.Json.JsonValueKind valueKind, Func<System.Text.Json.Nodes.JsonNode, object?> getValue, object? expected)
        {
            System.Text.Json.Nodes.JsonNode? node = ValueConverter.ToJsonNode(value);
            if (node is not null)
            {
                _ = node.GetValueKind().Should().Be(valueKind);
                _ = getValue(node).Should().BeEquivalentTo(expected);
            }
        }

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
                    node => node.AsArray().Select(i => i.GetValue<double>()),
                    new double[] { 1D, 2D, 3D, 4D, 5D }
                }
            };
        }
    }
}
