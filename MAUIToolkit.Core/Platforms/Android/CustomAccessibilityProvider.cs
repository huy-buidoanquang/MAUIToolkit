using Android.OS;
using Android.Views;
using Android.Views.Accessibility;
using AndroidX.Core.View;
using AndroidX.Core.View.Accessibility;
using MAUIToolkit.Core.Primitives;
using MAUIToolkit.Core.Semantics;
using Microsoft.Maui.Platform;
using Rect = Android.Graphics.Rect;

namespace MAUIToolkit.Core.Platforms;

internal class CustomAccessibilityProvider : AccessibilityNodeProviderCompat
{
    //
    // Summary:
    //     The id value denotes the android view .
    private const int Root_ID = -1;

    //
    // Summary:
    //     The id value denotes the accessibility node between the android view and its
    //     children. It holds the drawn UI nodes as children. host node is needed because
    //     provider cannot differentiate the root node and its children(not drawn view)
    //     All android view(root and its children) id is -1.
    private const int Host_ID = 2222;

    //
    // Summary:
    //     Current focused(highlighted) node id.
    private int focusedVirtualID = -1;

    //
    // Summary:
    //     Hovered accessibility node id.
    private int hoveredVirtualID = -1;

    //
    // Summary:
    //     Native android view.
    private Android.Views.View host;

    //
    // Summary:
    //     Maui view instance.
    private Microsoft.Maui.Controls.View virtualView;

    //
    // Summary:
    //     Children is important means the children gets the accessibility event while the
    //     drawn UI and children are overlapped.
    private bool isChildNode;

    internal CustomAccessibilityProvider(Android.Views.View host, Microsoft.Maui.Controls.View virtualView, bool isChildNode)
    {
        this.host = host;
        this.virtualView = virtualView;
        this.isChildNode = isChildNode;
    }

    //
    // Summary:
    //     Invalidate while the child drawing order changed.
    //
    // Parameters:
    //   isChildNode:
    //     Is children important
    internal void UpdateChildOrder(bool isChildNode)
    {
        if (this.isChildNode != isChildNode)
        {
            this.isChildNode = isChildNode;
            InvalidateSemantics();
        }
    }

    //
    // Summary:
    //     Clear and create the children of accessibility node.
    internal void InvalidateSemantics()
    {
        SendEventForVirtualViewId(2222, EventTypes.WindowContentChanged, null);
    }

