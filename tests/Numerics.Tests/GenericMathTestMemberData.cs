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

    public static IEnumerable<Func<(double, double, double, double)>> ClampDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, 1.0, 63.0, 1.0);
            yield return () => (double.MinValue, 1.0, 63.0, 1.0);
            yield return () => (-1.0, 1.0, 63.0, 1.0);
            yield return () => (-MinNormalDouble, 1.0, 63.0, 1.0);
            yield return () => (-MaxSubnormalDouble, 1.0, 63.0, 1.0);
            yield return () => (-double.Epsilon, 1.0, 63.0, 1.0);
            yield return () => (double.NegativeZero, 1.0, 63.0, 1.0);
            yield return () => (double.NaN, 1.0, 63.0, double.NaN);
            yield return () => (default, 1.0, 63.0, 1.0);
            yield return () => (double.Epsilon, 1.0, 63.0, 1.0);
            yield return () => (MaxSubnormalDouble, 1.0, 63.0, 1.0);
            yield return () => (MinNormalDouble, 1.0, 63.0, 1.0);
            yield return () => (1.0, 1.0, 63.0, 1.0);
            yield return () => (double.MaxValue, 1.0, 63.0, 63.0);
            yield return () => (double.PositiveInfinity, 1.0, 63.0, 63.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> CopySignDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.NegativeInfinity, -double.Pi, double.NegativeInfinity);
            yield return () => (double.NegativeInfinity, double.NegativeZero, double.NegativeInfinity);
            yield return () => (double.NegativeInfinity, double.NaN, double.NegativeInfinity);
            yield return () => (double.NegativeInfinity, default, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.Pi, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (-double.Pi, double.NegativeInfinity, -double.Pi);
            yield return () => (-double.Pi, -double.Pi, -double.Pi);
            yield return () => (-double.Pi, double.NegativeZero, -double.Pi);
            yield return () => (-double.Pi, double.NaN, -double.Pi);
            yield return () => (-double.Pi, default, double.Pi);
            yield return () => (-double.Pi, double.Pi, double.Pi);
            yield return () => (-double.Pi, double.PositiveInfinity, double.Pi);
            yield return () => (double.NegativeZero, double.NegativeInfinity, double.NegativeZero);
            yield return () => (double.NegativeZero, -double.Pi, double.NegativeZero);
            yield return () => (double.NegativeZero, double.NegativeZero, double.NegativeZero);
            yield return () => (double.NegativeZero, double.NaN, double.NegativeZero);
            yield return () => (double.NegativeZero, default, default);
            yield return () => (double.NegativeZero, double.Pi, default);
            yield return () => (double.NegativeZero, double.PositiveInfinity, default);
            yield return () => (double.NaN, double.NegativeInfinity, double.NaN);
            yield return () => (double.NaN, -double.Pi, double.NaN);
            yield return () => (double.NaN, double.NegativeZero, double.NaN);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, default, double.NaN);
            yield return () => (double.NaN, double.Pi, double.NaN);
            yield return () => (double.NaN, double.PositiveInfinity, double.NaN);
            yield return () => (default, double.NegativeInfinity, double.NegativeZero);
            yield return () => (default, -double.Pi, double.NegativeZero);
            yield return () => (default, double.NegativeZero, double.NegativeZero);
            yield return () => (default, double.NaN, double.NegativeZero);
            yield return () => (default, default, default);
            yield return () => (default, double.Pi, default);
            yield return () => (default, double.PositiveInfinity, default);
            yield return () => (double.Pi, double.NegativeInfinity, -double.Pi);
            yield return () => (double.Pi, -double.Pi, -double.Pi);
            yield return () => (double.Pi, double.NegativeZero, -double.Pi);
            yield return () => (double.Pi, double.NaN, -double.Pi);
            yield return () => (double.Pi, default, double.Pi);
            yield return () => (double.Pi, double.Pi, double.Pi);
            yield return () => (double.Pi, double.PositiveInfinity, double.Pi);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, -double.Pi, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeZero, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, double.NaN, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, default, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, double.Pi, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        }
    }



    public static IEnumerable<Func<(double, double, double)>> CosDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NaN, default);
            yield return () => (-double.Pi, -1.0, DoubleCrossPlatformMachineEpsilon * 10); // value: -(pi)
            yield return () => (-2.7182818284590452, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon);      // value: -(e)
            yield return () => (-2.3025850929940457, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon);      // value: -(ln(10))
            yield return () => (-1.5707963267948966, default, DoubleCrossPlatformMachineEpsilon);      // value: -(pi / 2)
            yield return () => (-1.4426950408889634, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon);      // value: -(log2(e))
            yield return () => (-1.4142135623730950, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon);      // value: -(sqrt(2))
            yield return () => (-1.1283791670955126, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon);      // value: -(2 / sqrt(pi))
            yield return () => (-1.0, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon);
            yield return () => (-0.78539816339744831, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon);      // value: -(pi / 4),        expected:  (1 / sqrt(2))
            yield return () => (-0.70710678118654752, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon);      // value: -(1 / sqrt(2))
            yield return () => (-0.69314718055994531, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon);      // value: -(ln(2))
            yield return () => (-0.63661977236758134, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon);      // value: -(2 / pi)
            yield return () => (-0.43429448190325183, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon);      // value: -(log10(e))
            yield return () => (-0.31830988618379067, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon);      // value: -(1 / pi)
            yield return () => (double.NegativeZero, 1.0, DoubleCrossPlatformMachineEpsilon * 10);
            yield return () => (double.NaN, double.NaN, default);
            yield return () => (default, 1.0, DoubleCrossPlatformMachineEpsilon * 10);
            yield return () => (0.31830988618379067, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon);      // value:  (1 / pi)
            yield return () => (0.43429448190325183, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon);      // value:  (log10(e))
            yield return () => (0.63661977236758134, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon);      // value:  (2 / pi)
            yield return () => (0.69314718055994531, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon);      // value:  (ln(2))
            yield return () => (0.70710678118654752, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon);      // value:  (1 / sqrt(2))
            yield return () => (0.78539816339744831, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon);      // value:  (pi / 4),        expected:  (1 / sqrt(2))
            yield return () => (1.0, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon);
            yield return () => (1.1283791670955126, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon);      // value:  (2 / sqrt(pi))
            yield return () => (1.4142135623730950, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon);      // value:  (sqrt(2))
            yield return () => (1.4426950408889634, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon);      // value:  (log2(e))
            yield return () => (1.5707963267948966, default, DoubleCrossPlatformMachineEpsilon);      // value:  (pi / 2)
            yield return () => (2.3025850929940457, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon);      // value:  (ln(10))
            yield return () => (2.7182818284590452, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon);      // value:  (e)
            yield return () => (double.Pi, -1.0, DoubleCrossPlatformMachineEpsilon * 10); // value:  (pi)
            yield return () => (double.PositiveInfinity, double.NaN, default);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> DegreesToRadiansDouble
    {
        get
        {
            yield return () => (double.NaN, double.NaN, default);
            yield return () => (default, default, default);
            yield return () => (0.31830988618379067, 0.005555555555555556, DoubleCrossPlatformMachineEpsilon);       // value:  (1 / pi)
            yield return () => (0.43429448190325183, 0.007579868632454674, DoubleCrossPlatformMachineEpsilon);       // value:  (log10(e))
            yield return () => (0.5, 0.008726646259971648, DoubleCrossPlatformMachineEpsilon);
            yield return () => (0.63661977236758134, 0.011111111111111112, DoubleCrossPlatformMachineEpsilon);       // value:  (2 / pi)
            yield return () => (0.69314718055994531, 0.01209770050168668, DoubleCrossPlatformMachineEpsilon);       // value:  (ln(2))
            yield return () => (0.70710678118654752, 0.012341341494884351, DoubleCrossPlatformMachineEpsilon);       // value:  (1 / sqrt(2))
            yield return () => (0.78539816339744831, 0.013707783890401885, DoubleCrossPlatformMachineEpsilon);       // value:  (pi / 4)
            yield return () => (1.0, 0.017453292519943295, DoubleCrossPlatformMachineEpsilon);
            yield return () => (1.1283791670955126, 0.019693931676727953, DoubleCrossPlatformMachineEpsilon);       // value:  (2 / sqrt(pi))
            yield return () => (1.4142135623730950, 0.024682682989768702, DoubleCrossPlatformMachineEpsilon);       // value:  (sqrt(2))
            yield return () => (1.4426950408889634, 0.02517977856570663, DoubleCrossPlatformMachineEpsilon);       // value:  (log2(e))
            yield return () => (1.5, 0.02617993877991494, DoubleCrossPlatformMachineEpsilon);
            yield return () => (1.5707963267948966, 0.02741556778080377, DoubleCrossPlatformMachineEpsilon);       // value:  (pi / 2)
            yield return () => (2.0, 0.03490658503988659, DoubleCrossPlatformMachineEpsilon);
            yield return () => (2.3025850929940457, 0.040187691180085916, DoubleCrossPlatformMachineEpsilon);       // value:  (ln(10))
            yield return () => (2.5, 0.04363323129985824, DoubleCrossPlatformMachineEpsilon);
            yield return () => (2.7182818284590452, 0.047442967903742035, DoubleCrossPlatformMachineEpsilon);       // value:  (e)
            yield return () => (3.0, 0.05235987755982988, DoubleCrossPlatformMachineEpsilon);
            yield return () => (double.Pi, 0.05483113556160754, DoubleCrossPlatformMachineEpsilon);       // value:  (pi)
            yield return () => (3.5, 0.061086523819801536, DoubleCrossPlatformMachineEpsilon);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, default);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> ExpDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, default, default);
            yield return () => (-double.Pi, 0.043213918263772250, DoubleCrossPlatformMachineEpsilon / 10);   // value: -(pi)
            yield return () => (-2.7182818284590452, 0.065988035845312537, DoubleCrossPlatformMachineEpsilon / 10);   // value: -(e)
            yield return () => (-2.3025850929940457, 0.1, DoubleCrossPlatformMachineEpsilon);        // value: -(ln(10))
            yield return () => (-1.5707963267948966, 0.20787957635076191, DoubleCrossPlatformMachineEpsilon);        // value: -(pi / 2)
            yield return () => (-1.4426950408889634, 0.23629008834452270, DoubleCrossPlatformMachineEpsilon);        // value: -(log2(e))
            yield return () => (-1.4142135623730950, 0.24311673443421421, DoubleCrossPlatformMachineEpsilon);        // value: -(sqrt(2))
            yield return () => (-1.1283791670955126, 0.32355726390307110, DoubleCrossPlatformMachineEpsilon);        // value: -(2 / sqrt(pi))
            yield return () => (-1.0, 0.36787944117144232, DoubleCrossPlatformMachineEpsilon);
            yield return () => (-0.78539816339744831, 0.45593812776599624, DoubleCrossPlatformMachineEpsilon);        // value: -(pi / 4)
            yield return () => (-0.70710678118654752, 0.49306869139523979, DoubleCrossPlatformMachineEpsilon);        // value: -(1 / sqrt(2))
            yield return () => (-0.69314718055994531, 0.5, default);                                     // value: -(ln(2))
            yield return () => (-0.63661977236758134, 0.52907780826773535, DoubleCrossPlatformMachineEpsilon);        // value: -(2 / pi)
            yield return () => (-0.43429448190325183, 0.64772148514180065, DoubleCrossPlatformMachineEpsilon);        // value: -(log10(e))
            yield return () => (-0.31830988618379067, 0.72737734929521647, DoubleCrossPlatformMachineEpsilon);        // value: -(1 / pi)
            yield return () => (double.NegativeZero, 1.0, default);
            yield return () => (double.NaN, double.NaN, default);
            yield return () => (default, 1.0, default);
            yield return () => (0.31830988618379067, 1.3748022274393586, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (1 / pi)
            yield return () => (0.43429448190325183, 1.5438734439711811, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (log10(e))
            yield return () => (0.63661977236758134, 1.8900811645722220, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (2 / pi)
            yield return () => (0.69314718055994531, 2.0, default);                                      // value:  (ln(2))
            yield return () => (0.70710678118654752, 2.0281149816474725, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (1 / sqrt(2))
            yield return () => (0.78539816339744831, 2.1932800507380155, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (pi / 4)
            yield return () => (1.0, 2.7182818284590452, DoubleCrossPlatformMachineEpsilon * 10);   //                          expected: (e)
            yield return () => (1.1283791670955126, 3.0906430223107976, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (2 / sqrt(pi))
            yield return () => (1.4142135623730950, 4.1132503787829275, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (sqrt(2))
            yield return () => (1.4426950408889634, 4.2320861065570819, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (log2(e))
            yield return () => (1.5707963267948966, 4.8104773809653517, DoubleCrossPlatformMachineEpsilon * 10);   // value:  (pi / 2)
            yield return () => (2.3025850929940457, 10.0, DoubleCrossPlatformMachineEpsilon * 10);                                      // value:  (ln(10))
            yield return () => (2.7182818284590452, 15.154262241479264, DoubleCrossPlatformMachineEpsilon * 100);  // value:  (e)
            yield return () => (double.Pi, 23.140692632779269, DoubleCrossPlatformMachineEpsilon * 100);  // value:  (pi)
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, default);
        }
    }

    public static IEnumerable<Func<(double, double, double, double)>> FusedMultiplyAddDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeInfinity, double.NegativeZero, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeInfinity, double.NegativeZero, -double.Pi, double.NaN);
            yield return () => (double.NegativeInfinity, double.NegativeZero, double.NegativeZero, double.NaN);
            yield return () => (double.NegativeInfinity, double.NegativeZero, double.NaN, double.NaN);
            yield return () => (double.NegativeInfinity, double.NegativeZero, default, double.NaN);
            yield return () => (double.NegativeInfinity, double.NegativeZero, double.Pi, double.NaN);
            yield return () => (double.NegativeInfinity, double.NegativeZero, double.PositiveInfinity, double.NaN);
            yield return () => (double.NegativeInfinity, default, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeInfinity, default, -double.Pi, double.NaN);
            yield return () => (double.NegativeInfinity, default, double.NegativeZero, double.NaN);
            yield return () => (double.NegativeInfinity, default, double.NaN, double.NaN);
            yield return () => (double.NegativeInfinity, default, default, double.NaN);
            yield return () => (double.NegativeInfinity, default, double.Pi, double.NaN);
            yield return () => (double.NegativeInfinity, default, double.PositiveInfinity, double.NaN);
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity, double.NaN);
            yield return () => (-1e308, 2.0, 1e308, -1e308);
            yield return () => (-1e308, 2.0, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (-5, 4, -3, -23);
            yield return () => (double.NegativeZero, double.NegativeInfinity, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeZero, double.NegativeInfinity, -double.Pi, double.NaN);
            yield return () => (double.NegativeZero, double.NegativeInfinity, double.NegativeZero, double.NaN);
            yield return () => (double.NegativeZero, double.NegativeInfinity, double.NaN, double.NaN);
            yield return () => (double.NegativeZero, double.NegativeInfinity, default, double.NaN);
            yield return () => (double.NegativeZero, double.NegativeInfinity, double.Pi, double.NaN);
            yield return () => (double.NegativeZero, double.NegativeInfinity, double.PositiveInfinity, double.NaN);
            yield return () => (double.NegativeZero, double.PositiveInfinity, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeZero, double.PositiveInfinity, -double.Pi, double.NaN);
            yield return () => (double.NegativeZero, double.PositiveInfinity, double.NegativeZero, double.NaN);
            yield return () => (double.NegativeZero, double.PositiveInfinity, double.NaN, double.NaN);
            yield return () => (double.NegativeZero, double.PositiveInfinity, default, double.NaN);
            yield return () => (double.NegativeZero, double.PositiveInfinity, double.Pi, double.NaN);
            yield return () => (double.NegativeZero, double.PositiveInfinity, double.PositiveInfinity, double.NaN);
            yield return () => (default, double.NegativeInfinity, double.NegativeInfinity, double.NaN);
            yield return () => (default, double.NegativeInfinity, -double.Pi, double.NaN);
            yield return () => (default, double.NegativeInfinity, double.NegativeZero, double.NaN);
            yield return () => (default, double.NegativeInfinity, double.NaN, double.NaN);
            yield return () => (default, double.NegativeInfinity, default, double.NaN);
            yield return () => (default, double.NegativeInfinity, double.Pi, double.NaN);
            yield return () => (default, double.NegativeInfinity, double.PositiveInfinity, double.NaN);
            yield return () => (default, double.PositiveInfinity, double.NegativeInfinity, double.NaN);
            yield return () => (default, double.PositiveInfinity, -double.Pi, double.NaN);
            yield return () => (default, double.PositiveInfinity, double.NegativeZero, double.NaN);
            yield return () => (default, double.PositiveInfinity, double.NaN, double.NaN);
            yield return () => (default, double.PositiveInfinity, default, double.NaN);
            yield return () => (default, double.PositiveInfinity, double.Pi, double.NaN);
            yield return () => (default, double.PositiveInfinity, double.PositiveInfinity, double.NaN);
            yield return () => (5, 4, 3, 23);
            yield return () => (1e308, 2.0, -1e308, 1e308);
            yield return () => (1e308, 2.0, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeZero, double.NegativeInfinity, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeZero, -double.Pi, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeZero, double.NegativeZero, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeZero, double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeZero, default, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeZero, double.Pi, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeZero, double.PositiveInfinity, double.NaN);
            yield return () => (double.PositiveInfinity, default, double.NegativeInfinity, double.NaN);
            yield return () => (double.PositiveInfinity, default, -double.Pi, double.NaN);
            yield return () => (double.PositiveInfinity, default, double.NegativeZero, double.NaN);
            yield return () => (double.PositiveInfinity, default, double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, default, default, double.NaN);
            yield return () => (double.PositiveInfinity, default, double.Pi, double.NaN);
            yield return () => (double.PositiveInfinity, default, double.PositiveInfinity, double.NaN);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NaN);
        }
    }

    public static IEnumerable<Func<(double, double, double, double)>> HypotDouble
    {
        get
        {
            yield return () => (double.NaN, double.NaN, double.NaN, default);
            yield return () => (double.NaN, default, double.NaN, default);
            yield return () => (double.NaN, 1.0, double.NaN, default);
            yield return () => (double.NaN, 2.7182818284590452, double.NaN, default);
            yield return () => (double.NaN, 10.0, double.NaN, default);
            yield return () => (default, default, default, default);
            yield return () => (default, 1.0, 1.0, default);
            yield return () => (default, 1.5707963267948966, 1.5707963267948966, default);
            yield return () => (default, 2.0, 2.0, default);
            yield return () => (default, 2.7182818284590452, 2.7182818284590452, default);
            yield return () => (default, 3.0, 3.0, default);
            yield return () => (default, 10.0, 10.0, default);
            yield return () => (1.0, 1.0, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10);
            yield return () => (1.0, 1e+10, 1e+10, default); // dotnet/runtime#75651
            yield return () => (1.0, 1e+20, 1e+20, default); // dotnet/runtime#75651
            yield return () => (2.7182818284590452, 0.31830988618379067, 2.7368553638387594, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (1 / pi)
            yield return () => (2.7182818284590452, 0.43429448190325183, 2.7527563996732919, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (log10(e))
            yield return () => (2.7182818284590452, 0.63661977236758134, 2.7918346715914253, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (2 / pi)
            yield return () => (2.7182818284590452, 0.69314718055994531, 2.8052645352709344, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (ln(2))
            yield return () => (2.7182818284590452, 0.70710678118654752, 2.8087463571726533, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (1 / sqrt(2))
            yield return () => (2.7182818284590452, 0.78539816339744831, 2.8294710413783590, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (pi / 4)
            yield return () => (2.7182818284590452, 1.0, 2.8963867315900082, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)
            yield return () => (2.7182818284590452, 1.1283791670955126, 2.9431778138036127, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (2 / sqrt(pi))
            yield return () => (2.7182818284590452, 1.4142135623730950, 3.0641566701020120, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (sqrt(2))
            yield return () => (2.7182818284590452, 1.4426950408889634, 3.0774055761202907, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (log2(e))
            yield return () => (2.7182818284590452, 1.5707963267948966, 3.1394995141268918, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (pi / 2)
            yield return () => (2.7182818284590452, 2.3025850929940457, 3.5624365551415857, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (ln(10))
            yield return () => (2.7182818284590452, 2.7182818284590452, 3.8442310281591168, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (e)
            yield return () => (2.7182818284590452, double.Pi, 4.1543544023133136, DoubleCrossPlatformMachineEpsilon * 10);   // x: (e)   y: (pi)
            yield return () => (10.0, 0.31830988618379067, 10.005064776584025, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (1 / pi)
            yield return () => (10.0, 0.43429448190325183, 10.009426142242702, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (log10(e))
            yield return () => (10.0, 0.63661977236758134, 10.020243746265325, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (2 / pi)
            yield return () => (10.0, 0.69314718055994531, 10.023993865417028, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (ln(2))
            yield return () => (10.0, 0.70710678118654752, 10.024968827881711, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (1 / sqrt(2))
            yield return () => (10.0, 0.78539816339744831, 10.030795096853892, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (pi / 4)
            yield return () => (10.0, 1.0, 10.049875621120890, DoubleCrossPlatformMachineEpsilon * 100);  //
            yield return () => (10.0, 1.1283791670955126, 10.063460614755501, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (2 / sqrt(pi))
            yield return () => (10.0, 1.4142135623730950, 10.099504938362078, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (sqrt(2))
            yield return () => (10.0, 1.4426950408889634, 10.103532500121213, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (log2(e))
            yield return () => (10.0, 1.5707963267948966, 10.122618292728040, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (pi / 2)
            yield return () => (10.0, 2.3025850929940457, 10.261671311754163, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (ln(10))
            yield return () => (10.0, 2.7182818284590452, 10.362869105558106, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (e)
            yield return () => (10.0, double.Pi, 10.481870272097884, DoubleCrossPlatformMachineEpsilon * 100);  //          y: (pi)
            yield return () => (double.PositiveInfinity, double.NaN, double.PositiveInfinity, default);
            yield return () => (double.PositiveInfinity, default, double.PositiveInfinity, default);
            yield return () => (double.PositiveInfinity, 1.0, double.PositiveInfinity, default);
            yield return () => (double.PositiveInfinity, 2.7182818284590452, double.PositiveInfinity, default);
            yield return () => (double.PositiveInfinity, 10.0, double.PositiveInfinity, default);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, default);
        }
    }

    public static IEnumerable<Func<double>> IsTestDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity);
            yield return () => (double.MinValue);
            yield return () => (-1.0);
            yield return () => (-MinNormalDouble);
            yield return () => (-MaxSubnormalDouble);
            yield return () => (-double.Epsilon);
            yield return () => (double.NegativeZero);
            yield return () => (double.NaN);
            yield return () => (default);
            yield return () => (double.Epsilon);
            yield return () => (MaxSubnormalDouble);
            yield return () => (MinNormalDouble);
            yield return () => (1.0);
            yield return () => (double.MaxValue);
            yield return () => (double.PositiveInfinity);
        }
    }

    public static IEnumerable<Func<(double, bool)>> IsNaNDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, false);
            yield return () => (double.MinValue, false);
            yield return () => (-MinNormalDouble, false);
            yield return () => (-MaxSubnormalDouble, false);
            yield return () => (-double.Epsilon, false);
            yield return () => (double.NegativeZero, false);
            yield return () => (double.NaN, true);
            yield return () => (default, false);
            yield return () => (double.Epsilon, false);
            yield return () => (MaxSubnormalDouble, false);
            yield return () => (MinNormalDouble, false);
            yield return () => (double.MaxValue, false);
            yield return () => (double.PositiveInfinity, false);
        }
    }

    public static IEnumerable<Func<(double, bool)>> IsNegativeDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, true);
            yield return () => (double.MinValue, true);
            yield return () => (-MinNormalDouble, true);
            yield return () => (-MaxSubnormalDouble, true);
            yield return () => (double.NegativeZero, true);
            yield return () => (double.NaN, true);
            yield return () => (default, false);
            yield return () => (MaxSubnormalDouble, false);
            yield return () => (MinNormalDouble, false);
            yield return () => (double.MaxValue, false);
            yield return () => (double.PositiveInfinity, false);
        }
    }

    public static IEnumerable<Func<(double, bool)>> IsPositiveDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, false);
            yield return () => (double.MinValue, false);
            yield return () => (-MinNormalDouble, false);
            yield return () => (-MaxSubnormalDouble, false);
            yield return () => (double.NegativeZero, false);
            yield return () => (double.NaN, false);
            yield return () => (default, true);
            yield return () => (MaxSubnormalDouble, true);
            yield return () => (MinNormalDouble, true);
            yield return () => (double.MaxValue, true);
            yield return () => (double.PositiveInfinity, true);
        }
    }

    public static IEnumerable<Func<(double, bool)>> IsPositiveInfinityDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, false);
            yield return () => (double.MinValue, false);
            yield return () => (-MinNormalDouble, false);
            yield return () => (-MaxSubnormalDouble, false);
            yield return () => (-double.Epsilon, false);
            yield return () => (double.NegativeZero, false);
            yield return () => (double.NaN, false);
            yield return () => (default, false);
            yield return () => (double.Epsilon, false);
            yield return () => (MaxSubnormalDouble, false);
            yield return () => (MinNormalDouble, false);
            yield return () => (double.MaxValue, false);
            yield return () => (double.PositiveInfinity, true);
        }
    }

    public static IEnumerable<Func<(double, bool)>> IsZeroDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, false);
            yield return () => (double.MinValue, false);
            yield return () => (-MinNormalDouble, false);
            yield return () => (-MaxSubnormalDouble, false);
            yield return () => (-double.Epsilon, false);
            yield return () => (double.NegativeZero, true);
            yield return () => (double.NaN, false);
            yield return () => (default, true);
            yield return () => (double.Epsilon, false);
            yield return () => (MaxSubnormalDouble, false);
            yield return () => (MinNormalDouble, false);
            yield return () => (double.MaxValue, false);
            yield return () => (double.PositiveInfinity, false);
        }
    }

    public static IEnumerable<Func<(double, double, double, double)>> LerpDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NegativeInfinity, 0.5, double.NegativeInfinity);
            yield return () => (double.NegativeInfinity, double.NaN, 0.5, double.NaN);
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, 0.5, double.NaN);
            yield return () => (double.NegativeInfinity, default, 0.5, double.NegativeInfinity);
            yield return () => (double.NegativeInfinity, 1.0, 0.5, double.NegativeInfinity);
            yield return () => (double.NaN, double.NegativeInfinity, 0.5, double.NaN);
            yield return () => (double.NaN, double.NaN, 0.5, double.NaN);
            yield return () => (double.NaN, double.PositiveInfinity, 0.5, double.NaN);
            yield return () => (double.NaN, default, 0.5, double.NaN);
            yield return () => (double.NaN, 1.0, 0.5, double.NaN);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, 0.5, double.NaN);
            yield return () => (double.PositiveInfinity, double.NaN, 0.5, double.NaN);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, 0.5, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, default, 0.5, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, 1.0, 0.5, double.PositiveInfinity);
            yield return () => (1.0, 3.0, default, 1.0);
            yield return () => (1.0, 3.0, 0.5, 2.0);
            yield return () => (1.0, 3.0, 1.0, 3.0);
            yield return () => (1.0, 3.0, 2.0, 5.0);
            yield return () => (2.0, 4.0, default, 2.0);
            yield return () => (2.0, 4.0, 0.5, 3.0);
            yield return () => (2.0, 4.0, 1.0, 4.0);
            yield return () => (2.0, 4.0, 2.0, 6.0);
            yield return () => (3.0, 1.0, default, 3.0);
            yield return () => (3.0, 1.0, 0.5, 2.0);
            yield return () => (3.0, 1.0, 1.0, 1.0);
            yield return () => (3.0, 1.0, 2.0, -1.0);
            yield return () => (4.0, 2.0, default, 4.0);
            yield return () => (4.0, 2.0, 0.5, 3.0);
            yield return () => (4.0, 2.0, 1.0, 2.0);
            yield return () => (4.0, 2.0, 2.0, default);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> LogDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NaN, default);
            yield return () => (-double.Pi, double.NaN, default);                                     //                              value: -(pi)
            yield return () => (-2.7182818284590452, double.NaN, default);                                     //                              value: -(e)
            yield return () => (-1.4142135623730950, double.NaN, default);                                     //                              value: -(sqrt(2))
            yield return () => (-1.0, double.NaN, default);
            yield return () => (-0.69314718055994531, double.NaN, default);                                     //                              value: -(ln(2))
            yield return () => (-0.43429448190325183, double.NaN, default);                                     //                              value: -(log10(e))
            yield return () => (double.NegativeZero, double.NegativeInfinity, default);
            yield return () => (double.NaN, double.NaN, default);
            yield return () => (default, double.NegativeInfinity, default);
            yield return () => (0.043213918263772250, -double.Pi, DoubleCrossPlatformMachineEpsilon * 10);  // expected: -(pi)
            yield return () => (0.065988035845312537, -2.7182818284590452, DoubleCrossPlatformMachineEpsilon * 10);  // expected: -(e)
            yield return () => (0.1, -2.3025850929940457, DoubleCrossPlatformMachineEpsilon * 10);  // expected: -(ln(10))
            yield return () => (0.20787957635076191, -1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10);  // expected: -(pi / 2)
            yield return () => (0.23629008834452270, -1.4426950408889634, DoubleCrossPlatformMachineEpsilon * 10);  // expected: -(log2(e))
            yield return () => (0.24311673443421421, -1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10);  // expected: -(sqrt(2))
            yield return () => (0.32355726390307110, -1.1283791670955126, DoubleCrossPlatformMachineEpsilon * 10);  // expected: -(2 / sqrt(pi))
            yield return () => (0.36787944117144232, -1.0, default);
            yield return () => (0.45593812776599624, -0.78539816339744831, DoubleCrossPlatformMachineEpsilon);       // expected: -(pi / 4)
            yield return () => (0.49306869139523979, -0.70710678118654752, DoubleCrossPlatformMachineEpsilon);       // expected: -(1 / sqrt(2))
            yield return () => (0.5, -0.69314718055994531, DoubleCrossPlatformMachineEpsilon);       // expected: -(ln(2))
            yield return () => (0.52907780826773535, -0.63661977236758134, DoubleCrossPlatformMachineEpsilon);       // expected: -(2 / pi)
            yield return () => (0.64772148514180065, -0.43429448190325183, DoubleCrossPlatformMachineEpsilon);       // expected: -(log10(e))
            yield return () => (0.72737734929521647, -0.31830988618379067, DoubleCrossPlatformMachineEpsilon);       // expected: -(1 / pi)
            yield return () => (1.0, default, default);
            yield return () => (1.3748022274393586, 0.31830988618379067, DoubleCrossPlatformMachineEpsilon);       // expected:  (1 / pi)
            yield return () => (1.5438734439711811, 0.43429448190325183, DoubleCrossPlatformMachineEpsilon);       // expected:  (log10(e))
            yield return () => (1.8900811645722220, 0.63661977236758134, DoubleCrossPlatformMachineEpsilon);       // expected:  (2 / pi)
            yield return () => (2.0, 0.69314718055994531, DoubleCrossPlatformMachineEpsilon);       // expected:  (ln(2))
            yield return () => (2.0281149816474725, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon);       // expected:  (1 / sqrt(2))
            yield return () => (2.1932800507380155, 0.78539816339744831, DoubleCrossPlatformMachineEpsilon);       // expected:  (pi / 4)
            yield return () => (2.7182818284590452, 1.0, DoubleCrossPlatformMachineEpsilon * 10);  //                              value: (e)
            yield return () => (3.0906430223107976, 1.1283791670955126, DoubleCrossPlatformMachineEpsilon * 10);  // expected:  (2 / sqrt(pi))
            yield return () => (4.1132503787829275, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10);  // expected:  (sqrt(2))
            yield return () => (4.2320861065570819, 1.4426950408889634, DoubleCrossPlatformMachineEpsilon * 10);  // expected:  (log2(e))
            yield return () => (4.8104773809653517, 1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10);  // expected:  (pi / 2)
            yield return () => (10.0, 2.3025850929940457, DoubleCrossPlatformMachineEpsilon * 10);  // expected:  (ln(10))
            yield return () => (15.154262241479264, 2.7182818284590452, DoubleCrossPlatformMachineEpsilon * 10);  // expected:  (e)
            yield return () => (23.140692632779269, double.Pi, DoubleCrossPlatformMachineEpsilon * 10);  // expected:  (pi)
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, default);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> Log2Double
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NaN, default);
            yield return () => (-0.11331473229676087, double.NaN, default);
            yield return () => (double.NegativeZero, double.NegativeInfinity, default);
            yield return () => (double.NaN, double.NaN, default);
            yield return () => (default, double.NegativeInfinity, default);
            yield return () => (0.11331473229676087, -double.Pi, DoubleCrossPlatformMachineEpsilon * 10);    // expected: -(pi)
            yield return () => (0.15195522325791297, -2.7182818284590453, DoubleCrossPlatformMachineEpsilon * 10);    // expected: -(e)
            yield return () => (0.20269956628651730, -2.3025850929940460, DoubleCrossPlatformMachineEpsilon * 10);    // expected: -(ln(10))
            yield return () => (0.33662253682241906, -1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10);    // expected: -(pi / 2)
            yield return () => (0.36787944117144232, -1.4426950408889634, DoubleCrossPlatformMachineEpsilon * 10);    // expected: -(log2(e))
            yield return () => (0.37521422724648177, -1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10);    // expected: -(sqrt(2))
            yield return () => (0.45742934732229695, -1.1283791670955126, DoubleCrossPlatformMachineEpsilon * 10);    // expected: -(2 / sqrt(pi))
            yield return () => (0.5, -1.0, default);
            yield return () => (0.58019181037172444, -0.78539816339744840, DoubleCrossPlatformMachineEpsilon);         // expected: -(pi / 4)
            yield return () => (0.61254732653606592, -0.70710678118654750, DoubleCrossPlatformMachineEpsilon);         // expected: -(1 / sqrt(2))
            yield return () => (0.61850313780157598, -0.69314718055994537, DoubleCrossPlatformMachineEpsilon);         // expected: -(ln(2))
            yield return () => (0.64321824193300488, -0.63661977236758126, DoubleCrossPlatformMachineEpsilon);         // expected: -(2 / pi)
            yield return () => (0.74005557395545179, -0.43429448190325190, DoubleCrossPlatformMachineEpsilon);         // expected: -(log10(e))
            yield return () => (0.80200887896145195, -0.31830988618379073, DoubleCrossPlatformMachineEpsilon);         // expected: -(1 / pi)
            yield return () => (1, default, default);
            yield return () => (1.2468689889006383, 0.31830988618379073, DoubleCrossPlatformMachineEpsilon);         // expected:  (1 / pi)
            yield return () => (1.3512498725672678, 0.43429448190325226, DoubleCrossPlatformMachineEpsilon);         // expected:  (log10(e))
            yield return () => (1.5546822754821001, 0.63661977236758126, DoubleCrossPlatformMachineEpsilon);         // expected:  (2 / pi)
            yield return () => (1.6168066722416747, 0.69314718055994537, DoubleCrossPlatformMachineEpsilon);         // expected:  (ln(2))
            yield return () => (1.6325269194381528, 0.70710678118654750, DoubleCrossPlatformMachineEpsilon);         // expected:  (1 / sqrt(2))
            yield return () => (1.7235679341273495, 0.78539816339744830, DoubleCrossPlatformMachineEpsilon);         // expected:  (pi / 4)
            yield return () => (2, 1.0, default);                                       //                              value: (e)
            yield return () => (2.1861299583286618, 1.1283791670955128, DoubleCrossPlatformMachineEpsilon * 10);    // expected:  (2 / sqrt(pi))
            yield return () => (2.6651441426902252, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon * 10);    // expected:  (sqrt(2))
            yield return () => (2.7182818284590452, 1.4426950408889632, DoubleCrossPlatformMachineEpsilon * 10);    // expected:  (log2(e))
            yield return () => (2.9706864235520193, 1.5707963267948966, DoubleCrossPlatformMachineEpsilon * 10);    // expected:  (pi / 2)
            yield return () => (4.9334096679145963, 2.3025850929940460, DoubleCrossPlatformMachineEpsilon * 10);    // expected:  (ln(10))
            yield return () => (6.5808859910179210, 2.7182818284590455, DoubleCrossPlatformMachineEpsilon * 10);    // expected:  (e)
            yield return () => (8.8249778270762876, double.Pi, DoubleCrossPlatformMachineEpsilon * 10);    // expected:  (pi)
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, default);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MaxDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MaxValue);
            yield return () => (double.MaxValue, double.MinValue, double.MaxValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, double.NaN);
            yield return () => (1.0, double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, double.NaN, double.NaN);
            yield return () => (double.NegativeInfinity, double.NaN, double.NaN);
            yield return () => (double.NaN, double.PositiveInfinity, double.NaN);
            yield return () => (double.NaN, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeZero, default, default);
            yield return () => (default, double.NegativeZero, default);
            yield return () => (2.0, -3.0, 2.0);
            yield return () => (-3.0, 2.0, 2.0);
            yield return () => (3.0, -2.0, 3.0);
            yield return () => (-2.0, 3.0, 3.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MaxMagnitudeDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MaxValue);
            yield return () => (double.MaxValue, double.MinValue, double.MaxValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, double.NaN);
            yield return () => (1.0, double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, double.NaN, double.NaN);
            yield return () => (double.NegativeInfinity, double.NaN, double.NaN);
            yield return () => (double.NaN, double.PositiveInfinity, double.NaN);
            yield return () => (double.NaN, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeZero, default, default);
            yield return () => (default, double.NegativeZero, default);
            yield return () => (2.0, -3.0, -3.0);
            yield return () => (-3.0, 2.0, -3.0);
            yield return () => (3.0, -2.0, 3.0);
            yield return () => (-2.0, 3.0, 3.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MaxMagnitudeNumberDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MaxValue);
            yield return () => (double.MaxValue, double.MinValue, double.MaxValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, 1.0);
            yield return () => (1.0, double.NaN, 1.0);
            yield return () => (double.PositiveInfinity, double.NaN, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.NaN, double.NegativeInfinity);
            yield return () => (double.NaN, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.NaN, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.NegativeZero, default, default);
            yield return () => (default, double.NegativeZero, default);
            yield return () => (2.0, -3.0, -3.0);
            yield return () => (-3.0, 2.0, -3.0);
            yield return () => (3.0, -2.0, 3.0);
            yield return () => (-2.0, 3.0, 3.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MaxNumberDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MaxValue);
            yield return () => (double.MaxValue, double.MinValue, double.MaxValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, 1.0);
            yield return () => (1.0, double.NaN, 1.0);
            yield return () => (double.PositiveInfinity, double.NaN, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.NaN, double.NegativeInfinity);
            yield return () => (double.NaN, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.NaN, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.NegativeZero, default, default);
            yield return () => (default, double.NegativeZero, default);
            yield return () => (2.0, -3.0, 2.0);
            yield return () => (-3.0, 2.0, 2.0);
            yield return () => (3.0, -2.0, 3.0);
            yield return () => (-2.0, 3.0, 3.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MinDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MinValue);
            yield return () => (double.MaxValue, double.MinValue, double.MinValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, double.NaN);
            yield return () => (1.0, double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, double.NaN, double.NaN);
            yield return () => (double.NegativeInfinity, double.NaN, double.NaN);
            yield return () => (double.NaN, double.PositiveInfinity, double.NaN);
            yield return () => (double.NaN, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeZero, default, double.NegativeZero);
            yield return () => (default, double.NegativeZero, double.NegativeZero);
            yield return () => (2.0, -3.0, -3.0);
            yield return () => (-3.0, 2.0, -3.0);
            yield return () => (3.0, -2.0, -2.0);
            yield return () => (-2.0, 3.0, -2.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MinMagnitudeDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MinValue);
            yield return () => (double.MaxValue, double.MinValue, double.MinValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, double.NaN);
            yield return () => (1.0, double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, double.NaN, double.NaN);
            yield return () => (double.NegativeInfinity, double.NaN, double.NaN);
            yield return () => (double.NaN, double.PositiveInfinity, double.NaN);
            yield return () => (double.NaN, double.NegativeInfinity, double.NaN);
            yield return () => (double.NegativeZero, default, double.NegativeZero);
            yield return () => (default, double.NegativeZero, double.NegativeZero);
            yield return () => (2.0, -3.0, 2.0);
            yield return () => (-3.0, 2.0, 2.0);
            yield return () => (3.0, -2.0, -2.0);
            yield return () => (-2.0, 3.0, -2.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MinMagnitudeNumberDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MinValue);
            yield return () => (double.MaxValue, double.MinValue, double.MinValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, 1.0);
            yield return () => (1.0, double.NaN, 1.0);
            yield return () => (double.PositiveInfinity, double.NaN, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.NaN, double.NegativeInfinity);
            yield return () => (double.NaN, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.NaN, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.NegativeZero, default, double.NegativeZero);
            yield return () => (default, double.NegativeZero, double.NegativeZero);
            yield return () => (2.0, -3.0, 2.0);
            yield return () => (-3.0, 2.0, 2.0);
            yield return () => (3.0, -2.0, -2.0);
            yield return () => (-2.0, 3.0, -2.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> MinNumberDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);
            yield return () => (double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.MinValue, double.MaxValue, double.MinValue);
            yield return () => (double.MaxValue, double.MinValue, double.MinValue);
            yield return () => (double.NaN, double.NaN, double.NaN);
            yield return () => (double.NaN, 1.0, 1.0);
            yield return () => (1.0, double.NaN, 1.0);
            yield return () => (double.PositiveInfinity, double.NaN, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.NaN, double.NegativeInfinity);
            yield return () => (double.NaN, double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.NaN, double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (double.NegativeZero, default, double.NegativeZero);
            yield return () => (default, double.NegativeZero, double.NegativeZero);
            yield return () => (2.0, -3.0, -3.0);
            yield return () => (-3.0, 2.0, -3.0);
            yield return () => (3.0, -2.0, -2.0);
            yield return () => (-2.0, 3.0, -2.0);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> RadiansToDegreesDouble
    {
        get
        {
            yield return () => (double.NaN, double.NaN, default);
            yield return () => (default, default, default);
            yield return () => (0.0055555555555555567, 0.3183098861837906, DoubleCrossPlatformMachineEpsilon);       // expected:  (1 / pi)
            yield return () => (0.0075798686324546743, 0.4342944819032518, DoubleCrossPlatformMachineEpsilon);       // expected:  (log10(e))
            yield return () => (0.008726646259971648, 0.5, DoubleCrossPlatformMachineEpsilon);
            yield return () => (0.0111111111111111124, 0.6366197723675813, DoubleCrossPlatformMachineEpsilon);       // expected:  (2 / pi)
            yield return () => (0.0120977005016866801, 0.6931471805599453, DoubleCrossPlatformMachineEpsilon);       // expected:  (ln(2))
            yield return () => (0.0123413414948843512, 0.7071067811865475, DoubleCrossPlatformMachineEpsilon);       // expected:  (1 / sqrt(2))
            yield return () => (0.0137077838904018851, 0.7853981633974483, DoubleCrossPlatformMachineEpsilon);       // expected:  (pi / 4)
            yield return () => (0.017453292519943295, 1.0, DoubleCrossPlatformMachineEpsilon);
            yield return () => (0.019693931676727953, 1.1283791670955126, DoubleCrossPlatformMachineEpsilon);       // expected:  (2 / sqrt(pi))
            yield return () => (0.024682682989768702, 1.4142135623730950, DoubleCrossPlatformMachineEpsilon);       // expected:  (sqrt(2))
            yield return () => (0.025179778565706630, 1.4426950408889634, DoubleCrossPlatformMachineEpsilon);       // expected:  (log2(e))
            yield return () => (0.026179938779914940, 1.5, DoubleCrossPlatformMachineEpsilon);
            yield return () => (0.027415567780803770, 1.5707963267948966, DoubleCrossPlatformMachineEpsilon);       // expected:  (pi / 2)
            yield return () => (0.034906585039886590, 2.0, DoubleCrossPlatformMachineEpsilon);
            yield return () => (0.040187691180085916, 2.3025850929940457, DoubleCrossPlatformMachineEpsilon);       // expected:  (ln(10))
            yield return () => (0.043633231299858240, 2.5, DoubleCrossPlatformMachineEpsilon);
            yield return () => (0.047442967903742035, 2.7182818284590452, DoubleCrossPlatformMachineEpsilon);       // expected:  (e)
            yield return () => (0.052359877559829880, 3.0, DoubleCrossPlatformMachineEpsilon);
            yield return () => (0.054831135561607540, double.Pi, DoubleCrossPlatformMachineEpsilon);       // expected:  (pi)
            yield return () => (0.061086523819801536, 3.5, DoubleCrossPlatformMachineEpsilon);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity, default);
        }
    }

    public static IEnumerable<Func<(double, double)>> RoundDouble
    {
        get
        {
            yield return () => (default, default);
            yield return () => (1.4, 1.0);
            yield return () => (1.5, 2.0);
            yield return () => (2e7, 2e7);
            yield return () => (double.NegativeZero, double.NegativeZero);
            yield return () => (-1.4, -1.0);
            yield return () => (-1.5, -2.0);
            yield return () => (-2e7, -2e7);
        }
    }

    public static IEnumerable<Func<(double, double)>> RoundAwayFromZeroDouble
    {
        get
        {
            yield return () => (1, 1);
            yield return () => (0.5, 1);
            yield return () => (1.5, 2);
            yield return () => (2.5, 3);
            yield return () => (3.5, 4);
            yield return () => (0.49999999999999994, 0);
            yield return () => (1.5, 2);
            yield return () => (2.5, 3);
            yield return () => (3.5, 4);
            yield return () => (4.5, 5);
            yield return () => (double.Pi, 3);
            yield return () => (2.718281828459045, 3);
            yield return () => (1385.4557313670111, 1385);
            yield return () => (3423423.43432, 3423423);
            yield return () => (535345.5, 535346);
            yield return () => (535345.50001, 535346);
            yield return () => (535345.5, 535346);
            yield return () => (535345.4, 535345);
            yield return () => (535345.6, 535346);
            yield return () => (-2.718281828459045, -3);
            yield return () => (10, 10);
            yield return () => (-10, -10);
            yield return () => (-0, -0);
            yield return () => (0, 0);
            yield return () => (double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (1.7976931348623157E+308, 1.7976931348623157E+308);
            yield return () => (-1.7976931348623157E+308, -1.7976931348623157E+308);
        }
    }

    public static IEnumerable<Func<(double, double)>> RoundToEvenDouble
    {
        get
        {
            yield return () => (1, 1);
            yield return () => (0.5, 0);
            yield return () => (1.5, 2);
            yield return () => (2.5, 2);
            yield return () => (3.5, 4);
            yield return () => (1.5, 2);
            yield return () => (2.5, 2);
            yield return () => (3.5, 4);
            yield return () => (4.5, 4);
            yield return () => (double.Pi, 3);
            yield return () => (2.718281828459045, 3);
            yield return () => (1385.4557313670111, 1385);
            yield return () => (3423423.43432, 3423423);
            yield return () => (535345.5, 535346);
            yield return () => (535345.50001, 535346);
            yield return () => (535345.5, 535346);
            yield return () => (535345.4, 535345);
            yield return () => (535345.6, 535346);
            yield return () => (-2.718281828459045, -3);
            yield return () => (10, 10);
            yield return () => (-10, -10);
            yield return () => (-0, -0);
            yield return () => (0, 0);
            yield return () => (double.NaN, double.NaN);
            yield return () => (double.PositiveInfinity, double.PositiveInfinity);
            yield return () => (double.NegativeInfinity, double.NegativeInfinity);
            yield return () => (1.7976931348623157E+308, 1.7976931348623157E+308);
            yield return () => (-1.7976931348623157E+308, -1.7976931348623157E+308);
        }
    }

    public static IEnumerable<Func<(double, double, double)>> SinDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NaN, default);
            yield return () => (-double.Pi, double.NegativeZero, DoubleCrossPlatformMachineEpsilon);      // value: -(pi)
            yield return () => (-2.7182818284590452, -0.41078129050290870, DoubleCrossPlatformMachineEpsilon);      // value: -(e)
            yield return () => (-2.3025850929940457, -0.74398033695749319, DoubleCrossPlatformMachineEpsilon);      // value: -(ln(10))
            yield return () => (-1.5707963267948966, -1.0, DoubleCrossPlatformMachineEpsilon * 10); // value: -(pi / 2)
            yield return () => (-1.4426950408889634, -0.99180624439366372, DoubleCrossPlatformMachineEpsilon);      // value: -(log2(e))
            yield return () => (-1.4142135623730950, -0.98776594599273553, DoubleCrossPlatformMachineEpsilon);      // value: -(sqrt(2))
            yield return () => (-1.1283791670955126, -0.90371945743584630, DoubleCrossPlatformMachineEpsilon);      // value: -(2 / sqrt(pi))
            yield return () => (-1.0, -0.84147098480789651, DoubleCrossPlatformMachineEpsilon);
            yield return () => (-0.78539816339744831, -0.70710678118654752, DoubleCrossPlatformMachineEpsilon);      // value: -(pi / 4),        expected: -(1 / sqrt(2))
            yield return () => (-0.70710678118654752, -0.64963693908006244, DoubleCrossPlatformMachineEpsilon);      // value: -(1 / sqrt(2))
            yield return () => (-0.69314718055994531, -0.63896127631363480, DoubleCrossPlatformMachineEpsilon);      // value: -(ln(2))
            yield return () => (-0.63661977236758134, -0.59448076852482208, DoubleCrossPlatformMachineEpsilon);      // value: -(2 / pi)
            yield return () => (-0.43429448190325183, -0.42077048331375735, DoubleCrossPlatformMachineEpsilon);      // value: -(log10(e))
            yield return () => (-0.31830988618379067, -0.31296179620778659, DoubleCrossPlatformMachineEpsilon);      // value: -(1 / pi)
            yield return () => (double.NegativeZero, double.NegativeZero, default);
            yield return () => (double.NaN, double.NaN, default);
            yield return () => (default, default, default);
            yield return () => (0.31830988618379067, 0.31296179620778659, DoubleCrossPlatformMachineEpsilon);      // value:  (1 / pi)
            yield return () => (0.43429448190325183, 0.42077048331375735, DoubleCrossPlatformMachineEpsilon);      // value:  (log10(e))
            yield return () => (0.63661977236758134, 0.59448076852482208, DoubleCrossPlatformMachineEpsilon);      // value:  (2 / pi)
            yield return () => (0.69314718055994531, 0.63896127631363480, DoubleCrossPlatformMachineEpsilon);      // value:  (ln(2))
            yield return () => (0.70710678118654752, 0.64963693908006244, DoubleCrossPlatformMachineEpsilon);      // value:  (1 / sqrt(2))
            yield return () => (0.78539816339744831, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon);      // value:  (pi / 4),        expected:  (1 / sqrt(2))
            yield return () => (1.0, 0.84147098480789651, DoubleCrossPlatformMachineEpsilon);
            yield return () => (1.1283791670955126, 0.90371945743584630, DoubleCrossPlatformMachineEpsilon);      // value:  (2 / sqrt(pi))
            yield return () => (1.4142135623730950, 0.98776594599273553, DoubleCrossPlatformMachineEpsilon);      // value:  (sqrt(2))
            yield return () => (1.4426950408889634, 0.99180624439366372, DoubleCrossPlatformMachineEpsilon);      // value:  (log2(e))
            yield return () => (1.5707963267948966, 1.0, DoubleCrossPlatformMachineEpsilon * 10); // value:  (pi / 2)
            yield return () => (2.3025850929940457, 0.74398033695749319, DoubleCrossPlatformMachineEpsilon);      // value:  (ln(10))
            yield return () => (2.7182818284590452, 0.41078129050290870, DoubleCrossPlatformMachineEpsilon);      // value:  (e)
            yield return () => (double.Pi, default, DoubleCrossPlatformMachineEpsilon);      // value:  (pi)
            yield return () => (double.PositiveInfinity, double.NaN, default);
        }
    }

    public static IEnumerable<Func<(double, double, double, double, double)>> SinCosDouble
    {
        get
        {
            yield return () => (double.NegativeInfinity, double.NaN, double.NaN, default, default);
            yield return () => (-1e18, 0.9929693207404051, 0.11837199021871073, 0.0002, 0.002);                                  // https://github.com/dotnet/runtime/issues/98204
            yield return () => (-double.Pi, double.NegativeZero, -1.0, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon * 10); // value: -(pi)
            yield return () => (-2.7182818284590452, -0.41078129050290870, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(e)
            yield return () => (-2.3025850929940457, -0.74398033695749319, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(ln(10))
            yield return () => (-1.5707963267948966, -1.0, default, DoubleCrossPlatformMachineEpsilon * 10, DoubleCrossPlatformMachineEpsilon);      // value: -(pi / 2)
            yield return () => (-1.4426950408889634, -0.99180624439366372, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(log2(e))
            yield return () => (-1.4142135623730950, -0.98776594599273553, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(sqrt(2))
            yield return () => (-1.1283791670955126, -0.90371945743584630, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(2 / sqrt(pi))
            yield return () => (-1.0, -0.84147098480789651, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);
            yield return () => (-0.78539816339744831, -0.70710678118654752, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(pi / 4),        expected_sin: -(1 / sqrt(2)),    expected_cos: 1
            yield return () => (-0.70710678118654752, -0.64963693908006244, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(1 / sqrt(2))
            yield return () => (-0.69314718055994531, -0.63896127631363480, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(ln(2))
            yield return () => (-0.63661977236758134, -0.59448076852482208, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(2 / pi)
            yield return () => (-0.43429448190325183, -0.42077048331375735, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(log10(e))
            yield return () => (-0.31830988618379067, -0.31296179620778659, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value: -(1 / pi)
            yield return () => (double.NegativeZero, double.NegativeZero, 1.0, default, DoubleCrossPlatformMachineEpsilon * 10);
            yield return () => (double.NaN, double.NaN, double.NaN, default, default);
            yield return () => (default, default, 1.0, default, DoubleCrossPlatformMachineEpsilon * 10);
            yield return () => (0.31830988618379067, 0.31296179620778659, 0.94976571538163866, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (1 / pi)
            yield return () => (0.43429448190325183, 0.42077048331375735, 0.90716712923909839, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (log10(e))
            yield return () => (0.63661977236758134, 0.59448076852482208, 0.80410982822879171, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (2 / pi)
            yield return () => (0.69314718055994531, 0.63896127631363480, 0.76923890136397213, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (ln(2))
            yield return () => (0.70710678118654752, 0.64963693908006244, 0.76024459707563015, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (1 / sqrt(2))
            yield return () => (0.78539816339744831, 0.70710678118654752, 0.70710678118654752, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (pi / 4),        expected_sin:  (1 / sqrt(2)),    expected_cos: 1
            yield return () => (1.0, 0.84147098480789651, 0.54030230586813972, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);
            yield return () => (1.1283791670955126, 0.90371945743584630, 0.42812514788535792, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (2 / sqrt(pi))
            yield return () => (1.4142135623730950, 0.98776594599273553, 0.15594369476537447, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (sqrt(2))
            yield return () => (1.4426950408889634, 0.99180624439366372, 0.12775121753523991, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (log2(e))
            yield return () => (1.5707963267948966, 1.0, default, DoubleCrossPlatformMachineEpsilon * 10, DoubleCrossPlatformMachineEpsilon);      // value:  (pi / 2)
            yield return () => (2.3025850929940457, 0.74398033695749319, -0.66820151019031295, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (ln(10))
            yield return () => (2.7182818284590452, 0.41078129050290870, -0.91173391478696510, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon);      // value:  (e)
            yield return () => (double.Pi, default, -1.0, DoubleCrossPlatformMachineEpsilon, DoubleCrossPlatformMachineEpsilon * 10); // value:  (pi)
            yield return () => (1e18, -0.9929693207404051, 0.11837199021871073, 0.0002, 0.002);                                  // https://github.com/dotnet/runtime/issues/98204
            yield return () => (double.PositiveInfinity, double.NaN, double.NaN, default, default);
        }
    }

    public static IEnumerable<Func<(double, double)>> TruncateDouble
    {
        get
        {
            yield return () => (0.12345, default);
            yield return () => (3.14159, 3.0);
            yield return () => (-3.14159, -3.0);
        }
    }
}