namespace MAUIToolkit.Graphics.Core.Primitives;

internal class TimedPoint
{
    internal float X { get; set; }

    internal float Y { get; set; }

    internal long TimeStamp { get; set; }

    internal void Update(float x, float y)
    {
        X = x;
        Y = y;
        TimeStamp = DateTime.Now.Millisecond;
    }

    internal float VelocityFrom(TimedPoint start)
    {
        long num = TimeStamp - start.TimeStamp;
        if (num <= 0)
        {
            num = 1L;
        }

        float num2 = DistanceTo(start) / (float)num;
        if (float.IsInfinity(num2) || float.IsNaN(num2))
        {
            num2 = 0f;
        }

        return num2;
    }

    internal float DistanceTo(TimedPoint point)
    {
        return (float)Math.Sqrt(Math.Pow(point.X - X, 2.0) + Math.Pow(point.Y - Y, 2.0));
    }

    internal TimedPoint Copy()
    {
        return new TimedPoint
        {
            X = X,
            Y = Y,
            TimeStamp = TimeStamp
        };
    }
}
