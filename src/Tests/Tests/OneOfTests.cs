// -----------------------------------------------------------------------
// <copyright file="OneOfTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class OneOfTests
{
    [Fact]
    public void DefaultConstructorSetsValueToDefaultValueOfT0() => new OneOf<int, bool>().Match(static n => n == default, static n => false).Should().BeTrue();

    [Fact]
    public void DefaultSetsValueToDefaultValueOfT0() => default(OneOf<int, bool>).Match(static n => n == default, static n => false).Should().BeTrue();

    [Fact]
    public void AreEqual()
    {
        var a = OneOf.From(1);
        var b = a;
        _ = a.Should().Be(b);
    }

    [Fact]
    public void ResolveIFooFromResultMethod() => OneOf.From<IFoo, int>(new Foo()).AsT0.Should().BeOfType<Foo>();

    [Fact]
    public void MapValue()
    {
        _ = ResolveString(2.1).Should().Be("2.1");
        _ = ResolveString(4).Should().Be("4");
        _ = ResolveString("6").Should().Be("6");
        static string? ResolveString(OneOf<double, int, string> input)
        {
            return input
                .MapT0(static d => d.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .MapT1(static i => i.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .Match(static t1 => t1, static t2 => t2, static t3 => t3);
        }
    }

    private static readonly System.Text.Json.JsonSerializerOptions options = new() { Converters = { new OneOfJsonConverter() } };

    [Fact]
    public void CanSerializeOneOfValueTransparently() => System.Text.Json.JsonSerializer.Serialize(new SomeThing { Value = "A string value" }, options).Should().Be("{\"Value\":\"A string value\"}");

    [Fact]
    public void TheValueAndTypeNameAreFormattedCorrectly() => OneOf.From<string, int, DateTime, decimal>(42).ToString().Should().Be("System.Int32: 42");

    [Fact]
    public void CallingToStringOnANestedNonRecursiveTypeWorks() => OneOf.From<OneOf<string, bool>, OneOf<bool, string>>(OneOf.From<string, bool>(true)).ToString().Should().Be($"{nameof(Altemiq)}.{nameof(OneOf)}`2[[System.String, {typeof(string).Assembly.FullName}],[System.Boolean, {typeof(bool).Assembly.FullName}]]: System.Boolean: True");

    [Theory]
    [MemberData(nameof(DateData))]
    public void LeftSideFormatsWithCurrentCulture(string cultureName, DateTime dateTime, string expectedResult) => RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<DateTime, string>(dateTime).ToString).Should().Be(expectedResult);

    [Theory]
    [MemberData(nameof(DateData))]
    public void RightSideFormatsWithCurrentCulture(string cultureName, DateTime dateTime, string expectedResult) => RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<string, DateTime>(dateTime).ToString).Should().Be(expectedResult);

    [Theory]
    [MemberData(nameof(DateData))]
    public void SpecifyCulture(string cultureName, DateTime dateTime, string expectedResult) => OneOf.From<string, DateTime>(dateTime).ToString(new System.Globalization.CultureInfo(cultureName, false)).Should().Be(expectedResult);

    public static TheoryData<string, DateTime, string> DateData()
    {
        var theoryData = new TheoryData<string, DateTime, string>();

        Add(theoryData, new System.Globalization.CultureInfo("en-NZ"), new DateTime(2019, 1, 2, 1, 2, 3));
        Add(theoryData, new System.Globalization.CultureInfo("en-US"), new DateTime(2019, 1, 2, 1, 2, 3));

        return theoryData;

        static void Add(TheoryData<string, DateTime, string> data, System.Globalization.CultureInfo culture, DateTime dateTime)
        {
            data.Add(culture.Name, dateTime, $"System.DateTime: {DateTime(dateTime, culture)}");

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