[assembly: Xunit.Sdk.RegisterXunitSerializer(typeof(EnumerableSerializer), typeof(IEnumerable<>), typeof(System.Collections.IEnumerable))]

namespace Altemiq;

using Xunit.Sdk;

internal class EnumerableSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        if (GetEnumerableType(type) is { } enumerableType)
        {
            return DeserializeEnumerable(serializedValue, enumerableType);
        }

        throw new InvalidOperationException();

        static object DeserializeEnumerable(string serializedValue, Type enumerableType)
        {
            var values = serializedValue.TrimStart('[').TrimEnd(']').Split(',');
            if (typeof(EnumerableSerializer).GetMethod(nameof(CreateEnumerable), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static) is { IsGenericMethod: true } methodInfo)
            {
                return methodInfo.MakeGenericMethod(enumerableType).Invoke(null, [values])!;
            }

            throw new InvalidOperationException();
        }
    }

    public bool IsSerializable(Type type, object? value, [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? failureReason)
    {
        if (GetEnumerableType(type) is { } enumerableType)
        {
            if (SerializationHelper.Instance.IsSerializable(null, enumerableType))
            {
                failureReason = default;
                return true;
            }

            failureReason = $"Type {enumerableType.FullName} is not serializable";
            return false;
        }

        failureReason = $"Type {type.FullName} is not an enumerable type";
        return false;
    }

    public string Serialize(object value)
    {
        if (value is System.Collections.IEnumerable enumerable)
        {
            var stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append('[');
            var first = true;
            foreach (var item in enumerable)
            {

                stringBuilder.Append(SerializationHelper.Instance.Serialize(item));
                if (!first)
                {
                    stringBuilder.Append(',');
                }
                
                first = false;
            }

            return stringBuilder.ToString();
        }

        throw new NotImplementedException();
    }

    // see if this is a type we can serialize
    private static Type? GetEnumerableType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return type.GetGenericArguments()[0];
        }

        return default;
    }

    private static IEnumerable<T?> CreateEnumerable<T>(IEnumerable<string> values)
    {
        foreach (var value in values)
        {
            yield return SerializationHelper.Instance.Deserialize<T>(value);
        }
    }
}
