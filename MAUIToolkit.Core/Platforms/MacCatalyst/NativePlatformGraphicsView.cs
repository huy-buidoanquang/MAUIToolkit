using CoreGraphics;
using Foundation;
using MAUIToolkit.Core.Controls;
using MAUIToolkit.Core.Primitives;
using MAUIToolkit.Core.Semantics;
using Microsoft.Maui.Graphics.Platform;
using ObjCRuntime;
using UIKit;

namespace MAUIToolkit.Core.Platforms;

internal class NativePlatformGraphicsView : PlatformGraphicsView, IUIAccessibilityContainer, INativeObject, IDisposable
{
    private CView? mauiView;

    private CGRect? availableBounds;

    internal NativePlatformGraphicsView(CView? mauiView)
        : base(mauiView)
    {
        IsAccessibilityElement = false;
        this.mauiView = mauiView;
    }

    public override void Draw(CGRect dirtyRect)
    {
        base.Draw(dirtyRect);
        if (this.GetAccessibilityElements() == null || availableBounds != dirtyRect)
        {
            CreateSemantics();
            availableBounds = dirtyRect;
        }
    }

    //
    // Summary:
    //     Clear and create the semantics nodes.
    internal void InvalidateSemantics()
    {
        CreateSemantics();
    }

    private void CreateSemantics()
    {
        if (!UIAccessibility.IsVoiceOverRunning && !UIAccessibility.IsSwitchControlRunning && !UIAccessibility.IsSpeakScreenEnabled)
        {
            this.SetAccessibilityElements(null);
            return;
        }

        List<UIAccessibilityElement> list = new List<UIAccessibilityElement>();
        List<SemanticsNode> list2 = null;
        if (mauiView != null)
        {
            list2 = ((ISemanticsProvider)mauiView).GetSemanticsNodes((double)Bounds.Width, (double)Bounds.Height);
        }

        if (list2 != null)
        {
            CGRect accessibilityFrame = AccessibilityFrame;
            for (int i = 0; i < list2.Count; i++)
            {
                SemanticsNode semanticsNode = list2[i];
                CustomAccessibilityElement item = new CustomAccessibilityElement(this)
                {
                    AccessibilityHint = semanticsNode.Text,
                    AccessibilityLabel = semanticsNode.Text,
                    AccessibilityIdentifier = semanticsNode.Id.ToString(),
                    IsAccessibilityElement = true,
                    AccessibilityTraits = (ulong)(semanticsNode.IsTouchEnabled ? 1 : 0),
                    AccessibilityFrame = UIAccessibility.ConvertFrameToScreenCoordinates(new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height), this),
                    Bounds = new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height),
                    Parent = this,
                    ParentBounds = accessibilityFrame
                };
                list.Add(item);
            }
        }

        object[] items = list.ToArray();
        this.SetAccessibilityElements(NSArray.FromObjects(items));
    }
}
