using MAUIToolkit.Core.Platforms;
using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     Defines the pop-up view of MAUIToolkit.Core.Controls.Popup.CPopup.
internal class PopupView : CView
{
    //
    // Summary:
    //     Gets or sets the MAUIToolkit.Core.Controls.Popup.CPopup.
    internal CPopup Popup;

    //
    // Summary:
    //     Gets or sets the CustomView to be loaded in the header of the PopupView.
    internal PopupHeader? HeaderView;

    //
    // Summary:
    //     Gets or sets the CustomView to be loaded in the footer of the PopupView.
    //
    // Value:
    //     The footer of the PopupView.
    internal PopupFooter? FooterView;

    //
    // Summary:
    //     Gets or sets the CustomView to be loaded in the body of the PopupView.
    //
    // Value:
    //     The footer of the PopupView.
    internal PopupMessageView? PopupMessageView;

    //
    // Summary:
    //     Indicates whether the popup was closed by using the accept button.
    internal bool AcceptButtonClicked = false;

    //
    // Summary:
    //     Backing field for the MAUIToolkit.Core.Controls.Popup.PopupView.AppliedHeaderHeight property.
    private int appliedHeaderHeight;

    //
    // Summary:
    //     Backing field for the MAUIToolkit.Core.Controls.Popup.PopupView.AppliedFooterHeight property.
    private int appliedFooterHeight;

    //
    // Summary:
    //     Backing field for the MAUIToolkit.Core.Controls.Popup.PopupView.AppliedBodyHeight property.
    private int appliedBodyHeight;

    //
    // Summary:
    //     Backing field for the MAUIToolkit.Core.Controls.Popup.PopupView.IsViewLoaded property.
    private bool isViewLoaded = false;

