using MAUIToolkit.Core.Platforms;
using MAUIToolkit.Core.Primitives;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;

namespace MAUIToolkit.Core.Semantics;

internal class CustomAutomationPeer : FrameworkElementAutomationPeer
{
    //
    // Summary:
    //     Holds the information for the semantics nodes.
    private View mauiView;

    //
    // Summary:
    //     Holds the semantics nodes for the view.
    private List<SemanticsNode>? semanticNodes;

    //
    // Summary:
    //     Holds the automation peers for the semantics nodes.
    private IList<AutomationPeer> automationPeers;

    internal CustomAutomationPeer(NativePlatformGraphicsView owner, View mauiView)
        : base(owner)
    {
        this.mauiView = mauiView;
        automationPeers = new List<AutomationPeer>();
    }

    //
    // Summary:
    //     Return the class name and the value added to name value while hovering.
    //
    // Returns:
    //     The class name.
    protected override string GetClassNameCore()
    {
        return "";
    }

    //
    // Summary:
    //     Return the control type. Here, this class used to hold list of children automation
    //     peer.
    //
    // Returns:
    //     The control type.
    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Custom;
    }

    //
    // Summary:
    //     Information of the automation peer.
    //
    // Returns:
    //     Name of automation peer.
    protected override string GetNameCore()
    {
        return "";
    }

    //
    // Summary:
    //     Invalidate the children automation peer.
    internal void InvalidateSemantics()
    {
        GetChildren().Clear();
    }

    //
    // Summary:
    //     Return the children of the automation peer.
    //
    // Returns:
    //     The children.
    protected override IList<AutomationPeer> GetChildrenCore()
    {
        automationPeers.Clear();
        semanticNodes = ((ISemanticsProvider)mauiView).GetSemanticsNodes(mauiView.Width, mauiView.Height);
        AutomationPeer automationPeer = null;
        SemanticsNode prevNode = null;
        if (semanticNodes != null)
        {
            for (int i = 0; i < semanticNodes.Count; i++)
            {
                SemanticsNode semanticsNode = semanticNodes[i];
                AutomationPeer automationPeer2 = ((!semanticsNode.IsTouchEnabled) ? ((AutomationPeer)new CustomTextAutomationPeer((FrameworkElement)base.Owner, semanticsNode, mauiView)) : ((AutomationPeer)new CustomButtonAutomationPeer((FrameworkElement)base.Owner, semanticsNode, mauiView)));
                if (automationPeer != null && automationPeer is ICustomAutomationPeer)
                {
                    ((ICustomAutomationPeer)automationPeer).NextSibling = automationPeer2;
                    ((ICustomAutomationPeer)automationPeer).NextNode = semanticsNode;
                }

                if (automationPeer2 is ICustomAutomationPeer)
                {
                    ((ICustomAutomationPeer)automationPeer2).PrevSibling = automationPeer;
                    ((ICustomAutomationPeer)automationPeer2).PrevNode = prevNode;
                }

                automationPeer = automationPeer2;
                prevNode = semanticsNode;
                automationPeers.Add(automationPeer2);
            }
        }

        return automationPeers;
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
        if (direction == AutomationNavigationDirection.FirstChild && automationPeers != null && automationPeers.Count > 0)
        {
            if (semanticNodes != null && semanticNodes.Count > 0)
            {
                ((ISemanticsProvider)mauiView).ScrollTo(semanticNodes[0]);
            }

            return automationPeers[0];
        }

        if (direction == AutomationNavigationDirection.LastChild && automationPeers != null && automationPeers.Count > 0)
        {
            if (semanticNodes != null && semanticNodes.Count > 0)
            {
                ((ISemanticsProvider)mauiView).ScrollTo(semanticNodes[semanticNodes.Count - 1]);
            }

            return automationPeers[automationPeers.Count - 1];
        }

        return base.NavigateCore(direction);
    }

    //
    // Summary:
    //     Return the automation peer based on its position.
    //
    // Parameters:
    //   pointInWindowCoordinates:
    //     The interacted point.
    //
    // Returns:
    //     The automation peer.
    protected override object GetElementFromPointCore(Windows.Foundation.Point pointInWindowCoordinates)
    {
        if (semanticNodes != null)
        {
            NativePlatformGraphicsView nativeGraphicsView = (NativePlatformGraphicsView)base.Owner;
            double rasterizationScale = nativeGraphicsView.XamlRoot.RasterizationScale;
            Microsoft.Maui.Graphics.Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(nativeGraphicsView, rasterizationScale);
            for (int i = 0; i < automationPeers.Count; i++)
            {
                ICustomAutomationPeer customAutomationPeer = (ICustomAutomationPeer)automationPeers[i];
                Microsoft.Maui.Graphics.Rect rect = new Microsoft.Maui.Graphics.Rect(customAutomationPeer.Node.Bounds.Left * rasterizationScale, customAutomationPeer.Node.Bounds.Top * rasterizationScale, customAutomationPeer.Node.Bounds.Width * rasterizationScale, customAutomationPeer.Node.Bounds.Height * rasterizationScale);
                if (new Microsoft.Maui.Graphics.Rect(viewStartPosition.X + rect.Left, viewStartPosition.Y + rect.Top, rect.Width, rect.Height).Contains(new Microsoft.Maui.Graphics.Point(pointInWindowCoordinates.X, pointInWindowCoordinates.Y)))
                {
                    return customAutomationPeer;
                }
            }
        }

        return base.GetElementFromPointCore(pointInWindowCoordinates);
    }
}
