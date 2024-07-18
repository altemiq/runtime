# Altemiq Core Library

This is the code library for base classes, extension methods, etc.

This contains code for LINQ extensions, as well as base string parsing, and options types.

# OneOf

Provides F# style unions for C#, using a custom type `OneOf<T0, ... Tn>`.
An instance of this type holds a single value, which is one of the types in its generic argument list.

## Creation

These can be created using the `From` methods, on the static `OneOf` class.

``` csharp
var oneOf = OneOf.From<int, string>(1);
var result = oneOf.AsT0;
```

You can also use the implicit operators.

``` csharp
OneOf<int, string> oneOf = "1";
var result = oneOf.AsT1;
```

## Use Cases

### As a method return

This is used to return different values from a method.

``` csharp
public OneOf<User, InvalidName, NameTaken> CreateUser(string username)
{
    if (!IsValid(username))
    {
        return new InvalidName();
    }

    var user = this.repo.FindByUsername(username);
    if (user is not null)
    {
        return new NameTaken();
    }

    var user = new User(username);
    this.repo.Save(user);

    return user;
}
```

### As an 'Option' Type

This is used to pass in different values to a method, just declare a `OneOf<Something, None>`.

### As a method parameter value

You can use also use `OneOf` as a parameter type, without additional overloads. Having multiple parameters, the number of overloads required increases rapidly.

### As a property/variable

You can use `OneOf` when a value is constrained to certain types, rather than using `object`.

## Matching/Switching

You use the `TOut Match(Func<T0, TOut> f0, ... Func<Tn,TOut> fn)` method to get a value out, with the number of handles matching the number of generic arguments.

For example:

``` csharp
OneOf<string, ColorName, Color> backgroundColor = ...;
var color = backgroundColor.Match(
    str => CssHelper.GetColorFromString(str),
    name => new Color(name),
    col => col,
);

this.window.BackgroundColor = color;
```

There is also a `.Switch` method, for when you aren't returning a value:

``` csharp
OneOf<string, DateTime> dateValue = ...;
dateValue.Switch(
    str => AddEntry(DateTime.Parse(str), foo),
    dateTime => AddEntry(dateTime, foo),
);
```

# Splitting Spans

With `string`, there are methods to split the string into an array, based on separators.

This library allows the same for `ReadOnlySpan<char>` values.

For example:

``` csharp
ReadOnlySpan<char> span = ...;
var enumerator = span.Split(',');

var firstValueAsString = span.GetNextString(ref enumerator);
var secondValueAsInt32 = span.GetNextInt32(ref enumerator);

while (enumerator.MoveNext())
{
    yield return span[enumerator.Current];
}
```

# String Extensions

This contains extensions to help with quoted strings, such as data coming from CSV files.

## Reading quoted strings

``` csharp
var value = """"
    1,2,"This is a quoted string
    With a newline in it and ""embedded quotes""",4,5
    """";

value.SplitQuoted(',');
```

## Writing quoted strings

This can allow for embedded delimiters, quotes, new-lines, and also non 7-bit ASCII characters.

This will also quote an empty string as `""` to distinguish it from a `null` string which will be empty.

``` csharp
// will be quoted
"value,\rsecond".Quote(',', StringQuoteOptions.QuoteAll);

// quotes not necessary
"value".Quote(',', StringQuoteOptions.QuoteAll);

// this will always be quoted
"".Quote(',');

// this will never be quoted
((string)null).Quote(',');
```

# LINQ methods

## `IndexOfClosest`
This contains extensions to get the closest value in a list, based on how close a value is.

For example:
``` csharp
IReadOnlyList<double> list = new[] { 1D, 5D, 10D, 15D, 20D };

# this will return 1
list.IndexOfClosest(7D);
```

## `QuickSort`
This performs a quick sort of a list with an [`IComparable`](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) implementation or a [`Comparison`](https://learn.microsoft.com/en-us/dotnet/api/system.comparison-1).

``` csharp
var first = new SimpleStruct(0, 0D);
var second = new SimpleStruct(1, 1D);
var third = new SimpleStruct(2, 2D);
var forth = new SimpleStruct(3, 3D);
var list = new List<SimpleStruct>
{
    third,
    forth,
    second,
    first
};

list.QuickSort();
```

# Streams

## `EchoStream`

This class echos the data written in to be able to be read. This can be useful for reading data that only writes to a stream.

``` csharp
var stream = new EchoStream();

Task.Run(() =>
{
    stream.Write(/* data to write */);
});

Task.Run(() =>
{
    stream.Read(/* data read */);
});
```

## `MultipleStream`

This class have multiple backing streams, represented as a single stream. This can be useful to attempt to allow writing to streams in a backward sense when we can only seek forwards.

This is highlighted by data formats that have a header than contains information about the data in the file, such as the number of records.
Generally these are written at the end of the process, once the number of records is known. For forwards only streams, this is not possible.

``` csharp
var stream = new MultipleStream();
stream.SwitchTo("header");
// write uninitialized header.

stream.SwitchTo("data");
// write data

stream.SwitchTo("header");
// write updated header

stream.CopyTo(/* output stream */);
```

### `MultipleMemoryStream`

Implementation of `MultipleStream` backed by `MemoryStream` instances.

## `SeekableStream`

This class allows forwards only seeking through a stream that does not allow seeking.
This is done by reading the data until the required position is reached.

``` csharp
var stream = new SeekableStream(/* stream that is not seekable */);

// seek should not throw
stream.Seek(123);
```

# Bit Manipulation

## `BitConverter`

Replica of [`System.BitConverter`](https://learn.microsoft.com/en-us/dotnet/api/system.bitconverter) with all types allowed for, and the ability to specify the byte ordering.

## `BinaryPrimitives`

Replica of [`System.Buffers.Binary.BinaryPrimitives`](https://learn.microsoft.com/en-us/dotnet/api/system.buffers.binary.binaryprimitives) with all types allow for.

# NanoId

Implementation of [nanoid](https://github.com/ai/nanoid).

# Security

## Random

Implementation of [`System.Random`](https://learn.microsoft.com/en-us/dotnet/api/system.random) using [`System.Security.Cryptography.RandomNumberGenerator`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator) as the generator.