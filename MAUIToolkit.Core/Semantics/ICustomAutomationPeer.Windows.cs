using MAUIToolkit.Core.Primitives;
using Microsoft.UI.Xaml.Automation.Peers;

namespace MAUIToolkit.Core.Semantics;

internal interface ICustomAutomationPeer
{
    //
    // Summary:
    //     Holds the previous sibling of the automation peer.
    AutomationPeer? PrevSibling { get; set; }

    //
    // Summary:
    //     Holds the previous sibling node information.
    SemanticsNode? PrevNode { get; set; }

    //
    // Summary:
    //     Hold the next sibling if the automation peer.
    AutomationPeer? NextSibling { get; set; }

    //
    // Summary:
    //     Holds the next sibling node information.
    SemanticsNode? NextNode { get; set; }

    //
    // Summary:
    //     Used to get the node information.
    SemanticsNode Node { get; }

    //
    // Summary:
    //     Holds the parent MauiView instance for scroll action.
    View MauiView { get; set; }
}
