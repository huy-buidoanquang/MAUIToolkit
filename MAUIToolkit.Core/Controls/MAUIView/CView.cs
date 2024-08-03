using MAUIToolkit.Core.Handlers;
using MAUIToolkit.Core.Primitives;
using MAUIToolkit.Core.Semantics;
using Microsoft.Maui.Layouts;
using System.Collections;

namespace MAUIToolkit.Core.Controls;

public abstract partial class CView : Microsoft.Maui.Controls.View, IDrawableLayout, IDrawable, IAbsoluteLayout, Microsoft.Maui.ILayout, IView, IElement, ITransform, IContainer, IList<IView>, ICollection<IView>, IEnumerable<IView>, IEnumerable, ISafeAreaView, IPadding, ICrossPlatformLayout, IVisualTreeElement, ISemanticsProvider
{
    private readonly ILayoutManager layoutManager;

    private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

    //
    // Summary:
    //     This property is used to ignore the safe area.
    private bool ignoreSafeArea = false;

    private bool clipToBounds = true;

    private Thickness padding = new Thickness(0.0);

    private readonly List<IView> children = new List<IView>();

    private CViewHandler? LayoutHandler => base.Handler as CViewHandler;

    internal DrawingOrder DrawingOrder
    {
        get
        {
            return drawingOrder;
        }
        set
        {
            drawingOrder = value;
            LayoutHandler?.SetDrawingOrder(value);
        }
    }

    //
    // Summary:
    //     This property is used to ignore the safe area.
    internal bool IgnoreSafeArea
    {
        get
        {
            return ignoreSafeArea;
        }
        set
        {
            ignoreSafeArea = value;
        }
    }

    public IList<IView> Children => this;

    public bool ClipToBounds
    {
        get
        {
            return clipToBounds;
        }
        set
        {
            clipToBounds = value;
            LayoutHandler?.UpdateClipToBounds(value);
        }
    }

    public Thickness Padding
    {
        get
        {
            return padding;
        }
        set
        {
            padding = value;
            MeasureContent(base.Width, base.Height);
            ArrangeContent(base.Bounds);
        }
    }

    bool Microsoft.Maui.ILayout.ClipsToBounds => ClipToBounds;

    int ICollection<IView>.Count => children.Count;

    bool ICollection<IView>.IsReadOnly => ((ICollection<IView>)children).IsReadOnly;

    bool ISafeAreaView.IgnoreSafeArea => IgnoreSafeArea;

    Thickness IPadding.Padding => Padding;

    DrawingOrder IDrawableLayout.DrawingOrder
    {
        get
        {
            return DrawingOrder;
        }
        set
        {
            DrawingOrder = value;
        }
    }

    IView IList<IView>.this[int index]
    {
        get
        {
            return children[index];
        }
        set
        {
            children[index] = value;
        }
    }

    IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren()
    {
        return Children.Cast<IVisualTreeElement>().ToList().AsReadOnly();
    }

    public CView()
    {
        layoutManager = new DrawableLayoutManager(this);
    }

    //
    // Parameters:
    //   canvas:
    //
    //   dirtyRect:
    protected virtual void OnDraw(ICanvas canvas, RectF dirtyRect)
    {
    }

    //
    // Parameters:
    //   widthConstraint:
    //
    //   heightConstraint:
    protected virtual Size MeasureContent(double widthConstraint, double heightConstraint)
    {
        return layoutManager.Measure(widthConstraint, heightConstraint);
    }

    //
    // Parameters:
    //   bounds:
    protected virtual Size ArrangeContent(Rect bounds)
    {
        return layoutManager.ArrangeChildren(bounds);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        foreach (IView child in children)
        {
            if (child is BindableObject view)
            {
                UpdateBindingContextToChild(view);
            }
        }
    }

