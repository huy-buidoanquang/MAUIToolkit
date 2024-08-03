namespace MAUIToolkit.Core.Primitives;

//
// Summary:
//     Holds the semantics node information.
public class SemanticsNode
{
    //
    // Summary:
    //     Gets and sets the bounds for semantics node.
    public Rect Bounds = Rect.Zero;

    //
    // Summary:
    //     Gets and sets the text or hint for semantics node.
    public string Text = string.Empty;

    //
    // Summary:
    //     Gets and sets the unique id for semantics node.
    public int Id;

    //
    // Summary:
    //     Gets and sets to define the semantics node is interactive or not.
    public bool IsTouchEnabled;

    //
    // Summary:
    //     Gets and sets the action need to be performed on semantics node interaction.
    public Action<SemanticsNode>? OnClick;
}
