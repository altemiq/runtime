[assembly: Xunit.Sdk.RegisterXunitSerializer(typeof(XUnitSerializer))]

namespace Altemiq;

using Xunit.Sdk;

internal class XUnitSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
#if NET5_0_OR_GREATER
        if (type == typeof(Half))
        {
            return Half.Parse(serializedValue);
        }
#endif

        throw new InvalidOperationException();
    }

    public bool IsSerializable(Type type, object? value, [System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out string? failureReason)
    {
#if NET5_0_OR_GREATER
        if (type == typeof(Half))
        {
            failureReason = default;
            return value is Half;
        }
#endif

        failureReason = "Type is not supported";
        return false;
    }

    public string Serialize(object value)
    {
        return value switch
        {
#if NET5_0_OR_GREATER
            Half half => half.ToString(),
#endif
            _ => throw new InvalidOperationException(),
        };
    }
}