    //
    // Summary:
    //     Create the accessibility node of the virtual id.
    //
    // Parameters:
    //   virtualViewId:
    //     View id value.
    //
    // Returns:
    //     Accessibility node.
    public override AccessibilityNodeInfoCompat? CreateAccessibilityNodeInfo(int virtualViewId)
    {
        if (virtualViewId == -1)
        {
            AccessibilityNodeInfoCompat accessibilityNodeInfoCompat = AccessibilityNodeInfoCompat.Obtain(host);
            AccessibilityNodeInfoCompat accessibilityNodeInfoCompat2 = AccessibilityNodeInfoCompat.Obtain(host);
            if (accessibilityNodeInfoCompat == null || accessibilityNodeInfoCompat2 == null)
            {
                return null;
            }

            ViewCompat.OnInitializeAccessibilityNodeInfo(host, accessibilityNodeInfoCompat2);
            Rect rect = new Rect();
            Rect rect2 = new Rect();
            accessibilityNodeInfoCompat2.GetBoundsInParent(rect);
            accessibilityNodeInfoCompat2.GetBoundsInScreen(rect2);
            accessibilityNodeInfoCompat.SetBoundsInParent(rect);
            accessibilityNodeInfoCompat.SetBoundsInScreen(rect2);
            IViewParent parentForAccessibility = ViewCompat.GetParentForAccessibility(host);
            if (parentForAccessibility != null && parentForAccessibility is Android.Views.View)
            {
                accessibilityNodeInfoCompat.SetParent((Android.Views.View)parentForAccessibility);
            }

            accessibilityNodeInfoCompat.VisibleToUser = accessibilityNodeInfoCompat2.VisibleToUser;
            accessibilityNodeInfoCompat.PackageName = accessibilityNodeInfoCompat2.PackageName;
            accessibilityNodeInfoCompat.ClassName = accessibilityNodeInfoCompat2.ClassName;
            accessibilityNodeInfoCompat.AddChild(host, 2222);
            return accessibilityNodeInfoCompat;
        }

        if (virtualViewId == 2222)
        {
            AccessibilityNodeInfoCompat accessibilityNodeInfoCompat3 = AccessibilityNodeInfoCompat.Obtain(host, virtualViewId);
            if (accessibilityNodeInfoCompat3 == null)
            {
                return null;
            }

            ViewCompat.OnInitializeAccessibilityNodeInfo(host, accessibilityNodeInfoCompat3);
            List<SemanticsNode> list = null;
            if (virtualView is ISemanticsProvider)
            {
                list = ((ISemanticsProvider)virtualView).GetSemanticsNodes(virtualView.Width, virtualView.Height);
            }

            List<Android.Views.View> list2 = new List<Android.Views.View>();
            AddAccessibilityChildren(host, list2);
            if (isChildNode)
            {
                foreach (Android.Views.View item in list2)
                {
                    accessibilityNodeInfoCompat3.AddChild(item);
                }
            }

            if (list != null)
            {
                foreach (SemanticsNode item2 in list)
                {
                    accessibilityNodeInfoCompat3.AddChild(host, item2.Id);
                }
            }

            if (!isChildNode)
            {
                foreach (Android.Views.View item3 in list2)
                {
                    accessibilityNodeInfoCompat3.AddChild(item3);
                }
            }

            accessibilityNodeInfoCompat3.SetParent(host);
            accessibilityNodeInfoCompat3.SetSource(host, 2222);
            return accessibilityNodeInfoCompat3;
        }

        List<SemanticsNode> list3 = null;
        if (virtualView is ISemanticsProvider)
        {
            list3 = ((ISemanticsProvider)virtualView).GetSemanticsNodes(virtualView.Width, virtualView.Height);
        }

        SemanticsNode semanticsNode = null;
        if (list3 != null)
        {
            semanticsNode = list3.FirstOrDefault((SemanticsNode info) => info.Id == virtualViewId);
        }

        if (semanticsNode != null)
        {
            AccessibilityNodeInfoCompat accessibilityNodeInfoCompat4 = AccessibilityNodeInfoCompat.Obtain();
            if (accessibilityNodeInfoCompat4 == null)
            {
                return null;
            }

            accessibilityNodeInfoCompat4.Enabled = true;
            accessibilityNodeInfoCompat4.ClassName = host.Class.Name;
            accessibilityNodeInfoCompat4.ContentDescription = semanticsNode.Text;
            Func<double, float> func = Android.App.Application.Context.ToPixels;
            Rect rect3 = new Rect();
            rect3.Left = (int)func(semanticsNode.Bounds.X);
            rect3.Top = (int)func(semanticsNode.Bounds.Y);
            rect3.Right = (int)func(semanticsNode.Bounds.X + semanticsNode.Bounds.Width);
            rect3.Bottom = (int)func(semanticsNode.Bounds.Y + semanticsNode.Bounds.Height);
            accessibilityNodeInfoCompat4.SetBoundsInParent(rect3);
            if (semanticsNode.IsTouchEnabled)
            {
                accessibilityNodeInfoCompat4.AddAction(AccessibilityNodeInfoCompat.AccessibilityActionCompat.ActionClick);
            }

            accessibilityNodeInfoCompat4.PackageName = host.Context?.PackageName;
            accessibilityNodeInfoCompat4.SetParent(host, 2222);
            accessibilityNodeInfoCompat4.SetSource(host, virtualViewId);
            if (focusedVirtualID == virtualViewId)
            {
                accessibilityNodeInfoCompat4.AccessibilityFocused = true;
                accessibilityNodeInfoCompat4.AddAction(AccessibilityNodeInfoCompat.AccessibilityActionCompat.ActionClearAccessibilityFocus);
            }
            else
            {
                accessibilityNodeInfoCompat4.AccessibilityFocused = false;
                accessibilityNodeInfoCompat4.AddAction(AccessibilityNodeInfoCompat.AccessibilityActionCompat.ActionAccessibilityFocus);
            }

            Rect rect4 = new Rect();
            accessibilityNodeInfoCompat4.GetBoundsInParent(rect4);
            if (IntersectVisibleToUser(rect4))
            {
                accessibilityNodeInfoCompat4.VisibleToUser = true;
                accessibilityNodeInfoCompat4.SetBoundsInParent(rect4);
            }

            accessibilityNodeInfoCompat4.VisibleToUser = true;
            accessibilityNodeInfoCompat4.Focusable = true;
            int[] array = new int[2];
            host.GetLocationOnScreen(array);
            Rect rect5 = new Rect();
            rect5.Set(rect4);
            rect5.Offset(array[0], array[1]);
            accessibilityNodeInfoCompat4.SetBoundsInScreen(rect5);
            return accessibilityNodeInfoCompat4;
        }

        return base.CreateAccessibilityNodeInfo(virtualViewId);
    }

    private bool IntersectVisibleToUser(Rect localRect)
    {
        if (localRect.IsEmpty)
        {
            return false;
        }

        if (host.WindowVisibility != 0)
        {
            return false;
        }

        Rect r = new Rect();
        if (!host.GetLocalVisibleRect(r))
        {
            return false;
        }

        return localRect.Intersect(r);
    }

