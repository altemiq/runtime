namespace Altemiq.Protobuf.Converters;

using System.Reflection;

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

        [Test]
        public async Task CreateFromJson()
        {
            await Assert.That(ValueConverter.ToValue(System.Text.Json.JsonDocument.Parse(Json).RootElement)).IsNotNull();
        }

        [Test]
        public async Task CreateFromEmpty()
        {
            await Assert.That(ValueConverter.ToValue(System.Text.Json.JsonDocument.Parse("{}").RootElement)).IsNotNull();
        }

        [Test]
        public async Task CreateFromNull()
        {
            await Assert.That(ValueConverter.ToValue(System.Text.Json.JsonDocument.Parse("null").RootElement).KindCase).IsEqualTo(Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue);
        }

        [Test]
        [MethodDataSource(nameof(GetElementData))]
        public async Task CreateFromValue(Google.Protobuf.WellKnownTypes.Value value, System.Text.Json.JsonValueKind valueKind, Func<System.Text.Json.JsonElement, object?> getValue, object? expected)
        {
            var element = ValueConverter.ToJsonElement(value);
            await Assert.That(element.ValueKind).IsEqualTo(valueKind);
            await Assert.That(getValue(element)).IsEquivalentTo(expected);
        }

        public static IEnumerable<Func<(Google.Protobuf.WellKnownTypes.Value, System.Text.Json.JsonValueKind, Func<System.Text.Json.JsonElement, object?>, object?)>> GetElementData()
        {
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForNumber(1), System.Text.Json.JsonValueKind.Number, (element) => element.GetUInt32(), 1U);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForNumber(123.456), System.Text.Json.JsonValueKind.Number, (element) => element.GetDouble(), 123.456);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForBool(false), System.Text.Json.JsonValueKind.False, (element) => element.GetBoolean(), false);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForBool(true), System.Text.Json.JsonValueKind.True, (element) => element.GetBoolean(), true);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForNull(), System.Text.Json.JsonValueKind.Null, (element) => null, null);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForString("value"), System.Text.Json.JsonValueKind.String, (element) => element.GetString(), "value");
            yield return () => (
                Google.Protobuf.WellKnownTypes.Value.ForList(
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(1),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(2),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(3),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(4),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(5)),
                System.Text.Json.JsonValueKind.Array,
                element => element.EnumerateArray().Select(e => e.GetInt32()),
                intArray);
            yield return () => (
                Google.Protobuf.WellKnownTypes.Value.ForStruct(new Google.Protobuf.WellKnownTypes.Struct { Fields = { { "Id", Google.Protobuf.WellKnownTypes.Value.ForNumber(1) } } }),
                System.Text.Json.JsonValueKind.Object,
                element => element.ToString(),
                """{ "Id": 1 }""");
        }
    }

    public class JsonNode
    {
        [Test]
        public async Task CreateFromJson()
        {
            await Assert.That(ValueConverter.ToValue(System.Text.Json.Nodes.JsonNode.Parse(Json))).IsNotNull();
        }

        [Test]
        public async Task CreateFromEmpty()
        {
            await Assert.That(ValueConverter.ToValue(System.Text.Json.Nodes.JsonNode.Parse("{}"))).IsNotNull();
        }

        [Test]
        public async Task CreateFromNull()
        {
            await Assert.That(ValueConverter.ToValue(System.Text.Json.Nodes.JsonNode.Parse("null")).KindCase).IsEqualTo(Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue);
        }

        [Test]
        [MethodDataSource(nameof(GetElementData))]
        public async Task CreateFromValue(Google.Protobuf.WellKnownTypes.Value value, System.Text.Json.JsonValueKind valueKind, Func<System.Text.Json.Nodes.JsonNode, object?> getValue, object? expected)
        {
            if (ValueConverter.ToJsonNode(value) is { } node)
            {
                await Assert.That(node.GetValueKind()).IsEqualTo(valueKind);
                var nodeValue = getValue(node);
                if (nodeValue is System.Collections.IEnumerable enumberable && expected is not null)
                {
                    await IsEquivalentTo(enumberable, expected);
                }
                else
                {
                    await Assert.That(nodeValue).IsEqualTo(expected);
                }
            }
        }

        private static readonly double[] doubleArray = [1D, 2D, 3D, 4D, 5D];

        public static IEnumerable<Func<(Google.Protobuf.WellKnownTypes.Value, System.Text.Json.JsonValueKind, Func<System.Text.Json.Nodes.JsonNode, object?>, object?)>> GetElementData()
        {
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForNumber(1), System.Text.Json.JsonValueKind.Number, (node) => (int)node.GetValue<double>(), 1);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForNumber(123.456), System.Text.Json.JsonValueKind.Number, (node) => node.GetValue<double>(), 123.456);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForBool(false), System.Text.Json.JsonValueKind.False, (node) => node.GetValue<bool>(), false);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForBool(true), System.Text.Json.JsonValueKind.True, (node) => node.GetValue<bool>(), true);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForNull(), System.Text.Json.JsonValueKind.Null, (node) => null, null);
            yield return () => (Google.Protobuf.WellKnownTypes.Value.ForString("value"), System.Text.Json.JsonValueKind.String, (node) => node.GetValue<string>(), "value");
            yield return () => (
                Google.Protobuf.WellKnownTypes.Value.ForList(
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(1),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(2),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(3),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(4),
                    Google.Protobuf.WellKnownTypes.Value.ForNumber(5)),
                System.Text.Json.JsonValueKind.Array,
                node => node.AsArray().Select(i => i!.GetValue<double>()),
                doubleArray);
            yield return () => (
                Google.Protobuf.WellKnownTypes.Value.ForStruct(
                    new Google.Protobuf.WellKnownTypes.Struct
                    {
                        Fields = { { "Id", Google.Protobuf.WellKnownTypes.Value.ForNumber(1) } }
                    }),
                System.Text.Json.JsonValueKind.Object,
                node => node.AsObject().ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = false }),
                """{"Id":1}""");
        }
    }



    static async Task IsEquivalentTo(System.Collections.IEnumerable actual, object expected)
    {
        // get the type
        var type = GetGenericIEnumerables(actual);
        var innerType = GetGenericIEnumerables(expected);

        var method = await Assert.That(typeof(ValueConverterTests).GetMethod(nameof(AssertIsEquivalentTo), BindingFlags.Static | BindingFlags.NonPublic)).IsNotNull();
        var genericMethod = method!.MakeGenericMethod(type, innerType);

        if (genericMethod.Invoke(null, [actual, expected]) is Task task)
        {
            await task;
        }

        static Type GetGenericIEnumerables(object o)
        {
            return o
                .GetType()
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .Single();
        }
    }

    private static async Task AssertIsEquivalentTo<T, TInner>(IEnumerable<T> actual, IEnumerable<TInner> expected)
    {
        await Assert.That(actual).IsEquivalentTo(expected);
    }
}