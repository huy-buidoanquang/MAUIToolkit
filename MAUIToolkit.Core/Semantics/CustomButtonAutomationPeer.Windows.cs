using MAUIToolkit.Core.Platforms;
using MAUIToolkit.Core.Primitives;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;

namespace MAUIToolkit.Core.Semantics;

//
// Summary:
//     Button automation peer. Custom button automation class created because button
//     instance needed while create framework automation peer.
internal class CustomButtonAutomationPeer : FrameworkElementAutomationPeer, ICustomAutomationPeer, IInvokeProvider
{
    //
    // Summary:
    //     The information for the automation peer.
    private SemanticsNode semanticsNode;

    //
    // Summary:
    //     Holds the previous sibling of the automation peer.
    public AutomationPeer? PrevSibling { get; set; }

    //
    // Summary:
    //     Holds the previous sibling of the automation peer.
    public AutomationPeer? NextSibling { get; set; }

    //
    // Summary:
    //     Holds the next sibling node information.
    public SemanticsNode? NextNode { get; set; }

    //
    // Summary:
    //     Holds the previous sibling node information.
    public SemanticsNode? PrevNode { get; set; }

    //
    // Summary:
    //     Gets the node information.
    public SemanticsNode Node => semanticsNode;

    //
    // Summary:
    //     Holds the parent View instance for scroll action.
    public View MauiView { get; set; }

    internal CustomButtonAutomationPeer(FrameworkElement owner, SemanticsNode semanticsNode, View mauiView)
        : base(owner)
    {
        this.semanticsNode = semanticsNode;
        MauiView = mauiView;
    }

    //
    // Summary:
    //     Handles and return the automation peer while navigation.
    //
    // Parameters:
    //   direction:
    //     Action performed on automation peer.
    //
    // Returns:
    //     Return the automation peer.
    protected override object NavigateCore(AutomationNavigationDirection direction)
    {
        if (direction == AutomationNavigationDirection.NextSibling && NextSibling != null)
        {
            if (NextNode != null)
            {
                ((ISemanticsProvider)MauiView).ScrollTo(NextNode);
            }

            return NextSibling;
        }

        if (direction == AutomationNavigationDirection.PreviousSibling && PrevSibling != null)
        {
            if (PrevNode != null)
            {
                ((ISemanticsProvider)MauiView).ScrollTo(PrevNode);
            }

            return PrevSibling;
        }

        return base.NavigateCore(direction);
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Button;
    }

    //
    // Summary:
    //     Decide to handle the actions that related to this automation peer.
    //
    // Parameters:
    //   patternInterface:
    //     Performed action.
    //
    // Returns:
    //     The automation peer.
    protected override object GetPatternCore(PatternInterface patternInterface)
    {
        if (patternInterface == PatternInterface.Invoke)
        {
            return this;
        }

        return base.GetPatternCore(patternInterface);
    }

    //
    // Summary:
    //     Return the information that needed on hovering.
    //
    // Returns:
    //     The name information.
    protected override string GetNameCore()
    {
        return semanticsNode.Text;
    }

    //
    // Summary:
    //     Return the current automation peer bounds value related to screen.
    //
    // Returns:
    //     The rectangle bounds value.
    protected override Windows.Foundation.Rect GetBoundingRectangleCore()
    {
        Windows.Foundation.Rect boundingRectangle = GetParent().GetBoundingRectangle();
        NativePlatformGraphicsView nativeGraphicsView = (NativePlatformGraphicsView)base.Owner;
        double rasterizationScale = nativeGraphicsView.XamlRoot.RasterizationScale;
        Microsoft.Maui.Graphics.Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(nativeGraphicsView, rasterizationScale);
        Microsoft.Maui.Graphics.Rect rect = new Microsoft.Maui.Graphics.Rect(semanticsNode.Bounds.Left * rasterizationScale, semanticsNode.Bounds.Top * rasterizationScale, semanticsNode.Bounds.Width * rasterizationScale, semanticsNode.Bounds.Height * rasterizationScale);
        Microsoft.Maui.Graphics.Rect rect2 = new Microsoft.Maui.Graphics.Rect(viewStartPosition.X + rect.Left, viewStartPosition.Y + rect.Top, rect.Width, rect.Height);
        double num = rect2.Y;
        double num2 = rect2.Y + rect2.Height;
        double num3 = rect2.X;
        double num4 = rect2.X + rect2.Width;
        if (num < boundingRectangle.Top)
        {
            num = boundingRectangle.Top;
        }

        if (num2 > boundingRectangle.Top + boundingRectangle.Height)
        {
            num2 = boundingRectangle.Top + boundingRectangle.Height;
        }

        if (num3 < boundingRectangle.Left)
        {
            num3 = boundingRectangle.Left;
        }

        if (num4 > boundingRectangle.Left + boundingRectangle.Width)
        {
            num4 = boundingRectangle.Left + boundingRectangle.Width;
        }

        double num5 = num4 - num3;
        if (num5 < 0.0)
        {
            num5 = 0.0;
        }

        double num6 = num2 - num;
        if (num6 < 0.0)
        {
            num6 = 0.0;
        }

        return new Windows.Foundation.Rect(num3, num, num5, num6);
    }

    //
    // Summary:
    //     Sends a request to initiate or perform the invoke action of the provider control.
    void IInvokeProvider.Invoke()
    {
        if (semanticsNode.OnClick != null)
        {
            semanticsNode.OnClick(semanticsNode);
        }
    }
}