    //
    // Summary:
    //     Check the native view children have hovering.
    //
    // Parameters:
    //   p0:
    //     x position value.
    //
    //   p1:
    //     y position value
    //
    //   view:
    //     Android view
    //
    // Returns:
    //     the view have hovering point.
    private bool HandleChildHover(float p0, float p1, Android.Views.View view)
    {
        ViewGroup viewGroup = (ViewGroup)view;
        if (viewGroup == null)
        {
            return false;
        }

        for (int i = 0; i < viewGroup.ChildCount; i++)
        {
            Android.Views.View childAt = viewGroup.GetChildAt(i);
            if (childAt == null || childAt.Visibility != 0)
            {
                continue;
            }

            if (childAt.IsImportantForAccessibility)
            {
                int[] array = new int[2];
                childAt.GetLocationOnScreen(array);
                Rect rect = new Rect(array[0], array[1], array[0] + childAt.Width, array[1] + childAt.Height);
                if (rect.Contains((int)p0, (int)p1))
                {
                    return true;
                }
            }
            else if (childAt is ViewGroup && HandleChildHover(p0, p1, childAt))
            {
                return true;
            }
        }

        return false;
    }

    //
    // Summary:
    //     Handle the hovering children.
    //
    // Parameters:
    //   e:
    internal bool DispatchHoverEvent(MotionEvent e)
    {
        if (HandleChildHover(e.GetX(), e.GetY(), host))
        {
            return false;
        }

        int virtualViewAt = GetVirtualViewAt(e.GetX(), e.GetY());
        switch (e.Action)
        {
            case MotionEventActions.HoverMove:
            case MotionEventActions.HoverEnter:
                UpdateHoveredVirtualViewId(virtualViewAt);
                return virtualViewAt != -1;
            case MotionEventActions.HoverExit:
                if (hoveredVirtualID != -1)
                {
                    UpdateHoveredVirtualViewId(-1);
                    return true;
                }

                return false;
            default:
                return false;
        }
    }

