namespace Altemiq.Protobuf.WellKnownTypes;

public class SemanticVersionTests
{
    [Test]
    [MethodDataSource(nameof(ParsingTestCases))]
    public async Task Parse(string input, int major, int minor, int patch, IEnumerable<string>? releaseLabels, string? metadata)
    {
        var version = SemanticVersion.Parse(input);
        await Assert.That(version)
            .Satisfies(x => x.Major, m => m.IsEqualTo(major)).And
            .Satisfies(x => x.Minor, m => m.IsEqualTo(minor)).And
            .Satisfies(x => x.Patch, m => m.IsEqualTo(patch)).And
            .Satisfies(IEnumerable<string> (x) => x.ReleaseLabels, m => releaseLabels is null ? m.IsEmpty() : m.IsEquivalentTo(releaseLabels)).And
            .Satisfies(x => x.HasMetadata, m => m.IsEqualTo(metadata is not null))
            .Satisfies(x => x.Metadata, m => metadata is null ? m.IsEmpty() : m.IsEqualTo(metadata));
    }

    [Test]
    [MethodDataSource(nameof(ParsingTestCases))]
    public async Task TryParse(string input, int major, int minor, int patch, IEnumerable<string>? releaseLabels, string? metadata)
    {
        await Assert.That(SemanticVersion.TryParse(input, out var version)).IsTrue();
        await Assert.That(version)
            .IsNotNull()
            .Satisfies(x => x.Major, m => m.IsEqualTo(major)).And
            .Satisfies(x => x.Minor, m => m.IsEqualTo(minor)).And
            .Satisfies(x => x.Patch, m => m.IsEqualTo(patch)).And
            .Satisfies(IEnumerable<string> (x) => x.ReleaseLabels, m => releaseLabels is null ? m.IsEmpty() : m.IsEquivalentTo(releaseLabels)).And
            .Satisfies(x => x.HasMetadata, m => m.IsEqualTo(metadata is not null))
            .Satisfies(x => x.Metadata, m => metadata is null ? m.IsEmpty() : m.IsEqualTo(metadata));
    }

    [Test]
    [MethodDataSource(nameof(ParsingTestCases))]
    public async Task SpanParse(string input, int major, int minor, int patch, IEnumerable<string>? releaseLabels, string? metadata)
    {
        var version = SemanticVersion.Parse(input.AsSpan());
        await Assert.That(version)
            .Satisfies(x => x.Major, m => m.IsEqualTo(major)).And
            .Satisfies(x => x.Minor, m => m.IsEqualTo(minor)).And
            .Satisfies(x => x.Patch, m => m.IsEqualTo(patch)).And
            .Satisfies(IEnumerable<string> (x) => x.ReleaseLabels, m => releaseLabels is null ? m.IsEmpty() : m.IsEquivalentTo(releaseLabels)).And
            .Satisfies(x => x.HasMetadata, m => m.IsEqualTo(metadata is not null))
            .Satisfies(x => x.Metadata, m => metadata is null ? m.IsEmpty() : m.IsEqualTo(metadata));
    }

    [Test]
    [MethodDataSource(nameof(ParsingTestCases))]
    public async Task TrySpanParse(string input, int major, int minor, int patch, IEnumerable<string>? releaseLabels, string? metadata)
    {
        await Assert.That(SemanticVersion.TryParse(input.AsSpan(), out var version)).IsTrue();
        await Assert.That(version)
            .IsNotNull()
            .Satisfies(x => x.Major, m => m.IsEqualTo(major)).And
            .Satisfies(x => x.Minor, m => m.IsEqualTo(minor)).And
            .Satisfies(x => x.Patch, m => m.IsEqualTo(patch)).And
            .Satisfies(IEnumerable<string> (x) => x.ReleaseLabels, m => releaseLabels is null ? m.IsEmpty() : m.IsEquivalentTo(releaseLabels)).And
            .Satisfies(x => x.HasMetadata, m => m.IsEqualTo(metadata is not null))
            .Satisfies(x => x.Metadata, m => metadata is null ? m.IsEmpty() : m.IsEqualTo(metadata));
    }