    //
    // Parameters:
    //   widthConstraint:
    //
    //   heightConstraint:
    protected sealed override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        return base.MeasureOverride(widthConstraint, heightConstraint);
    }

    //
    // Parameters:
    //   bounds:
    protected sealed override Size ArrangeOverride(Rect bounds)
    {
        return base.ArrangeOverride(bounds);
    }

    //
    // Parameters:
    //   widthConstraint:
    //
    //   heightConstraint:
    protected sealed override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
    {
        return base.OnMeasure(widthConstraint, heightConstraint);
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (base.Handler != null)
        {
            LayoutHandler?.SetDrawingOrder(DrawingOrder);
            LayoutHandler?.UpdateClipToBounds(ClipToBounds);
            LayoutHandler?.Invalidate();
        }
    }

    //
    // Parameters:
    //   view:
    internal void Add(Microsoft.Maui.Controls.View view)
    {
        ((ICollection<IView>)this).Add((IView)view);
    }

    //
    // Parameters:
    //   view:
    private void UpdateBindingContextToChild(BindableObject view)
    {
        BindableObject.SetInheritedBindingContext(view, base.BindingContext);
    }

    //
    // Parameters:
    //   view:
    internal void Remove(Microsoft.Maui.Controls.View view)
    {
        ((ICollection<IView>)this).Remove((IView)view);
    }

    //
    // Parameters:
    //   index:
    //
    //   view:
    internal void Insert(int index, Microsoft.Maui.Controls.View view)
    {
        ((IList<IView>)this).Insert(index, (IView)view);
    }

    internal void Clear()
    {
        ((ICollection<IView>)this).Clear();
    }

    internal void InvalidateDrawable()
    {
        LayoutHandler?.Invalidate();
    }

    void IDrawableLayout.InvalidateDrawable()
    {
        InvalidateDrawable();
    }

    Size Microsoft.Maui.ILayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
    {
        return MeasureContent(widthConstraint, heightConstraint);
    }

    Size Microsoft.Maui.ILayout.CrossPlatformArrange(Rect bounds)
    {
        return ArrangeContent(bounds);
    }

    int IList<IView>.IndexOf(IView child)
    {
        return children.IndexOf(child);
    }

    void IList<IView>.Insert(int index, IView child)
    {
        children.Insert(index, child);
        LayoutHandler?.Insert(index, child);
        if (child is Element child2)
        {
            OnChildAdded(child2);
        }
    }

    void IList<IView>.RemoveAt(int index)
    {
        IView view = children[index];
        LayoutHandler?.Remove(children[index]);
        children.RemoveAt(index);
        if (view is Element child)
        {
            OnChildRemoved(child, index);
        }
    }

    void ICollection<IView>.Add(IView child)
    {
        children.Add(child);
        LayoutHandler?.Add(child);
        if (child is Element child2)
        {
            OnChildAdded(child2);
        }
    }

    void ICollection<IView>.Clear()
    {
        children.Clear();
        LayoutHandler?.Clear();
    }

    bool ICollection<IView>.Contains(IView child)
    {
        return children.Contains(child);
    }

    void ICollection<IView>.CopyTo(IView[] array, int arrayIndex)
    {
        children.CopyTo(array, arrayIndex);
    }

    bool ICollection<IView>.Remove(IView child)
    {
        int oldLogicalIndex = children.IndexOf(child);
        LayoutHandler?.Remove(child);
        bool result = children.Remove(child);
        if (child is Element child2)
        {
            OnChildRemoved(child2, oldLogicalIndex);
        }

        return result;
    }

    IEnumerator<IView> IEnumerable<IView>.GetEnumerator()
    {
        return children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return children.GetEnumerator();
    }

    void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
    {
        OnDraw(canvas, dirtyRect);
    }

    Rect IAbsoluteLayout.GetLayoutBounds(IView view)
    {
        BindableObject bindableObject = (BindableObject)view;
        return (Rect)bindableObject.GetValue(AbsoluteLayout.LayoutBoundsProperty);
    }

    AbsoluteLayoutFlags IAbsoluteLayout.GetLayoutFlags(IView view)
    {
        BindableObject bindableObject = (BindableObject)view;
        return (AbsoluteLayoutFlags)bindableObject.GetValue(AbsoluteLayout.LayoutFlagsProperty);
    }

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
    List<SemanticsNode>? ISemanticsProvider.GetSemanticsNodes(double width, double height)
    {
        return GetSemanticsNodesCore(width, height);
    }

    //
    // Summary:
    //     Used to scroll the view based on the node position while the view inside the
    //     scroll view.
    //
    // Parameters:
    //   node:
    //     Current navigated semantics node.
    void ISemanticsProvider.ScrollTo(SemanticsNode node)
    {
        ScrollToCore(node);
    }

    //
    // Summary:
    //     Invalidates the semantics nodes.
    internal void InvalidateSemantics()
    {
        LayoutHandler?.InvalidateSemantics();
    }

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
    protected virtual List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
    {
        return null;
    }

    //
    // Summary:
    //     Used to scroll the view based on the node position while the view inside the
    //     scroll view.
    //
    // Parameters:
    //   node:
    //     Current navigated semantics node.
    protected virtual void ScrollToCore(SemanticsNode node)
    {
    }
}
