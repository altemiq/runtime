// -----------------------------------------------------------------------
// <copyright file="OneOfTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class OneOfTests
{
    [Fact]
    public void DefaultConstructorSetsValueToDefaultValueOfT0() => Assert.True(new OneOf<int, bool>().Match(static n => n == default, static n => false));

    [Fact]
    public void DefaultSetsValueToDefaultValueOfT0() => Assert.True(default(OneOf<int, bool>).Match(static n => n == default, static n => false));

    [Fact]
    public void AreEqual()
    {
        var a = OneOf.From(1);
        var b = a;
        Array.Equals(a, b);
    }

    [Fact]
    public void ResolveIFooFromResultMethod() => Assert.IsType<Foo>(OneOf.From<IFoo, int>(new Foo()).AsT0);

    [Fact]
    public void MapValue()
    {
        Assert.Equal("2.1", ResolveString(2.1));
        Assert.Equal("4", ResolveString(4));
        Assert.Equal("6", ResolveString("6"));

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
    public void CanSerializeOneOfValueTransparently() => Assert.Equal("{\"Value\":\"A string value\"}", System.Text.Json.JsonSerializer.Serialize(new SomeThing { Value = "A string value" }, options));

    [Fact]
    public void TheValueAndTypeNameAreFormattedCorrectly() => Assert.Equal("System.Int32: 42", OneOf.From<string, int, DateTime, decimal>(42).ToString());

    [Fact]
    public void CallingToStringOnANestedNonRecursiveTypeWorks() => Assert.Equal($"{nameof(Altemiq)}.{nameof(OneOf)}`2[[System.String, {typeof(string).Assembly.FullName}],[System.Boolean, {typeof(bool).Assembly.FullName}]]: System.Boolean: True", OneOf.From<OneOf<string, bool>, OneOf<bool, string>>(OneOf.From<string, bool>(true)).ToString());

    [Theory]
    [MemberData(nameof(DateData))]
    public void LeftSideFormatsWithCurrentCulture(string cultureName, DateTime dateTime, string expectedResult) => Assert.Equal(expectedResult, RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<DateTime, string>(dateTime).ToString));

    [Theory]
    [MemberData(nameof(DateData))]
    public void RightSideFormatsWithCurrentCulture(string cultureName, DateTime dateTime, string expectedResult) => Assert.Equal(expectedResult, RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<string, DateTime>(dateTime).ToString));

    [Theory]
    [MemberData(nameof(DateData))]
    public void SpecifyCulture(string cultureName, DateTime dateTime, string expectedResult) => Assert.Equal(expectedResult, OneOf.From<string, DateTime>(dateTime).ToString(new System.Globalization.CultureInfo(cultureName, false)));

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