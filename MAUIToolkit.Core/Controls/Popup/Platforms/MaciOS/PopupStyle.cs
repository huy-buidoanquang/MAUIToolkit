using MAUIToolkit.Core.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     Represents the style of MAUIToolkit.Core.Controls.Popup.CPopup.
public class PopupStyle : Element, IThemeElement
{
    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.OverlayColor Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty OverlayColorProperty = BindableProperty.Create("OverlayColor", typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#80000000")), BindingMode.Default);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HeaderBackground Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty HeaderBackgroundProperty = BindableProperty.Create("HeaderBackground", typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#00FFFFFF")));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HeaderTextColor Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty HeaderTextColorProperty = BindableProperty.Create("HeaderTextColor", typeof(Color), typeof(PopupStyle), Color.FromArgb("#1C1B1F"));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.MessageTextColor Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty MessageTextColorProperty = BindableProperty.Create("MessageTextColor", typeof(Color), typeof(PopupStyle), Color.FromArgb("#49454F"));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.MessageBackground Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty MessageBackgroundProperty = BindableProperty.Create("MessageBackground", typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#00FFFFFF")));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.PopupBackground Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty PopupBackgroundProperty = BindableProperty.Create("PopupBackground", typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#EEE8F4")));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.MessageFontSize Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty MessageFontSizeProperty = BindableProperty.Create("MessageFontSize", typeof(double), typeof(PopupStyle), 14.0);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.MessageFontFamily Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty MessageFontFamilyProperty = BindableProperty.Create("MessageFontFamily", typeof(string), typeof(PopupStyle));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.MessageFontAttribute Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty MessageFontAttributeProperty = BindableProperty.Create("MessageFontAttribute", typeof(FontAttributes), typeof(PopupStyle), FontAttributes.None);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.MessageTextAlignment Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty MessageTextAlignmentProperty = BindableProperty.Create("MessageTextAlignment", typeof(TextAlignment), typeof(PopupStyle), TextAlignment.Start);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.FooterBackground Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty FooterBackgroundProperty = BindableProperty.Create("FooterBackground", typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#00FFFFFF")), BindingMode.Default);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.FooterFontSize Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty FooterFontSizeProperty = BindableProperty.Create("FooterFontSize", typeof(double), typeof(PopupStyle), 14.0);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.FooterFontFamily Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty FooterFontFamilyProperty = BindableProperty.Create("FooterFontFamily", typeof(string), typeof(PopupStyle));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.FooterFontAttribute Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty FooterFontAttributeProperty = BindableProperty.Create("FooterFontAttribute", typeof(FontAttributes), typeof(PopupStyle), FontAttributes.None);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.AcceptButtonBackground Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty AcceptButtonBackgroundProperty = BindableProperty.Create("AcceptButtonBackground", typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Colors.Transparent));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.AcceptButtonTextColor Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty AcceptButtonTextColorProperty = BindableProperty.Create("AcceptButtonTextColor", typeof(Color), typeof(PopupStyle), Color.FromArgb("#6750A4"));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.DeclineButtonTextColor Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty DeclineButtonTextColorProperty = BindableProperty.Create("DeclineButtonTextColor", typeof(Color), typeof(PopupStyle), Color.FromArgb("#6750A4"));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.DeclineButtonBackground Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty DeclineButtonBackgroundProperty = BindableProperty.Create("DeclineButtonBackground", typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Colors.Transparent));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.Stroke Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty StrokeProperty = BindableProperty.Create("Stroke", typeof(Color), typeof(PopupStyle), Colors.Transparent);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HeaderFontSize Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty HeaderFontSizeProperty = BindableProperty.Create("HeaderFontSize", typeof(double), typeof(PopupStyle), 24.0);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HeaderFontFamily Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty HeaderFontFamilyProperty = BindableProperty.Create("HeaderFontFamily", typeof(string), typeof(PopupStyle));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HeaderFontAttribute Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty HeaderFontAttributeProperty = BindableProperty.Create("HeaderFontAttribute", typeof(FontAttributes), typeof(PopupStyle), FontAttributes.None);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HeaderTextAlignment Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty HeaderTextAlignmentProperty = BindableProperty.Create("HeaderTextAlignment", typeof(TextAlignment), typeof(PopupStyle), TextAlignment.Start);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.StrokeThickness Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create("StrokeThickness", typeof(int), typeof(PopupStyle), 0);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HasShadow Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(PopupStyle), false);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.BlurIntensity Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty BlurIntensityProperty = BindableProperty.Create("BlurIntensity", typeof(PopupBlurIntensity), typeof(PopupStyle), PopupBlurIntensity.Light);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.BlurRadius Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty BlurRadiusProperty = BindableProperty.Create("BlurRadius", typeof(float), typeof(PopupStyle), 5f);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.CornerRadius Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius", typeof(CornerRadius), typeof(PopupStyle), new CornerRadius(28.0));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.FooterButtonCornerRadius Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty FooterButtonCornerRadiusProperty = BindableProperty.Create("FooterButtonCornerRadius", typeof(CornerRadius), typeof(PopupStyle), new CornerRadius(20.0));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.CloseButtonIcon Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty CloseButtonIconProperty = BindableProperty.Create("CloseButtonIcon", typeof(ImageSource), typeof(PopupStyle));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.CloseButtonIconStroke Microsoft.Maui.Controls.BindableProperty.
    internal static BindableProperty CloseButtonIconStrokeProperty = BindableProperty.Create("CloseButtonIconStroke", typeof(Color), typeof(PopupStyle), Color.FromArgb("#49454F"));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.CloseButtonIconStrokeThickness
    //     Microsoft.Maui.Controls.BindableProperty.
    internal static BindableProperty CloseButtonIconStrokeThicknessProperty = BindableProperty.Create("CloseButtonIconStrokeThickness", typeof(double), typeof(PopupStyle), 1.8);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.HoveredCloseButtonIconBackground
    //     Microsoft.Maui.Controls.BindableProperty.
    internal static BindableProperty HoveredCloseButtonIconBackgroundProperty = BindableProperty.Create("HoveredCloseButtonIconBackground", typeof(Color), typeof(PopupStyle), Color.FromArgb("#1449454F"));

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.PopupStyle.PressedCloseButtonIconBackground
    //     Microsoft.Maui.Controls.BindableProperty.
    internal static BindableProperty PressedCloseButtonIconBackgroundProperty = BindableProperty.Create("PressedCloseButtonIconBackground", typeof(Color), typeof(PopupStyle), Color.FromArgb("#00000000"));

    //
    // Summary:
    //     Gets or sets the overlay color when PopupView is displayed.
    //
    // Remarks:
    //     Opacity of the MAUIToolkit.Core.Controls.Popup.PopupStyle.OverlayColor can be customized
    //     using Alpha value.
    public Brush OverlayColor
    {
        get
        {
            return (Brush)GetValue(OverlayColorProperty);
        }
        set
        {
            SetValue(OverlayColorProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background color to be applied for the header.
    public Brush HeaderBackground
    {
        get
        {
            return (Brush)GetValue(HeaderBackgroundProperty);
        }
        set
        {
            SetValue(HeaderBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the text color to be applied for the header title.
    public Color HeaderTextColor
    {
        get
        {
            return (Color)GetValue(HeaderTextColorProperty);
        }
        set
        {
            SetValue(HeaderTextColorProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background color of content.
    public Brush MessageBackground
    {
        get
        {
            return (Brush)GetValue(MessageBackgroundProperty);
        }
        set
        {
            SetValue(MessageBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background color of the PopupView.
    public Brush PopupBackground
    {
        get
        {
            return (Brush)GetValue(PopupBackgroundProperty);
        }
        set
        {
            SetValue(PopupBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the foreground color of content.
    public Color MessageTextColor
    {
        get
        {
            return (Color)GetValue(MessageTextColorProperty);
        }
        set
        {
            SetValue(MessageTextColorProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font size of the content.
    public double MessageFontSize
    {
        get
        {
            return (double)GetValue(MessageFontSizeProperty);
        }
        set
        {
            SetValue(MessageFontSizeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font style to be applied for the content.
    public string MessageFontFamily
    {
        get
        {
            return (string)GetValue(MessageFontFamilyProperty);
        }
        set
        {
            SetValue(MessageFontFamilyProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font attribute to be applied for the content.
    public FontAttributes MessageFontAttribute
    {
        get
        {
            return (FontAttributes)GetValue(MessageFontAttributeProperty);
        }
        set
        {
            SetValue(MessageFontAttributeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the text alignment of the content.
    public TextAlignment MessageTextAlignment
    {
        get
        {
            return (TextAlignment)GetValue(MessageTextAlignmentProperty);
        }
        set
        {
            SetValue(MessageTextAlignmentProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background color of the MAUIToolkit.Core.Controls.Popup.CPopup footer.
    public Brush FooterBackground
    {
        get
        {
            return (Brush)GetValue(FooterBackgroundProperty);
        }
        set
        {
            SetValue(FooterBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font size of the footer buttons.
    public double FooterFontSize
    {
        get
        {
            return (double)GetValue(FooterFontSizeProperty);
        }
        set
        {
            SetValue(FooterFontSizeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font style to be applied for the footer buttons.
    public string FooterFontFamily
    {
        get
        {
            return (string)GetValue(FooterFontFamilyProperty);
        }
        set
        {
            SetValue(FooterFontFamilyProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font attribute to be applied for the footer buttons.
    public FontAttributes FooterFontAttribute
    {
        get
        {
            return (FontAttributes)GetValue(FooterFontAttributeProperty);
        }
        set
        {
            SetValue(FooterFontAttributeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background color of accept button in the footer.
    public Brush AcceptButtonBackground
    {
        get
        {
            return (Brush)GetValue(AcceptButtonBackgroundProperty);
        }
        set
        {
            SetValue(AcceptButtonBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the foreground color of accept button in the footer.
    public Color AcceptButtonTextColor
    {
        get
        {
            return (Color)GetValue(AcceptButtonTextColorProperty);
        }
        set
        {
            SetValue(AcceptButtonTextColorProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background color of decline button in the footer.
    public Brush DeclineButtonBackground
    {
        get
        {
            return (Brush)GetValue(DeclineButtonBackgroundProperty);
        }
        set
        {
            SetValue(DeclineButtonBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the foreground color of decline button in the footer.
    public Color DeclineButtonTextColor
    {
        get
        {
            return (Color)GetValue(DeclineButtonTextColorProperty);
        }
        set
        {
            SetValue(DeclineButtonTextColorProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the border color for the MAUIToolkit.Core.Controls.Popup.PopupView.
    public Color Stroke
    {
        get
        {
            return (Color)GetValue(StrokeProperty);
        }
        set
        {
            SetValue(StrokeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font size of the header title.
    public double HeaderFontSize
    {
        get
        {
            return (double)GetValue(HeaderFontSizeProperty);
        }
        set
        {
            SetValue(HeaderFontSizeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font style to be applied for the header title.
    public string HeaderFontFamily
    {
        get
        {
            return (string)GetValue(HeaderFontFamilyProperty);
        }
        set
        {
            SetValue(HeaderFontFamilyProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the font attribute to be applied for the header title.
    public FontAttributes HeaderFontAttribute
    {
        get
        {
            return (FontAttributes)GetValue(HeaderFontAttributeProperty);
        }
        set
        {
            SetValue(HeaderFontAttributeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the text alignment of the header.
    public TextAlignment HeaderTextAlignment
    {
        get
        {
            return (TextAlignment)GetValue(HeaderTextAlignmentProperty);
        }
        set
        {
            SetValue(HeaderTextAlignmentProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the border thickness for the MAUIToolkit.Core.Controls.Popup.PopupView.
    public int StrokeThickness
    {
        get
        {
            return (int)GetValue(StrokeThicknessProperty);
        }
        set
        {
            SetValue(StrokeThicknessProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether a drop shadow is displayed around the
    //     Popupview. The default value is true.
    public bool HasShadow
    {
        get
        {
            return (bool)GetValue(HasShadowProperty);
        }
        set
        {
            SetValue(HasShadowProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value that indicates the intensity of the blur effect in the overlay.
    public PopupBlurIntensity BlurIntensity
    {
        get
        {
            return (PopupBlurIntensity)GetValue(BlurIntensityProperty);
        }
        set
        {
            SetValue(BlurIntensityProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the blur radius of the blur effect applied to the overlay when the
    //     MAUIToolkit.Core.Controls.Popup.PopupStyle.BlurIntensity is BlurIntensity.Custom. Does
    //     not have any effect when MAUIToolkit.Core.Controls.Popup.PopupStyle.BlurIntensity has values
    //     other than BlurIntensity.Custom.
    public float BlurRadius
    {
        get
        {
            return (float)GetValue(BlurRadiusProperty);
        }
        set
        {
            SetValue(BlurRadiusProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the corner radius for the MAUIToolkit.Core.Controls.Popup.PopupView.
    //
    // Remarks:
    //     On Android 33 and above, it is possible to set different corner radii for each
    //     corner using the MAUIToolkit.Core.Controls.Popup.PopupStyle.CornerRadius class. However,
    //     on versions below Android 33, if the same value is provided for all corners,
    //     a corner radius will be applied. If different values are provided for each corner,
    //     the corner radius may not be applied.
    public CornerRadius CornerRadius
    {
        get
        {
            return (CornerRadius)GetValue(CornerRadiusProperty);
        }
        set
        {
            SetValue(CornerRadiusProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the corner radius of the accept and decline buttons in the footer.
    //     The default value is 20.
    public CornerRadius FooterButtonCornerRadius
    {
        get
        {
            return (CornerRadius)GetValue(FooterButtonCornerRadiusProperty);
        }
        set
        {
            SetValue(FooterButtonCornerRadiusProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the image to be placed in the header close button for the MAUIToolkit.Core.Controls.Popup.PopupView.
    public ImageSource CloseButtonIcon
    {
        get
        {
            return (ImageSource)GetValue(CloseButtonIconProperty);
        }
        set
        {
            SetValue(CloseButtonIconProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the icon color for close button.
    internal Color CloseButtonIconStroke
    {
        get
        {
            return (Color)GetValue(CloseButtonIconStrokeProperty);
        }
        set
        {
            SetValue(CloseButtonIconStrokeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the icon stroke thickness for close button.
    internal double CloseButtonIconStrokeThickness
    {
        get
        {
            return (double)GetValue(CloseButtonIconStrokeThicknessProperty);
        }
        set
        {
            SetValue(CloseButtonIconStrokeThicknessProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background for close button icon when pointer hover.
    internal Color HoveredCloseButtonIconBackground
    {
        get
        {
            return (Color)GetValue(HoveredCloseButtonIconBackgroundProperty);
        }
        set
        {
            SetValue(HoveredCloseButtonIconBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the background for close button icon when pointer pressed.
    internal Color PressedCloseButtonIconBackground
    {
        get
        {
            return (Color)GetValue(PressedCloseButtonIconBackgroundProperty);
        }
        set
        {
            SetValue(PressedCloseButtonIconBackgroundProperty, value);
        }
    }

    //
    // Summary:
    //     Method invoke when theme changes.
    //
    // Parameters:
    //   oldTheme:
    //     Old theme name.
    //
    //   newTheme:
    //     New theme name.
    void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
    {
    }

    //
    // Summary:
    //     Method invoke at whenever common theme changes.
    //
    // Parameters:
    //   oldTheme:
    //     Old theme name.
    //
    //   newTheme:
    //     New theme name.
    void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
    {
    }

    //
    // Summary:
    //     Gets the background color to be applied for the header.
    //
    // Returns:
    //     background color to be applied for the header.
    internal Brush GetHeaderBackground()
    {
        return HeaderBackground;
    }

    //
    // Summary:
    //     Gets the text color to be applied for the header title.
    //
    // Returns:
    //     text color to be applied for the header title.
    internal Color GetHeaderTextColor()
    {
        return HeaderTextColor;
    }

    //
    // Summary:
    //     Gets the background color of content.
    //
    // Returns:
    //     background color of content.
    internal Brush GetMessageBackground()
    {
        return MessageBackground;
    }

    //
    // Summary:
    //     Gets the background color of PopupView.
    //
    // Returns:
    //     background color of PopupView.
    internal Brush GetPopupBackground()
    {
        return PopupBackground;
    }

    //
    // Summary:
    //     Gets the foreground color of content.
    //
    // Returns:
    //     foreground color of content.
    internal Color GetMessageTextColor()
    {
        return MessageTextColor;
    }

    //
    // Summary:
    //     Gets the background color of the footer.
    //
    // Returns:
    //     background color of the footer.
    internal Brush GetFooterBackground()
    {
        return FooterBackground;
    }

    //
    // Summary:
    //     Gets the background color of accept button in the footer.
    //
    // Returns:
    //     background color of accept button in the footer.
    internal Brush GetAcceptButtonBackground()
    {
        return AcceptButtonBackground;
    }

    //
    // Summary:
    //     Gets the foreground color of accept button in the footer.
    //
    // Returns:
    //     foreground color of accept button in the footer.
    internal Color GetAcceptButtonTextColor()
    {
        return AcceptButtonTextColor;
    }

    //
    // Summary:
    //     Gets the background color of decline button in the footer.
    //
    // Returns:
    //     background color of decline button in the footer.
    internal Brush GetDeclineButtonBackground()
    {
        return DeclineButtonBackground;
    }

    //
    // Summary:
    //     Gets the foreground color of decline button in the footer.
    //
    // Returns:
    //     foreground color of decline button in the footer.
    internal Color GetDeclineButtonTextColor()
    {
        return DeclineButtonTextColor;
    }

    //
    // Summary:
    //     Gets the border color for the MAUIToolkit.Core.Controls.Popup.PopupView.
    //
    // Returns:
    //     border color for the PopupView.
    internal Color GetStroke()
    {
        return Stroke;
    }

    //
    // Summary:
    //     Gets the border thickness for the MAUIToolkit.Core.Controls.Popup.PopupView.
    //
    // Returns:
    //     border thickness for the PopupView.
    internal virtual double GetStrokeThickness()
    {
        return StrokeThickness;
    }

    //
    // Summary:
    //     Gets the font size of the header title.
    //
    // Returns:
    //     font size of the header title.
    internal virtual double GetHeaderFontSize()
    {
        return HeaderFontSize;
    }

    //
    // Summary:
    //     Gets the font size of the content.
    //
    // Returns:
    //     font size of the content.
    internal virtual double GetMessageFontSize()
    {
        return MessageFontSize;
    }

    //
    // Summary:
    //     Gets the font size of the footer buttons.
    //
    // Returns:
    //     font size of the footer buttons.
    internal virtual double GetFooterFontSize()
    {
        return FooterFontSize;
    }

    //
    // Summary:
    //     Gets the font attribute to be applied for the header title.
    //
    // Returns:
    //     font attribute to be applied for the header title.
    internal FontAttributes GetHeaderFontAttribute()
    {
        return HeaderFontAttribute;
    }

    //
    // Summary:
    //     Gets the font attribute to be applied for the content.
    //
    // Returns:
    //     font attribute to be applied for the content.
    internal FontAttributes GetMessageFontAttribute()
    {
        return MessageFontAttribute;
    }

    //
    // Summary:
    //     Gets the font attribute to be applied for the footer buttons.
    //
    // Returns:
    //     font attribute to be applied for the footer buttons.
    internal FontAttributes GetFooterFontAttribute()
    {
        return FooterFontAttribute;
    }

    //
    // Summary:
    //     Gets the icon color for close button.
    //
    // Returns:
    //     icon color for close button.
    internal Color GetCloseButtonIconStroke()
    {
        return CloseButtonIconStroke;
    }

    //
    // Summary:
    //     Gets the icon stroke thickness for close button.
    //
    // Returns:
    //     icon stroke thickness for close button.
    internal virtual double GetCloseButtonIconStrokeThickness()
    {
        return CloseButtonIconStrokeThickness;
    }

    //
    // Summary:
    //     Gets the background for close button icon when pointer hover.
    //
    // Returns:
    //     background for close button icon when pointer hover.
    internal Color GetHoveredCloseButtonIconBackground()
    {
        return HoveredCloseButtonIconBackground;
    }

    //
    // Summary:
    //     Gets the background for close button icon when pointer press.
    //
    // Returns:
    //     background for close button icon when pointer press.
    internal Color GetPressedCloseButtonIconBackground()
    {
        return PressedCloseButtonIconBackground;
    }

    //
    // Summary:
    //     Gets the overlay color when PopupView is displayed.
    //
    // Returns:
    //     overlay color when PopupView is displayed.
    internal Brush GetOverlayColor()
    {
        return OverlayColor;
    }

    //
    // Summary:
    //     Method to set the theme properties for the MAUIToolkit.Core.Controls.Popup.CPopup.
    //
    // Parameters:
    //   popup:
    //     Instance of CPopup.
    internal void SetThemeProperties(CPopup popup)
    {
        base.Parent = popup;
        ApplyDynamicResource(HeaderBackgroundProperty, "PopupNormalHeaderBackground");
        ApplyDynamicResource(HeaderTextColorProperty, "PopupNormalHeaderTextColor");
        ApplyDynamicResource(MessageBackgroundProperty, "PopupNormalMessageBackground");
        ApplyDynamicResource(MessageTextColorProperty, "popupNormalMessageTextColor");
        ApplyDynamicResource(FooterBackgroundProperty, "PopupNormalFooterBackground");
        ApplyDynamicResource(AcceptButtonBackgroundProperty, "PopupNormalAcceptButtonBackground");
        ApplyDynamicResource(AcceptButtonTextColorProperty, "PopupNormalAcceptButtonTextColor");
        ApplyDynamicResource(DeclineButtonTextColorProperty, "PopupNormalDeclineButtonTextColor");
        ApplyDynamicResource(DeclineButtonBackgroundProperty, "PopupNormalDeclineButtonBackground");
        ApplyDynamicResource(StrokeProperty, "PopupNormalStroke");
        ApplyDynamicResource(StrokeThicknessProperty, "PopupNormalStrokeThickness");
        ApplyDynamicResource(HeaderFontSizeProperty, "PopupNormalHeaderFontSize");
        ApplyDynamicResource(MessageFontSizeProperty, "PopupNormalMessageFontSize");
        ApplyDynamicResource(FooterFontSizeProperty, "PopupNormalFooterFontSize");
        ApplyDynamicResource(CloseButtonIconStrokeProperty, "PopupNormalCloseButtonIconStroke");
        ApplyDynamicResource(CloseButtonIconStrokeThicknessProperty, "PopupNormalCloseButtonIconStrokeThickness");
        ApplyDynamicResource(HoveredCloseButtonIconBackgroundProperty, "PopupHoverCloseButtonIconBackground");
        ApplyDynamicResource(PressedCloseButtonIconBackgroundProperty, "PopupPressedCloseButtonIconBackground");
        ApplyDynamicResource(OverlayColorProperty, "PopupNormalOverlayBackground");
        ApplyDynamicResource(PopupBackgroundProperty, "PopupNormalBackground");
        ThemeElement.InitializeThemeResources(this, "PopupTheme");
    }

    //
    // Summary:
    //     Method to set dynamic resource for PopupStyle properties.
    //
    // Parameters:
    //   bindableProperty:
    //     Bindable property.
    //
    //   key:
    //     Key name for property.
    private void ApplyDynamicResource(BindableProperty bindableProperty, string key)
    {
        if (!IsSet(bindableProperty))
        {
            SetDynamicResource(bindableProperty, key);
        }
    }
}
