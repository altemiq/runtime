// -----------------------------------------------------------------------
// <copyright file="OneOfTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class OneOfTests
{
    [Test]
    public async Task DefaultConstructorSetsValueToDefaultValueOfT0() => await Assert.That(new OneOf<int, bool>().Match(static n => n == default, static n => false)).IsTrue();

    [Test]
    public async Task DefaultSetsValueToDefaultValueOfT0() => await Assert.That(default(OneOf<int, bool>).Match(static n => n == default, static n => false)).IsTrue();

    [Test]
    public async Task AreEqual()
    {
        var a = OneOf.From(1);
        var b = a;
        await Assert.That(a).IsEqualTo(b);
    }

    [Test]
    public async Task ResolveIFooFromResultMethod() => await Assert.That(OneOf.From<IFoo, int>(new Foo()).AsT0).IsTypeOf<Foo>();

    [Test]
    public async Task MapValue()
    {
        await Assert.That(ResolveString(2.1)).IsEqualTo("2.1");
        await Assert.That(ResolveString(4)).IsEqualTo("4");
        await Assert.That(ResolveString("6")).IsEqualTo("6");

        static string? ResolveString(OneOf<double, int, string> input)
        {
            return input
                .MapT0(static d => d.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .MapT1(static i => i.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .Match(static t1 => t1, static t2 => t2, static t3 => t3);
        }
    }

    private static readonly System.Text.Json.JsonSerializerOptions options = new() { Converters = { new OneOfJsonConverter() } };

    [Test]
    public async Task CanSerializeOneOfValueTransparently() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(new SomeThing { Value = "A string value" }, options)).IsEqualTo("{\"Value\":\"A string value\"}");

    [Test]
    public async Task TheValueAndTypeNameAreFormattedCorrectly() => await Assert.That(OneOf.From<string, int, DateTime, decimal>(42).ToString()).IsEqualTo("System.Int32: 42");

    [Test]
    public async Task CallingToStringOnANestedNonRecursiveTypeWorks() => await Assert.That(OneOf.From<OneOf<string, bool>, OneOf<bool, string>>(OneOf.From<string, bool>(true)).ToString()).IsEqualTo($"{nameof(Altemiq)}.{nameof(OneOf)}`2[[System.String, {typeof(string).Assembly.FullName}],[System.Boolean, {typeof(bool).Assembly.FullName}]]: System.Boolean: True");

    [Test]
    [MethodDataSource(nameof(DateData))]
    public async Task LeftSideFormatsWithCurrentCulture(string cultureName, DateTime dateTime, string expectedResult) => await Assert.That( RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<DateTime, string>(dateTime).ToString)).IsEqualTo(expectedResult);

    [Test]
    [MethodDataSource(nameof(DateData))]
    public async Task RightSideFormatsWithCurrentCulture(string cultureName, DateTime dateTime, string expectedResult) => await Assert.That( RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<string, DateTime>(dateTime).ToString)).IsEqualTo(expectedResult);

    [Test]
    [MethodDataSource(nameof(DateData))]
    public async Task SpecifyCulture(string cultureName, DateTime dateTime, string expectedResult) => await Assert.That( OneOf.From<string, DateTime>(dateTime).ToString(new System.Globalization.CultureInfo(cultureName, false))).IsEqualTo(expectedResult);

    public static IEnumerable<Func<(string, DateTime, string)>> DateData()
    {
        yield return () => Create(new System.Globalization.CultureInfo("en-NZ"), new DateTime(2019, 1, 2, 1, 2, 3));
        yield return () => Create(new System.Globalization.CultureInfo("en-US"), new DateTime(2019, 1, 2, 1, 2, 3));

        static (string, DateTime, string) Create(System.Globalization.CultureInfo culture, DateTime dateTime)
        {
            return (culture.Name, dateTime, $"System.DateTime: {DateTime(dateTime, culture)}");

            // get date time
            static string DateTime(DateTime dateTime, IFormatProvider provider)
            {
                return dateTime.ToString(provider);
            }
        }
    }

    private static T RunInCulture<T>(System.Globalization.CultureInfo culture, Func<T> action)
    {
        var originalCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = culture;
        try
        {
            return action();
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }

    private class SomeThing
    {
        public OneOf<string, SomeOtherThing> Value { get; set; }
    }

    private class SomeOtherThing
    {
        public int Value { get; set; }
    }

    private class Foo : IFoo
    {
    }

    private interface IFoo
    {
    }

    private class OneOfJsonConverter : System.Text.Json.Serialization.JsonConverter<IOneOf>
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.GetInterfaces().Contains(typeof(IOneOf)) || base.CanConvert(typeToConvert);

        public override void Write(System.Text.Json.Utf8JsonWriter writer, IOneOf value, System.Text.Json.JsonSerializerOptions options) => System.Text.Json.JsonSerializer.Serialize(writer, value.Value, options);

        public override IOneOf Read(ref System.Text.Json.Utf8JsonReader reader, Type objectType, System.Text.Json.JsonSerializerOptions options) => throw new NotImplementedException();
    }
}