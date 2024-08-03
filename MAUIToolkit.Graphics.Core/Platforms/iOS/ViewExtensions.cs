using UIKit;

namespace MAUIToolkit.Graphics.Core
{
    public static class ViewExtensions
    {
        public static void UpdateIsEnabled(this UIView platformView, IView view)
        {
            platformView.UserInteractionEnabled = view.IsEnabled;
        }
    }
}
