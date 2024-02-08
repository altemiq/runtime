// -----------------------------------------------------------------------
// <copyright file="OneOfTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class OneOfTests
{
    [Fact]
    public void DefaultConstructorSetsValueToDefaultValueOfT0() => new OneOf<int, bool>().Match(n => n == default, n => false).Should().BeTrue();

    [Fact]
    public void DefaultSetsValueToDefaultValueOfT0() => default(OneOf<int, bool>).Match(n => n == default, n => false).Should().BeTrue();

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
                .MapT0(d => d.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .MapT1(i => i.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .Match(t1 => t1, t2 => t2, t3 => t3);
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
    [InlineData("en-NZ", "System.DateTime: 2/01/2019 1:02:03 am")]
    [InlineData("en-US", "System.DateTime: 1/2/2019 1:02:03 AM")]
    public void LeftSideFormatsWithCurrentCulture(string cultureName, string expectedResult) => RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<DateTime, string>(new DateTime(2019, 1, 2, 1, 2, 3)).ToString).Should().Be(expectedResult);

    [Theory]
    [InlineData("en-NZ", "System.DateTime: 2/01/2019 1:02:03 am")]
    [InlineData("en-US", "System.DateTime: 1/2/2019 1:02:03 AM")]
    public void RightSideFormatsWithCurrentCulture(string cultureName, string expectedResult) => RunInCulture(new System.Globalization.CultureInfo(cultureName, false), OneOf.From<string, DateTime>(new DateTime(2019, 1, 2, 1, 2, 3)).ToString).Should().Be(expectedResult);

    [Theory]
    [InlineData("en-NZ", "System.DateTime: 2/01/2019 1:02:03 am")]
    [InlineData("en-US", "System.DateTime: 1/2/2019 1:02:03 AM")]
    public void SpecifyCulture(string cultureName, string expectedResult) => OneOf.From<string, DateTime>(new DateTime(2019, 1, 2, 1, 2, 3)).ToString(new System.Globalization.CultureInfo(cultureName, false)).Should().Be(expectedResult);

    private static string? RunInCulture(System.Globalization.CultureInfo culture, Func<string?> action)
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