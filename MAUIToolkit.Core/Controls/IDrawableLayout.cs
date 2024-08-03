using System.Collections;

namespace MAUIToolkit.Core;

public interface IDrawableLayout : IDrawable, IAbsoluteLayout, Microsoft.Maui.ILayout, IView, IElement, ITransform, IContainer, IList<IView>, ICollection<IView>, IEnumerable<IView>, IEnumerable, ISafeAreaView, IPadding, ICrossPlatformLayout
{
    DrawingOrder DrawingOrder { get; set; }

    void InvalidateDrawable();
}
