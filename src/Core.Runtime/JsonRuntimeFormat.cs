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
        var values = new List<Runtime>();
#if NET461_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        var runtimes = JsonDocument.Parse(stream).RootElement.GetProperty("runtimes");

        foreach (var json in runtimes.EnumerateObject())
        {
            var name = json.Name;
            var imports = json.Value.GetProperty("#import").EnumerateArray().ToArray();
            values.Add(new Runtime(name, imports.Select(p => p.GetString()!)));
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
            var name = json.Key;
            var imports = json.Value["#import"] is JArray i
                ? i.Select(s => s.Value<string>())
                : Enumerable.Empty<string>();
            values.Add(new Runtime(name, imports));
        }

        static IEnumerable<KeyValuePair<string, JToken>> EachProperty(JToken json)
        {
            return (json as IEnumerable<KeyValuePair<string, JToken>>)
                ?? Enumerable.Empty<KeyValuePair<string, JToken>>();
        }
#endif

        // flatten this
        return Flatten(values).ToList();
    }

    private static IEnumerable<RuntimeFallbacks> Flatten(IList<Runtime> runtimes)
    {
        foreach (var runtime in runtimes)
        {
            var imports = runtime.Imports;
            var fallbacks = new SortedList<int, string>(DuplicateComparer<int>.Instance);

            foreach (var import in imports)
            {
                fallbacks.Add(0, import);

                // see if this has any other imports
                foreach (var other in GetImports(import, runtimes, 1))
                {
                    fallbacks.Add(other.Key, other.Value);
                }
            }

            yield return new RuntimeFallbacks(runtime.Name, fallbacks.Values.Distinct(StringComparer.Ordinal));

            IEnumerable<KeyValuePair<int, string>> GetImports(string import, IList<Runtime> runtimes, int depth)
            {
                if (runtimes.FirstOrDefault(runtime => string.Equals(runtime.Name, import, StringComparison.Ordinal)) is not Runtime runtime)
                {
                    yield break;
                }

                foreach (var i in runtime.Imports)
                {
                    yield return new KeyValuePair<int, string>(depth, i);
                    foreach (var other in GetImports(i, runtimes, depth + 1))
                    {
                        yield return other;
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
        public string Name { get; } = name;

        public IEnumerable<string> Imports { get; } = imports;
    }
}