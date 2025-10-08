using System.Diagnostics;

namespace Altemiq.Diagnostics;

public class ProcessStartInfoBuilderTests
{
    private const string MyStringValue = "value";
    private const string FileName = "notepad.exe";
    private static readonly string Arguments = $"--{nameof(MyProcessStartInfoBuilder.MySwitch).ToKebabCase()} --{nameof(MyProcessStartInfoBuilder.MyString).ToKebabCase()} {MyStringValue}";
    
    [Test]
    public async Task SetFromProcessStartInfo()
    {
        var builder = new MyProcessStartInfoBuilder(new(FileName, Arguments));
        await Assert.That(ProcessStartInfoBuilder.GetArguments(builder.ProcessStartInfo)).IsEqualTo(Arguments);
    }
    
    [Test]
    public async Task SetFromProperties()
    {
        var builder = new MyProcessStartInfoBuilder()
        {
            MySwitch = true,
            MyString = MyStringValue,
        };
        
        await Assert.That(ProcessStartInfoBuilder.GetArguments(builder.ProcessStartInfo)).IsEqualTo(Arguments);
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
            var mySwitchOption = new System.CommandLine.Option<bool>($"--{nameof(this.MySwitch).ToKebabCase()}");
            var myStringOption = new System.CommandLine.Option<string>($"--{nameof(this.MyString).ToKebabCase()}");
            var mySecondStringOption = new System.CommandLine.Option<string>($"--{nameof(this.MySecondString).ToKebabCase()}");
            var rootCommand = new System.CommandLine.RootCommand { mySwitchOption, myStringOption, mySecondStringOption };
            var parseResult = rootCommand.Parse(GetArgumentList(processStartInfo));

            this.MySwitch = parseResult.GetValue(mySwitchOption);
            this.MyString = parseResult.GetValue(myStringOption);
            this.MySecondString = parseResult.GetValue(mySecondStringOption);
        }
        
        public bool MySwitch { get; set; }
        
        public string? MyString { get; set; }
        
        public string? MySecondString { get; set; }

        protected override IEnumerable<string> GetArguments()
        {
            foreach (var argument in base.GetArguments())
            {
                yield return argument;
            }

            if (this.MySwitch)
            {
                yield return $"--{nameof(this.MySwitch).ToKebabCase()}";
            }

            if (this.MyString is not null)
            {
                yield return $"--{nameof(this.MyString).ToKebabCase()}";
                yield return this.MyString;
            }
             
            if (this.MySecondString is not null)
            {
                yield return $"--{nameof(this.MySecondString).ToKebabCase()}";
                yield return this.MySecondString;
            }
        }
    }
}