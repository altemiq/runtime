namespace Altemiq.Numerics;

internal static class GenericMathTestMemberData
{
    // binary64 (double) has a machine epsilon of 2^-52 (approx. 2.22e-16). However, this
    // is slightly too accurate when writing tests meant to run against libm implementations
    // for various platforms. 2^-50 (approx. 8.88e-16) seems to be as accurate as we can get.
    //
    // The tests themselves will take CrossPlatformMachineEpsilon and adjust it according to the expected result
    // so that the delta used for comparison will compare the most significant digits and ignore
    // any digits that are outside the double precision range (15-17 digits).
    //
    // For example, a test with an expect result in the format of 0.xxxxxxxxxxxxxxxxx will use
    // CrossPlatformMachineEpsilon for the variance, while an expected result in the format of 0.0xxxxxxxxxxxxxxxxx
    // will use CrossPlatformMachineEpsilon / 10 and expected result in the format of x.xxxxxxxxxxxxxxxx will
    // use CrossPlatformMachineEpsilon * 10.
    internal const double DoubleCrossPlatformMachineEpsilon = 8.8817841970012523e-16;

    internal const double MinNormalDouble = 2.2250738585072014E-308;

    internal const double MaxSubnormalDouble = 2.2250738585072009E-308;

    public static IEnumerable<object[]> ClampDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, 1.0f, 63.0f, 1.0f };
            yield return new object[] { double.MinValue, 1.0f, 63.0f, 1.0f };
            yield return new object[] { -1.0f, 1.0f, 63.0f, 1.0f };
            yield return new object[] { -MinNormalDouble, 1.0f, 63.0f, 1.0f };
            yield return new object[] { -MaxSubnormalDouble, 1.0f, 63.0f, 1.0f };
            yield return new object[] { -double.Epsilon, 1.0f, 63.0f, 1.0f };
            yield return new object[] { -0.0f, 1.0f, 63.0f, 1.0f };
            yield return new object[] { double.NaN, 1.0f, 63.0f, double.NaN };
            yield return new object[] { 0.0f, 1.0f, 63.0f, 1.0f };
            yield return new object[] { double.Epsilon, 1.0f, 63.0f, 1.0f };
            yield return new object[] { MaxSubnormalDouble, 1.0f, 63.0f, 1.0f };
            yield return new object[] { MinNormalDouble, 1.0f, 63.0f, 1.0f };
            yield return new object[] { 1.0f, 1.0f, 63.0f, 1.0f };
            yield return new object[] { double.MaxValue, 1.0f, 63.0f, 63.0f };
            yield return new object[] { double.PositiveInfinity, 1.0f, 63.0f, 63.0f };
        }
    }

    public static IEnumerable<object[]> CopySignDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { double.NegativeInfinity, -3.1415926535897932, double.NegativeInfinity };
            yield return new object[] { double.NegativeInfinity, -0.0, double.NegativeInfinity };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NegativeInfinity };
            yield return new object[] { double.NegativeInfinity, 0.0, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, 3.1415926535897932, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { -3.1415926535897932, double.NegativeInfinity, -3.1415926535897932 };
            yield return new object[] { -3.1415926535897932, -3.1415926535897932, -3.1415926535897932 };
            yield return new object[] { -3.1415926535897932, -0.0, -3.1415926535897932 };
            yield return new object[] { -3.1415926535897932, double.NaN, -3.1415926535897932 };
            yield return new object[] { -3.1415926535897932, 0.0, 3.1415926535897932 };
            yield return new object[] { -3.1415926535897932, 3.1415926535897932, 3.1415926535897932 };
            yield return new object[] { -3.1415926535897932, double.PositiveInfinity, 3.1415926535897932 };
            yield return new object[] { -0.0, double.NegativeInfinity, -0.0 };
            yield return new object[] { -0.0, -3.1415926535897932, -0.0 };
            yield return new object[] { -0.0, -0.0, -0.0 };
            yield return new object[] { -0.0, double.NaN, -0.0 };
            yield return new object[] { -0.0, 0.0, 0.0 };
            yield return new object[] { -0.0, 3.1415926535897932, 0.0 };
            yield return new object[] { -0.0, double.PositiveInfinity, 0.0 };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NaN };
            yield return new object[] { double.NaN, -3.1415926535897932, double.NaN };
            yield return new object[] { double.NaN, -0.0, double.NaN };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 0.0, double.NaN };
            yield return new object[] { double.NaN, 3.1415926535897932, double.NaN };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, -0.0 };
            yield return new object[] { 0.0, -3.1415926535897932, -0.0 };
            yield return new object[] { 0.0, -0.0, -0.0 };
            yield return new object[] { 0.0, double.NaN, -0.0 };
            yield return new object[] { 0.0, 0.0, 0.0 };
            yield return new object[] { 0.0, 3.1415926535897932, 0.0 };
            yield return new object[] { 0.0, double.PositiveInfinity, 0.0 };
            yield return new object[] { 3.1415926535897932, double.NegativeInfinity, -3.1415926535897932 };
            yield return new object[] { 3.1415926535897932, -3.1415926535897932, -3.1415926535897932 };
            yield return new object[] { 3.1415926535897932, -0.0, -3.1415926535897932 };
            yield return new object[] { 3.1415926535897932, double.NaN, -3.1415926535897932 };
            yield return new object[] { 3.1415926535897932, 0.0, 3.1415926535897932 };
            yield return new object[] { 3.1415926535897932, 3.1415926535897932, 3.1415926535897932 };
            yield return new object[] { 3.1415926535897932, double.PositiveInfinity, 3.1415926535897932 };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, -3.1415926535897932, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, -0.0, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, 0.0, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, 3.1415926535897932, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity };
        }
    }



    public static IEnumerable<object[]> CosDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NaN, 0.0 };
            yield return new object[] { -3.1415926535897932, -1.0, DoubleCrossPlatformMachineEpsilon * 10 }; // value: -(pi)
            yield return new object[] { -2.7182818284590452, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon };      // value: -(e)
            yield return new object[] { -2.3025850929940457, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon };      // value: -(ln(10))
            yield return new object[] { -1.5707963267948966, 0.0, DoubleCrossPlatformMachineEpsilon };      // value: -(pi / 2)
            yield return new object[] { -1.4426950408889634, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon };      // value: -(log2(e))
            yield return new object[] { -1.4142135623730950, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon };      // value: -(sqrt(2))
            yield return new object[] { -1.1283791670955126, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon };      // value: -(2 / sqrt(pi))
            yield return new object[] { -1.0, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { -0.78539816339744831, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon };      // value: -(pi / 4),        expected:  (1 / sqrt(2))
            yield return new object[] { -0.70710678118654752, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon };      // value: -(1 / sqrt(2))
            yield return new object[] { -0.69314718055994531, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon };      // value: -(ln(2))
            yield return new object[] { -0.63661977236758134, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon };      // value: -(2 / pi)
            yield return new object[] { -0.43429448190325183, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon };      // value: -(log10(e))
            yield return new object[] { -0.31830988618379067, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon };      // value: -(1 / pi)
            yield return new object[] { -0.0, 1.0, DoubleCrossPlatformMachineEpsilon * 10 };
            yield return new object[] { double.NaN, double.NaN, 0.0 };
            yield return new object[] { 0.0, 1.0, DoubleCrossPlatformMachineEpsilon * 10 };
            yield return new object[] { 0.31830988618379067, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon };      // value:  (1 / pi)
            yield return new object[] { 0.43429448190325183, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon };      // value:  (log10(e))
            yield return new object[] { 0.63661977236758134, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon };      // value:  (2 / pi)
            yield return new object[] { 0.69314718055994531, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon };      // value:  (ln(2))
            yield return new object[] { 0.70710678118654752, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon };      // value:  (1 / sqrt(2))
            yield return new object[] { 0.78539816339744831, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon };      // value:  (pi / 4),        expected:  (1 / sqrt(2))
            yield return new object[] { 1.0, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 1.1283791670955126, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon };      // value:  (2 / sqrt(pi))
            yield return new object[] { 1.4142135623730950, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon };      // value:  (sqrt(2))
            yield return new object[] { 1.4426950408889634, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon };      // value:  (log2(e))
            yield return new object[] { 1.5707963267948966, 0.0, DoubleCrossPlatformMachineEpsilon };      // value:  (pi / 2)
            yield return new object[] { 2.3025850929940457, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon };      // value:  (ln(10))
            yield return new object[] { 2.7182818284590452, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon };      // value:  (e)
            yield return new object[] { 3.1415926535897932, -1.0, DoubleCrossPlatformMachineEpsilon * 10 }; // value:  (pi)
            yield return new object[] { double.PositiveInfinity, double.NaN, 0.0 };
        }
    }

    public static IEnumerable<object[]> DegreesToRadiansDouble
    {
        get
        {
            yield return new object[] { double.NaN, double.NaN, 0.0 };
            yield return new object[] { 0.0, 0.0, 0.0 };
            yield return new object[] { 0.31830988618379067, 0.005555555555555556, DoubleCrossPlatformMachineEpsilon };       // value:  (1 / pi)
            yield return new object[] { 0.43429448190325183, 0.007579868632454674, DoubleCrossPlatformMachineEpsilon };       // value:  (log10(e))
            yield return new object[] { 0.5, 0.008726646259971648, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 0.63661977236758134, 0.011111111111111112, DoubleCrossPlatformMachineEpsilon };       // value:  (2 / pi)
            yield return new object[] { 0.69314718055994531, 0.01209770050168668, DoubleCrossPlatformMachineEpsilon };       // value:  (ln(2))
            yield return new object[] { 0.70710678118654752, 0.012341341494884351, DoubleCrossPlatformMachineEpsilon };       // value:  (1 / sqrt(2))
            yield return new object[] { 0.78539816339744831, 0.013707783890401885, DoubleCrossPlatformMachineEpsilon };       // value:  (pi / 4)
            yield return new object[] { 1.0, 0.017453292519943295, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 1.1283791670955126, 0.019693931676727953, DoubleCrossPlatformMachineEpsilon };       // value:  (2 / sqrt(pi))
            yield return new object[] { 1.4142135623730950, 0.024682682989768702, DoubleCrossPlatformMachineEpsilon };       // value:  (sqrt(2))
            yield return new object[] { 1.4426950408889634, 0.02517977856570663, DoubleCrossPlatformMachineEpsilon };       // value:  (log2(e))
            yield return new object[] { 1.5, 0.02617993877991494, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 1.5707963267948966, 0.02741556778080377, DoubleCrossPlatformMachineEpsilon };       // value:  (pi / 2)
            yield return new object[] { 2.0, 0.03490658503988659, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 2.3025850929940457, 0.040187691180085916, DoubleCrossPlatformMachineEpsilon };       // value:  (ln(10))
            yield return new object[] { 2.5, 0.04363323129985824, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 2.7182818284590452, 0.047442967903742035, DoubleCrossPlatformMachineEpsilon };       // value:  (e)
            yield return new object[] { 3.0, 0.05235987755982988, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 3.1415926535897932, 0.05483113556160754, DoubleCrossPlatformMachineEpsilon };       // value:  (pi)
            yield return new object[] { 3.5, 0.061086523819801536, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, 0.0 };
        }
    }

    public static IEnumerable<object[]> ExpDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, 0.0, 0.0 };
            yield return new object[] { -3.1415926535897932, 0.043213918263772250, DoubleCrossPlatformMachineEpsilon / 10 };   // value: -(pi)
            yield return new object[] { -2.7182818284590452, 0.065988035845312537, DoubleCrossPlatformMachineEpsilon / 10 };   // value: -(e)
            yield return new object[] { -2.3025850929940457, 0.1, DoubleCrossPlatformMachineEpsilon };        // value: -(ln(10))
            yield return new object[] { -1.5707963267948966, 0.20787957635076191, DoubleCrossPlatformMachineEpsilon };        // value: -(pi / 2)
            yield return new object[] { -1.4426950408889634, 0.23629008834452270, DoubleCrossPlatformMachineEpsilon };        // value: -(log2(e))
            yield return new object[] { -1.4142135623730950, 0.24311673443421421, DoubleCrossPlatformMachineEpsilon };        // value: -(sqrt(2))
            yield return new object[] { -1.1283791670955126, 0.32355726390307110, DoubleCrossPlatformMachineEpsilon };        // value: -(2 / sqrt(pi))
            yield return new object[] { -1.0, 0.36787944117144232, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { -0.78539816339744831, 0.45593812776599624, DoubleCrossPlatformMachineEpsilon };        // value: -(pi / 4)
            yield return new object[] { -0.70710678118654752, 0.49306869139523979, DoubleCrossPlatformMachineEpsilon };        // value: -(1 / sqrt(2))
            yield return new object[] { -0.69314718055994531, 0.5, 0.0 };                                     // value: -(ln(2))
            yield return new object[] { -0.63661977236758134, 0.52907780826773535, DoubleCrossPlatformMachineEpsilon };        // value: -(2 / pi)
            yield return new object[] { -0.43429448190325183, 0.64772148514180065, DoubleCrossPlatformMachineEpsilon };        // value: -(log10(e))
            yield return new object[] { -0.31830988618379067, 0.72737734929521647, DoubleCrossPlatformMachineEpsilon };        // value: -(1 / pi)
            yield return new object[] { -0.0, 1.0, 0.0 };
            yield return new object[] { double.NaN, double.NaN, 0.0 };
            yield return new object[] { 0.0, 1.0, 0.0 };
            yield return new object[] { 0.31830988618379067, 1.3748022274393586, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (1 / pi)
            yield return new object[] { 0.43429448190325183, 1.5438734439711811, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (log10(e))
            yield return new object[] { 0.63661977236758134, 1.8900811645722220, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (2 / pi)
            yield return new object[] { 0.69314718055994531, 2.0, 0.0 };                                      // value:  (ln(2))
            yield return new object[] { 0.70710678118654752, 2.0281149816474725, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (1 / sqrt(2))
            yield return new object[] { 0.78539816339744831, 2.1932800507380155, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (pi / 4)
            yield return new object[] { 1.0, 2.7182818284590452, DoubleCrossPlatformMachineEpsilon * 10 };   //                          expected: (e)
            yield return new object[] { 1.1283791670955126, 3.0906430223107976, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (2 / sqrt(pi))
            yield return new object[] { 1.4142135623730950, 4.1132503787829275, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (sqrt(2))
            yield return new object[] { 1.4426950408889634, 4.2320861065570819, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (log2(e))
            yield return new object[] { 1.5707963267948966, 4.8104773809653517, DoubleCrossPlatformMachineEpsilon * 10 };   // value:  (pi / 2)
            yield return new object[] { 2.3025850929940457, 10.0, DoubleCrossPlatformMachineEpsilon * 10 };                                      // value:  (ln(10))
            yield return new object[] { 2.7182818284590452, 15.154262241479264, DoubleCrossPlatformMachineEpsilon * 100 };  // value:  (e)
            yield return new object[] { 3.1415926535897932, 23.140692632779269, DoubleCrossPlatformMachineEpsilon * 100 };  // value:  (pi)
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, 0.0 };
        }
    }

    public static IEnumerable<object[]> FusedMultiplyAddDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity, double.NaN };
            yield return new object[] { double.NegativeInfinity, -0.0, double.NegativeInfinity, double.NaN };
            yield return new object[] { double.NegativeInfinity, -0.0, -3.1415926535897932, double.NaN };
            yield return new object[] { double.NegativeInfinity, -0.0, -0.0, double.NaN };
            yield return new object[] { double.NegativeInfinity, -0.0, double.NaN, double.NaN };
            yield return new object[] { double.NegativeInfinity, -0.0, 0.0, double.NaN };
            yield return new object[] { double.NegativeInfinity, -0.0, 3.1415926535897932, double.NaN };
            yield return new object[] { double.NegativeInfinity, -0.0, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, double.NegativeInfinity, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, -3.1415926535897932, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, -0.0, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, double.NaN, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, 0.0, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, 3.1415926535897932, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity, double.NaN };
            yield return new object[] { -1e308, 2.0, 1e308, -1e308 };
            yield return new object[] { -1e308, 2.0, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { -5, 4, -3, -23 };
            yield return new object[] { -0.0, double.NegativeInfinity, double.NegativeInfinity, double.NaN };
            yield return new object[] { -0.0, double.NegativeInfinity, -3.1415926535897932, double.NaN };
            yield return new object[] { -0.0, double.NegativeInfinity, -0.0, double.NaN };
            yield return new object[] { -0.0, double.NegativeInfinity, double.NaN, double.NaN };
            yield return new object[] { -0.0, double.NegativeInfinity, 0.0, double.NaN };
            yield return new object[] { -0.0, double.NegativeInfinity, 3.1415926535897932, double.NaN };
            yield return new object[] { -0.0, double.NegativeInfinity, double.PositiveInfinity, double.NaN };
            yield return new object[] { -0.0, double.PositiveInfinity, double.NegativeInfinity, double.NaN };
            yield return new object[] { -0.0, double.PositiveInfinity, -3.1415926535897932, double.NaN };
            yield return new object[] { -0.0, double.PositiveInfinity, -0.0, double.NaN };
            yield return new object[] { -0.0, double.PositiveInfinity, double.NaN, double.NaN };
            yield return new object[] { -0.0, double.PositiveInfinity, 0.0, double.NaN };
            yield return new object[] { -0.0, double.PositiveInfinity, 3.1415926535897932, double.NaN };
            yield return new object[] { -0.0, double.PositiveInfinity, double.PositiveInfinity, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, double.NegativeInfinity, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, -3.1415926535897932, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, -0.0, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, double.NaN, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, 0.0, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, 3.1415926535897932, double.NaN };
            yield return new object[] { 0.0, double.NegativeInfinity, double.PositiveInfinity, double.NaN };
            yield return new object[] { 0.0, double.PositiveInfinity, double.NegativeInfinity, double.NaN };
            yield return new object[] { 0.0, double.PositiveInfinity, -3.1415926535897932, double.NaN };
            yield return new object[] { 0.0, double.PositiveInfinity, -0.0, double.NaN };
            yield return new object[] { 0.0, double.PositiveInfinity, double.NaN, double.NaN };
            yield return new object[] { 0.0, double.PositiveInfinity, 0.0, double.NaN };
            yield return new object[] { 0.0, double.PositiveInfinity, 3.1415926535897932, double.NaN };
            yield return new object[] { 0.0, double.PositiveInfinity, double.PositiveInfinity, double.NaN };
            yield return new object[] { 5, 4, 3, 23 };
            yield return new object[] { 1e308, 2.0, -1e308, 1e308 };
            yield return new object[] { 1e308, 2.0, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.PositiveInfinity, -0.0, double.NegativeInfinity, double.NaN };
            yield return new object[] { double.PositiveInfinity, -0.0, -3.1415926535897932, double.NaN };
            yield return new object[] { double.PositiveInfinity, -0.0, -0.0, double.NaN };
            yield return new object[] { double.PositiveInfinity, -0.0, double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, -0.0, 0.0, double.NaN };
            yield return new object[] { double.PositiveInfinity, -0.0, 3.1415926535897932, double.NaN };
            yield return new object[] { double.PositiveInfinity, -0.0, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.PositiveInfinity, 0.0, double.NegativeInfinity, double.NaN };
            yield return new object[] { double.PositiveInfinity, 0.0, -3.1415926535897932, double.NaN };
            yield return new object[] { double.PositiveInfinity, 0.0, -0.0, double.NaN };
            yield return new object[] { double.PositiveInfinity, 0.0, double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, 0.0, 0.0, double.NaN };
            yield return new object[] { double.PositiveInfinity, 0.0, 3.1415926535897932, double.NaN };
            yield return new object[] { double.PositiveInfinity, 0.0, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NaN };
        }
    }

    public static IEnumerable<object[]> HypotDouble
    {
        get
        {
            yield return new object[] { double.NaN, double.NaN, double.NaN, 0.0 };
            yield return new object[] { double.NaN, 0.0f, double.NaN, 0.0 };
            yield return new object[] { double.NaN, 1.0f, double.NaN, 0.0 };
            yield return new object[] { double.NaN, 2.7182818284590452, double.NaN, 0.0 };
            yield return new object[] { double.NaN, 10.0, double.NaN, 0.0 };
            yield return new object[] { 0.0, 0.0, 0.0, 0.0 };
            yield return new object[] { 0.0, 1.0, 1.0, 0.0 };
            yield return new object[] { 0.0, 1.5707963267948966, 1.5707963267948966, 0.0 };
            yield return new object[] { 0.0, 2.0, 2.0, 0.0 };
            yield return new object[] { 0.0, 2.7182818284590452, 2.7182818284590452, 0.0 };
            yield return new object[] { 0.0, 3.0, 3.0, 0.0 };
            yield return new object[] { 0.0, 10.0, 10.0, 0.0 };
            yield return new object[] { 1.0, 1.0, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10 };
            yield return new object[] { 1.0, 1e+10, 1e+10, 0.0 }; // dotnet/runtime#75651
            yield return new object[] { 1.0, 1e+20, 1e+20, 0.0 }; // dotnet/runtime#75651
            yield return new object[] { 2.7182818284590452, 0.31830988618379067, 2.7368553638387594, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (1 / pi)
            yield return new object[] { 2.7182818284590452, 0.43429448190325183, 2.7527563996732919, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (log10(e))
            yield return new object[] { 2.7182818284590452, 0.63661977236758134, 2.7918346715914253, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (2 / pi)
            yield return new object[] { 2.7182818284590452, 0.69314718055994531, 2.8052645352709344, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (ln(2))
            yield return new object[] { 2.7182818284590452, 0.70710678118654752, 2.8087463571726533, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (1 / sqrt(2))
            yield return new object[] { 2.7182818284590452, 0.78539816339744831, 2.8294710413783590, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (pi / 4)
            yield return new object[] { 2.7182818284590452, 1.0, 2.8963867315900082, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)
            yield return new object[] { 2.7182818284590452, 1.1283791670955126, 2.9431778138036127, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (2 / sqrt(pi))
            yield return new object[] { 2.7182818284590452, 1.4142135623730950, 3.0641566701020120, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (sqrt(2))
            yield return new object[] { 2.7182818284590452, 1.4426950408889634, 3.0774055761202907, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (log2(e))
            yield return new object[] { 2.7182818284590452, 1.5707963267948966, 3.1394995141268918, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (pi / 2)
            yield return new object[] { 2.7182818284590452, 2.3025850929940457, 3.5624365551415857, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (ln(10))
            yield return new object[] { 2.7182818284590452, 2.7182818284590452, 3.8442310281591168, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (e)
            yield return new object[] { 2.7182818284590452, 3.1415926535897932, 4.1543544023133136, DoubleCrossPlatformMachineEpsilon * 10 };   // x: (e)   y: (pi)
            yield return new object[] { 10.0, 0.31830988618379067, 10.005064776584025, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (1 / pi)
            yield return new object[] { 10.0, 0.43429448190325183, 10.009426142242702, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (log10(e))
            yield return new object[] { 10.0, 0.63661977236758134, 10.020243746265325, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (2 / pi)
            yield return new object[] { 10.0, 0.69314718055994531, 10.023993865417028, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (ln(2))
            yield return new object[] { 10.0, 0.70710678118654752, 10.024968827881711, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (1 / sqrt(2))
            yield return new object[] { 10.0, 0.78539816339744831, 10.030795096853892, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (pi / 4)
            yield return new object[] { 10.0, 1.0, 10.049875621120890, DoubleCrossPlatformMachineEpsilon * 100 };  //
            yield return new object[] { 10.0, 1.1283791670955126, 10.063460614755501, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (2 / sqrt(pi))
            yield return new object[] { 10.0, 1.4142135623730950, 10.099504938362078, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (sqrt(2))
            yield return new object[] { 10.0, 1.4426950408889634, 10.103532500121213, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (log2(e))
            yield return new object[] { 10.0, 1.5707963267948966, 10.122618292728040, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (pi / 2)
            yield return new object[] { 10.0, 2.3025850929940457, 10.261671311754163, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (ln(10))
            yield return new object[] { 10.0, 2.7182818284590452, 10.362869105558106, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (e)
            yield return new object[] { 10.0, 3.1415926535897932, 10.481870272097884, DoubleCrossPlatformMachineEpsilon * 100 };  //          y: (pi)
            yield return new object[] { double.PositiveInfinity, double.NaN, double.PositiveInfinity, 0.0 };
            yield return new object[] { double.PositiveInfinity, 0.0, double.PositiveInfinity, 0.0 };
            yield return new object[] { double.PositiveInfinity, 1.0, double.PositiveInfinity, 0.0 };
            yield return new object[] { double.PositiveInfinity, 2.7182818284590452, double.PositiveInfinity, 0.0 };
            yield return new object[] { double.PositiveInfinity, 10.0, double.PositiveInfinity, 0.0 };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, 0.0 };
        }
    }

    public static IEnumerable<object[]> IsTestDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity };
            yield return new object[] { double.MinValue };
            yield return new object[] { -1.0 };
            yield return new object[] { -MinNormalDouble };
            yield return new object[] { -MaxSubnormalDouble };
            yield return new object[] { -double.Epsilon };
            yield return new object[] { -0.0 };
            yield return new object[] { double.NaN };
            yield return new object[] { 0.0 };
            yield return new object[] { double.Epsilon };
            yield return new object[] { MaxSubnormalDouble };
            yield return new object[] { MinNormalDouble };
            yield return new object[] { 1.0 };
            yield return new object[] { double.MaxValue };
            yield return new object[] { double.PositiveInfinity };
        }
    }

    public static IEnumerable<object[]> IsNaNDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, false };
            yield return new object[] { double.MinValue, false };
            yield return new object[] { -MinNormalDouble, false };
            yield return new object[] { -MaxSubnormalDouble, false };
            yield return new object[] { -double.Epsilon, false };
            yield return new object[] { -0.0, false };
            yield return new object[] { double.NaN, true };
            yield return new object[] { 0.0, false };
            yield return new object[] { double.Epsilon, false };
            yield return new object[] { MaxSubnormalDouble, false };
            yield return new object[] { MinNormalDouble, false };
            yield return new object[] { double.MaxValue, false };
            yield return new object[] { double.PositiveInfinity, false };
        }
    }

    public static IEnumerable<object[]> IsNegativeDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, true };
            yield return new object[] { double.MinValue, true };
            yield return new object[] { -MinNormalDouble, true };
            yield return new object[] { -MaxSubnormalDouble, true };
            yield return new object[] { -0.0, true };
            yield return new object[] { double.NaN, true };
            yield return new object[] { 0.0, false };
            yield return new object[] { MaxSubnormalDouble, false };
            yield return new object[] { MinNormalDouble, false };
            yield return new object[] { double.MaxValue, false };
            yield return new object[] { double.PositiveInfinity, false };
        }
    }

    public static IEnumerable<object[]> IsPositiveDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, false };
            yield return new object[] { double.MinValue, false };
            yield return new object[] { -MinNormalDouble, false };
            yield return new object[] { -MaxSubnormalDouble, false };
            yield return new object[] { -0.0, false };
            yield return new object[] { double.NaN, false };
            yield return new object[] { 0.0, true };
            yield return new object[] { MaxSubnormalDouble, true };
            yield return new object[] { MinNormalDouble, true };
            yield return new object[] { double.MaxValue, true };
            yield return new object[] { double.PositiveInfinity, true };
        }
    }

    public static IEnumerable<object[]> IsPositiveInfinityDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, false };
            yield return new object[] { double.MinValue, false };
            yield return new object[] { -MinNormalDouble, false };
            yield return new object[] { -MaxSubnormalDouble, false };
            yield return new object[] { -double.Epsilon, false };
            yield return new object[] { -0.0, false };
            yield return new object[] { double.NaN, false };
            yield return new object[] { 0.0, false };
            yield return new object[] { double.Epsilon, false };
            yield return new object[] { MaxSubnormalDouble, false };
            yield return new object[] { MinNormalDouble, false };
            yield return new object[] { double.MaxValue, false };
            yield return new object[] { double.PositiveInfinity, true };
        }
    }

    public static IEnumerable<object[]> IsZeroDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, false };
            yield return new object[] { double.MinValue, false };
            yield return new object[] { -MinNormalDouble, false };
            yield return new object[] { -MaxSubnormalDouble, false };
            yield return new object[] { -double.Epsilon, false };
            yield return new object[] { -0.0, true };
            yield return new object[] { double.NaN, false };
            yield return new object[] { 0.0, true };
            yield return new object[] { double.Epsilon, false };
            yield return new object[] { MaxSubnormalDouble, false };
            yield return new object[] { MinNormalDouble, false };
            yield return new object[] { double.MaxValue, false };
            yield return new object[] { double.PositiveInfinity, false };
        }
    }

    public static IEnumerable<object[]> LerpDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity, 0.5, double.NegativeInfinity };
            yield return new object[] { double.NegativeInfinity, double.NaN, 0.5, double.NaN };
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, 0.5, double.NaN };
            yield return new object[] { double.NegativeInfinity, 0.0, 0.5, double.NegativeInfinity };
            yield return new object[] { double.NegativeInfinity, 1.0, 0.5, double.NegativeInfinity };
            yield return new object[] { double.NaN, double.NegativeInfinity, 0.5, double.NaN };
            yield return new object[] { double.NaN, double.NaN, 0.5, double.NaN };
            yield return new object[] { double.NaN, double.PositiveInfinity, 0.5, double.NaN };
            yield return new object[] { double.NaN, 0.0, 0.5, double.NaN };
            yield return new object[] { double.NaN, 1.0, 0.5, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, 0.5, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.NaN, 0.5, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, 0.5, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, 0.0, 0.5, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, 1.0, 0.5, double.PositiveInfinity };
            yield return new object[] { 1.0, 3.0, 0.0, 1.0 };
            yield return new object[] { 1.0, 3.0, 0.5, 2.0 };
            yield return new object[] { 1.0, 3.0, 1.0, 3.0 };
            yield return new object[] { 1.0, 3.0, 2.0, 5.0 };
            yield return new object[] { 2.0, 4.0, 0.0, 2.0 };
            yield return new object[] { 2.0, 4.0, 0.5, 3.0 };
            yield return new object[] { 2.0, 4.0, 1.0, 4.0 };
            yield return new object[] { 2.0, 4.0, 2.0, 6.0 };
            yield return new object[] { 3.0, 1.0, 0.0, 3.0 };
            yield return new object[] { 3.0, 1.0, 0.5, 2.0 };
            yield return new object[] { 3.0, 1.0, 1.0, 1.0 };
            yield return new object[] { 3.0, 1.0, 2.0, -1.0 };
            yield return new object[] { 4.0, 2.0, 0.0, 4.0 };
            yield return new object[] { 4.0, 2.0, 0.5, 3.0 };
            yield return new object[] { 4.0, 2.0, 1.0, 2.0 };
            yield return new object[] { 4.0, 2.0, 2.0, 0.0 };
        }
    }

    public static IEnumerable<object[]> LogDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NaN, 0.0 };
            yield return new object[] { -3.1415926535897932, double.NaN, 0.0 };                                     //                              value: -(pi)
            yield return new object[] { -2.7182818284590452, double.NaN, 0.0 };                                     //                              value: -(e)
            yield return new object[] { -1.4142135623730950, double.NaN, 0.0 };                                     //                              value: -(sqrt(2))
            yield return new object[] { -1.0, double.NaN, 0.0 };
            yield return new object[] { -0.69314718055994531, double.NaN, 0.0 };                                     //                              value: -(ln(2))
            yield return new object[] { -0.43429448190325183, double.NaN, 0.0 };                                     //                              value: -(log10(e))
            yield return new object[] { -0.0, double.NegativeInfinity, 0.0 };
            yield return new object[] { double.NaN, double.NaN, 0.0 };
            yield return new object[] { 0.0, double.NegativeInfinity, 0.0 };
            yield return new object[] { 0.043213918263772250, -3.1415926535897932, DoubleCrossPlatformMachineEpsilon * 10 };  // expected: -(pi)
            yield return new object[] { 0.065988035845312537, -2.7182818284590452, DoubleCrossPlatformMachineEpsilon * 10 };  // expected: -(e)
            yield return new object[] { 0.1, -2.3025850929940457, DoubleCrossPlatformMachineEpsilon * 10 };  // expected: -(ln(10))
            yield return new object[] { 0.20787957635076191, -1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10 };  // expected: -(pi / 2)
            yield return new object[] { 0.23629008834452270, -1.4426950408889634, DoubleCrossPlatformMachineEpsilon * 10 };  // expected: -(log2(e))
            yield return new object[] { 0.24311673443421421, -1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10 };  // expected: -(sqrt(2))
            yield return new object[] { 0.32355726390307110, -1.1283791670955126, DoubleCrossPlatformMachineEpsilon * 10 };  // expected: -(2 / sqrt(pi))
            yield return new object[] { 0.36787944117144232, -1.0, 0.0f };
            yield return new object[] { 0.45593812776599624, -0.78539816339744831, DoubleCrossPlatformMachineEpsilon };       // expected: -(pi / 4)
            yield return new object[] { 0.49306869139523979, -0.70710678118654752, DoubleCrossPlatformMachineEpsilon };       // expected: -(1 / sqrt(2))
            yield return new object[] { 0.5, -0.69314718055994531, DoubleCrossPlatformMachineEpsilon };       // expected: -(ln(2))
            yield return new object[] { 0.52907780826773535, -0.63661977236758134, DoubleCrossPlatformMachineEpsilon };       // expected: -(2 / pi)
            yield return new object[] { 0.64772148514180065, -0.43429448190325183, DoubleCrossPlatformMachineEpsilon };       // expected: -(log10(e))
            yield return new object[] { 0.72737734929521647, -0.31830988618379067, DoubleCrossPlatformMachineEpsilon };       // expected: -(1 / pi)
            yield return new object[] { 1.0, 0.0, 0.0 };
            yield return new object[] { 1.3748022274393586, 0.31830988618379067, DoubleCrossPlatformMachineEpsilon };       // expected:  (1 / pi)
            yield return new object[] { 1.5438734439711811, 0.43429448190325183, DoubleCrossPlatformMachineEpsilon };       // expected:  (log10(e))
            yield return new object[] { 1.8900811645722220, 0.63661977236758134, DoubleCrossPlatformMachineEpsilon };       // expected:  (2 / pi)
            yield return new object[] { 2.0, 0.69314718055994531, DoubleCrossPlatformMachineEpsilon };       // expected:  (ln(2))
            yield return new object[] { 2.0281149816474725, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon };       // expected:  (1 / sqrt(2))
            yield return new object[] { 2.1932800507380155, 0.78539816339744831, DoubleCrossPlatformMachineEpsilon };       // expected:  (pi / 4)
            yield return new object[] { 2.7182818284590452, 1.0, DoubleCrossPlatformMachineEpsilon * 10 };  //                              value: (e)
            yield return new object[] { 3.0906430223107976, 1.1283791670955126, DoubleCrossPlatformMachineEpsilon * 10 };  // expected:  (2 / sqrt(pi))
            yield return new object[] { 4.1132503787829275, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10 };  // expected:  (sqrt(2))
            yield return new object[] { 4.2320861065570819, 1.4426950408889634, DoubleCrossPlatformMachineEpsilon * 10 };  // expected:  (log2(e))
            yield return new object[] { 4.8104773809653517, 1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10 };  // expected:  (pi / 2)
            yield return new object[] { 10.0, 2.3025850929940457, DoubleCrossPlatformMachineEpsilon * 10 };  // expected:  (ln(10))
            yield return new object[] { 15.154262241479264, 2.7182818284590452, DoubleCrossPlatformMachineEpsilon * 10 };  // expected:  (e)
            yield return new object[] { 23.140692632779269, 3.1415926535897932, DoubleCrossPlatformMachineEpsilon * 10 };  // expected:  (pi)
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, 0.0 };
        }
    }

    public static IEnumerable<object[]> Log2Double
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NaN, 0.0 };
            yield return new object[] { -0.11331473229676087, double.NaN, 0.0 };
            yield return new object[] { -0.0, double.NegativeInfinity, 0.0 };
            yield return new object[] { double.NaN, double.NaN, 0.0 };
            yield return new object[] { 0.0, double.NegativeInfinity, 0.0 };
            yield return new object[] { 0.11331473229676087, -3.1415926535897932, DoubleCrossPlatformMachineEpsilon * 10 };    // expected: -(pi)
            yield return new object[] { 0.15195522325791297, -2.7182818284590453, DoubleCrossPlatformMachineEpsilon * 10 };    // expected: -(e)
            yield return new object[] { 0.20269956628651730, -2.3025850929940460, DoubleCrossPlatformMachineEpsilon * 10 };    // expected: -(ln(10))
            yield return new object[] { 0.33662253682241906, -1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10 };    // expected: -(pi / 2)
            yield return new object[] { 0.36787944117144232, -1.4426950408889634, DoubleCrossPlatformMachineEpsilon * 10 };    // expected: -(log2(e))
            yield return new object[] { 0.37521422724648177, -1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10 };    // expected: -(sqrt(2))
            yield return new object[] { 0.45742934732229695, -1.1283791670955126, DoubleCrossPlatformMachineEpsilon * 10 };    // expected: -(2 / sqrt(pi))
            yield return new object[] { 0.5, -1.0, 0.0f };
            yield return new object[] { 0.58019181037172444, -0.78539816339744840, DoubleCrossPlatformMachineEpsilon };         // expected: -(pi / 4)
            yield return new object[] { 0.61254732653606592, -0.70710678118654750, DoubleCrossPlatformMachineEpsilon };         // expected: -(1 / sqrt(2))
            yield return new object[] { 0.61850313780157598, -0.69314718055994537, DoubleCrossPlatformMachineEpsilon };         // expected: -(ln(2))
            yield return new object[] { 0.64321824193300488, -0.63661977236758126, DoubleCrossPlatformMachineEpsilon };         // expected: -(2 / pi)
            yield return new object[] { 0.74005557395545179, -0.43429448190325190, DoubleCrossPlatformMachineEpsilon };         // expected: -(log10(e))
            yield return new object[] { 0.80200887896145195, -0.31830988618379073, DoubleCrossPlatformMachineEpsilon };         // expected: -(1 / pi)
            yield return new object[] { 1, 0.0, 0.0 };
            yield return new object[] { 1.2468689889006383, 0.31830988618379073, DoubleCrossPlatformMachineEpsilon };         // expected:  (1 / pi)
            yield return new object[] { 1.3512498725672678, 0.43429448190325226, DoubleCrossPlatformMachineEpsilon };         // expected:  (log10(e))
            yield return new object[] { 1.5546822754821001, 0.63661977236758126, DoubleCrossPlatformMachineEpsilon };         // expected:  (2 / pi)
            yield return new object[] { 1.6168066722416747, 0.69314718055994537, DoubleCrossPlatformMachineEpsilon };         // expected:  (ln(2))
            yield return new object[] { 1.6325269194381528, 0.70710678118654750, DoubleCrossPlatformMachineEpsilon };         // expected:  (1 / sqrt(2))
            yield return new object[] { 1.7235679341273495, 0.78539816339744830, DoubleCrossPlatformMachineEpsilon };         // expected:  (pi / 4)
            yield return new object[] { 2, 1.0, 0.0 };                                       //                              value: (e)
            yield return new object[] { 2.1861299583286618, 1.1283791670955128, DoubleCrossPlatformMachineEpsilon * 10 };    // expected:  (2 / sqrt(pi))
            yield return new object[] { 2.6651441426902252, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10 };    // expected:  (sqrt(2))
            yield return new object[] { 2.7182818284590452, 1.4426950408889632, DoubleCrossPlatformMachineEpsilon * 10 };    // expected:  (log2(e))
            yield return new object[] { 2.9706864235520193, 1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10 };    // expected:  (pi / 2)
            yield return new object[] { 4.9334096679145963, 2.3025850929940460, DoubleCrossPlatformMachineEpsilon * 10 };    // expected:  (ln(10))
            yield return new object[] { 6.5808859910179210, 2.7182818284590455, DoubleCrossPlatformMachineEpsilon * 10 };    // expected:  (e)
            yield return new object[] { 8.8249778270762876, 3.1415926535897932, DoubleCrossPlatformMachineEpsilon * 10 };    // expected:  (pi)
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, 0.0 };
        }
    }

    public static IEnumerable<object[]> MaxDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MaxValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MaxValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, double.NaN };
            yield return new object[] { 1.0f, double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NaN };
            yield return new object[] { -0.0f, 0.0f, 0.0f };
            yield return new object[] { 0.0f, -0.0f, 0.0f };
            yield return new object[] { 2.0f, -3.0f, 2.0f };
            yield return new object[] { -3.0f, 2.0f, 2.0f };
            yield return new object[] { 3.0f, -2.0f, 3.0f };
            yield return new object[] { -2.0f, 3.0f, 3.0f };
        }
    }

    public static IEnumerable<object[]> MaxMagnitudeDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MaxValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MaxValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, double.NaN };
            yield return new object[] { 1.0f, double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NaN };
            yield return new object[] { -0.0f, 0.0f, 0.0f };
            yield return new object[] { 0.0f, -0.0f, 0.0f };
            yield return new object[] { 2.0f, -3.0f, -3.0f };
            yield return new object[] { -3.0f, 2.0f, -3.0f };
            yield return new object[] { 3.0f, -2.0f, 3.0f };
            yield return new object[] { -2.0f, 3.0f, 3.0f };
        }
    }

    public static IEnumerable<object[]> MaxMagnitudeNumberDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MaxValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MaxValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, 1.0f };
            yield return new object[] { 1.0f, double.NaN, 1.0f };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NegativeInfinity };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { -0.0f, 0.0f, 0.0f };
            yield return new object[] { 0.0f, -0.0f, 0.0f };
            yield return new object[] { 2.0f, -3.0f, -3.0f };
            yield return new object[] { -3.0f, 2.0f, -3.0f };
            yield return new object[] { 3.0f, -2.0f, 3.0f };
            yield return new object[] { -2.0f, 3.0f, 3.0f };
        }
    }

    public static IEnumerable<object[]> MaxNumberDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MaxValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MaxValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, 1.0f };
            yield return new object[] { 1.0f, double.NaN, 1.0f };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NegativeInfinity };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { -0.0f, 0.0f, 0.0f };
            yield return new object[] { 0.0f, -0.0f, 0.0f };
            yield return new object[] { 2.0f, -3.0f, 2.0f };
            yield return new object[] { -3.0f, 2.0f, 2.0f };
            yield return new object[] { 3.0f, -2.0f, 3.0f };
            yield return new object[] { -2.0f, 3.0f, 3.0f };
        }
    }

    public static IEnumerable<object[]> MinDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MinValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MinValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, double.NaN };
            yield return new object[] { 1.0f, double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NaN };
            yield return new object[] { -0.0f, 0.0f, -0.0f };
            yield return new object[] { 0.0f, -0.0f, -0.0f };
            yield return new object[] { 2.0f, -3.0f, -3.0f };
            yield return new object[] { -3.0f, 2.0f, -3.0f };
            yield return new object[] { 3.0f, -2.0f, -2.0f };
            yield return new object[] { -2.0f, 3.0f, -2.0f };
        }
    }

    public static IEnumerable<object[]> MinMagnitudeDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MinValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MinValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, double.NaN };
            yield return new object[] { 1.0f, double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NaN };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.NaN };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NaN };
            yield return new object[] { -0.0f, 0.0f, -0.0f };
            yield return new object[] { 0.0f, -0.0f, -0.0f };
            yield return new object[] { 2.0f, -3.0f, 2.0f };
            yield return new object[] { -3.0f, 2.0f, 2.0f };
            yield return new object[] { 3.0f, -2.0f, -2.0f };
            yield return new object[] { -2.0f, 3.0f, -2.0f };
        }
    }

    public static IEnumerable<object[]> MinMagnitudeNumberDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MinValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MinValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, 1.0f };
            yield return new object[] { 1.0f, double.NaN, 1.0f };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NegativeInfinity };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { -0.0f, 0.0f, -0.0f };
            yield return new object[] { 0.0f, -0.0f, -0.0f };
            yield return new object[] { 2.0f, -3.0f, 2.0f };
            yield return new object[] { -3.0f, 2.0f, 2.0f };
            yield return new object[] { 3.0f, -2.0f, -2.0f };
            yield return new object[] { -2.0f, 3.0f, -2.0f };
        }
    }

    public static IEnumerable<object[]> MinNumberDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { double.MinValue, double.MaxValue, double.MinValue };
            yield return new object[] { double.MaxValue, double.MinValue, double.MinValue };
            yield return new object[] { double.NaN, double.NaN, double.NaN };
            yield return new object[] { double.NaN, 1.0f, 1.0f };
            yield return new object[] { 1.0f, double.NaN, 1.0f };
            yield return new object[] { double.PositiveInfinity, double.NaN, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NegativeInfinity };
            yield return new object[] { double.NaN, double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.NaN, double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { -0.0f, 0.0f, -0.0f };
            yield return new object[] { 0.0f, -0.0f, -0.0f };
            yield return new object[] { 2.0f, -3.0f, -3.0f };
            yield return new object[] { -3.0f, 2.0f, -3.0f };
            yield return new object[] { 3.0f, -2.0f, -2.0f };
            yield return new object[] { -2.0f, 3.0f, -2.0f };
        }
    }

    public static IEnumerable<object[]> RadiansToDegreesDouble
    {
        get
        {
            yield return new object[] { double.NaN, double.NaN, 0.0 };
            yield return new object[] { 0.0, 0.0, 0.0 };
            yield return new object[] { 0.0055555555555555567, 0.3183098861837906, DoubleCrossPlatformMachineEpsilon };       // expected:  (1 / pi)
            yield return new object[] { 0.0075798686324546743, 0.4342944819032518, DoubleCrossPlatformMachineEpsilon };       // expected:  (log10(e))
            yield return new object[] { 0.008726646259971648, 0.5, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 0.0111111111111111124, 0.6366197723675813, DoubleCrossPlatformMachineEpsilon };       // expected:  (2 / pi)
            yield return new object[] { 0.0120977005016866801, 0.6931471805599453, DoubleCrossPlatformMachineEpsilon };       // expected:  (ln(2))
            yield return new object[] { 0.0123413414948843512, 0.7071067811865475, DoubleCrossPlatformMachineEpsilon };       // expected:  (1 / sqrt(2))
            yield return new object[] { 0.0137077838904018851, 0.7853981633974483, DoubleCrossPlatformMachineEpsilon };       // expected:  (pi / 4)
            yield return new object[] { 0.017453292519943295, 1.0, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 0.019693931676727953, 1.1283791670955126, DoubleCrossPlatformMachineEpsilon };       // expected:  (2 / sqrt(pi))
            yield return new object[] { 0.024682682989768702, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon };       // expected:  (sqrt(2))
            yield return new object[] { 0.025179778565706630, 1.4426950408889634, DoubleCrossPlatformMachineEpsilon };       // expected:  (log2(e))
            yield return new object[] { 0.026179938779914940, 1.5, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 0.027415567780803770, 1.5707963267948966, DoubleCrossPlatformMachineEpsilon };       // expected:  (pi / 2)
            yield return new object[] { 0.034906585039886590, 2.0, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 0.040187691180085916, 2.3025850929940457, DoubleCrossPlatformMachineEpsilon };       // expected:  (ln(10))
            yield return new object[] { 0.043633231299858240, 2.5, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 0.047442967903742035, 2.7182818284590452, DoubleCrossPlatformMachineEpsilon };       // expected:  (e)
            yield return new object[] { 0.052359877559829880, 3.0, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 0.054831135561607540, 3.1415926535897932, DoubleCrossPlatformMachineEpsilon };       // expected:  (pi)
            yield return new object[] { 0.061086523819801536, 3.5, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity, 0.0 };
        }
    }

    public static IEnumerable<object[]> RoundDouble
    {
        get
        {
            yield return new object[] { 0.0, 0.0 };
            yield return new object[] { 1.4, 1.0 };
            yield return new object[] { 1.5, 2.0 };
            yield return new object[] { 2e7, 2e7 };
            yield return new object[] { -0.0, -0.0 };
            yield return new object[] { -1.4, -1.0 };
            yield return new object[] { -1.5, -2.0 };
            yield return new object[] { -2e7, -2e7 };
        }
    }

    public static IEnumerable<object[]> RoundAwayFromZeroDouble
    {
        get
        {
            yield return new object[] { 1, 1 };
            yield return new object[] { 0.5, 1 };
            yield return new object[] { 1.5, 2 };
            yield return new object[] { 2.5, 3 };
            yield return new object[] { 3.5, 4 };
            yield return new object[] { 0.49999999999999994, 0 };
            yield return new object[] { 1.5, 2 };
            yield return new object[] { 2.5, 3 };
            yield return new object[] { 3.5, 4 };
            yield return new object[] { 4.5, 5 };
            yield return new object[] { 3.141592653589793, 3 };
            yield return new object[] { 2.718281828459045, 3 };
            yield return new object[] { 1385.4557313670111, 1385 };
            yield return new object[] { 3423423.43432, 3423423 };
            yield return new object[] { 535345.5, 535346 };
            yield return new object[] { 535345.50001, 535346 };
            yield return new object[] { 535345.5, 535346 };
            yield return new object[] { 535345.4, 535345 };
            yield return new object[] { 535345.6, 535346 };
            yield return new object[] { -2.718281828459045, -3 };
            yield return new object[] { 10, 10 };
            yield return new object[] { -10, -10 };
            yield return new object[] { -0, -0 };
            yield return new object[] { 0, 0 };
            yield return new object[] { double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { 1.7976931348623157E+308, 1.7976931348623157E+308 };
            yield return new object[] { -1.7976931348623157E+308, -1.7976931348623157E+308 };
        }
    }

    public static IEnumerable<object[]> RoundToEvenDouble
    {
        get
        {
            yield return new object[] { 1, 1 };
            yield return new object[] { 0.5, 0 };
            yield return new object[] { 1.5, 2 };
            yield return new object[] { 2.5, 2 };
            yield return new object[] { 3.5, 4 };
            yield return new object[] { 1.5, 2 };
            yield return new object[] { 2.5, 2 };
            yield return new object[] { 3.5, 4 };
            yield return new object[] { 4.5, 4 };
            yield return new object[] { 3.141592653589793, 3 };
            yield return new object[] { 2.718281828459045, 3 };
            yield return new object[] { 1385.4557313670111, 1385 };
            yield return new object[] { 3423423.43432, 3423423 };
            yield return new object[] { 535345.5, 535346 };
            yield return new object[] { 535345.50001, 535346 };
            yield return new object[] { 535345.5, 535346 };
            yield return new object[] { 535345.4, 535345 };
            yield return new object[] { 535345.6, 535346 };
            yield return new object[] { -2.718281828459045, -3 };
            yield return new object[] { 10, 10 };
            yield return new object[] { -10, -10 };
            yield return new object[] { -0, -0 };
            yield return new object[] { 0, 0 };
            yield return new object[] { double.NaN, double.NaN };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity };
            yield return new object[] { 1.7976931348623157E+308, 1.7976931348623157E+308 };
            yield return new object[] { -1.7976931348623157E+308, -1.7976931348623157E+308 };
        }
    }

    public static IEnumerable<object[]> SinDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NaN, 0.0 };
            yield return new object[] { -3.1415926535897932, -0.0, DoubleCrossPlatformMachineEpsilon };      // value: -(pi)
            yield return new object[] { -2.7182818284590452, -0.41078129050290870, DoubleCrossPlatformMachineEpsilon };      // value: -(e)
            yield return new object[] { -2.3025850929940457, -0.74398033695749319, DoubleCrossPlatformMachineEpsilon };      // value: -(ln(10))
            yield return new object[] { -1.5707963267948966, -1.0, DoubleCrossPlatformMachineEpsilon * 10 }; // value: -(pi / 2)
            yield return new object[] { -1.4426950408889634, -0.99180624439366372, DoubleCrossPlatformMachineEpsilon };      // value: -(log2(e))
            yield return new object[] { -1.4142135623730950, -0.98776594599273553, DoubleCrossPlatformMachineEpsilon };      // value: -(sqrt(2))
            yield return new object[] { -1.1283791670955126, -0.90371945743584630, DoubleCrossPlatformMachineEpsilon };      // value: -(2 / sqrt(pi))
            yield return new object[] { -1.0, -0.84147098480789651, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { -0.78539816339744831, -0.70710678118654752, DoubleCrossPlatformMachineEpsilon };      // value: -(pi / 4),        expected: -(1 / sqrt(2))
            yield return new object[] { -0.70710678118654752, -0.64963693908006244, DoubleCrossPlatformMachineEpsilon };      // value: -(1 / sqrt(2))
            yield return new object[] { -0.69314718055994531, -0.63896127631363480, DoubleCrossPlatformMachineEpsilon };      // value: -(ln(2))
            yield return new object[] { -0.63661977236758134, -0.59448076852482208, DoubleCrossPlatformMachineEpsilon };      // value: -(2 / pi)
            yield return new object[] { -0.43429448190325183, -0.42077048331375735, DoubleCrossPlatformMachineEpsilon };      // value: -(log10(e))
            yield return new object[] { -0.31830988618379067, -0.31296179620778659, DoubleCrossPlatformMachineEpsilon };      // value: -(1 / pi)
            yield return new object[] { -0.0, -0.0, 0.0 };
            yield return new object[] { double.NaN, double.NaN, 0.0 };
            yield return new object[] { 0.0, 0.0, 0.0 };
            yield return new object[] { 0.31830988618379067, 0.31296179620778659, DoubleCrossPlatformMachineEpsilon };      // value:  (1 / pi)
            yield return new object[] { 0.43429448190325183, 0.42077048331375735, DoubleCrossPlatformMachineEpsilon };      // value:  (log10(e))
            yield return new object[] { 0.63661977236758134, 0.59448076852482208, DoubleCrossPlatformMachineEpsilon };      // value:  (2 / pi)
            yield return new object[] { 0.69314718055994531, 0.63896127631363480, DoubleCrossPlatformMachineEpsilon };      // value:  (ln(2))
            yield return new object[] { 0.70710678118654752, 0.64963693908006244, DoubleCrossPlatformMachineEpsilon };      // value:  (1 / sqrt(2))
            yield return new object[] { 0.78539816339744831, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon };      // value:  (pi / 4),        expected:  (1 / sqrt(2))
            yield return new object[] { 1.0, 0.84147098480789651, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 1.1283791670955126, 0.90371945743584630, DoubleCrossPlatformMachineEpsilon };      // value:  (2 / sqrt(pi))
            yield return new object[] { 1.4142135623730950, 0.98776594599273553, DoubleCrossPlatformMachineEpsilon };      // value:  (sqrt(2))
            yield return new object[] { 1.4426950408889634, 0.99180624439366372, DoubleCrossPlatformMachineEpsilon };      // value:  (log2(e))
            yield return new object[] { 1.5707963267948966, 1.0, DoubleCrossPlatformMachineEpsilon * 10 }; // value:  (pi / 2)
            yield return new object[] { 2.3025850929940457, 0.74398033695749319, DoubleCrossPlatformMachineEpsilon };      // value:  (ln(10))
            yield return new object[] { 2.7182818284590452, 0.41078129050290870, DoubleCrossPlatformMachineEpsilon };      // value:  (e)
            yield return new object[] { 3.1415926535897932, 0.0, DoubleCrossPlatformMachineEpsilon };      // value:  (pi)
            yield return new object[] { double.PositiveInfinity, double.NaN, 0.0 };
        }
    }

    public static IEnumerable<object[]> SinCosDouble
    {
        get
        {
            yield return new object[] { double.NegativeInfinity, double.NaN, double.NaN, 0.0, 0.0 };
            yield return new object[] { -1e18, 0.9929693207404051, 0.11837199021871073, 0.0002, 0.002 };                                  // https://github.com/dotnet/runtime/issues/98204
            yield return new object[] { -3.1415926535897932, -0.0, -1.0, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon * 10 }; // value: -(pi)
            yield return new object[] { -2.7182818284590452, -0.41078129050290870, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(e)
            yield return new object[] { -2.3025850929940457, -0.74398033695749319, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(ln(10))
            yield return new object[] { -1.5707963267948966, -1.0, 0.0, DoubleCrossPlatformMachineEpsilon * 10, DoubleCrossPlatformMachineEpsilon };      // value: -(pi / 2)
            yield return new object[] { -1.4426950408889634, -0.99180624439366372, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(log2(e))
            yield return new object[] { -1.4142135623730950, -0.98776594599273553, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(sqrt(2))
            yield return new object[] { -1.1283791670955126, -0.90371945743584630, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(2 / sqrt(pi))
            yield return new object[] { -1.0, -0.84147098480789651, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { -0.78539816339744831, -0.70710678118654752, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(pi / 4),        expected_sin: -(1 / sqrt(2)),    expected_cos: 1
            yield return new object[] { -0.70710678118654752, -0.64963693908006244, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(1 / sqrt(2))
            yield return new object[] { -0.69314718055994531, -0.63896127631363480, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(ln(2))
            yield return new object[] { -0.63661977236758134, -0.59448076852482208, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(2 / pi)
            yield return new object[] { -0.43429448190325183, -0.42077048331375735, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(log10(e))
            yield return new object[] { -0.31830988618379067, -0.31296179620778659, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value: -(1 / pi)
            yield return new object[] { -0.0, -0.0, 1.0, 0.0, DoubleCrossPlatformMachineEpsilon * 10 };
            yield return new object[] { double.NaN, double.NaN, double.NaN, 0.0, 0.0 };
            yield return new object[] { 0.0, 0.0, 1.0, 0.0, DoubleCrossPlatformMachineEpsilon * 10 };
            yield return new object[] { 0.31830988618379067, 0.31296179620778659, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (1 / pi)
            yield return new object[] { 0.43429448190325183, 0.42077048331375735, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (log10(e))
            yield return new object[] { 0.63661977236758134, 0.59448076852482208, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (2 / pi)
            yield return new object[] { 0.69314718055994531, 0.63896127631363480, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (ln(2))
            yield return new object[] { 0.70710678118654752, 0.64963693908006244, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (1 / sqrt(2))
            yield return new object[] { 0.78539816339744831, 0.70710678118654752, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (pi / 4),        expected_sin:  (1 / sqrt(2)),    expected_cos: 1
            yield return new object[] { 1.0, 0.84147098480789651, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };
            yield return new object[] { 1.1283791670955126, 0.90371945743584630, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (2 / sqrt(pi))
            yield return new object[] { 1.4142135623730950, 0.98776594599273553, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (sqrt(2))
            yield return new object[] { 1.4426950408889634, 0.99180624439366372, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (log2(e))
            yield return new object[] { 1.5707963267948966, 1.0, 0.0, DoubleCrossPlatformMachineEpsilon * 10, DoubleCrossPlatformMachineEpsilon };      // value:  (pi / 2)
            yield return new object[] { 2.3025850929940457, 0.74398033695749319, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (ln(10))
            yield return new object[] { 2.7182818284590452, 0.41078129050290870, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon };      // value:  (e)
            yield return new object[] { 3.1415926535897932, 0.0, -1.0, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon * 10 }; // value:  (pi)
            yield return new object[] { 1e18, -0.9929693207404051, 0.11837199021871073, 0.0002, 0.002 };                                  // https://github.com/dotnet/runtime/issues/98204
            yield return new object[] { double.PositiveInfinity, double.NaN, double.NaN, 0.0, 0.0 };
        }
    }

    public static IEnumerable<object[]> TruncateDouble
    {
        get
        {
            yield return new object[] { 0.12345, 0.0f };
            yield return new object[] { 3.14159, 3.0f };
            yield return new object[] { -3.14159, -3.0f };
        }
    }
}