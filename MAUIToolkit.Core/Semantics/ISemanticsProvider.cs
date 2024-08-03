using MAUIToolkit.Core.Primitives;

namespace MAUIToolkit.Core.Semantics;

public interface ISemanticsProvider
{
    //
    // Summary:
    //     Return the semantics nodes for the view.
    //
    // Parameters:
    //   width:
    //     The view width.
    //
    //   height:
    //     The view height.
    //
    // Returns:
    //     The semantics nodes of the view.
    List<SemanticsNode>? GetSemanticsNodes(double width, double height);

    //
    // Summary:
    //     Used to scroll the view based on the node position while the view inside the
    //     scroll view.
    //
    // Parameters:
    //   node:
    //     Current navigated semantics node.
    void ScrollTo(SemanticsNode node);
}
