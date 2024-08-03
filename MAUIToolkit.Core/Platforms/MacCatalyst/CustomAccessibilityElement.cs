using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace MAUIToolkit.Core.Platforms;

internal class CustomAccessibilityElement : UIAccessibilityElement
{
    //
    // Summary:
    //     Hold the bounds of the accessibility element.
    public CGRect Bounds { get; set; }

    //
    // Summary:
    //     Holds the owner or parent of the accessibility element.
    public UIView? Parent { get; set; }

    //
    // Summary:
    //     Holds the owner or parent accessiblity frame value.
    public CGRect ParentBounds { get; set; }

    public override CGRect AccessibilityFrame
    {
        get
        {
            if (Parent != null && Parent.Handle != IntPtr.Zero && Parent.AccessibilityFrame != ParentBounds)
            {
                ParentBounds = Parent.AccessibilityFrame;
                AccessibilityFrame = UIAccessibility.ConvertFrameToScreenCoordinates(Bounds, Parent);
            }

            return base.AccessibilityFrame;
        }
        set
        {
            base.AccessibilityFrame = value;
        }
    }

    public CustomAccessibilityElement(NSObject container)
        : base(container)
    {
        Bounds = CGRect.Empty;
        ParentBounds = CGRect.Empty;
        Parent = (UIView)container;
    }

    protected CustomAccessibilityElement(NSObjectFlag t)
        : base(t)
    {
    }

    protected internal CustomAccessibilityElement(NativeHandle handle)
        : base(handle)
    {
    }
}
