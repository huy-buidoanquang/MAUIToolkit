using Microsoft.Maui.Layouts;
using System.Reflection;

namespace MAUIToolkit.Core;

public class DrawableLayoutManager : AbsoluteLayoutManager
{
    internal new IAbsoluteLayout Layout;

    private const double AutoSize = -1.0;

    private MethodInfo? methodInfos;

    private object[]? parameters;

    //
    // Parameters:
    //   layout:
    public DrawableLayoutManager(IAbsoluteLayout layout)
        : base(layout)
    {
        Layout = layout;
        methodInfos = typeof(LayoutExtensions).GetMethod("ShouldArrangeLeftToRight", BindingFlags.Static | BindingFlags.Public);
        parameters = new object[1] { Layout };
    }

    //
    // Parameters:
    //   widthConstraint:
    //
    //   heightConstraint:
    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        Thickness padding = Layout.Padding;
        double num = widthConstraint - padding.HorizontalThickness;
        double num2 = heightConstraint - padding.VerticalThickness;
        double num3 = 0.0;
        double num4 = 0.0;
        if (Layout.Count > 0)
        {
            for (int i = 0; i < Layout.Count; i++)
            {
                IView view = Layout[i];
                if (view.Visibility != Visibility.Collapsed)
                {
                    Rect layoutBounds = Layout.GetLayoutBounds(view);
                    AbsoluteLayoutFlags layoutFlags = Layout.GetLayoutFlags(view);
                    bool flag = HasFlag(layoutFlags, AbsoluteLayoutFlags.WidthProportional);
                    bool flag2 = HasFlag(layoutFlags, AbsoluteLayoutFlags.HeightProportional);
                    if (layoutBounds.Width > 0.0 && layoutBounds.Height > 0.0)
                    {
                        double widthConstraint2 = ResolveChildMeasureConstraint(layoutBounds.Width, flag, widthConstraint);
                        double heightConstraint2 = ResolveChildMeasureConstraint(layoutBounds.Height, flag2, heightConstraint);
                        Size size = view.Measure(widthConstraint2, heightConstraint2);
                        double num5 = ResolveDimension(flag, layoutBounds.Width, num, size.Width);
                        double num6 = ResolveDimension(flag2, layoutBounds.Height, num2, size.Height);
                        num3 = Math.Max(num3, layoutBounds.Top + num6);
                        num4 = Math.Max(num4, layoutBounds.Left + num5);
                    }
                    else
                    {
                        Size size2 = view.Measure(num, num2);
                        num3 = size2.Height;
                        num4 = size2.Width;
                    }
                }
            }
        }
        else
        {
            if (heightConstraint > 0.0 && heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity && heightConstraint != double.NaN)
            {
                num3 = heightConstraint;
            }

            if (widthConstraint > 0.0 && widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity && widthConstraint != double.NaN)
            {
                num4 = widthConstraint;
            }
        }

        double height = LayoutManager.ResolveConstraints(heightConstraint, Layout.Height, num3, Layout.MinimumHeight, Layout.MaximumHeight);
        double width = LayoutManager.ResolveConstraints(widthConstraint, Layout.Width, num4, Layout.MinimumWidth, Layout.MaximumWidth);
        return new Size(width, height);
    }

    //
    // Parameters:
    //   bounds:
    public override Size ArrangeChildren(Rect bounds)
    {
        Thickness padding = Layout.Padding;
        double num = padding.Top + bounds.Top;
        double num2 = padding.Left + bounds.Left;
        double num3 = bounds.Right - padding.Right;
        double num4 = bounds.Width - padding.HorizontalThickness;
        double num5 = bounds.Height - padding.VerticalThickness;
        bool flag = true;
        if (methodInfos != null && methodInfos.Invoke(this, parameters) is bool flag2)
        {
            flag = flag2;
        }

        for (int i = 0; i < Layout.Count; i++)
        {
            IView view = Layout[i];
            if (view.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            Rect bounds2 = Layout.GetLayoutBounds(view);
            if (bounds2.Width > 0.0 && bounds2.Height > 0.0)
            {
                AbsoluteLayoutFlags layoutFlags = Layout.GetLayoutFlags(view);
                bool isProportional = HasFlag(layoutFlags, AbsoluteLayoutFlags.WidthProportional);
                bool isProportional2 = HasFlag(layoutFlags, AbsoluteLayoutFlags.HeightProportional);
                bounds2.Width = ResolveDimension(isProportional, bounds2.Width, num4, view.DesiredSize.Width);
                bounds2.Height = ResolveDimension(isProportional2, bounds2.Height, num5, view.DesiredSize.Height);
                if (HasFlag(layoutFlags, AbsoluteLayoutFlags.XProportional))
                {
                    bounds2.X = (num4 - bounds2.Width) * bounds2.X;
                }

                if (HasFlag(layoutFlags, AbsoluteLayoutFlags.YProportional))
                {
                    bounds2.Y = (num5 - bounds2.Height) * bounds2.Y;
                }
            }
            else
            {
                bounds2 = new Rect(0.0, 0.0, num4, num5);
            }

            if (flag)
            {
                bounds2.X += num2;
            }
            else
            {
                bounds2.X = num3 - bounds2.X - bounds2.Width;
            }

            bounds2.Y += num;
            view.Arrange(bounds2);
        }

        return new Size(num4, num5);
    }

    private static bool HasFlag(AbsoluteLayoutFlags a, AbsoluteLayoutFlags b)
    {
        return (a & b) == b;
    }

    private static double ResolveDimension(bool isProportional, double fromBounds, double available, double measured)
    {
        double num = fromBounds;
        if (isProportional && !double.IsInfinity(available))
        {
            num *= available;
        }
        else if (num == -1.0)
        {
            num = measured;
        }

        return num;
    }

    private static double ResolveChildMeasureConstraint(double boundsValue, bool proportional, double constraint)
    {
        if (boundsValue < 0.0)
        {
            return double.PositiveInfinity;
        }

        if (proportional)
        {
            return boundsValue * constraint;
        }

        return boundsValue;
    }
}