    //
    // Summary:
    //     Return the virtual id for the virtual view placed on the point.
    //
    // Parameters:
    //   p0:
    //     x position.
    //
    //   p1:
    //     Y position.
    //
    // Returns:
    //     Virtual view id.
    private int GetVirtualViewAt(float p0, float p1)
    {
        List<SemanticsNode> list = null;
        if (virtualView is ISemanticsProvider)
        {
            list = ((ISemanticsProvider)virtualView).GetSemanticsNodes(virtualView.Width, virtualView.Height);
        }

        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                SemanticsNode semanticsNode = list[i];
                Func<double, float> func = Android.App.Application.Context.ToPixels;
                Rect rect = new Rect();
                rect.Left = (int)func(semanticsNode.Bounds.X);
                rect.Top = (int)func(semanticsNode.Bounds.Y);
                rect.Right = (int)func(semanticsNode.Bounds.X + semanticsNode.Bounds.Width);
                rect.Bottom = (int)func(semanticsNode.Bounds.Y + semanticsNode.Bounds.Height);
                if (rect.Contains((int)p0, (int)p1))
                {
                    return semanticsNode.Id;
                }
            }
        }

        return -1;
    }

    //
    // Summary:
    //     Update and trigger hover event for virtual view id.
    //
    // Parameters:
    //   virtualViewId:
    //     Virtual view id value.
    private void UpdateHoveredVirtualViewId(int virtualViewId)
    {
        if (hoveredVirtualID == virtualViewId)
        {
            return;
        }

        int previousVirtualViewId = hoveredVirtualID;
        hoveredVirtualID = virtualViewId;
        List<SemanticsNode> list = null;
        if (virtualView is ISemanticsProvider)
        {
            list = ((ISemanticsProvider)virtualView).GetSemanticsNodes(virtualView.Width, virtualView.Height);
        }

        if (virtualViewId != -1)
        {
            SemanticsNode semanticsNode = list?.FirstOrDefault((SemanticsNode info) => info.Id == virtualViewId);
            SendEventForVirtualViewId(virtualViewId, EventTypes.ViewHoverEnter, semanticsNode);
        }

        if (previousVirtualViewId != -1)
        {
            SemanticsNode semanticsNode2 = list?.FirstOrDefault((SemanticsNode info) => info.Id == previousVirtualViewId);
            SendEventForVirtualViewId(previousVirtualViewId, EventTypes.ViewHoverExit, semanticsNode2);
        }
    }

    //
    // Summary:
    //     Add accessibility for children of the android view.
    //
    // Parameters:
    //   host:
    //
    //   children:
    private void AddAccessibilityChildren(Android.Views.View host, List<Android.Views.View> children)
    {
        ViewGroup viewGroup = (ViewGroup)host;
        if (viewGroup == null)
        {
            return;
        }

        for (int i = 0; i < viewGroup.ChildCount; i++)
        {
            Android.Views.View childAt = viewGroup.GetChildAt(i);
            if (childAt != null && childAt.Visibility == ViewStates.Visible)
            {
                if (childAt.IsImportantForAccessibility)
                {
                    children.Add(childAt);
                }
                else if (childAt is ViewGroup)
                {
                    AddAccessibilityChildren(childAt, children);
                }
            }
        }
    }

    //
    // Summary:
    //     Performs the specified accessibility action on the view
    //
    // Parameters:
    //   virtualViewId:
    //     Virtual view id value.
    //
    //   action:
    //     Action performed.
    //
    //   arguments:
    //     Action arguments.
    public override bool PerformAction(int virtualViewId, int action, Bundle? arguments)
    {
        List<SemanticsNode> list = null;
        if (virtualView is ISemanticsProvider)
        {
            list = ((ISemanticsProvider)virtualView).GetSemanticsNodes(virtualView.Width, virtualView.Height);
        }

        SemanticsNode semanticsNode = list?.FirstOrDefault((SemanticsNode info) => info.Id == virtualViewId);
        if (semanticsNode != null || virtualViewId == 2222)
        {
            switch (action)
            {
                case 64:
                    {
                        bool result = false;
                        if (focusedVirtualID != virtualViewId)
                        {
                            focusedVirtualID = virtualViewId;
                            host.Invalidate();
                            SendEventForVirtualViewId(virtualViewId, EventTypes.ViewAccessibilityFocused, semanticsNode);
                            result = true;
                        }

                        return result;
                    }
                case 128:
                    if (focusedVirtualID == virtualViewId)
                    {
                        focusedVirtualID = -1;
                    }

                    host.Invalidate();
                    SendEventForVirtualViewId(virtualViewId, EventTypes.ViewAccessibilityFocusCleared, semanticsNode);
                    return true;
                default:
                    if ((action == 16 && semanticsNode != null && semanticsNode.IsTouchEnabled) || virtualViewId == 2222)
                    {
                        return ViewCompat.PerformAccessibilityAction(host, action, arguments);
                    }

                    if (virtualViewId != 2222)
                    {
                        return false;
                    }

                    return base.PerformAction(virtualViewId, action, arguments);
            }
        }

        return base.PerformAction(virtualViewId, action, arguments);
    }

    //
    // Summary:
    //     Populates an event of the specified type with information about an item and attempts
    //     to send it up through the view hierarchy. Should call this method after performing
    //     a user action that normally fires an accessibility event
    //
    // Parameters:
    //   virtualViewId:
    //
    //   eventType:
    //
    //   semanticsNode:
    internal void SendEventForVirtualViewId(int virtualViewId, EventTypes eventType, SemanticsNode? semanticsNode)
    {
        if (virtualViewId == -1)
        {
            return;
        }

        ViewGroup viewGroup = (ViewGroup)host.Parent;
        if (viewGroup == null)
        {
            return;
        }

        AccessibilityEvent accessibilityEvent;
        if (virtualViewId == 2222)
        {
            accessibilityEvent = GetAccessibilityEvent(eventType);
            host.OnInitializeAccessibilityEvent(accessibilityEvent);
        }
        else
        {
            if (semanticsNode == null)
            {
                return;
            }

            accessibilityEvent = GetAccessibilityEvent(eventType);
            if (accessibilityEvent != null)
            {
                accessibilityEvent.Enabled = true;
                accessibilityEvent.ClassName = host.Class.Name;
                accessibilityEvent.ContentDescription = semanticsNode.Text;
                accessibilityEvent.PackageName = host.Context?.PackageName;
                accessibilityEvent.SetSource(host, virtualViewId);
            }
        }

        viewGroup.RequestSendAccessibilityEvent(host, accessibilityEvent);
    }

    //
    // Summary:
    //     Create the accessibility event based on android API version.
    //
    // Parameters:
    //   eventTypes:
    //     Event type.
    private AccessibilityEvent? GetAccessibilityEvent(EventTypes eventTypes)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            AccessibilityEvent accessibilityEvent = new AccessibilityEvent();
            accessibilityEvent.EventType = eventTypes;
            return accessibilityEvent;
        }

        AccessibilityEvent accessibilityEvent2 = AccessibilityEvent.Obtain();
        if (accessibilityEvent2 != null)
        {
            accessibilityEvent2.EventType = eventTypes;
        }

        return accessibilityEvent2;
    }
}
