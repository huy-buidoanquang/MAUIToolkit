namespace MAUIToolkit.Core;

internal class MathUtils
{
    internal static double GetAngle(double x1, double x2, double y1, double y2)
    {
        double num = Math.Atan2(0.0 - (y2 - y1), x2 - x1);
        num = ((num < 0.0) ? Math.Abs(num) : (Math.PI * 2.0 - num));
        return num * (180.0 / Math.PI);
    }

    //
    // Summary:
    //     Provides the distance between two points based on the Euclidean distance formula.
    //
    //
    // Parameters:
    //   x1:
    //     X value of Point 1
    //
    //   x2:
    //     X value of Point 2
    //
    //   y1:
    //     Y value of Point 1
    //
    //   y2:
    //     Y value of Point 2
    //
    // Returns:
    //     The distance between two points
    internal static double GetDistance(double x1, double x2, double y1, double y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2.0) + Math.Pow(y1 - y2, 2.0));
    }
}
