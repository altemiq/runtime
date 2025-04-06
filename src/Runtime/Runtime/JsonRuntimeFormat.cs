// -----------------------------------------------------------------------
// <copyright file="JsonRuntimeFormat.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

using Microsoft.Extensions.DependencyModel;
#if NET461_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Text.Json;
#else
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif

/// <summary>
/// Methods for the JSON runtime format.
/// </summary>
internal static class JsonRuntimeFormat
{
    /// <summary>
    /// Reads the runtime graph from the specified path.
    /// </summary>
    /// <param name="path">The file containing the JSON runtime format.</param>
    /// <returns>The runtime graph.</returns>
    public static IEnumerable<RuntimeFallbacks> ReadRuntimeGraph(string path)
    {
        using var fileStream = File.OpenRead(path);
        return ReadRuntimeGraph(fileStream);
    }

    /// <summary>
    /// Reads the runtime graph from the specified stream.
    /// </summary>
    /// <param name="stream">The stream containing the JSON runtime format.</param>
    /// <returns>The runtime graph.</returns>
    public static IReadOnlyList<RuntimeFallbacks> ReadRuntimeGraph(Stream stream)
    {
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        return [.. Flatten([.. GetRuntimes(stream)])];
#else
#pragma warning disable IDE0305 // Simplify collection initialization
        return Flatten([.. GetRuntimes(stream)]).ToList().AsReadOnly();
#pragma warning restore IDE0305 // Simplify collection initialization
#endif

        static IEnumerable<Runtime> GetRuntimes(Stream stream)
        {
#if NET461_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            foreach (var json in JsonDocument.Parse(stream).RootElement.GetProperty("runtimes").EnumerateObject())
            {
                yield return new Runtime(
                    json.Name,
                    [.. json.Value.GetProperty("#import").EnumerateArray().Select(static p => p.GetString()!)]);
            }
#else
            var loadSettings = new JsonLoadSettings()
            {
                LineInfoHandling = LineInfoHandling.Ignore,
                CommentHandling = CommentHandling.Ignore,
            };

            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);
            var token = JToken.Load(jsonReader, loadSettings);

            foreach (var json in EachProperty(token["runtimes"]))
            {
                var imports = json.Value["#import"] is JArray import
                    ? import.Select(static s => s.Value<string>()).ToArray()
                    : [];
                yield return new Runtime(json.Key, imports);
            }

            static IEnumerable<KeyValuePair<string, JToken>> EachProperty(JToken json)
            {
                return json as IEnumerable<KeyValuePair<string, JToken>> ?? [];
            }
#endif
        }
    }

    private static IEnumerable<RuntimeFallbacks> Flatten(List<Runtime> runtimes)
    {
        foreach (var runtime in runtimes)
        {
            var imports = runtime.Imports;
            var fallbacks = new SortedList<int, string>(DuplicateComparer<int>.Instance);

            foreach (var import in imports)
            {
                fallbacks.Add(0, import);

                // see if this has any other imports
                foreach (var runtimeImport in GetImports(import, runtimes, 1))
                {
                    fallbacks.Add(runtimeImport.Key, runtimeImport.Value);
                }
            }

            yield return new RuntimeFallbacks(runtime.Name, fallbacks.Values.Distinct(StringComparer.Ordinal));

            static IEnumerable<KeyValuePair<int, string>> GetImports(string import, List<Runtime> runtimes, int depth)
            {
                if (runtimes.Find(runtime => string.Equals(runtime.Name, import, StringComparison.Ordinal)) is { } runtime)
                {
                    foreach (var runtimeImport in runtime.Imports)
                    {
                        yield return new KeyValuePair<int, string>(depth, runtimeImport);
                        foreach (var other in GetImports(runtimeImport, runtimes, depth + 1))
                        {
                            yield return other;
                        }
                    }
                }
            }
        }
    }

    private readonly struct DuplicateComparer<TKey> : IComparer<TKey>
        where TKey : struct, IComparable
    {
        public static readonly IComparer<TKey> Instance = default(DuplicateComparer<TKey>);

        public readonly int Compare(TKey x, TKey y) => x.CompareTo(y) switch
        {
            // Handle equality as being lesser.
            0 => -1,

            // IndexOfKey(key) since the comparer never returns 0 to signal key equality
            var result => result,
        };
    }

    private sealed class Runtime(string name, IEnumerable<string> imports)
    {
        public string Name => name;

        public IEnumerable<string> Imports => imports;
    }
}