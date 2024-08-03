using Android.Views;
using Android.Views.Accessibility;
using AndroidX.Core.View;
using AndroidX.Core.View.Accessibility;

namespace MAUIToolkit.Core.Platforms;

internal class CustomAccessibilityDelegate : AccessibilityDelegateCompat
{
    private CustomAccessibilityProvider accessibilityNodeProvider;

    internal CustomAccessibilityDelegate(Android.Views.View host, Microsoft.Maui.Controls.View virtualView, bool isChildrenImportant)
    {
        accessibilityNodeProvider = new CustomAccessibilityProvider(host, virtualView, isChildrenImportant);
    }

    public override AccessibilityNodeProviderCompat GetAccessibilityNodeProvider(Android.Views.View host)
    {
        return accessibilityNodeProvider;
    }

    //
    // Summary:
    //     Check the hovering triggered for the drawn UI.
    //
    // Parameters:
    //   e:
    internal bool DispatchHoverEvent(MotionEvent? e)
    {
        if (e == null)
        {
            return false;
        }

        return accessibilityNodeProvider.DispatchHoverEvent(e);
    }

    //
    // Summary:
    //     Update the drawing order(like above children or below children).
    //
    // Parameters:
    //   isChildrenImportant:
    internal void UpdateChildOrder(bool isChildrenImportant)
    {
        accessibilityNodeProvider.UpdateChildOrder(isChildrenImportant);
    }

    //
    // Summary:
    //     Clear and create the children of accessibility node.
    internal void InvalidateSemantics()
    {
        accessibilityNodeProvider.InvalidateSemantics();
    }

    //
    // Summary:
    //     Dispatch the accessibility event to process its children for adding their text
    //     for the event.
    //
    // Parameters:
    //   host:
    //
    //   e:
    public override bool DispatchPopulateAccessibilityEvent(Android.Views.View host, AccessibilityEvent e)
    {
        if (host.IsImportantForAccessibility && base.DispatchPopulateAccessibilityEvent(host, e))
        {
            return true;
        }

        ViewGroup viewGroup = (ViewGroup)host;
        if (viewGroup != null)
        {
            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                Android.Views.View childAt = viewGroup.GetChildAt(i);
                if (childAt != null && childAt.Visibility == ViewStates.Visible && childAt.DispatchPopulateAccessibilityEvent(e))
                {
                    return true;
                }
            }
        }

        return base.DispatchPopulateAccessibilityEvent(host, e);
    }
}
