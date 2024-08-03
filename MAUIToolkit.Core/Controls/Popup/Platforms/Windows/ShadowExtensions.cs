using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Imaging;

namespace MAUIToolkit.Core.Controls.Popup;

internal static class ShadowExtensions
{
    public static async Task<CompositionBrush> GetAlphaMaskAsync(this UIElement element)
    {
        CompositionBrush mask = null;

        try
        {
            //For some reason, using  TextBlock and getting the AlphaMask
            //generates a shadow with a size more smaller than the control size. 
            if (element is TextBlock textElement)
            {
                return textElement.GetAlphaMask();
            }
            if (element is Microsoft.UI.Xaml.Controls.Image image)
            {
                return image.GetAlphaMask();
            }
            if (element is Microsoft.UI.Xaml.Shapes.Shape shape)
            {
                return shape.GetAlphaMask();
            }
            else if (element is FrameworkElement frameworkElement)
            {
                var height = (int)frameworkElement.ActualHeight;
                var width = (int)frameworkElement.ActualWidth;

                if (height > 0 && width > 0)
                {
                    var visual = ElementCompositionPreview.GetElementVisual(element);
                    var elementVisual = visual.Compositor.CreateSpriteVisual();
                    elementVisual.Size = element.RenderSize.ToVector2();
                    var bitmap = new RenderTargetBitmap();

                    await bitmap.RenderAsync(
                        element,
                        width,
                        height);

                    var pixels = await bitmap.GetPixelsAsync();

                    using (var softwareBitmap = SoftwareBitmap.CreateCopyFromBuffer(
                        pixels,
                        BitmapPixelFormat.Bgra8,
                        bitmap.PixelWidth,
                        bitmap.PixelHeight,
                        BitmapAlphaMode.Premultiplied))
                    {
                        var brush = CompositionImageBrush.FromBGRASoftwareBitmap(
                            visual.Compositor,
                            softwareBitmap,
                            new Windows.Foundation.Size(bitmap.PixelWidth, bitmap.PixelHeight));
                        mask = brush.Brush;
                    }
                }
            }
        }
        catch (Exception exc)
        {
            Debug.WriteLine($"Failed to get AlphaMask {exc}");
            mask = null;
        }

        return mask;
    }
}
