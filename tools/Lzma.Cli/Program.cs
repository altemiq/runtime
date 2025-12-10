// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CommandLine;
using System.IO.Compression;

var inputArgument = new Argument<FileInfo>("INPUT").AcceptExistingOnly();
var outputArgument = new Argument<FileInfo>("OUTPUT");

var dictionaryOption = new Option<int>("-d")
{
    Description = "set dictionary - [0, 29]",
    DefaultValueFactory = _ => 21,
    Validators = { CreateNumberValidator(0, 29) },
};

var numFastBytesOption = new Option<int>("-fb")
{
    Description = "set number of fast bytes - [5, 273]",
    DefaultValueFactory = _ => 128,
    Validators = { CreateNumberValidator(5, 273) },
};

var litContextBitsOption = new Option<int>("-lc")
{
    Description = "set number of literal context bits - [0, 8]",
    DefaultValueFactory = _ => 3,
    Validators = { CreateNumberValidator(0, 8) },
};

var litPosBitsOptions = new Option<int>("-lp")
{
    Description = "set number of literal pos bits - [0, 4]",
    DefaultValueFactory = _ => 0,
    Validators = { CreateNumberValidator(0, 4) },
};

var posBitsOption = new Option<int>("-pb")
{
    Description = "set number of pos bits - [0, 4]",
    DefaultValueFactory = _ => 2,
    Validators = { CreateNumberValidator(0, 4) },
};

var possibleMatchFinders = Enum.GetValues<LzmaMatchFinder>().Select(l => l.ToStringFast(useMetadataAttributes: true)).ToArray();
var matchFinderOption = new Option<LzmaMatchFinder>("-mf")
{
    HelpName = "MF_ID",
    Description = $"set Match Finder: [{string.Join(", ", possibleMatchFinders)}]",
    DefaultValueFactory = _ => LzmaMatchFinder.BinaryTree4,
};
matchFinderOption.AcceptOnlyFromAmong(possibleMatchFinders);

var eosOption = new Option<bool>("-eos")
{
    Description = "write End Of Stream marker",
};

var encodeCommand = new Command("encode")
{
    inputArgument,
    outputArgument,
    dictionaryOption,
    numFastBytesOption,
    litContextBitsOption,
    litPosBitsOptions,
    posBitsOption,
    matchFinderOption,
    eosOption,
};

encodeCommand.Aliases.Add("e");
encodeCommand.SetAction(parseResult =>
{
    var input = parseResult.GetValue(inputArgument);
    var output = parseResult.GetValue(outputArgument);

    var dictionarySize = 1 << parseResult.GetValue(dictionaryOption);
    var positionStateBits = parseResult.GetValue(posBitsOption);
    var literalContextBits = parseResult.GetValue(litContextBitsOption);
    var literalPositionBits = parseResult.GetValue(litPosBitsOptions);
    const int Algorithm = 2;
    var fastBytes = parseResult.GetValue(numFastBytesOption);
    var matchFinder = parseResult.GetValue(matchFinderOption);
    var eos = parseResult.GetValue(eosOption);

    var properties = new Dictionary<CoderPropId, object>
    {
        { CoderPropId.DictionarySize, dictionarySize },
        { CoderPropId.PositionStateBits, positionStateBits },
        { CoderPropId.LiteralContextBits, literalContextBits },
        { CoderPropId.LiteralPositionBits, literalPositionBits },
        { CoderPropId.Algorithm, Algorithm },
        { CoderPropId.FastBytes, fastBytes },
        { CoderPropId.MatchFinder, matchFinder },
        { CoderPropId.EndMarker, eos },
    };

    LzmaEncoder encoder = new(properties);
    output?.Directory?.Create();
    using var outStream = output?.OpenWrite();
    if (outStream is null)
    {
        return;
    }

    encoder.WriteCoderProperties(outStream);

    using var inStream = input!.OpenRead();
    var fileSize = eos ? -1L : inStream.Length;
    for (var i = 0; i < 8; i++)
    {
        outStream.WriteByte((byte)(fileSize >> (8 * i)));
    }

    encoder.Compress(inStream, outStream, progress: null);
});

var decodeCommand = new Command("decode")
{
    inputArgument,
    outputArgument,
};

decodeCommand.Aliases.Add("d");
decodeCommand.SetAction(parseResult =>
{
    using var input = parseResult.GetValue(inputArgument)!.OpenRead();

    var properties = new byte[5];
    if (input.Read(properties, 0, 5) is not 5)
    {
        throw new InvalidDataException("input .lzma is too short");
    }

    LzmaDecoder decoder = new(properties);

    var outputSize = 0L;
    for (var i = 0; i < 8; i++)
    {
        var v = input.ReadByte();
        if (v < 0)
        {
            throw new InvalidDataException("Can't Read 1");
        }

        outputSize |= ((long)(byte)v) << (8 * i);
    }

    using var output = parseResult.GetValue(outputArgument)!.OpenWrite();
    decoder.Decode(input, output, outputSize);
});

var iterationOption = new Option<int>("-i") { DefaultValueFactory = _ => 10 };

var benchmarkCommand = new Command("benchmark")
{
    dictionaryOption,
    iterationOption,
};

benchmarkCommand.Aliases.Add("b");
benchmarkCommand.SetAction(parseResult =>
{
    var iterations = parseResult.GetValue(iterationOption);
    var dictionary = 1U << parseResult.GetValue(dictionaryOption);
    _ = Altemiq.Lzma.Cli.Benchmark.Run(iterations, dictionary);
});

var rootCommand = new RootCommand
{
    encodeCommand,
    decodeCommand,
    benchmarkCommand,
};

SetHelpCustomSource(rootCommand, matchFinderOption, LzmaMatchFinder.BinaryTree4.ToStringFast(useMetadataAttributes: true));

await rootCommand.Parse(args).InvokeAsync().ConfigureAwait(false);

static Action<System.CommandLine.Parsing.OptionResult> CreateNumberValidator<T>(T minimum, T maximum)
    where T : IParsable<T>, IComparable<T>
{
    return result =>
    {
        if (result.Tokens is [{ Value: { } value }, ..]
            && T.TryParse(value, provider: null, out var t)
            && (t.CompareTo(minimum) < 0 || t.CompareTo(maximum) > 0))
        {
            result.AddError($"Value must be between {minimum} and {maximum}");
        }
    };
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "This is required")]
static void SetHelpCustomSource(Command command, Symbol symbol, string defaultValue)
{
    if (command.Options.OfType<System.CommandLine.Help.HelpOption>().FirstOrDefault() is { Action: System.CommandLine.Help.HelpAction helpAction }
        && helpAction.GetType().GetProperty("Builder", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.GetValue(helpAction) is { } builder
        && builder.GetType().GetMethod("CustomizeSymbol", [typeof(Symbol), typeof(string), typeof(string), typeof(string)]) is { } method)
    {
        method.Invoke(builder, [symbol, default(string), default(string), defaultValue]);
    }
}