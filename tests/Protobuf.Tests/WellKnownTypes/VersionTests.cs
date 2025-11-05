namespace Altemiq.Protobuf.WellKnownTypes;

public class VersionTests
{
    [Test]
    [Arguments("1.2", 1, 2, -1, -1)]
    [Arguments("1.2.3", 1, 2, 3, -1)]
    [Arguments("1.2.3.4", 1, 2, 3, 4)]
    public async Task Parse(string input, int major, int minor, int build, int revision)
    {
        await Assert.That(Version.Parse(input))
            .Member(x => x.Major, m => m.IsEqualTo(major)).And
            .Member(x => x.Minor, m => m.IsEqualTo(minor)).And
            .Member(x => x.HasBuild, m => m.IsEqualTo(build >= 0)).And
            .Member(x => x.Build, m => build >= 0 ? m.IsEqualTo(build) : m.IsEqualTo(0)).And
            .Member(x => x.HasRevision, m => m.IsEqualTo(revision >= 0)).And
            .Member(x => x.Revision, m => revision >= 0 ? m.IsEqualTo(revision) : m.IsEqualTo(0));
    }

    [Test]
    [Arguments("1.2", 1, 2, -1, -1)]
    [Arguments("1.2.3", 1, 2, 3, -1)]
    [Arguments("1.2.3.4", 1, 2, 3, 4)]
    public async Task TryParse(string input, int major, int minor, int build, int revision)
    {
        await Assert.That(Version.TryParse(input, out var output)).IsTrue();
        await Assert.That(output)
            .IsNotNull().And
            .Member(x => x.Major, m => m.IsEqualTo(major)).And
            .Member(x => x.Minor, m => m.IsEqualTo(minor)).And
            .Member(x => x.HasBuild, m => m.IsEqualTo(build >= 0)).And
            .Member(x => x.Build, m => build >= 0 ? m.IsEqualTo(build) : m.IsEqualTo(0)).And
            .Member(x => x.HasRevision, m => m.IsEqualTo(revision >= 0)).And
            .Member(x => x.Revision, m => revision >= 0 ? m.IsEqualTo(revision) : m.IsEqualTo(0));
    }

    [Test]
    [Arguments("1.2", 1, 2, -1, -1)]
    [Arguments("1.2.3", 1, 2, 3, -1)]
    [Arguments("1.2.3.4", 1, 2, 3, 4)]
    public async Task Format(string input, int major, int minor, int build, int revision)
    {
        var fieldCount = (build, revision) switch
        {
            ( < 0, _) => 2,
            (_, < 0) => 3,
            _ => 4,
        };
        await Assert.That(Version.ForVersion(
            fieldCount switch
            {
                4 => new(major, minor, build, revision),
                3 => new(major, minor, build),
                _ => new(major, minor),
            }).ToString(fieldCount)).IsEqualTo(input);
    }

#if NET7_0_OR_GREATER
    [Test]
    [Arguments("1.2", 1, 2, -1, -1)]
    [Arguments("1.2.3", 1, 2, 3, -1)]
    [Arguments("1.2.3.4", 1, 2, 3, 4)]
    public async Task FormatSpan(string input, int major, int minor, int build, int revision)
    {
        string? output = default;
        await Assert.That(() =>
        {
            Span<char> outputSpan = stackalloc char[input.Length];
            var response = ((ISpanFormattable)Version.ForVersion(
                (build, revision) switch
                {
                    (_, >= 0) => new(major, minor, build, revision),
                    ( >= 0, _) => new(major, minor, build),
                    _ => new(major, minor),
                })).TryFormat(outputSpan, out var charsWritten, default, default);
            output = outputSpan[..charsWritten].ToString();
            return response;
        }).IsTrue();
        await Assert.That(output).IsEqualTo(input);
    }
#endif

    [Test]
    [Arguments(1, 2, -1, -1, 1, 2, -1, -1, 0)]
    [Arguments(1, 2, -1, -1, 1, 2, 3, -1, -1)]
    [Arguments(1, 2, 3, -1, 1, 2, -1, -1, 1)]
    [Arguments(1, 2, 3, -1, 1, 2, 3, -1, 0)]
    [Arguments(1, 2, 3, 4, 1, 2, 3, 4, 0)]
    [Arguments(1, 2, 3, -1, 1, 2, 3, 4, -1)]
    [Arguments(1, 2, 3, 4, 1, 2, 3, -1, 1)]
    public async Task CompareTo(int leftMajor, int leftMinor, int leftBuild, int leftRevision, int rightMajor, int rightMinor, int rightBuild, int rightRevision, int expected)
    {
        await Assert.That(Create(leftMajor, leftMinor, leftBuild, leftRevision).CompareTo(Create(rightMajor, rightMinor, rightBuild, rightRevision))).IsEqualTo(expected);

        static Version Create(int major, int minor, int build, int revision)
        {
            var version = new Version { Major = major, Minor = minor };
            if (build >= 0)
            {
                version.Build = build;
            }

            if (revision >= 0)
            {
                version.Revision = revision;
            }

            return version;
        }
    }

}