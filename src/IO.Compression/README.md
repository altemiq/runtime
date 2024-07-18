# Altemiq Compression library

This extends the classes in [`System.IO.Compression`](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression).

## `DisposableStream`

Wraps a stream from a [`System.IO.Compression.ZipArchiveEntry`](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchiveentry) so that when it is disposed, it disposes the [`System.IO.Compression.ZipArchive`](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive) as well.

## `SeekableStream`

Wraps a stream from a [`System.IO.Compression.ZipArchiveEntry`](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchiveentry) so that it is possible to seek forwards, and when it is disposed, it disposes the [`System.IO.Compression.ZipArchive`](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive) as well.

## `System.IO.Compression.ZipArchiveEntry.OpenSeekable`

Creates a seekable stream from a [`System.IO.Compression.ZipArchiveEntry`](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchiveentry).