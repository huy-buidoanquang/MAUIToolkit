﻿using System.Globalization;

namespace MAUIToolkit.Core.Drawing;

public struct RectangleF
{
    private float x;

    private float y;

    private float width;

    private float height;

    public static readonly RectangleF Empty;

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
    //     Gets or sets the width.
    //
    // Value:
    //     The width.
    public float Width
    {
        get
        {
            return width;
        }
        set
        {
            width = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the height.
    //
    // Value:
    //     The height.
    public float Height
    {
        get
        {
            return height;
        }
        set
        {
            height = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the location.
    //
    // Value:
    //     The location.
    public PointF Location
    {
        get
        {
            return new PointF(x, y);
        }
        set
        {
            x = value.X;
            y = value.Y;
        }
    }

    //
    // Summary:
    //     Gets or sets the size.
    //
    // Value:
    //     The size.
    public SizeF Size
    {
        get
        {
            return new SizeF(width, height);
        }
        set
        {
            width = value.Width;
            height = value.Height;
        }
    }

    //
    // Summary:
    //     Gets the left.
    //
    // Value:
    //     The left.
    public float Left => x;

    //
    // Summary:
    //     Gets the top.
    //
    // Value:
    //     The top.
    public float Top => y;

    //
    // Summary:
    //     Gets the right.
    //
    // Value:
    //     The right.
    public float Right => x + width;

    //
    // Summary:
    //     Gets the bottom.
    //
    // Value:
    //     The bottom.
    public float Bottom => y + height;

    internal bool IsEmpty
    {
        get
        {
            if (!(width <= 0f))
            {
                return height <= 0f;
            }

            return true;
        }
    }

    //
    // Summary:
    //     Initializes the MAUIToolkit.Core.Drawing.RectangleF class.
    static RectangleF()
    {
        Empty = default;
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Drawing.RectangleF class.
    //
    // Parameters:
    //   x:
    //     The horizontal location to get the starting point
    //
    //   y:
    //     The vertical location to get the starting point
    //
    //   width:
    //     The width.
    //
    //   height:
    //     The height.
    public RectangleF(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Drawing.RectangleF class.
    //
    // Parameters:
    //   location:
    //     The location.
    //
    //   size:
    //     The size.
    public RectangleF(PointF location, SizeF size)
    {
        x = location.X;
        y = location.Y;
        width = size.Width;
        height = size.Height;
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
        if (!(obj is RectangleF rectangleF))
        {
            return false;
        }

        if (rectangleF.X == X && rectangleF.Y == Y && rectangleF.Width == Width)
        {
            return rectangleF.Height == Height;
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
        return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) + ",Width=" + Width.ToString(CultureInfo.CurrentCulture) + ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
    }

    //
    // Summary:
    //     Froms the LTRB.
    //
    // Parameters:
    //   left:
    //     The left.
    //
    //   top:
    //     The top.
    //
    //   right:
    //     The right.
    //
    //   bottom:
    //     The bottom.
    public static RectangleF FromLTRB(float left, float top, float right, float bottom)
    {
        return new RectangleF(left, top, right, bottom);
    }

    //
    // Summary:
    //     Determines whether given X and Y position is exists with in the bounds.
    //
    // Parameters:
    //   x:
    //     Position of the X
    //
    //   y:
    //     Position of the Y
    internal bool Contains(float x, float y)
    {
        if (X <= x && x < X + Width && Y <= y)
        {
            return y < Y + Height;
        }

        return false;
    }

    //
    // Summary:
    //     Determines whether given PointF is exists with in the bounds.
    //
    // Parameters:
    //   pt:
    //     Represents the point
    internal bool Contains(PointF pt)
    {
        return Contains(pt.X, pt.Y);
    }

    internal bool Contains(RectangleF rect)
    {
        if (X <= rect.X && rect.X + rect.Width <= X + Width && Y <= rect.Y)
        {
            return rect.Y + rect.Height <= Y + Height;
        }

        return false;
    }

    public static implicit operator RectangleF(Rectangle rect)
    {
        return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
    }

    public static bool operator ==(RectangleF rectangle1, RectangleF rectangle2)
    {
        if (rectangle1.X == rectangle2.X && rectangle1.Y == rectangle2.Y && rectangle1.Width == rectangle2.Width)
        {
            return rectangle1.Height == rectangle2.Height;
        }

        return false;
    }

    public static bool operator !=(RectangleF rectangle1, RectangleF rectangle2)
    {
        return !(rectangle1 == rectangle2);
    }

    public void Inflate(float x, float y)
    {
        X -= x;
        Y -= y;
        Width += 2f * x;
        Height += 2f * y;
    }

    public void Inflate(SizeF size)
    {
        Inflate(size.Width, size.Height);
    }

    internal bool IntersectsWith(RectangleF rect)
    {
        if (rect.X < X + Width && X < rect.X + rect.Width && rect.Y < Y + Height)
        {
            return Y < rect.Y + rect.Height;
        }

        return false;
    }

    internal void Intersect(RectangleF rect)
    {
        RectangleF rectangleF = Intersect(rect, this);
        X = rectangleF.X;
        Y = rectangleF.Y;
        Width = rectangleF.Width;
        Height = rectangleF.Height;
    }

    internal static RectangleF Intersect(RectangleF a, RectangleF b)
    {
        float num = Math.Max(a.X, b.X);
        float num2 = Math.Min(a.X + a.Width, b.X + b.Width);
        float num3 = Math.Max(a.Y, b.Y);
        float num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
        if (num2 >= num && num4 >= num3)
        {
            return new RectangleF(num, num3, num2 - num, num4 - num3);
        }

        return Empty;
    }

    internal static RectangleF Union(RectangleF a, RectangleF b)
    {
        float num = Math.Min(a.X, b.X);
        float num2 = Math.Max(a.X + a.Width, b.X + b.Width);
        float num3 = Math.Min(a.Y, b.Y);
        float num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
        return new RectangleF(num, num3, num2 - num, num4 - num3);
    }

    internal static RectangleF Inflate(RectangleF rect, float x, float y)
    {
        RectangleF result = rect;
        result.Inflate(x, y);
        return result;
    }

    internal void Offset(PointF pos)
    {
        Offset(pos.X, pos.Y);
    }

    internal void Offset(float x, float y)
    {
        X += x;
        Y += y;
    }
}