    //
    // Summary:
    //     Gets or sets the height applied to the header of the PopupView.
    internal int AppliedHeaderHeight
    {
        get
        {
            return appliedHeaderHeight;
        }
        set
        {
            appliedHeaderHeight = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the height applied to the header of the PopupView.
    internal int AppliedFooterHeight
    {
        get
        {
            return appliedFooterHeight;
        }
        set
        {
            appliedFooterHeight = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the height applied to the body of the PopupView.
    internal int AppliedBodyHeight
    {
        get
        {
            return appliedBodyHeight;
        }
        set
        {
            appliedBodyHeight = value;
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether the popupview is loaded or not.
    internal bool IsViewLoaded
    {
        get
        {
            return isViewLoaded;
        }
        set
        {
            isViewLoaded = value;
        }
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupView class.
    //
    // Parameters:
    //   cPopup:
    //     MAUIToolkit.Core.Controls.Popup.CPopup instance.
    public PopupView(CPopup cPopup)
    {
        Popup = cPopup;
        base.DrawingOrder = DrawingOrder.AboveContent;
        if (DeviceInfo.Platform != DevicePlatform.Android && DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            base.Shadow = new Shadow
            {
                Brush = Colors.Transparent
            };
        }

        Initialize();
        AddChildViews();
    }

    //
    // Summary:
    //     Rasises the Accept command.
    //
    // Parameters:
    //   command:
    //     The command to be raised.
    internal void RaiseAcceptCommand(ICommand command)
    {
        if (command != null)
        {
            if (command.CanExecute(Popup.AcceptCommand))
            {
                command.Execute(Popup);
                AcceptButtonClicked = true;
                Popup.IsOpen = false;
            }
        }
        else
        {
            AcceptButtonClicked = true;
            Popup.IsOpen = false;
        }
    }

    //
    // Summary:
    //     Rasises the Declline command.
    //
    // Parameters:
    //   command:
    //     The command to be raised.
    internal void RaiseDeclineCommand(ICommand command)
    {
        if (command != null && Popup.DeclineCommand != null)
        {
            if (command.CanExecute(Popup.DeclineCommand))
            {
                command.Execute(Popup);
                Popup.IsOpen = false;
            }
        }
        else
        {
            Popup.IsOpen = false;
        }
    }

    //
    // Summary:
    //     used to update footer view.
    internal void UpdateFooterView()
    {
        FooterView?.UpdateChildViews();
    }

    //
    // Summary:
    //     used to set the accept button text.
    //
    // Parameters:
    //   text:
    //     text to be shown in the accept button.
    internal void SetAcceptButtonText(string text)
    {
        if (FooterView != null && FooterView.AcceptButton != null)
        {
            FooterView.AcceptButton.Text = text;
        }
    }

    //
    // Summary:
    //     used to set the decline button text.
    //
    // Parameters:
    //   text:
    //     text to be shown in the decline button.
    internal void SetDeclineButtonText(string text)
    {
        if (FooterView != null && FooterView.DeclineButton != null)
        {
            FooterView.DeclineButton.Text = text;
        }
    }

    //
    // Summary:
    //     Used to set the header title text.
    //
    // Parameters:
    //   title:
    //     text to be shown in the header title.
    internal void SetHeaderTitleText(string title)
    {
        if (HeaderView != null && HeaderView.TitleLabel != null)
        {
            HeaderView.TitleLabel.Text = title;
        }
    }

    //
    // Summary:
    //     Used to set the popup message text.
    //
    // Parameters:
    //   message:
    //     text to be shown in the popup message.
    internal void SetMessageText(string message)
    {
        if (PopupMessageView != null && PopupMessageView.MessageView != null)
        {
            PopupMessageView.MessageView.Text = message;
        }
    }

    //
    // Summary:
    //     used to update header view.
    internal void UpdateHeaderView()
    {
        HeaderView?.UpdateChildViews();
    }

    //
    // Summary:
    //     used to update the popup message view.
    internal void UpdateMessageView()
    {
        PopupMessageView?.UpdateChildViews();
    }

    //
    // Summary:
    //     Method to invalidate the measure to triger the measure pass.
    internal void InvalidateForceLayout()
    {
        ((IView)this).InvalidateMeasure();
    }

    //
    // Summary:
    //     Render the border around the MAUIToolkit.Core.Controls.Popup.PopupView.
    internal void ApplyShadowAndCornerRadius()
    {
        Popup.ApplyNativePopupViewShadow();
        if (DeviceInfo.Platform == DevicePlatform.WinUI && (Popup.PopupStyle.CornerRadius.TopRight != 0.0 || Popup.PopupStyle.CornerRadius.TopLeft != 0.0 || Popup.PopupStyle.CornerRadius.BottomLeft != 0.0 || Popup.PopupStyle.CornerRadius.BottomRight != 0.0))
        {
            base.Clip = new RoundRectangleGeometry(Popup.PopupStyle.CornerRadius, new Rect(0.0, 0.0, Popup.PopupViewWidth, Popup.PopupViewHeight));
        }
        else
        {
            base.Clip = null;
        }

        if (base.Shadow != null && DeviceInfo.Platform != DevicePlatform.Android && DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            if (Popup.PopupStyle.HasShadow)
            {
                base.Shadow.Brush = Color.FromRgba(0.0, 0.0, 0.0, 0.3);
            }
            else
            {
                base.Shadow.Brush = Brush.Transparent;
            }
        }
    }

    //
    // Summary:
    //     Measures the PopupView.
    //
    // Parameters:
    //   widthConstraint:
    //     Width of the PopupView.
    //
    //   heightConstraint:
    //     Height of the PopupView.
    //
    // Returns:
    //     Returns the size of the view.
    protected override Size MeasureContent(double widthConstraint, double heightConstraint)
    {
        MeasureChildren();
        return new Size(Math.Max(0.0, Popup.PopupViewWidth), Math.Max(0.0, Popup.PopupViewHeight));
    }

    //
    // Summary:
    //     Arranges the view.
    //
    // Parameters:
    //   bounds:
    //     The bounds of the view.
    //
    // Returns:
    //     Returns the size of the view.
    protected override Size ArrangeContent(Rect bounds)
    {
        double strokeThickness = Popup.PopupStyle.GetStrokeThickness();
        double num = strokeThickness / 2.0;
        ((IView)HeaderView)?.Arrange(new Rect(Math.Max(0.0, num), Math.Max(0.0, num), Math.Max(0.0, Popup.PopupViewWidth - strokeThickness), Math.Max(0.0, Popup.AppliedHeaderHeight)));
        ((IView)PopupMessageView)?.Arrange(new Rect(Math.Max(0.0, num), Math.Max(0.0, num + Popup.AppliedHeaderHeight), Math.Max(0.0, Popup.PopupViewWidth - strokeThickness), Math.Max(0.0, Popup.AppliedBodyHeight)));
        ((IView)FooterView)?.Arrange(new Rect(Math.Max(0.0, num), Math.Max(0.0, num + Popup.AppliedHeaderHeight + Popup.AppliedBodyHeight), Math.Max(0.0, Popup.PopupViewWidth - strokeThickness), Math.Max(0.0, Popup.AppliedFooterHeight)));
        return new Size(Math.Max(0.0, Popup.PopupViewWidth), Math.Max(0.0, Popup.PopupViewHeight));
    }

    //
    // Summary:
    //     To draw the border for the PopupView.
    //
    // Parameters:
    //   canvas:
    //     The canvas to be drawn.
    //
    //   dirtyRect:
    //     The bounds of the view.
    protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
    {
        PathF pathF = new PathF();
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            if (!Popup.IsRTL)
            {
                pathF.AppendRoundedRectangle(dirtyRect, (float)Popup.PopupStyle.CornerRadius.TopLeft, (float)Popup.PopupStyle.CornerRadius.TopRight, (float)Popup.PopupStyle.CornerRadius.BottomLeft, (float)Popup.PopupStyle.CornerRadius.BottomRight);
            }
            else
            {
                pathF.AppendRoundedRectangle(dirtyRect, (float)Popup.PopupStyle.CornerRadius.TopRight, (float)Popup.PopupStyle.CornerRadius.TopLeft, (float)Popup.PopupStyle.CornerRadius.BottomRight, (float)Popup.PopupStyle.CornerRadius.BottomLeft);
            }
        }
        else
        {
            pathF.AppendRoundedRectangle(dirtyRect, (float)Popup.RadiusValue);
        }

        canvas.ClipPath(pathF);
        double strokeThickness = Popup.PopupStyle.GetStrokeThickness();
        if (!(strokeThickness > 0.0))
        {
            return;
        }

        canvas.StrokeColor = Popup.PopupStyle.GetStroke();
        canvas.StrokeSize = (float)strokeThickness;
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            if (!Popup.IsRTL || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                canvas.DrawRoundedRectangle(dirtyRect, Popup.PopupStyle.CornerRadius.TopLeft, Popup.PopupStyle.CornerRadius.TopRight, Popup.PopupStyle.CornerRadius.BottomLeft, Popup.PopupStyle.CornerRadius.BottomRight);
            }
            else
            {
                canvas.DrawRoundedRectangle(dirtyRect, Popup.PopupStyle.CornerRadius.TopRight, Popup.PopupStyle.CornerRadius.TopLeft, Popup.PopupStyle.CornerRadius.BottomRight, Popup.PopupStyle.CornerRadius.BottomLeft);
            }
        }
        else
        {
            canvas.DrawRoundedRectangle(dirtyRect, Popup.RadiusValue);
        }
    }

    //
    // Summary:
    //     Rasied when the handler changes.
    protected override void OnHandlerChanged()
    {
        if (base.Handler != null && base.Handler.PlatformView != null)
        {
            LayoutViewGroupExt layoutViewGroupExt = (LayoutViewGroupExt)base.Handler.PlatformView;
            layoutViewGroupExt.ClipToOutline = true;
        }

        if (base.Handler != null && !IsViewLoaded)
        {
            IsViewLoaded = true;
            Popup.PopupView.HeaderView.AddChildViews();
            Popup.PopupView.PopupMessageView.AddChildViews();
            Popup.PopupView.FooterView.AddChildViews();
        }

        base.OnHandlerChanged();
    }

    //
    // Summary:
    //     Initializes the proprties of the MAUIToolkit.Core.Controls.Popup.PopupView class.
    private void Initialize()
    {
        HeaderView = new PopupHeader(this);
        FooterView = new PopupFooter(this);
        PopupMessageView = new PopupMessageView(this);
    }

    //
    // Summary:
    //     Add the child views of the PopupView.
    private void AddChildViews()
    {
        base.Children.Add(HeaderView);
        base.Children.Add(PopupMessageView);
        base.Children.Add(FooterView);
    }

    //
    // Summary:
    //     Measures the children of the PopupView.
    private void MeasureChildren()
    {
        double strokeThickness = Popup.PopupStyle.GetStrokeThickness();
        ((IView)HeaderView)?.Measure(Math.Max(0.0, Popup.PopupViewWidth - strokeThickness), Math.Max(0.0, Popup.AppliedHeaderHeight));
        ((IView)PopupMessageView)?.Measure(Math.Max(0.0, Popup.PopupViewWidth - strokeThickness), Math.Max(0.0, Popup.AppliedBodyHeight));
        ((IView)FooterView)?.Measure(Math.Max(0.0, Popup.PopupViewWidth - strokeThickness), Math.Max(0.0, Popup.AppliedFooterHeight));
    }
}
