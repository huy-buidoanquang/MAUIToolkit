using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Microsoft.UI.Composition;
using Microsoft.Graphics.DirectX;

namespace MAUIToolkit.Core.Controls.Popup
{
    class CompositionImageBrush : IDisposable
    {
        CompositionGraphicsDevice _graphicsDevice;
        CompositionDrawingSurface _drawingSurface;
        CompositionSurfaceBrush _drawingBrush;

        public CompositionBrush Brush => _drawingBrush;

        public CompositionImageBrush()
        {
        }

        void CreateDevice(Compositor compositor)
        {
            _graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(
                compositor, CanvasDevice.GetSharedDevice());
        }

        void CreateDrawingSurface(global::Windows.Foundation.Size drawSize)
        {
            _drawingSurface = _graphicsDevice.CreateDrawingSurface(
                drawSize,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);
        }

        void CreateSurfaceBrush(Compositor compositor)
        {
            _drawingBrush = compositor.CreateSurfaceBrush(_drawingSurface);
        }

        public static CompositionImageBrush FromBGRASoftwareBitmap(
            Compositor compositor,
            SoftwareBitmap bitmap,
            Windows.Foundation.Size outputSize)
        {
            CompositionImageBrush brush = new CompositionImageBrush();

            brush.CreateDevice(compositor);

            brush.CreateDrawingSurface(outputSize);
            brush.DrawSoftwareBitmap(bitmap, outputSize);
            brush.CreateSurfaceBrush(compositor);

            return (brush);
        }

        void DrawSoftwareBitmap(SoftwareBitmap softwareBitmap, Windows.Foundation.Size renderSize)
        {
            using (var drawingSession = CanvasComposition.CreateDrawingSession(_drawingSurface))
            using (var bitmap = CanvasBitmap.CreateFromSoftwareBitmap(drawingSession.Device, softwareBitmap))
            {
                drawingSession.DrawImage(bitmap,
                    new Windows.Foundation.Rect(0, 0, renderSize.Width, renderSize.Height));
            }
        }

        public void Dispose()
        {
            _drawingBrush.Dispose();
            _drawingSurface.Dispose();
            _graphicsDevice.Dispose();
        }
    }
}