    [Test]
    [MethodDataSource(nameof(ParsingTestCases))]
    public async Task ToFullString(string input, int major, int minor, int patch, IEnumerable<string>? releaseLabels,
        string? metadata)
    {
        var version = new SemanticVersion(new System.Version(major, minor, patch), releaseLabels, metadata);
        await Assert.That(version.ToFullString()).IsEqualTo(input);
    }

#if NET7_0_OR_GREATER
    [Test]
    [MethodDataSource(nameof(ParsingTestCases))]
    public async Task TryFormat(string input, int major, int minor, int patch, IEnumerable<string>? releaseLabels, string? metadata)
    {
        var version = new SemanticVersion(new System.Version(major, minor, patch), releaseLabels, metadata);
        int charsWritten = -1;
        string? output = default;
        await Assert.That(() =>
        {
            Span<char> inputSpan = stackalloc char[input.Length * 2];
            var result = ((ISpanFormattable)version).TryFormat(inputSpan, out charsWritten, "F", default);
            output = inputSpan[..charsWritten].ToString();
            return result;
        }).IsTrue();
        await Assert.That(charsWritten).IsEqualTo(input.Length);
        await Assert.That(output).IsEqualTo(input);
    }
#endif

    [Test]
    [MethodDataSource(nameof(ToStringTestCases))]
    public async Task ToString(int major, int minor, int patch, IEnumerable<string>? releaseLabels, string? metadata, string? expected)
    {
        var version = SemanticVersion.ForVersion(new System.Version(major, minor, patch), releaseLabels, metadata);
        await Assert.That(version.ToString()).IsEqualTo(expected);
    }

    public static IEnumerable<Func<(int, int, int, IEnumerable<string>?, string?, string)>> ToStringTestCases()
    {
        yield return () => (1, 2, 3, null, null, """{ "major": 1, "minor": 2, "patch": 3 }""");
        yield return () => (1, 2, 3, ["first", "second"], null, """{ "major": 1, "minor": 2, "patch": 3, "releaseLabels": [ "first", "second" ] }""");
        yield return () => (1, 2, 3, ["first", "second"], "third", """{ "major": 1, "minor": 2, "patch": 3, "releaseLabels": [ "first", "second" ], "metadata": "third" }""");
        yield return () => (1, 2, 3, null, "third", """{ "major": 1, "minor": 2, "patch": 3, "metadata": "third" }""");
    }

