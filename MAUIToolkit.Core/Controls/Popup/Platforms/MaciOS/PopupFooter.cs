namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     The view that is loaded as the footer of the PopupView in MAUIToolkit.Core.Controls.Popup.SfPopup.
internal class PopupFooter : CView
{
    //
    // Summary:
    //     Gets PopupView's instance.
    internal PopupView? PopupView;

    //
    // Summary:
    //     Gets or sets the accept button in the footer.
    //
    // Value:
    //     The accept button in the footer.
    internal PopupButton? AcceptButton;

    //
    // Summary:
    //     Gets or sets the decline button in the footer.
    //
    // Value:
    //     The decline button in the footer.
    internal PopupButton? DeclineButton;

    //
    // Summary:
    //     Gets or sets the CustomView to be loaded in the footer of the PopupView.
    //
    // Value:
    //     The CustomView to be loaded in the header of the PopupView.
    internal View? FooterView;

    //
    // Summary:
    //     Default padding for MAUIToolkit.Core.Controls.Popup.PopupFooter.FooterView.
    private int footerPadding = 24;

    //
    // Summary:
    //     Height of the footer buttons.
    private int footerButtonHeight = 40;

    //
    // Summary:
    //     The distance between footer buttons.
    private int distanceBetweenFooterButtons = 8;

    //
    // Summary:
    //     Width of the accept button.
    private double acceptButtonWidth;

    //
    // Summary:
    //     Width of the decline button.
    private double declineButtonWidth;

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupFooter class.
    public PopupFooter()
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupFooter class.
    //
    // Parameters:
    //   popupView:
    //     The instance of PopupView.
    public PopupFooter(PopupView popupView)
    {
        PopupView = popupView;
        Initialize();
    }

    //
    // Summary:
    //     update the child views and appearance of the footer.
    internal void UpdateChildViews()
    {
        UpdateFooterChild();
        UpdateFooterAppearance();
    }

    //
    // Summary:
    //     Update the child views of the footer.
    internal void UpdateFooterChild()
    {
        if (FooterView is PopupGrid sfGrid && base.Children.Count > 0)
        {
            sfGrid.ColumnDefinitions.Clear();
            sfGrid.Children.Clear();
            base.Children.Clear();
        }

        AddChildViews();
    }

    //
    // Summary:
    //     Updates the appearance of the footer.
    internal void UpdateFooterAppearance()
    {
        UpdateFooterStyle();
        UpdateFooterChildView();
    }

