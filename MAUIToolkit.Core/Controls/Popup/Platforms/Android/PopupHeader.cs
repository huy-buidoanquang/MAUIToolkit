namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     The view that is loaded as the header of the PopupView in MAUIToolkit.Core.Controls.Popup.CPopup.
internal class PopupHeader : CView
{
    //
    // Summary:
    //     Gets PopupView's instance.
    internal PopupView? PopupView;

    //
    // Summary:
    //     Gets or sets the CustomView to be loaded in the header of the PopupView.
    //
    // Value:
    //     The CustomView to be loaded in the header of the PopupView.
    internal View? HeaderView;

    //
    // Summary:
    //     Gets or sets the text view in the header of the PopupView.
    //
    // Value:
    //     The text view in the header of the PopupView.
    internal PopupLabel? TitleLabel;

    //
    // Summary:
    //     Gets or sets the close button in the header of the PopupView.
    //
    // Value:
    //     The button in the header of the PopupView.
    internal PopupCloseButton? PopupCloseButton;

    //
    // Summary:
    //     Default left and top padding for MAUIToolkit.Core.Controls.Popup.PopupHeader.HeaderView.
    private int headerViewPadding = 16;

    //
    // Summary:
    //     Default padding for MAUIToolkit.Core.Controls.Popup.PopupHeader.HeaderView.
    private int headerPadding = 8;

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupHeader class.
    public PopupHeader()
    {
        Initialize();
        AddChildViews();
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupHeader class.
    //
    // Parameters:
    //   popup:
    //     The instance of PopupView.
    public PopupHeader(PopupView popup)
    {
        PopupView = popup;
        Initialize();
    }

    //
    // Summary:
    //     Update the child views of the header.
    internal void UpdateChildViews()
    {
        if (HeaderView is PopupGrid cGrid && base.Children.Count > 0)
        {
            cGrid.ColumnDefinitions.Clear();
            cGrid.Children.Clear();
            base.Children.Clear();
        }

        AddChildViews();
        UpdateHeaderAppearance();
    }

    //
    // Summary:
    //     Updates the appearance of the header.
    internal void UpdateHeaderAppearance()
    {
        UpdateHeaderChildView();
        UpdateHeaderStyle();
    }

    //
    // Summary:
    //     Updates the popup view close button.
    internal void UpdateHeaderCloseButton()
    {
        if (PopupView.Popup.ShowCloseButton && PopupView.Popup.PopupStyle.CloseButtonIcon != null)
        {
            PopupView.HeaderView.PopupCloseButton.UpdatePopupCloseButtonContent();
        }
        else if (PopupView.Popup.ShowCloseButton && PopupView.Popup.PopupStyle.CloseButtonIcon == null)
        {
            PopupView.HeaderView.PopupCloseButton.InvalidateDrawable();
        }
    }

    //
    // Summary:
    //     Add the child views of the header.
    internal void AddChildViews()
    {
        PopupGrid cGrid = HeaderView as PopupGrid;
        bool canShowPopupInFullScreen = PopupView.Popup.CanShowPopupInFullScreen;
        if (cGrid != null)
        {
            if (canShowPopupInFullScreen)
            {
                if (PopupView.Popup.ShowCloseButton)
                {
                    PopupCloseButton.HorizontalOptions = LayoutOptions.Start;
                    cGrid.Padding = ((PopupView.Popup.HeaderTemplate == null) ? new Thickness(headerPadding, headerPadding, headerViewPadding, headerViewPadding) : new Thickness(0.0));
                    cGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = ((PopupView.Popup.HeaderTemplate == null) ? 40 : (canShowPopupInFullScreen ? 48 : 56))
                    });
                }
                else
                {
                    cGrid.Padding = ((PopupView.Popup.HeaderTemplate == null) ? new Thickness(headerViewPadding) : new Thickness(0.0));
                }

                cGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = GridLength.Star
                });
            }
            else
            {
                cGrid.Padding = ((PopupView.Popup.HeaderTemplate == null) ? new Thickness(headerViewPadding) : new Thickness(0.0));
                cGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = GridLength.Star
                });
                if (PopupView.Popup.ShowCloseButton)
                {
                    PopupCloseButton.HorizontalOptions = LayoutOptions.End;
                    cGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = ((PopupView.Popup.HeaderTemplate == null) ? 40 : (canShowPopupInFullScreen ? 48 : 56))
                    });
                }
            }

            if (PopupView?.Popup.HeaderTemplate == null)
            {
                TitleLabel.Padding = new Thickness(headerPadding, 5.0, headerPadding, 0.0);
                cGrid.Children.Add(TitleLabel);
                Grid.SetColumn((BindableObject)TitleLabel, (PopupView.Popup.ShowCloseButton && canShowPopupInFullScreen) ? 1 : 0);
            }
            else
            {
                CView view = (CView)PopupView.Popup.GetTemplate(PopupView.Popup.HeaderTemplate).CreateContent();
                cGrid.Children.Add(view);
                Grid.SetColumn((BindableObject)view, (PopupView.Popup.ShowCloseButton && canShowPopupInFullScreen) ? 1 : 0);
            }

            if (PopupView.Popup.ShowCloseButton)
            {
                cGrid.Children.Add(PopupCloseButton);
                Grid.SetColumn((BindableObject)PopupCloseButton, (!canShowPopupInFullScreen) ? 1 : 0);
                if (canShowPopupInFullScreen)
                {
                    PopupCloseButton.Margin = ((PopupView.Popup.HeaderTemplate != null) ? new Thickness(headerPadding, headerPadding, 0.0, 0.0) : new Thickness(0.0));
                }
                else
                {
                    PopupCloseButton.Margin = ((PopupView.Popup.HeaderTemplate != null) ? new Thickness(0.0, headerViewPadding, headerViewPadding, 0.0) : new Thickness(0.0));
                }
            }
        }

        base.Children.Add(HeaderView);
    }

    //
    // Summary:
    //     Updates width and height of the child view.
    private void UpdateHeaderChildView()
    {
        if (PopupView.Popup.HeaderTemplate == null)
        {
            PopupGrid cGrid = HeaderView as PopupGrid;
            CView view = cGrid.Children[0] as CView;
            bool canShowPopupInFullScreen = PopupView.Popup.CanShowPopupInFullScreen;
            double num = (PopupView.Popup.ShowCloseButton ? cGrid.ColumnDefinitions[(!canShowPopupInFullScreen) ? 1 : 0].Width.Value : 0.0);
            int num2 = ((canShowPopupInFullScreen && PopupView.Popup.ShowCloseButton) ? (headerPadding + headerViewPadding) : (headerViewPadding * 2));
            int num3 = ((canShowPopupInFullScreen && PopupView.Popup.ShowCloseButton) ? (headerPadding + headerViewPadding) : (headerViewPadding * 2));
            view.WidthRequest = PopupView.Popup.PopupViewWidth - num - (double)num2 - PopupView.Popup.PopupStyle.GetStrokeThickness() * 2.0;
            view.HeightRequest = Math.Max(0.0, PopupView.Popup.AppliedHeaderHeight - (double)num3);
        }
    }

    //
    // Summary:
    //     updates the popup header style.
    private void UpdateHeaderStyle()
    {
        TitleLabel.TextColor = PopupView?.Popup.PopupStyle.GetHeaderTextColor();
        base.Background = PopupView.Popup.PopupStyle.GetHeaderBackground();
        if (PopupView?.Popup.PopupStyle.HeaderFontFamily != null)
        {
            TitleLabel.FontFamily = PopupView?.Popup.PopupStyle.HeaderFontFamily;
        }

        TitleLabel.FontSize = PopupView.Popup.PopupStyle.GetHeaderFontSize();
        if (PopupView.Popup.PopupStyle.HeaderFontAttribute == FontAttributes.Bold)
        {
            TitleLabel.FontAttributes = FontAttributes.Bold;
        }
        else if (PopupView.Popup.PopupStyle.HeaderFontAttribute == FontAttributes.Italic)
        {
            TitleLabel.FontAttributes = FontAttributes.Italic;
        }
        else
        {
            TitleLabel.FontAttributes = FontAttributes.None;
        }

        if (PopupView.Popup.PopupStyle.HeaderTextAlignment == TextAlignment.Start)
        {
            TitleLabel.VerticalTextAlignment = TextAlignment.Start;
            TitleLabel.HorizontalTextAlignment = TextAlignment.Start;
        }
        else if (PopupView.Popup.PopupStyle.HeaderTextAlignment == TextAlignment.Center)
        {
            TitleLabel.VerticalTextAlignment = TextAlignment.Start;
            TitleLabel.HorizontalTextAlignment = TextAlignment.Center;
        }
        else
        {
            TitleLabel.VerticalTextAlignment = TextAlignment.Start;
            TitleLabel.HorizontalTextAlignment = TextAlignment.End;
        }
    }

    //
    // Summary:
    //     Used to initialize the popup header.
    private void Initialize()
    {
        TitleLabel = new PopupLabel
        {
            Text = CPopupResources.GetLocalizedString("Title"),
            Style = new Style(typeof(PopupLabel))
        };
        PopupCloseButton = new PopupCloseButton(PopupView)
        {
            VerticalOptions = LayoutOptions.Start
        };
        HeaderView = new PopupGrid();
        HeaderView.Style = new Style(typeof(PopupGrid));
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
}
