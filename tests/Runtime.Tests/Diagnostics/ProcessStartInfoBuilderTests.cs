namespace Altemiq.Diagnostics;

using System.Diagnostics;

public class ProcessStartInfoBuilderTests
{
    private const string MyStringValue = "value";
    private const string FileName = "notepad.exe";
    private static readonly string Arguments = $"--{nameof(MyProcessStartInfoBuilder.MySwitch).ToKebabCase()} --{nameof(MyProcessStartInfoBuilder.MyString).ToKebabCase()} {MyStringValue}";

    [Test]
    public async Task SetFromProcessStartInfo()
    {
        var builder = new MyProcessStartInfoBuilder(new(FileName, Arguments));
        _ = await Assert.That(ProcessStartInfoBuilder.GetArguments(builder.ProcessStartInfo)).IsEqualTo(Arguments);
    }

    [Test]
    public async Task SetFromProperties()
    {
        var builder = new MyProcessStartInfoBuilder
        {
            MySwitch = true,
            MyString = MyStringValue,
        };

        _ = await Assert.That(ProcessStartInfoBuilder.GetArguments(builder.ProcessStartInfo)).IsEqualTo(Arguments);
    }

    private sealed class MyProcessStartInfoBuilder : ProcessStartInfoBuilder
    {
        public MyProcessStartInfoBuilder()
            : base(FileName)
        {

        }


        public MyProcessStartInfoBuilder(ProcessStartInfo processStartInfo)
            : base(processStartInfo)
        {
            var mySwitchOption = new System.CommandLine.Option<bool>($"--{nameof(MySwitch).ToKebabCase()}");
            var myStringOption = new System.CommandLine.Option<string>($"--{nameof(MyString).ToKebabCase()}");
            var mySecondStringOption = new System.CommandLine.Option<string>($"--{nameof(MySecondString).ToKebabCase()}");
            var rootCommand = new System.CommandLine.RootCommand { mySwitchOption, myStringOption, mySecondStringOption };
            var parseResult = rootCommand.Parse(GetArgumentList(processStartInfo));

            MySwitch = parseResult.GetValue(mySwitchOption);
            MyString = parseResult.GetValue(myStringOption);
            MySecondString = parseResult.GetValue(mySecondStringOption);
        }

        public bool MySwitch { get; init; }

        public string? MyString { get; init; }

        public string? MySecondString { get; set; }

        protected override IEnumerable<string> GetArguments()
        {
            foreach (var argument in base.GetArguments())
            {
                yield return argument;
            }

            if (MySwitch)
            {
                yield return $"--{nameof(MySwitch).ToKebabCase()}";
            }

            if (MyString is not null)
            {
                yield return $"--{nameof(MyString).ToKebabCase()}";
                yield return MyString;
            }

            if (MySecondString is not null)
            {
                yield return $"--{nameof(MySecondString).ToKebabCase()}";
                yield return MySecondString;
            }
        }
    }
}