    //
    // Summary:
    //     Add the child views of the footer.
    internal void AddChildViews()
    {
        if (FooterView is PopupGrid sfGrid)
        {
            if (PopupView?.Popup.FooterTemplate == null)
            {
                PopupView? popupView = PopupView;
                if (popupView != null && popupView.Popup.AppearanceMode == PopupButtonAppearanceMode.OneButton)
                {
                    sfGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Star
                    });
                    sfGrid.Children.Add(AcceptButton);
                    Grid.SetColumn((BindableObject)AcceptButton, 0);
                }
                else
                {
                    sfGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Star
                    });
                    sfGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });
                    sfGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = distanceBetweenFooterButtons
                    });
                    sfGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });
                    sfGrid.Children.Add(DeclineButton);
                    Grid.SetColumn((BindableObject)DeclineButton, 1);
                    sfGrid.Children.Add(AcceptButton);
                    Grid.SetColumn((BindableObject)AcceptButton, 3);
                }
            }
            else
            {
                sfGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = GridLength.Star
                });
                View view = (View)PopupView.Popup.GetTemplate(PopupView.Popup.FooterTemplate).CreateContent();
                sfGrid.HorizontalOptions = LayoutOptions.Fill;
                sfGrid.Children.Add(view);
                Grid.SetColumn((BindableObject)view, 0);
            }

            sfGrid.Padding = ((PopupView.Popup.FooterTemplate == null) ? new Thickness(footerPadding) : new Thickness(0.0));
        }

        base.Children.Add(FooterView);
    }

    //
    // Summary:
    //     Updates the width and height of the child views.
    private void UpdateFooterChildView()
    {
        if (PopupView.Popup.AppearanceMode == PopupButtonAppearanceMode.OneButton)
        {
            if (!AcceptButton.IsVisible)
            {
                AcceptButton.HorizontalOptions = LayoutOptions.End;
                acceptButtonWidth = GetFooterButtonWidth(AcceptButton);
                AcceptButton.WidthRequest = Math.Max(0.0, acceptButtonWidth);
                AcceptButton.HeightRequest = Math.Max(0, footerButtonHeight);
            }
        }
        else if (PopupView.Popup.AppearanceMode == PopupButtonAppearanceMode.TwoButton)
        {
            if (!AcceptButton.IsVisible)
            {
                acceptButtonWidth = GetFooterButtonWidth(AcceptButton);
                AcceptButton.WidthRequest = Math.Max(0.0, acceptButtonWidth);
                AcceptButton.HeightRequest = Math.Max(0, footerButtonHeight);
            }

            if (!DeclineButton.IsVisible)
            {
                declineButtonWidth = GetFooterButtonWidth(DeclineButton);
                DeclineButton.WidthRequest = Math.Max(0.0, declineButtonWidth);
                DeclineButton.HeightRequest = Math.Max(0, footerButtonHeight);
            }

            DeclineButton.IsVisible = PopupView.Popup.ShowFooter;
        }

        AcceptButton.IsVisible = PopupView.Popup.ShowFooter;
    }

    //
    // Summary:
    //     Gets the footer button width based on text size.
    //
    // Parameters:
    //   button:
    //     Instance of the button.
    //
    // Returns:
    //     returns the button width.
    private double GetFooterButtonWidth(PopupButton button)
    {
        button.IsVisible = true;
        Size size = new Size(double.PositiveInfinity, PopupView.Popup.FooterHeight);
        return button.Measure(size.Width, size.Height, MeasureFlags.IncludeMargins).Request.Width + 10.0;
    }

    //
    // Summary:
    //     updates the popip footer style.
    private void UpdateFooterStyle()
    {
        base.Background = PopupView.Popup.PopupStyle.GetFooterBackground();
        //AcceptButton.CornerRadius = PopupView.Popup.PopupStyle.FooterButtonCornerRadius;
        //AcceptButton.Stroke = Colors.Transparent;
        AcceptButton.Background = PopupView.Popup.PopupStyle.GetAcceptButtonBackground();
        AcceptButton.TextColor = PopupView.Popup.PopupStyle.GetAcceptButtonTextColor();
        AcceptButton.FontAttributes = PopupView.Popup.PopupStyle.FooterFontAttribute;
        AcceptButton.FontSize = PopupView.Popup.PopupStyle.GetFooterFontSize();
        AcceptButton.FontFamily = PopupView.Popup.PopupStyle.FooterFontFamily;
        if (PopupView.Popup.AppearanceMode == PopupButtonAppearanceMode.TwoButton)
        {
            //DeclineButton.CornerRadius = PopupView.Popup.PopupStyle.FooterButtonCornerRadius;
            //DeclineButton.Stroke = Colors.Transparent;
            DeclineButton.FontAttributes = PopupView.Popup.PopupStyle.FooterFontAttribute;
            DeclineButton.FontSize = PopupView.Popup.PopupStyle.GetFooterFontSize();
            DeclineButton.FontFamily = PopupView.Popup.PopupStyle.FooterFontFamily;
            DeclineButton.Background = PopupView.Popup.PopupStyle.GetDeclineButtonBackground();
            DeclineButton.TextColor = PopupView.Popup.PopupStyle.GetDeclineButtonTextColor();
        }
    }

    //
    // Summary:
    //     Used to initialize the popup footer.
    private void Initialize()
    {
        FooterView = new PopupGrid();
        FooterView.Style = new Style(typeof(PopupGrid));
        AcceptButton = new PopupButton
        {
            IsVisible = false
        };
        AcceptButton.Style = new Style(typeof(PopupButton));
        AcceptButton.Text = PopupResources.GetLocalizedString("AcceptButtonText");
        AcceptButton.Clicked += OnAcceptButtonClicked;
        DeclineButton = new PopupButton
        {
            IsVisible = false
        };
        DeclineButton.Style = new Style(typeof(PopupButton));
        DeclineButton.Text = PopupResources.GetLocalizedString("DeclineButtonText");
        DeclineButton.Clicked += OnDeclineButtonClicked;
    }

    //
    // Summary:
    //     Handles, when click the decline button.
    //
    // Parameters:
    //   sender:
    //     The sender of the event.
    //
    //   e:
    //     The System.EventArgs.
    private void OnDeclineButtonClicked(object? sender, EventArgs e)
    {
        PopupView?.RaiseDeclineCommand(PopupView.Popup.DeclineCommand);
    }

    //
    // Summary:
    //     Handles, when click the accept button.
    //
    // Parameters:
    //   sender:
    //     The sender of the event.
    //
    //   e:
    //     The System.EventArgs.
    private void OnAcceptButtonClicked(object? sender, EventArgs e)
    {
        PopupView?.RaiseAcceptCommand(PopupView.Popup.AcceptCommand);
    }
}
