using System.Globalization;

namespace MAUIToolkit.Core.Drawing;

public struct PointF
{
    private float x;

    private float y;

    public static readonly PointF Empty;

    internal bool IsEmpty
    {
        get
        {
            if (x == 0f)
            {
                return y == 0f;
            }

            return false;
        }
    }

    //
    // Summary:
    //     Gets or sets the X.
    //
    // Value:
    //     The X.
    public float X
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the Y.
    //
    // Value:
    //     The Y.
    public float Y
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }

    //
    // Summary:
    //     Initializes the MAUIToolkit.Core.Drawing.PointF class.
    static PointF()
    {
        Empty = default(PointF);
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Drawing.PointF class.
    //
    // Parameters:
    //   x:
    //     The x.
    //
    //   y:
    //     The y.
    public PointF(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    //
    // Summary:
    //     Determines whether the specified System.Object is equal to the current System.Object.
    //
    //
    // Parameters:
    //   obj:
    //     The System.Object to compare with the current System.Object.
    //
    // Returns:
    //     true if the specified System.Object is equal to the current System.Object; otherwise,
    //     false.
    //
    // Exceptions:
    //   T:System.NullReferenceException:
    //     The obj parameter is null.
    public override bool Equals(object obj)
    {
        if (!(obj is PointF pointF))
        {
            return false;
        }

        if (pointF.x == X)
        {
            return pointF.y == Y;
        }

        return false;
    }

    //
    // Summary:
    //     Serves as a hash function for a particular type.
    //
    // Returns:
    //     A hash code for the current System.Object.
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    //
    // Summary:
    //     Returns a System.String that represents the current System.Object.
    //
    // Returns:
    //     A System.String that represents the current System.Object.
    public override string ToString()
    {
        return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) + "}";
    }

    public static bool operator ==(PointF point1, PointF point2)
    {
        if (point1.X == point2.X)
        {
            return point1.Y == point2.Y;
        }

        return false;
    }

    public static implicit operator PointF(Point point)
    {
        return new PointF(point.X, point.Y);
    }

    public static bool operator !=(PointF point1, PointF point2)
    {
        return !(point1 == point2);
    }

    internal static PointF Add(PointF pt, Size sz)
    {
        return new PointF(pt.X + (float)sz.Width, pt.Y + (float)sz.Height);
    }

    internal static PointF Add(PointF pt, SizeF sz)
    {
        return new PointF(pt.X + sz.Width, pt.Y + sz.Height);
    }
}
