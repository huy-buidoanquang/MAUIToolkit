namespace MAUIToolkit.Core.Internals;

internal class Bezier
{
    internal TimedPoint StartPoint { get; set; } = null;


    internal TimedPoint FirstControlPoint { get; set; } = null;


    internal TimedPoint SecondControlPoint { get; set; } = null;


    internal TimedPoint EndPoint { get; set; } = null;


    internal void Update(TimedPoint startPoint, TimedPoint firstControlPoint, TimedPoint secondControlPoint, TimedPoint endPoint)
    {
        StartPoint = startPoint;
        FirstControlPoint = firstControlPoint;
        SecondControlPoint = secondControlPoint;
        EndPoint = endPoint;
    }

    internal double Length()
    {
        int num = 10;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        for (int i = 0; i <= num; i++)
        {
            float t = (float)i / (float)num;
            double num5 = Point(t, StartPoint.X, FirstControlPoint.X, SecondControlPoint.X, EndPoint.X);
            double num6 = Point(t, StartPoint.Y, FirstControlPoint.Y, SecondControlPoint.Y, EndPoint.Y);
            if (i > 0)
            {
                double num7 = num5 - num3;
                double num8 = num6 - num4;
                num2 += Math.Sqrt(num7 * num7 + num8 * num8);
            }

            num3 = num5;
            num4 = num6;
        }

        return num2;
    }

    internal double Point(float t, float start, float c1, float c2, float end)
    {
        return (double)start * (1.0 - (double)t) * (1.0 - (double)t) * (1.0 - (double)t) + 3.0 * (double)c1 * (1.0 - (double)t) * (1.0 - (double)t) * (double)t + 3.0 * (double)c2 * (1.0 - (double)t) * (double)t * (double)t + (double)(end * t * t * t);
    }
}
