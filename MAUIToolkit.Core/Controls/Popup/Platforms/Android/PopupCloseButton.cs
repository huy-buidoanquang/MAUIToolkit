using MAUIToolkit.Core.ContentView;
using MAUIToolkit.Core.Internals;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     View that holds the close button in header.
internal class PopupCloseButton : CContentView, ITouchListener
{
    //
    // Summary:
    //     Gets PopupView's instance.
    internal PopupView? PopupView;

    //
    // Summary:
    //     Gets the close button image for popup view.
    private PopupImage? closeButtonImage;

    //
    // Summary:
    //     Gets the width of the close button in the header.
    private int closeButtonWidth = 40;

    //
    // Summary:
    //     Gets the height of the close button in the header.
    private int closeButtonHeight = 40;

    //
    // Summary:
    //     Gets whether the mouse hover effect is add to the close button.
    private bool isHover = false;

    //
    // Summary:
    //     Gets whether the pressed effect is add to the close button.
    private bool isPressed = false;

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupCloseButton class.
    public PopupCloseButton()
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupCloseButton class.
    //
    //
    // Parameters:
    //   popupView:
    //     The instance of PopupView.
    public PopupCloseButton(PopupView popupView)
    {
        PopupView = popupView;
        Initialize();
    }

    //
    // Summary:
    //     Raises when mouse hover on element.
    //
    // Parameters:
    //   e:
    //     Represents the event data.
    public void OnTouch(Internals.PointerEventArgs e)
    {
        if (PopupView.Popup.PopupStyle.CloseButtonIcon == null)
        {
            if (e.Action == PointerActions.Entered)
            {
                isHover = true;
                InvalidateDrawable();
            }
            else if (e.Action == PointerActions.Exited)
            {
                isHover = false;
                InvalidateDrawable();
            }
            else if (e.Action == PointerActions.Pressed)
            {
                isPressed = true;
                InvalidateDrawable();
            }
            else if (e.Action == PointerActions.Released)
            {
                isPressed = false;
                isHover = false;
                InvalidateDrawable();
            }
        }
    }

    //
    // Summary:
    //     Update the content for Popup close button when CloseButtonIcon is not null.
    internal void UpdatePopupCloseButtonContent()
    {
        if (PopupView.Popup.PopupStyle.CloseButtonIcon != null && closeButtonImage.Source != PopupView.Popup.PopupStyle.CloseButtonIcon)
        {
            closeButtonImage.IsVisible = true;
            closeButtonImage.Source = PopupView.Popup.PopupStyle.CloseButtonIcon;
            base.Content = closeButtonImage;
        }
    }

    //
    // Summary:
    //     Draw method.
    //
    // Parameters:
    //   canvas:
    //     The Canvas.
    //
    //   rectF:
    //     The rectangle.
    protected override void OnDraw(ICanvas canvas, RectF rectF)
    {
        rectF.Width = 14f;
        rectF.Height = 14f;
        rectF.X = 13f;
        rectF.Y = 13f;
        if (PopupView.Popup.PopupStyle.CloseButtonIcon == null)
        {
            closeButtonImage.IsVisible = false;
            canvas.StrokeColor = PopupView.Popup.PopupStyle.GetCloseButtonIconStroke();
            canvas.StrokeSize = (float)PopupView.Popup.PopupStyle.GetCloseButtonIconStrokeThickness();
            PointF point = new Point(0.0, 0.0);
            PointF point2 = new Point(0.0, 0.0);
            point.X = rectF.X;
            point.Y = rectF.Y;
            point2.X = rectF.X + rectF.Width;
            point2.Y = rectF.Y + rectF.Height;
            canvas.DrawLine(point, point2);
            point.X = rectF.X;
            point.Y = rectF.Y + rectF.Height;
            point2.X = rectF.X + rectF.Width;
            point2.Y = rectF.Y;
            canvas.DrawLine(point, point2);
            DrawHoverHighlight(canvas, new RectF(0f, 0f, closeButtonWidth, closeButtonHeight));
            DrawPressedHighlight(canvas, new RectF(0f, 0f, closeButtonWidth, closeButtonHeight));
        }
    }

    //
    // Summary:
    //     Add the tap gesture for popup close button.
    private void AddTapGestureForPopupCloseButton()
    {
        TapGestureRecognizer item = new TapGestureRecognizer
        {
            Command = new Command(OnCloseButtonTapped)
        };
        base.GestureRecognizers.Add(item);
    }

    //
    // Summary:
    //     Triggered when click the close icon.
    private void OnCloseButtonTapped()
    {
        if (PopupView != null)
        {
            PopupView.Popup.IsOpen = false;
        }
    }

    //
    // Summary:
    //     Used to initialize the popup close button.
    private void Initialize()
    {
        AddTapGestureForPopupCloseButton();
        base.DrawingOrder = DrawingOrder.BelowContent;
        closeButtonImage = new PopupImage();
        closeButtonImage.Style = new Style(typeof(PopupImage));
        base.WidthRequest = closeButtonWidth;
        base.HeightRequest = closeButtonHeight;
        closeButtonImage.HeightRequest = 24.0;
        closeButtonImage.WidthRequest = 24.0;
        closeButtonImage.HorizontalOptions = LayoutOptions.Center;
        closeButtonImage.VerticalOptions = LayoutOptions.Center;
        this.AddTouchListener(this);
    }

    //
    // Summary:
    //     Draw an hover effect for close button.
    //
    // Parameters:
    //   canvas:
    //     The Canvas.
    //
    //   rectF:
    //     The rectangle.
    private void DrawHoverHighlight(ICanvas canvas, RectF rectF)
    {
        if (PopupView.Popup.ShowCloseButton && PopupView.Popup.PopupStyle.CloseButtonIcon == null && isHover)
        {
            canvas.FillColor = PopupView.Popup.PopupStyle.GetHoveredCloseButtonIconBackground();
            canvas.FillCircle(rectF.Center, 20f);
        }
        else
        {
            canvas.FillColor = Colors.Transparent;
            canvas.FillCircle(rectF.Center, 20f);
        }
    }

    //
    // Summary:
    //     Draw a touch effect for close button.
    //
    // Parameters:
    //   canvas:
    //     The Canvas.
    //
    //   rectF:
    //     The rectangle.
    private void DrawPressedHighlight(ICanvas canvas, RectF rectF)
    {
        if (PopupView.Popup.ShowCloseButton && PopupView.Popup.PopupStyle.CloseButtonIcon == null && isPressed)
        {
            canvas.FillColor = PopupView.Popup.PopupStyle.GetPressedCloseButtonIconBackground();
            canvas.FillCircle(rectF.Center, 20f);
        }
        else
        {
            canvas.FillColor = Colors.Transparent;
            canvas.FillCircle(rectF.Center, 20f);
        }
    }
}