    public static IEnumerable<Func<(string, int, int, int, IEnumerable<string>?, string?)>> ParsingTestCases()
    {
        // version numbers given with the link in the spec to a regex for semver versions
        yield return () => ("0.0.4", 0, 0, 4, null, null);
        yield return () => ("1.2.3", 1, 2, 3, null, null);
        yield return () => ("10.20.30", 10, 20, 30, null, null);
        yield return () => ("1.1.2-prerelease+meta", 1, 1, 2, Pre("prerelease"), Meta("meta"));
        yield return () => ("1.1.2+meta", 1, 1, 2, Pre(), Meta("meta"));
        yield return () => ("1.1.2+meta-valid", 1, 1, 2, Pre(), Meta("meta-valid"));
        yield return () => ("1.0.0-alpha", 1, 0, 0, Pre("alpha"), null);
        yield return () => ("1.0.0-beta", 1, 0, 0, Pre("beta"), null);
        yield return () => ("1.0.0-alpha.beta", 1, 0, 0, Pre("alpha", "beta"), null);
        yield return () => ("1.0.0-alpha.beta.1", 1, 0, 0, Pre("alpha", "beta", "1"), null);
        yield return () => ("1.0.0-alpha.1", 1, 0, 0, Pre("alpha", 1), null);
        yield return () => ("1.0.0-alpha0.valid", 1, 0, 0, Pre("alpha0", "valid"), null);
        yield return () => ("1.0.0-alpha.0valid", 1, 0, 0, Pre("alpha", "0valid"), null);
        yield return () => ("1.0.0-alpha-a.b-c-somethinglong+build.1-aef.1-its-okay", 1, 0, 0, Pre("alpha-a", "b-c-somethinglong"), Meta("build", "1-aef", "1-its-okay"));
        yield return () => ("1.0.0-rc.1+build.1", 1, 0, 0, Pre("rc", 1), Meta("build", "1"));
        yield return () => ("2.0.0-rc.1+build.123", 2, 0, 0, Pre("rc", 1), Meta("build", "123"));
        yield return () => ("1.2.3-beta", 1, 2, 3, Pre("beta"), null);
        yield return () => ("10.2.3-DEV-SNAPSHOT", 10, 2, 3, Pre("DEV-SNAPSHOT"), null);
        yield return () => ("1.2.3-SNAPSHOT-123", 1, 2, 3, Pre("SNAPSHOT-123"), null);
        yield return () => ("1.0.0", 1, 0, 0, null, null);
        yield return () => ("2.0.0", 2, 0, 0, null, null);
        yield return () => ("1.1.7", 1, 1, 7, null, null);
        yield return () => ("2.0.0+build.1848", 2, 0, 0, Pre(), Meta("build", "1848"));
        yield return () => ("2.0.1-alpha.1227", 2, 0, 1, Pre("alpha", 1227), null);
        yield return () => ("1.0.0-alpha+beta", 1, 0, 0, Pre("alpha"), Meta("beta"));
        yield return () => ("1.2.3----RC-SNAPSHOT.12.9.1--.12+788", 1, 2, 3, Pre("---RC-SNAPSHOT", 12, 9, "1--", 12), Meta("788"));
        yield return () => ("1.2.3----R-S.12.9.1--.12+meta", 1, 2, 3, Pre("---R-S", 12, 9, "1--", 12), Meta("meta"));
        yield return () => ("1.2.3----RC-SNAPSHOT.12.9.1--.12", 1, 2, 3, Pre("---RC-SNAPSHOT", 12, 9, "1--", 12), null);
        yield return () => ("1.0.0+0.build.1-rc.10000aaa-kk-0.1", 1, 0, 0, Pre(), Meta("0", "build", "1-rc", "10000aaa-kk-0", "1"));
        yield return () => ("1.0.0-0A.is.legal", 1, 0, 0, Pre("0A", "is", "legal"), null);

        // Basic valid versions
        yield return () => ("1.2.3-a+b", 1, 2, 3, Pre("a"), Meta("b"));
        yield return () => ("1.2.3-a", 1, 2, 3, Pre("a"), null);
        yield return () => ("1.2.3+b", 1, 2, 3, Pre(), Meta("b"));
        yield return () => ("4.5.6", 4, 5, 6, null, null);

        // Valid letter Limits
        yield return () => ("1.2.3-A-Z.a-z.0-9+A-Z.a-z.0-9", 1, 2, 3, Pre("A-Z", "a-z", "0-9"), Meta("A-Z", "a-z", "0-9"));

        // Misc valid
        yield return () => ("1.2.45-alpha-beta+nightly.23.43-bla", 1, 2, 45, Pre("alpha-beta"), Meta("nightly", "23", "43-bla"));
        yield return () => ("1.2.45-alpha+nightly.23", 1, 2, 45, Pre("alpha"), Meta("nightly", "23"));
        yield return () => ("3.2.1-beta", 3, 2, 1, Pre("beta"), null);
        yield return () => ("2.0.0+nightly.23.43-bla", 2, 0, 0, Pre(), Meta("nightly", "23", "43-bla"));
        yield return () => ("5.6.7", 5, 6, 7, null, null);

        // Valid unusual versions
        yield return () => ("1.0.0--ci.1", 1, 0, 0, Pre("-ci", 1), null);
        yield return () => ("1.0.0-0A", 1, 0, 0, Pre("0A"), null);

        // Hyphen in strange place
        yield return () => ("1.2.3--+b", 1, 2, 3, Pre("-"), Meta("b"));
        yield return () => ("1.2.3---+b", 1, 2, 3, Pre("--"), Meta("b"));
        yield return () => ("1.2.3---", 1, 2, 3, Pre("--"), null);
        yield return () => ("1.2.3-a+-", 1, 2, 3, Pre("a"), Meta("-"));
        yield return () => ("1.2.3-a+--", 1, 2, 3, Pre("a"), Meta("--"));
        yield return () => ("1.2.3--a+b", 1, 2, 3, Pre("-a"), Meta("b"));
        yield return () => ("1.2.3--1+b", 1, 2, 3, Pre("-1"), Meta("b"));
        yield return () => ("1.2.3---a+b", 1, 2, 3, Pre("--a"), Meta("b"));
        yield return () => ("1.2.3---1+b", 1, 2, 3, Pre("--1"), Meta("b"));
        yield return () => ("1.2.3-a+-b", 1, 2, 3, Pre("a"), Meta("-b"));
        yield return () => ("1.2.3-a+--b", 1, 2, 3, Pre("a"), Meta("--b"));
        yield return () => ("1.2.3-a-+b", 1, 2, 3, Pre("a-"), Meta("b"));
        yield return () => ("1.2.3-1-+b", 1, 2, 3, Pre("1-"), Meta("b"));
        yield return () => ("1.2.3-a--+b", 1, 2, 3, Pre("a--"), Meta("b"));
        yield return () => ("1.2.3-1--+b", 1, 2, 3, Pre("1--"), Meta("b"));
        yield return () => ("1.2.3-a+b-", 1, 2, 3, Pre("a"), Meta("b-"));
        yield return () => ("1.2.3-a+b--", 1, 2, 3, Pre("a"), Meta("b--"));
        yield return () => ("1.2.3--.a+b", 1, 2, 3, Pre("-", "a"), Meta("b"));
        yield return () => ("1.2.3-a+-.b", 1, 2, 3, Pre("a"), Meta("-", "b"));
        yield return () => ("1.2.3-a.-+b", 1, 2, 3, Pre("a", "-"), Meta("b"));
        yield return () => ("1.2.3-a.-.c+b", 1, 2, 3, Pre("a", "-", "c"), Meta("b"));
        yield return () => ("1.2.3-a+b.-", 1, 2, 3, Pre("a"), Meta("b", "-"));
        yield return () => ("1.2.3-a+b.-.c", 1, 2, 3, Pre("a"), Meta("b", "-", "c"));

        // Leading zeros in alphanumeric prerelease identifiers
        yield return () => ("1.2.3-0a", 1, 2, 3, Pre("0a"), null);
        yield return () => ("1.2.3-00000a", 1, 2, 3, Pre("00000a"), null);
        yield return () => ("1.2.3-a.0c", 1, 2, 3, Pre("a", "0c"), null);
        yield return () => ("1.2.3-a.00000c", 1, 2, 3, Pre("a", "00000c"), null);

        // Simple zero in prerelease identifiers (leading zero check is correct)
        yield return () => ("1.2.3-0", 1, 2, 3, Pre(0), null);
        yield return () => ("1.2.3-pre.0", 1, 2, 3, Pre("pre", 0), null);
        yield return () => ("1.2.3-pre.0.42", 1, 2, 3, Pre("pre", 0, 42), null);


        static IEnumerable<string> Pre(params IEnumerable<object> input)
        {
            return input.Select(x => x.ToString()!);
        }

        static string Meta(params IEnumerable<object> input)
        {
            return string.Join(".", input.Select(x => x.ToString()!));
        }
    }
}