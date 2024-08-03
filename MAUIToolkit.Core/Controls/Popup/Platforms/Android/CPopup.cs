using Android.Views.InputMethods;
using MAUIToolkit.Core.Controls.Popup.Platforms.Android;
using MAUIToolkit.Core.Themes;
using MAUIToolkit.Core.Internals.Platforms;
using MAUIToolkit.Core.Localization;
using Microsoft.Maui.Controls.Platform;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     MAUIToolkit.Core.Controls.Popup.CPopup allows the user to display an alert message with
//     the customizable buttons or load any desired view inside the pop-up.
public class CPopup : CView, IParentThemeElement, IThemeElement
{
    //
    // Summary:
    //     List to store the views blurred by this popup.
    internal List<Android.Views.View>? BlurredViews;

    //
    // Summary:
    //     Backing field to store the decorView content.
    internal Android.Views.View DecorViewContent = null;

    //
    // Summary:
    //     Backing field to store the corner radius value.
    internal double RadiusValue = 0.0;

    private bool hasShrunk = false;

    private int decorViewContentBottomHeight = 0;

    //
    // Summary:
    //     Backing field to store the decorView content height before decorView layout was
    //     changed.
    private int oldDecorViewHeight = -1;

    //
    // Summary:
    //     Backing field to store the decorView content width before decorView layout was
    //     changed.
    private int oldDecorViewWidth = -1;

    //
    // Summary:
    //     Identifies the AutoCloseDuration bindable property.
    public static readonly BindableProperty AutoCloseDurationProperty = BindableProperty.Create("AutoCloseDuration", typeof(int), typeof(CPopup), 0, BindingMode.Default);

    //
    // Summary:
    //     Identifies the IsOpen bindable property.
    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create("IsOpen", typeof(bool), typeof(CPopup), false, BindingMode.TwoWay, null, OnIsOpenPropertyChanged);

    //
    // Summary:
    //     Identifies the StaysOpen bindable property.
    public static readonly BindableProperty StaysOpenProperty = BindableProperty.Create("StaysOpen", typeof(bool), typeof(CPopup), false, BindingMode.Default, null, OnStaysOpenChanged);

    //
    // Summary:
    //     Identifies the RelativePosition bindable property.
    public static readonly BindableProperty RelativePositionProperty = BindableProperty.Create("RelativePosition", typeof(PopupRelativePosition), typeof(CPopup), PopupRelativePosition.AlignTop, BindingMode.Default);

    //
    // Summary:
    //     Identifies the RelativeView bindable property.
    public static readonly BindableProperty RelativeViewProperty = BindableProperty.Create("RelativeView", typeof(Microsoft.Maui.Controls.View), typeof(CPopup), null, BindingMode.Default);

    //
    // Summary:
    //     Identifies the AbsoluteX bindable property.
    public static readonly BindableProperty AbsoluteXProperty = BindableProperty.Create("AbsoluteX", typeof(int), typeof(CPopup), 0, BindingMode.Default);

    //
    // Summary:
    //     Identifies the AbsoluteY bindable property.
    public static readonly BindableProperty AbsoluteYProperty = BindableProperty.Create("AbsoluteY", typeof(int), typeof(CPopup), 0, BindingMode.Default);

    //
    // Summary:
    //     Identifies the ShowOverlay bindable property.
    public static readonly BindableProperty ShowOverlayAlwaysProperty = BindableProperty.Create("ShowOverlayAlways", typeof(bool), typeof(CPopup), true, BindingMode.Default);

    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.Controls.Popup.CPopup.OverlayMode Microsoft.Maui.Controls.BindableProperty.
    public static readonly BindableProperty OverlayModeProperty = BindableProperty.Create("OverlayMode", typeof(PopupOverlayMode), typeof(CPopup), PopupOverlayMode.Transparent, BindingMode.Default);

    //
    // Summary:
    //     Identifies the AppearanceMode bindable property.
    public static readonly BindableProperty AppearanceModeProperty = BindableProperty.Create("AppearanceMode", typeof(PopupButtonAppearanceMode), typeof(CPopup), PopupButtonAppearanceMode.OneButton, BindingMode.Default, null, OnAppearanceModePropertyChanged);

    //
    // Summary:
    //     Identifies the HeaderTemplate bindable property.
    public static readonly BindableProperty HeaderTemplateProperty = BindableProperty.Create("HeaderTemplate", typeof(DataTemplate), typeof(CPopup), null, BindingMode.Default, null, OnHeaderTemplatePropertyChanged);

    //
    // Summary:
    //     Identifies the ContentTemplate bindable property.
    public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create("ContentTemplate", typeof(DataTemplate), typeof(CPopup), null, BindingMode.Default, null, OnContentTemplatePropertyChanged);

    //
    // Summary:
    //     Identifies the FooterTemplate bindable property.
    public static readonly BindableProperty FooterTemplateProperty = BindableProperty.Create("FooterTemplate", typeof(DataTemplate), typeof(CPopup), null, BindingMode.Default, null, OnFooterTemplatePropertyChanged);

    //
    // Summary:
    //     Identifies the ShowHeader bindable property.
    public static readonly BindableProperty ShowHeaderProperty = BindableProperty.Create("ShowHeader", typeof(bool), typeof(CPopup), true, BindingMode.Default, null, OnShowHeaderPropertyChanged);

    //
    // Summary:
    //     Identifies the ShowFooter bindable property.
    public static readonly BindableProperty ShowFooterProperty = BindableProperty.Create("ShowFooter", typeof(bool), typeof(CPopup), false, BindingMode.Default, null, OnShowFooterPropertyChanged);

    //
    // Summary:
    //     Identifies the HeaderTitle bindable property.
    public static readonly BindableProperty HeaderTitleProperty = BindableProperty.Create("HeaderTitle", typeof(string), typeof(CPopup), "Title", BindingMode.Default, null, OnHeaderTitlePropertyChanged);

    //
    // Summary:
    //     Identifies the AcceptButtonText bindable property.
    public static readonly BindableProperty AcceptButtonTextProperty = BindableProperty.Create("AcceptButtonText", typeof(string), typeof(CPopup), "ACCEPT", BindingMode.Default, null, OnAcceptButtonTextPropertyChanged);

    //
    // Summary:
    //     Identifies the DeclineButtonText bindable property.
    public static readonly BindableProperty DeclineButtonTextProperty = BindableProperty.Create("DeclineButtonText", typeof(string), typeof(CPopup), "DECLINE", BindingMode.Default, null, OnDeclineButtonTextPropertyChanged);

    //
    // Summary:
    //     Identifies the StartX bindable property.
    public static readonly BindableProperty StartXProperty = BindableProperty.Create("StartX", typeof(int), typeof(CPopup), -1, BindingMode.Default, null, OnStartXPropertyChanged);

    //
    // Summary:
    //     Identifies the StartY bindable property.
    public static readonly BindableProperty StartYProperty = BindableProperty.Create("StartY", typeof(int), typeof(CPopup), -1, BindingMode.Default, null, OnStartYPropertyChanged);

    //
    // Summary:
    //     Identifies the PopupStyle bindable property.
    public static readonly BindableProperty PopupStyleProperty = BindableProperty.Create("PopupStyle", typeof(PopupStyle), typeof(CPopup), null, BindingMode.Default, null, OnPopupStylePropertyChanged, null, null, CreatePopupStyle);

    //
    // Summary:
    //     Identifies the ShowCloseButton bindable property.
    public static readonly BindableProperty ShowCloseButtonProperty = BindableProperty.Create("ShowCloseButton", typeof(bool), typeof(CPopup), false, BindingMode.Default, null, OnShowCloseButtonPropertyChanged);

    //
    // Summary:
    //     Identifies the HeaderHeight bindable property.
    public static readonly BindableProperty HeaderHeightProperty = BindableProperty.Create("HeaderHeight", typeof(int), typeof(CPopup), 72, BindingMode.Default, null, OnHeaderHeightPropertyChanged);

    //
    // Summary:
    //     Identifies the FooterHeight bindable property.
    public static readonly BindableProperty FooterHeightProperty = BindableProperty.Create("FooterHeight", typeof(int), typeof(CPopup), 88, BindingMode.Default, null, OnFooterHeightPropertyChanged);

    //
    // Summary:
    //     Identifies the AcceptCommand bindable property.
    public static readonly BindableProperty AcceptCommandProperty = BindableProperty.Create("AcceptCommand", typeof(ICommand), typeof(CPopup), null, BindingMode.Default);

    //
    // Summary:
    //     Identifies the DeclineCommand bindable property.
    public static readonly BindableProperty DeclineCommandProperty = BindableProperty.Create("DeclineCommand", typeof(ICommand), typeof(CPopup), null, BindingMode.Default);

    //
    // Summary:
    //     Identifies the AutoSizeMode bindable property.
    public static readonly BindableProperty AutoSizeModeProperty = BindableProperty.Create("AutoSizeMode", typeof(PopupAutoSizeMode), typeof(CPopup), PopupAutoSizeMode.None, BindingMode.Default, null, OnAutoSizeModePropertyChanged);

    //
    // Summary:
    //     Identifies the IsFullScreen bindable property.
    public static readonly BindableProperty IsFullScreenProperty = BindableProperty.Create("IsFullScreen", typeof(bool), typeof(CPopup), false, BindingMode.Default, null, OnIsFullScreenPropertyChanged);

    //
    // Summary:
    //     Identifies the Message bindable property.
    public static readonly BindableProperty MessageProperty = BindableProperty.Create("Message", typeof(string), typeof(CPopup), "Popup Message", BindingMode.Default, null, OnMessagePropertyChanged);

    //
    // Summary:
    //     Identifies the AnimationDuration bindable property.
    public static readonly BindableProperty AnimationDurationProperty = BindableProperty.Create("AnimationDuration", typeof(double), typeof(CPopup), 300.0, BindingMode.Default, null, OnAnimationDurationPropertyChanged);

    //
    // Summary:
    //     Identifies the AnimationMode bindable property.
    public static readonly BindableProperty AnimationModeProperty = BindableProperty.Create("AnimationMode", typeof(PopupAnimationMode), typeof(CPopup), PopupAnimationMode.Fade, BindingMode.Default, null, OnAnimationModePropertyChanged);

    //
    // Summary:
    //     Identifies the AnimationEasing bindable property.
    public static readonly BindableProperty AnimationEasingProperty = BindableProperty.Create("AnimationEasing", typeof(PopupAnimationEasing), typeof(CPopup), PopupAnimationEasing.Linear, BindingMode.Default, null, OnAnimationEasingPropertyChanged);

    //
    // Summary:
    //     The overlay of the PopupView.
    internal Internals.Platforms.WindowOverlay? PopupOverlay;

    //
    // Summary:
    //     The popup overlay container view of the PopupView.
    internal PopupOverlayContainer? PopupOverlayContainer;

    //
    // Summary:
    //     The popup view of the MAUIToolkit.Core.Controls.Popup.CPopup that will be displayed when
    //     setting the MAUIToolkit.Core.Controls.Popup.CPopup.IsOpen property as true.
    internal PopupView? PopupView;

    //
    // Summary:
    //     Default height of the PopupView.
    internal double PopupViewHeight;

    //
    // Summary:
    //     Default width of the PopupView.
    internal double PopupViewWidth;

    //
    // Summary:
    //     Boolean value indicating whether the Popup opening or closing animation is in
    //     progress.
    internal bool IsPopupAnimationInProgress;

    //
    // Summary:
    //     Boolean value indicating whether the container opening or closing animation is
    //     in progress.
    internal bool IsContainerAnimationInProgress;

    //
    // Summary:
    //     Gets a value indicating whether the flow direction is RTL or not.
    internal bool IsRTL = false;

    //
    // Summary:
    //     Represents a task completion source that can be used to create and control a
    //     Tasks with a result of type bool.
    private TaskCompletionSource<bool>? taskCompletionSource;

    //
    // Summary:
    //     X-point of the Popup, after calculation of the popup view x position.
    private double popupXPosition;

    //
    // Summary:
    //     Y-point of the Popup, after calculation of the popup view y position.
    private double popupYPosition;

    //
    // Summary:
    //     X-point of the Popup, if Popup is layout relative to a view or Popup is displayed
    //     in the touch point.
    private double positionXPoint;

    //
    // Summary:
    //     Y-point of the Popup, if Popup is layout relative to a view or Popup is displayed
    //     in the touch point.
    private double positionYPoint;

    //
    // Summary:
    //     View relative to which popup should be displayed.
    private Microsoft.Maui.Controls.View? relativeView;

    //
    // Summary:
    //     Position relative to the RelativeView, from which popup should be displayed.
    private PopupRelativePosition relativePosition;

    //
    // Summary:
    //     absolute X-Point where the popup should be positioned from the relative view.
    private double absoluteXPoint;

    //
    // Summary:
    //     absolute Y-Point where the popup should be positioned from the relative view.
    private double absoluteYPoint;

    //
    // Summary:
    //     Backing field for the MAUIToolkit.Core.Controls.Popup.CPopup.AppliedHeaderHeight property.
    private double appliedHeaderHeight;

    //
    // Summary:
    //     Backing field for the MAUIToolkit.Core.Controls.Popup.CPopup.AppliedFooterHeight property.
    private double appliedFooterHeight;

    //
    // Summary:
    //     Backing field for the MAUIToolkit.Core.Controls.Popup.CPopup.AppliedBodyHeight property.
    private double appliedBodyHeight;

    //
    // Summary:
    //     Backing field to store the PopupView Y point before keyboard comes to the View.
    //     Used this field to reset the PopupView Y point after keyboard hides from the
    //     view.
    private int popupYPositionBeforeKeyboardInView = 0;

    //
    // Summary:
    //     Backing field to store the PopupView height before keyboard comes to the View.
    //     Used this field to reset the PopupView height after keyboard hides from the view.
    private double popupViewHeightBeforeKeyboardInView = 0.0;

    //
    // Summary:
    //     At Show(x,y) given x point in Sample to display the Popup.
    private double showXPosition;

    //
    // Summary:
    //     At Show(x,y) given y point in Sample to display the Popup.
    private double showYPosition;

    //
    // Summary:
    //     Minimal padding value used to identify whether PopupView is positioned at screen
    //     edges.
    private int minimalPadding = 5;

    //
    // Summary:
    //     Backing field to store the PopupView width before applying padding.
    private double popupViewWidthBeforePadding;

    //
    // Summary:
    //     Backing field to store the PopupView height before applying padding.
    private double popupViewHeightBeforePadding;

    //
    // Summary:
    //     Backing field for SemanticDescription.
    private string? semanticDescription;

    //
    // Summary:
    //     Backing field for CanShowPopupInFullScreen.
    private bool canShowPopupInFullScreen;

    //
    // Summary:
    //     Gets or sets a value indicating whether the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView
    //     is open or not.
    //
    // Value:
    //     The value indicating whether the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView is open
    //     or not.
    //
    // Remarks:
    //     The MAUIToolkit.Core.Controls.Popup.CPopup.PopupView will be opened and closed based on
    //     this value.
    public bool IsOpen
    {
        get
        {
            return (bool)GetValue(IsOpenProperty);
        }
        set
        {
            SetValue(IsOpenProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether overlay should be transparent or blurred.
    //
    //
    // Value:
    //     OverlayMode.Blur If the overlay should be blurred otherwiseOverlayMode.Transparent.The
    //     default value is OverlayMode.Transparent.
    //
    // Remarks:
    //     OverlayMode.Blur will be applied to android 31 and above. In below android 31,
    //     OverlayMode.Transparent will be applied by default.
    public PopupOverlayMode OverlayMode
    {
        get
        {
            return (PopupOverlayMode)GetValue(OverlayModeProperty);
        }
        set
        {
            SetValue(OverlayModeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the relative position, where the pop-up should be displayed relatively
    //     to MAUIToolkit.Core.Controls.Popup.CPopup.RelativeView. The relative position can also
    //     be absolutely adjusted using the MAUIToolkit.Core.Controls.Popup.CPopup.AbsoluteX and
    //     MAUIToolkit.Core.Controls.Popup.CPopup.AbsoluteY properties.
    public PopupRelativePosition RelativePosition
    {
        get
        {
            return (PopupRelativePosition)GetValue(RelativePositionProperty);
        }
        set
        {
            SetValue(RelativePositionProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the absolute x-point to display a pop-up when positioning it relatively
    //     to the specified MAUIToolkit.Core.Controls.Popup.CPopup.RelativeView based on the MAUIToolkit.Core.Controls.Popup.CPopup.RelativePosition.
    //     The pop-up will be displayed based on this property value only when relatively
    //     displaying it by using the MAUIToolkit.Core.Controls.Popup.CPopup.RelativeView property.
    public int AbsoluteX
    {
        get
        {
            return (int)GetValue(AbsoluteXProperty);
        }
        set
        {
            SetValue(AbsoluteXProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the absolute y-point to display a pop-up when positioning it relatively
    //     to the specified MAUIToolkit.Core.Controls.Popup.CPopup.RelativeView based on the MAUIToolkit.Core.Controls.Popup.CPopup.RelativePosition.
    //     The pop-up will be displayed based on this property value only when relatively
    //     displaying it by using the MAUIToolkit.Core.Controls.Popup.CPopup.RelativeView property.
    public int AbsoluteY
    {
        get
        {
            return (int)GetValue(AbsoluteYProperty);
        }
        set
        {
            SetValue(AbsoluteYProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the view relative to which the popup should be displayed based on
    //     the MAUIToolkit.Core.Controls.Popup.CPopup.RelativePosition. The relative position can
    //     also be absolutely adjusted using the MAUIToolkit.Core.Controls.Popup.CPopup.AbsoluteX
    //     and MAUIToolkit.Core.Controls.Popup.CPopup.AbsoluteY properties.
    public Microsoft.Maui.Controls.View RelativeView
    {
        get
        {
            return (Microsoft.Maui.Controls.View)GetValue(RelativeViewProperty);
        }
        set
        {
            SetValue(RelativeViewProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView
    //     should be opened, when interacting outside its boundary area.
    public bool StaysOpen
    {
        get
        {
            return (bool)GetValue(StaysOpenProperty);
        }
        set
        {
            SetValue(StaysOpenProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or Sets a value indicating whether an overlay can be shown around the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView.
    public bool ShowOverlayAlways
    {
        get
        {
            return (bool)GetValue(ShowOverlayAlwaysProperty);
        }
        set
        {
            SetValue(ShowOverlayAlwaysProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the type of layout template of the PopupView.
    //
    // Value:
    //     PopupButtonAppearanceMode.OneButton displays a single button in the PopupView
    //     footer. PopupButtonAppearanceMode.TwoButton displays two buttons in the PopupView
    //     footer.
    public PopupButtonAppearanceMode AppearanceMode
    {
        get
        {
            return (PopupButtonAppearanceMode)GetValue(AppearanceModeProperty);
        }
        set
        {
            SetValue(AppearanceModeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the template to be loaded in the header of the PopupView.
    //
    // Remarks:
    //     MAUIToolkit.Core.Controls.Popup.CPopup.PopupStyle does not apply to templated elements.
    public DataTemplate HeaderTemplate
    {
        get
        {
            return (DataTemplate)GetValue(HeaderTemplateProperty);
        }
        set
        {
            SetValue(HeaderTemplateProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the template to be loaded in the body of the PopupView.
    //
    // Remarks:
    //     MAUIToolkit.Core.Controls.Popup.CPopup.PopupStyle does not apply to templated elements.
    public DataTemplate ContentTemplate
    {
        get
        {
            return (DataTemplate)GetValue(ContentTemplateProperty);
        }
        set
        {
            SetValue(ContentTemplateProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the template to be loaded in the footer of the PopupView.
    //
    // Remarks:
    //     MAUIToolkit.Core.Controls.Popup.CPopup.PopupStyle does not apply to templated elements.
    //     MAUIToolkit.Core.Controls.Popup.CPopup.ShowFooter need to be enabled to display footer.
    public DataTemplate FooterTemplate
    {
        get
        {
            return (DataTemplate)GetValue(FooterTemplateProperty);
        }
        set
        {
            SetValue(FooterTemplateProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether the header is to be included in the PopupView.
    public bool ShowHeader
    {
        get
        {
            return (bool)GetValue(ShowHeaderProperty);
        }
        set
        {
            SetValue(ShowHeaderProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the time delay in milliseconds for automatically closing the Popup.
    public int AutoCloseDuration
    {
        get
        {
            return (int)GetValue(AutoCloseDurationProperty);
        }
        set
        {
            SetValue(AutoCloseDurationProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether the footer is to be included in the PopupView.
    //
    //
    // Value:
    //     true if show footer; otherwise, false. The default value is false.
    public bool ShowFooter
    {
        get
        {
            return (bool)GetValue(ShowFooterProperty);
        }
        set
        {
            SetValue(ShowFooterProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the header title of the PopupView.
    public string HeaderTitle
    {
        get
        {
            return (string)GetValue(HeaderTitleProperty);
        }
        set
        {
            SetValue(HeaderTitleProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the popup message of the PopupView.
    public string Message
    {
        get
        {
            return (string)GetValue(MessageProperty);
        }
        set
        {
            SetValue(MessageProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the text of accept button in the footer.
    public string AcceptButtonText
    {
        get
        {
            return (string)GetValue(AcceptButtonTextProperty);
        }
        set
        {
            SetValue(AcceptButtonTextProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the text of decline button in the footer.
    public string DeclineButtonText
    {
        get
        {
            return (string)GetValue(DeclineButtonTextProperty);
        }
        set
        {
            SetValue(DeclineButtonTextProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the x-position of the PopupView.
    public int StartX
    {
        get
        {
            return (int)GetValue(StartXProperty);
        }
        set
        {
            SetValue(StartXProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the y-position of the PopupView.
    public int StartY
    {
        get
        {
            return (int)GetValue(StartYProperty);
        }
        set
        {
            SetValue(StartYProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the style to be applied to the PopupView in MAUIToolkit.Core.Controls.Popup.CPopup.
    public PopupStyle PopupStyle
    {
        get
        {
            return (PopupStyle)GetValue(PopupStyleProperty);
        }
        set
        {
            SetValue(PopupStyleProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the header height of the PopupView.
    public int HeaderHeight
    {
        get
        {
            return (int)GetValue(HeaderHeightProperty);
        }
        set
        {
            SetValue(HeaderHeightProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the footer height of the PopupView.
    public int FooterHeight
    {
        get
        {
            return (int)GetValue(FooterHeightProperty);
        }
        set
        {
            SetValue(FooterHeightProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether to show the close button in the header.
    public bool ShowCloseButton
    {
        get
        {
            return (bool)GetValue(ShowCloseButtonProperty);
        }
        set
        {
            SetValue(ShowCloseButtonProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the command to invoke when the accept button in the footer is tapped.
    public ICommand AcceptCommand
    {
        get
        {
            return (ICommand)GetValue(AcceptCommandProperty);
        }
        set
        {
            SetValue(AcceptCommandProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the command to invoke when the decline button in the footer is tapped.
    public ICommand DeclineCommand
    {
        get
        {
            return (ICommand)GetValue(DeclineCommandProperty);
        }
        set
        {
            SetValue(DeclineCommandProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether to show the Popupview in full screen
    //     or not.
    //
    // Remarks:
    //     If MAUIToolkit.Core.Controls.Popup.CPopup.IsFullScreen is set as true, the height request
    //     and width request given for the PopupView will not be considered.
    public bool IsFullScreen
    {
        get
        {
            return (bool)GetValue(IsFullScreenProperty);
        }
        set
        {
            SetValue(IsFullScreenProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets a value that determines how to size the PopupView based on its template
    //     contents.
    //
    // Value:
    //     The default value is MAUIToolkit.Core.Controls.Popup.PopupAutoSizeMode.None.
    //
    // Remarks:
    //     MAUIToolkit.Core.Controls.Popup.CPopup.AutoSizeMode will be applied to MAUIToolkit.Core.Controls.Popup.CPopup
    //     only if MAUIToolkit.Core.Controls.Popup.CPopup.ContentTemplate is defined.
    public PopupAutoSizeMode AutoSizeMode
    {
        get
        {
            return (PopupAutoSizeMode)GetValue(AutoSizeModeProperty);
        }
        set
        {
            SetValue(AutoSizeModeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the animation to be applied for the PopupView when opening and closing
    //     it.
    public PopupAnimationMode AnimationMode
    {
        get
        {
            return (PopupAnimationMode)GetValue(AnimationModeProperty);
        }
        set
        {
            SetValue(AnimationModeProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the duration in milliseconds of the animation played when opening
    //     and closing the PopupView.
    //
    // Value:
    //     The duration in milliseconds of the animation played at the opening and closing
    //     of the PopupView.
    public double AnimationDuration
    {
        get
        {
            return (double)GetValue(AnimationDurationProperty);
        }
        set
        {
            SetValue(AnimationDurationProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the animation easing effect to be applied to the PopupView's opening
    //     and closing animation.
    //
    // Value:
    //     The animation easing effect to be applied for the PopupView when it opens and
    //     closes.
    public PopupAnimationEasing AnimationEasing
    {
        get
        {
            return (PopupAnimationEasing)GetValue(AnimationEasingProperty);
        }
        set
        {
            SetValue(AnimationEasingProperty, value);
        }
    }

    //
    // Summary:
    //     Gets or sets the height applied to the header of the PopupView.
    internal double AppliedHeaderHeight
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
    internal double AppliedFooterHeight
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
    internal double AppliedBodyHeight
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
    //     Gets or sets the Semantic description value.
    internal string SemanticDescription
    {
        get
        {
            if (string.IsNullOrEmpty(semanticDescription))
            {
                semanticDescription = SemanticProperties.GetDescription(this);
            }

            return semanticDescription;
        }
        set
        {
            semanticDescription = value;
        }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether CanShowPopupInFullScreen true or false.
    internal bool CanShowPopupInFullScreen
    {
        get
        {
            return canShowPopupInFullScreen;
        }
        set
        {
            canShowPopupInFullScreen = value;
            OnPropertyChanged("CanShowPopupInFullScreen");
        }
    }

    //
    // Summary:
    //     This event will be fired whenever the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView
    //     is shown in the view.
    //
    // Remarks:
    //     This event fires whenever the MAUIToolkit.Core.Controls.Popup.CPopup.IsOpen property is
    //     set as true.
    public event EventHandler? Opened;

    //
    // Summary:
    //     This event will be fired whenever the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView
    //     is dismissed from the view.
    //
    // Remarks:
    //     This event fires whenever the MAUIToolkit.Core.Controls.Popup.CPopup.IsOpen property is
    //     set as false.
    public event EventHandler? Closed;

    //
    // Summary:
    //     This event will be fired whenever the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView
    //     is opening in the view. Occurring of this event can be cancelled based on conditions.
    public event EventHandler<CancelEventArgs>? Opening;

    //
    // Summary:
    //     This event will be fired whenever the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView
    //     is closing in the view. Occurring of this event can be cancelled based on conditions.
    public event EventHandler<CancelEventArgs>? Closing;

    //
    // Summary:
    //     Used to wire the events.
    internal void WirePlatformSpecificEvents()
    {
    }

    //
    // Summary:
    //     Used to unwire the events.
    internal void UnWirePlatformSpecificEvents()
    {
    }

    //
    // Summary:
    //     Reposition and Resize the PopupView based on Keyboard.
    internal void PopupPositionBasedOnKeyboard()
    {
        PopupView popupView = PopupView;
        if (popupView != null)
        {
            popupView.HandlerChanged += OnPopupViewHandlerChanged;
        }
    }

    //
    // Summary:
    //     Applies shadow to the native popup view.
    internal void ApplyNativePopupViewShadow()
    {
        if (PopupView == null || PopupView.Handler == null || PopupView.Handler.PlatformView == null)
        {
            return;
        }

        Android.Views.View view = (Android.Views.View)PopupView.Handler.PlatformView;
        MauiDrawable mauiDrawable = new MauiDrawable(view.Context);
        Brush popupBackground = PopupStyle.GetPopupBackground();
        mauiDrawable.SetBackground(popupBackground);
        CornerRadius cornerRadius = (IsRTL ? new CornerRadius(PopupStyle.CornerRadius.TopRight, PopupStyle.CornerRadius.TopLeft, PopupStyle.CornerRadius.BottomRight, PopupStyle.CornerRadius.BottomLeft) : PopupStyle.CornerRadius);
        float[] array = new float[8]
        {
        (float)(cornerRadius.TopLeft * DeviceDisplay.MainDisplayInfo.Density),
        (float)(cornerRadius.TopLeft * DeviceDisplay.MainDisplayInfo.Density),
        (float)(cornerRadius.TopRight * DeviceDisplay.MainDisplayInfo.Density),
        (float)(cornerRadius.TopRight * DeviceDisplay.MainDisplayInfo.Density),
        (float)(cornerRadius.BottomRight * DeviceDisplay.MainDisplayInfo.Density),
        (float)(cornerRadius.BottomRight * DeviceDisplay.MainDisplayInfo.Density),
        (float)(cornerRadius.BottomLeft * DeviceDisplay.MainDisplayInfo.Density),
        (float)(cornerRadius.BottomLeft * DeviceDisplay.MainDisplayInfo.Density)
        };
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            mauiDrawable.SetCornerRadii(array);
        }
        else
        {
            float num = array.Average();
            if (num != 0f)
            {
                mauiDrawable.SetCornerRadius(num);
                RadiusValue = (double)num / DeviceDisplay.MainDisplayInfo.Density;
            }
            else
            {
                RadiusValue = 0.0;
            }
        }
        view.SetBackground(mauiDrawable);
        if (PopupStyle.HasShadow)
        {
            view.SetElevation((float)(6.0 * DeviceDisplay.MainDisplayInfo.Density));
        }
        else
        {
            view.SetElevation(0f);
        }
    }

    //
    // Summary:
    //     Handles, when popup view handler has changed.
    //
    // Parameters:
    //   sender:
    //     The sender of the event.
    //
    //   e:
    //     The System.EventArgs.
    private void OnPopupViewHandlerChanged(object? sender, EventArgs e)
    {
        IMauiContext mauiContext = PopupExtension.GetMainPage()?.Handler?.MauiContext;
        if (mauiContext == null)
        {
            return;
        }

        Android.Views.View view = (Android.Views.View)(PopupExtension.GetMainPage()?.ToPlatform(mauiContext));
        if (view != null)
        {
            view.ViewTreeObserver.GlobalLayout += OnViewTreeObserverGlobalLayout;
            DecorViewContent = WindowOverlayHelper.decorViewContent;
            if (DecorViewContent != null)
            {
                oldDecorViewHeight = DecorViewContent.Height;
                oldDecorViewWidth = DecorViewContent.Width;
            }
        }
    }

    //
    // Summary:
    //     Handles, when global layout of popupview has changed.
    //
    // Parameters:
    //   sender:
    //     The sender of the event.
    //
    //   e:
    //     The System.EventArgs.
    private void OnViewTreeObserverGlobalLayout(object? sender, EventArgs e)
    {
        if (DecorViewContent != null && oldDecorViewHeight != -1 && oldDecorViewWidth != -1 && (oldDecorViewHeight != DecorViewContent.Height || oldDecorViewWidth != DecorViewContent.Width))
        {
            AbortPopupViewAnimation();
            ResetAnimatedProperties();
            if (IsOpen)
            {
                PopupOverlay.UpdateWindowManagerLayoutParamsSize();
                ResetPopupWidthHeight();
                PopupView.InvalidateForceLayout();
            }
        }

        DecorViewContent = WindowOverlayHelper.decorViewContent;
        int bottom = WindowOverlayHelper.decorViewFrame.Bottom;
        if (DecorViewContent == null)
        {
            return;
        }

        if (decorViewContentBottomHeight == 0)
        {
            decorViewContentBottomHeight = DecorViewContent.Height - bottom;
        }

        oldDecorViewHeight = DecorViewContent.Height;
        oldDecorViewWidth = DecorViewContent.Width;
        InputMethodManager inputMethodManager = (InputMethodManager)DecorViewContent.Context.GetSystemService("input_method");
        Android.Graphics.Rect rect = new Android.Graphics.Rect();
        DecorViewContent.GetWindowVisibleDisplayFrame(rect);
        int num = 0;
        num = DecorViewContent.Height - (WindowOverlayHelper.decorViewFrame.Top + WindowOverlayHelper.decorViewFrame.Bottom);
        if (IsOpen)
        {
            if (num > 0 && inputMethodManager.IsAcceptingText && !hasShrunk)
            {
                hasShrunk = true;
                PositionPoupViewBasedOnKeyboard((float)rect.Bottom / WindowOverlayHelper.density);
            }
            else if ((!inputMethodManager.IsAcceptingText && num == 0 && hasShrunk) || (num == 0 && hasShrunk))
            {
                UnshrinkPoupViewOnKeyboardCollapse();
                hasShrunk = false;
            }
        }
        else if (!inputMethodManager.IsAcceptingText && hasShrunk)
        {
            inputMethodManager.HideSoftInputFromWindow(DecorViewContent.WindowToken, HideSoftInputFlags.None);
            UnshrinkPoupViewOnKeyboardCollapse();
            hasShrunk = false;
        }

        inputMethodManager = null;
        rect = null;
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.CPopup class.
    public CPopup()
    {
        Initialize();
        ThemeElement.InitializeThemeResources(this, "CPopupTheme");
    }

    //
    // Summary:
    //     Displays a popup with the specified title and message.
    //
    // Parameters:
    //   title:
    //     The title of the popup.
    //
    //   message:
    //     The message to be displayed in the popup.
    //
    //   autoCloseDuration:
    //     The delay in milliseconds after which the popup will automatically close.
    public static void Show(string title, string message, int autoCloseDuration = 0)
    {
        CPopup cPopup = new CPopup
        {
            HeaderTitle = title,
            Message = message,
            AutoCloseDuration = autoCloseDuration
        };
        cPopup.Show();
    }

    //
    // Summary:
    //     Displays a popup with the specified title and message, along with an accept button.
    //
    //
    // Parameters:
    //   title:
    //     The title of the popup.
    //
    //   message:
    //     The message to be displayed in the popup.
    //
    //   acceptText:
    //     The text to be displayed on the accept button in the footer.
    //
    //   autoCloseDuration:
    //     The delay in milliseconds after which the popup will automatically close.
    public static void Show(string title, string message, string acceptText, int autoCloseDuration = 0)
    {
        CPopup cPopup = new CPopup
        {
            HeaderTitle = title,
            Message = message,
            AcceptButtonText = acceptText,
            ShowFooter = true,
            AutoCloseDuration = autoCloseDuration
        };
        cPopup.Show();
    }

    //
    // Summary:
    //     Displays a popup with the specified title, message, accept button, and a decline
    //     button with the provided text.
    //
    // Parameters:
    //   title:
    //     The title of the popup.
    //
    //   message:
    //     The message to be displayed in the popup.
    //
    //   acceptText:
    //     The text to be displayed on the accept button in the footer.
    //
    //   declineText:
    //     The text to be displayed on the decline button in the footer.
    //
    //   autoCloseDuration:
    //     The delay in milliseconds after which the popup will automatically close.
    //
    // Returns:
    //     A task representing the asynchronous operation. The result will be true if the
    //     popup was closed using the accept button; otherwise, false.
    public static async Task<bool> Show(string title, string message, string acceptText, string declineText, int autoCloseDuration = 0)
    {
        CPopup popup = new CPopup
        {
            HeaderTitle = title,
            Message = message,
            AcceptButtonText = acceptText,
            DeclineButtonText = declineText,
            ShowHeader = true,
            ShowFooter = true,
            AppearanceMode = PopupButtonAppearanceMode.TwoButton,
            AutoCloseDuration = autoCloseDuration
        };
        popup.taskCompletionSource = new TaskCompletionSource<bool>();
        popup.Show();
        return await popup.taskCompletionSource.Task;
    }

    //
    // Summary:
    //     Displays a Popup.
    //
    // Returns:
    //     A task representing the asynchronous operation. The result will be true if the
    //     popup was closed using the accept button; otherwise, false.
    public async Task<bool> ShowAsync()
    {
        taskCompletionSource = new TaskCompletionSource<bool>();
        Show();
        return await taskCompletionSource.Task;
    }

    //
    // Summary:
    //     Displays the Popup in the screen.
    //
    // Parameters:
    //   isfullscreen:
    //     Specifies whether the popup should be displayed in full screen or not.
    public void Show(bool isfullscreen = false)
    {
        if (PopupOverlay != null)
        {
            if (!IsFullScreen)
            {
                CanShowPopupInFullScreen = isfullscreen;
            }
            else
            {
                CanShowPopupInFullScreen = IsFullScreen;
            }

            if (!IsOpen)
            {
                OpenOrClosePopup(open: true);
            }
        }
    }

    //
    // Summary:
    //     Displays the popup at the specific x and y point.
    //
    // Parameters:
    //   xPosition:
    //     The x point at which the popup should be displayed.
    //
    //   yPosition:
    //     The y point at which the popup should be displayed.
    public void Show(double xPosition, double yPosition)
    {
        if (PopupOverlay == null)
        {
            return;
        }

        showXPosition = xPosition;
        showYPosition = yPosition;
        Page mainPage = PopupExtension.GetMainPage();
        Application application = IPlatformApplication.Current?.Application as Application;
        int screenWidth;
        int screenHeight;
        if ((mainPage != null && mainPage.Window != null && mainPage.Handler != null) || (application != null && application.MainPage is Shell shell && shell.IsLoaded && application.MainPage.Handler != null))
        {
            screenWidth = PopupExtension.GetScreenWidth();
            screenHeight = PopupExtension.GetScreenHeight();
            if (IsRTL)
            {
                if (ContentTemplate == null)
                {
                    goto IL_0162;
                }

                if (!(base.WidthRequest > 0.0) && !(base.MinimumWidthRequest > 0.0))
                {
                    PopupView? popupView = PopupView;
                    if (popupView == null || !(popupView.PopupMessageView?.WidthRequest > 0.0))
                    {
                        goto IL_0162;
                    }
                }

                PopupViewWidth = Math.Max(GetFinalWidth(PopupView.PopupMessageView.Content), Math.Max(base.WidthRequest, base.MinimumWidthRequest));
                goto IL_01a9;
            }

            positionXPoint = (int)ValidatePopupPosition(xPosition, PopupViewWidth, screenWidth);
            goto IL_01de;
        }

        WireEvents();
        return;
    IL_0162:
        if (base.WidthRequest >= 0.0 || base.MinimumWidthRequest >= 0.0)
        {
            PopupViewWidth = Math.Max(base.WidthRequest, base.MinimumWidthRequest);
        }

        goto IL_01a9;
    IL_01a9:
        positionXPoint = (int)ValidatePopupXPositionForRTL(xPosition, PopupViewWidth, screenWidth);
        goto IL_01de;
    IL_01de:
        int num = ((!(DeviceInfo.Platform == DevicePlatform.Android) || OverlayMode != PopupOverlayMode.Blur) ? PopupExtension.GetStatusBarHeight() : 0);
        positionYPoint = (int)ValidatePopupPosition(yPosition, PopupViewHeight, screenHeight - num - PopupExtension.GetActionBarHeight()) + num + PopupExtension.GetActionBarHeight();
        if (!IsOpen)
        {
            OpenOrClosePopup(open: true);
        }
    }

    //
    // Summary:
    //     Displays popup relative to the given view.
    //
    // Parameters:
    //   relativeView:
    //     The view relative to which popup should be displayed.
    //
    //   relativePosition:
    //     The position where popup should be displayed relative to the given view.
    //
    //   absoluteX:
    //     Absolute x Point where the popup should be positioned from the relative view.
    //
    //
    //   absoluteY:
    //     Absolute y Point where the popup should be positioned from the relative view.
    public void ShowRelativeToView(Microsoft.Maui.Controls.View relativeView, PopupRelativePosition relativePosition, double absoluteX = double.NaN, double absoluteY = double.NaN)
    {
        if (PopupOverlay == null || PopupView == null)
        {
            return;
        }

        this.relativeView = relativeView;
        this.relativePosition = relativePosition;
        absoluteXPoint = (double.IsNaN(absoluteX) ? 0.0 : absoluteX);
        absoluteYPoint = (double.IsNaN(absoluteY) ? 0.0 : absoluteY);
        if (!IsOpen)
        {
            OpenOrClosePopup(open: true);
            if (RelativeView != null)
            {
                return;
            }
        }

        SetParent();
        if (PopupView.HeaderView != null)
        {
            PopupView.HeaderView.UpdateHeaderCloseButton();
        }

        CalculatePopupViewWidth();
        CalculatePopupViewHeight();
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            ApplyOverlayBackground();
        }

        PositionPopupRelativeToView(this.relativeView, this.relativePosition, absoluteXPoint, absoluteYPoint);
        if (DeviceInfo.Platform != DevicePlatform.Android)
        {
            ApplyOverlayBackground();
        }

        UpdatePopupView();
        if (!RaisePopupOpeningEvent())
        {
            ApplyContainerAnimation();
            ApplyPopupAnimation();
            WireEvents();
            WirePlatformSpecificEvents();
        }
        else
        {
            OpenOrClosePopup(open: false);
        }
    }

    //
    // Summary:
    //     Dismisses the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView from the view.
    public void Dismiss()
    {
        if (IsOpen && !StaysOpen)
        {
            OpenOrClosePopup(open: false);
        }
    }

    //
    // Summary:
    //     Refreshes the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView for run-time value changes.
    public void Refresh()
    {
        if (IsOpen && PopupView != null)
        {
            ResetPopupWidthHeight();
            PopupView.ApplyShadowAndCornerRadius();
            if (popupViewHeightBeforeKeyboardInView != 0.0)
            {
                popupViewHeightBeforeKeyboardInView = PopupViewHeight;
            }

            if (popupYPositionBeforeKeyboardInView != 0)
            {
                popupYPositionBeforeKeyboardInView = PopupView.GetY();
            }

            PopupView.InvalidateForceLayout();
        }
    }

    //
    // Summary:
    //     Method invoke to get the initial set of color's from theme dictionary.
    //
    // Returns:
    //     Return the Data Grid theme dictionary.
    ResourceDictionary IParentThemeElement.GetThemeDictionary()
    {
        return null;
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
        if (!newTheme.Equals(oldTheme))
        {
            PopupStyle.SetThemeProperties(this);
        }
    }

    //
    // Summary:
    //     Sets the semantic description value to the respective child elements.
    internal void SetSemanticDescription()
    {
        if (PopupView.HeaderView.PopupCloseButton != null)
        {
            SemanticProperties.SetDescription(PopupView.HeaderView.PopupCloseButton, SemanticDescription + " Close Button");
        }

        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            if (PopupView.HeaderView.TitleLabel != null)
            {
                SemanticProperties.SetDescription(PopupView.HeaderView.TitleLabel, SemanticDescription);
            }

            if (PopupView.PopupMessageView != null)
            {
                SemanticProperties.SetDescription(PopupView.PopupMessageView.MessageView, SemanticDescription);
            }
        }
        else
        {
            if (PopupView.HeaderView.TitleLabel != null)
            {
                SemanticProperties.SetDescription(PopupView.HeaderView.TitleLabel, SemanticDescription + " Title");
            }

            if (PopupView.PopupMessageView != null)
            {
                SemanticProperties.SetDescription(PopupView.PopupMessageView.MessageView, SemanticDescription + " Message view");
            }
        }
    }

    //
    // Summary:
    //     Dismisses the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView from the view.
    internal void DismissPopup()
    {
        if (PopupView == null || !PopupView.IsVisible)
        {
            return;
        }

        if (!RaisePopupClosingEvent())
        {
            if (PopupOverlay != null)
            {
                if (ShowOverlayAlways)
                {
                    PopupExtension.ClearBlurViews(this);
                }

                ApplyContainerAnimation();
                ApplyPopupAnimation();
                UnWireEvents();
                UnWirePlatformSpecificEvents();
            }

            if (PopupView.Popup.taskCompletionSource != null)
            {
                PopupView.Popup.taskCompletionSource.SetResult(PopupView.AcceptButtonClicked);
                PopupView.Popup.taskCompletionSource = null;
            }
        }

        PopupView.AcceptButtonClicked = false;
    }

    //
    // Summary:
    //     Raises the MAUIToolkit.Core.Controls.Popup.CPopup.Closed event.
    internal void RaisePopupClosedEvent()
    {
        if (this.Closed != null)
        {
            this.Closed(this, EventArgs.Empty);
        }
    }

    //
    // Summary:
    //     Raises the Popup Closing event.
    //
    // Returns:
    //     Returns whether to cancel closing of the Popup.
    internal bool RaisePopupClosingEvent()
    {
        if (this.Closing != null)
        {
            CancelEventArgs cancelEventArgs = new CancelEventArgs();
            this.Closing(this, cancelEventArgs);
            return cancelEventArgs.Cancel;
        }

        return false;
    }

    //
    // Summary:
    //     Raises the MAUIToolkit.Core.Controls.Popup.CPopup.Opened event.
    internal void RaisePopupOpenedEvent()
    {
        if (this.Opened != null)
        {
            this.Opened(this, EventArgs.Empty);
        }
    }

    //
    // Summary:
    //     Raises the Popup Opening event.
    //
    // Returns:
    //     Returns whether to cancel opening of the Popup.
    internal bool RaisePopupOpeningEvent()
    {
        if (this.Opening != null)
        {
            CancelEventArgs cancelEventArgs = new CancelEventArgs();
            this.Opening(this, cancelEventArgs);
            return cancelEventArgs.Cancel;
        }

        return false;
    }

    //
    // Summary:
    //     Closes the popup, when touch is made outside the popup view, provided StaysOpen
    //     is false.
    internal void ClosePopupIfRequired()
    {
        if (!StaysOpen)
        {
            IsOpen = false;
        }
    }

    //
    // Summary:
    //     Checks whether the layout of the popup view is made for the given size and position.
    //
    //
    // Returns:
    //     True, if popup layout for given size and position. False otherwise.
    //
    // Remarks:
    //     If the layout is made for the given size and position, PopupView will not be
    //     layout for the full view of the application. Hence in this case, the calculations
    //     must be made for the MAUIToolkit.Core.Controls.Popup.CPopup.PopupOverlay instead of the
    //     PopupView. In order to check whether the calculations must be made either to
    //     the PopupView or Container, this case is checked.
    internal bool CanLayoutForGivenSizeAndPosition()
    {
        return true;
    }

    //
    // Summary:
    //     Sets PopupView Y point and PopupViewHeight when keyboard comes to the View.
    //
    // Parameters:
    //   keyboardTopPoint:
    //     After keyboard comes to the view keyboard top point.
    internal void PositionPoupViewBasedOnKeyboard(double keyboardTopPoint)
    {
        double num = 0.0;
        if (IsOpen && keyboardTopPoint <= popupYPosition + PopupViewHeight)
        {
            if (popupYPositionBeforeKeyboardInView == 0)
            {
                popupYPositionBeforeKeyboardInView = (int)popupYPosition;
            }

            int num2 = ((!(DeviceInfo.Platform == DevicePlatform.Android) || OverlayMode != PopupOverlayMode.Blur) ? PopupExtension.GetStatusBarHeight() : 0);
            num = ((!CanShowPopupInFullScreen) ? (keyboardTopPoint - PopupViewHeight - (double)num2 - (double)PopupExtension.GetActionBarHeight() - (double)PopupExtension.GetSafeAreaHeight("Top")) : (keyboardTopPoint - PopupViewHeight - (double)num2));
            AbortPopupViewAnimation();
            ResetAnimatedProperties();
            if (num >= 0.0)
            {
                LayoutPopup(popupXPosition, keyboardTopPoint - PopupViewHeight);
            }
            else
            {
                ShrinkPopupToAvailableSize(keyboardTopPoint);
            }
        }
    }

    //
    // Summary:
    //     Reset the PopupView Y point and PopupViewHeight when keyboard hides from the
    //     View.
    internal void UnshrinkPoupViewOnKeyboardCollapse()
    {
        if (popupViewHeightBeforeKeyboardInView != 0.0)
        {
            if (IsOpen)
            {
                CalculatePopupViewHeight();
            }

            PopupViewHeight = popupViewHeightBeforeKeyboardInView;
            PopupView.InvalidateForceLayout();
            popupViewHeightBeforeKeyboardInView = 0.0;
        }

        if (popupYPositionBeforeKeyboardInView != 0)
        {
            if (IsOpen)
            {
                LayoutPopup(popupXPosition, popupYPositionBeforeKeyboardInView);
            }

            popupYPositionBeforeKeyboardInView = 0;
        }

        PopupView.ApplyShadowAndCornerRadius();
    }

    //
    // Summary:
    //     Get template for the popup view content, header and footer.
    //
    // Parameters:
    //   template:
    //     template to select.
    //
    // Returns:
    //     Selected template.
    internal DataTemplate GetTemplate(DataTemplate template)
    {
        if (template is DataTemplateSelector dataTemplateSelector)
        {
            template = dataTemplateSelector.SelectTemplate(this, null);
        }

        return template;
    }

    //
    // Summary:
    //     Used to reset the popupview width and height.
    internal void ResetPopupWidthHeight()
    {
        if (AutoSizeMode != PopupAutoSizeMode.None || CanShowPopupInFullScreen || base.HeightRequest >= 0.0 || base.MinimumHeightRequest >= 0.0 || base.WidthRequest >= 0.0 || base.MinimumWidthRequest >= 0.0)
        {
            CalculatePopupViewWidth();
            CalculatePopupViewHeight();
        }

        if (relativeView == null && positionXPoint != -1.0 && positionYPoint != -1.0)
        {
            if (IsRTL)
            {
                positionXPoint = (int)ValidatePopupXPositionForRTL(showXPosition, PopupViewWidth, PopupExtension.GetScreenWidth());
            }
            else
            {
                positionXPoint = (int)ValidatePopupPosition(showXPosition, PopupViewWidth, PopupExtension.GetScreenWidth());
            }

            int num = ((!(DeviceInfo.Platform == DevicePlatform.Android) || OverlayMode != PopupOverlayMode.Blur) ? PopupExtension.GetStatusBarHeight() : 0);
            positionYPoint = (int)ValidatePopupPosition(showYPosition, PopupViewHeight, PopupExtension.GetScreenHeight() - num - PopupExtension.GetActionBarHeight()) + num + PopupExtension.GetActionBarHeight();
        }

        if (relativeView == null)
        {
            PositionPopupView();
        }
        else
        {
            PositionPopupRelativeToView(relativeView, relativePosition, absoluteXPoint, absoluteYPoint);
        }

        UpdatePopupStyles();
    }

    //
    // Summary:
    //     Validates the position of the popup view.
    //
    // Parameters:
    //   point:
    //     The point at which the layout is expected.
    //
    //   actualSize:
    //     The actual size of the view.
    //
    //   availableSize:
    //     The available size of the view.
    //
    // Returns:
    //     Returns the validated position of the popup view.
    internal double ValidatePopupPosition(double point, double actualSize, double availableSize)
    {
        return Math.Max((point + actualSize > availableSize) ? (availableSize - actualSize) : point, 0.0);
    }

    //
    // Summary:
    //     Need to handle the run time changes of System.ComponentModel.PropertyChangedEventArgs
    //     of MAUIToolkit.Core.Controls.Popup.CPopup.
    //
    // Parameters:
    //   propertyName:
    //     Represents the property changed event arguments.
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        switch (propertyName)
        {
            case "Parent":
                SetRTL();
                break;
            case "CanShowPopupInFullScreen":
                if (PopupView != null && PopupView.IsViewLoaded)
                {
                    PopupView.UpdateHeaderView();
                    if (IsOpen)
                    {
                        ResetPopupWidthHeight();
                    }
                }

                break;
            case "FlowDirection":
                SetRTL();
                PopupView.FlowDirection = (IsRTL ? FlowDirection.RightToLeft : base.FlowDirection);
                if (IsOpen)
                {
                    ResetPopupWidthHeight();
                }

                break;
            default:
                if (!(propertyName == "WidthRequest"))
                {
                    break;
                }

                goto case "HeightRequest";
            case "HeightRequest":
                if (PopupView != null && IsOpen)
                {
                    ResetPopupWidthHeight();
                    PopupView.InvalidateForceLayout();
                }

                break;
        }

        base.OnPropertyChanged(propertyName);
    }

    //
    // Summary:
    //     Occurs when the PopupView binding context is changed.
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (PopupView != null)
        {
            PopupView.BindingContext = base.BindingContext;
        }
    }

    //
    // Summary:
    //     Delegate for MAUIToolkit.Core.Controls.Popup.CPopup.StaysOpen bindable property changed.
    //
    //
    // Parameters:
    //   bindable:
    //     Instance of the CPopupLayout class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnStaysOpenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = bindable as CPopup;
    }

    //
    // Summary:
    //     Delegate for MAUIToolkit.Core.Controls.Popup.CPopup.IsOpen bindable property changed.
    //
    //
    // Parameters:
    //   bindable:
    //     Instance of the CPopupLayout class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnIsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is CPopup cPopup) || (cPopup.IsOpen && (cPopup.IsContainerAnimationInProgress || cPopup.IsPopupAnimationInProgress)))
        {
            return;
        }

        if (cPopup.IsOpen && cPopup.AutoCloseDuration > 0)
        {
            cPopup.AutoClosePopup();
        }
        else if (cPopup.IsOpen)
        {
            if (cPopup.RelativeView != null)
            {
                cPopup.ShowRelativeToView(cPopup.RelativeView, cPopup.RelativePosition, cPopup.AbsoluteX, cPopup.AbsoluteY);
            }
            else
            {
                cPopup.DisplayPopup();
            }
        }
        else
        {
            cPopup.DismissPopup();
        }
    }

    //
    // Summary:
    //     Delegate for MAUIToolkit.Core.Controls.Popup.CPopup.IsFullScreen bindable property changed.
    //
    //
    // Parameters:
    //   bindable:
    //     Instance of the MAUIToolkit.Core.Controls.Popup.CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnIsFullScreenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        cPopup.CanShowPopupInFullScreen = (bool)newValue;
    }

    //
    // Summary:
    //     Delegate for ClosePopupOnBackButtonPressed bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     Instance of the MAUIToolkit.Core.Controls.Popup.CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnClosePopupOnBackButtonPressedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = bindable as CPopup;
    }

    //
    // Summary:
    //     Delegate for AppearanceMode bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnAppearanceModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.PopupView != null && cPopup.PopupView.IsViewLoaded)
        {
            cPopup.PopupView?.FooterView.UpdateFooterChild();
            if (cPopup.IsOpen)
            {
                cPopup.PopupView?.FooterView.UpdateFooterAppearance();
            }
        }
    }

    //
    // Summary:
    //     Delegate for HeaderTemplate bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnHeaderTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.PopupView != null && cPopup.PopupView.IsViewLoaded)
        {
            cPopup.PopupView?.UpdateHeaderView();
        }
    }

    //
    // Summary:
    //     Delegate for ContentTemplate bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnContentTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.PopupView != null && cPopup.PopupView.IsViewLoaded)
        {
            cPopup.PopupView?.UpdateMessageView();
        }
    }

    //
    // Summary:
    //     Delegate for FooterTemplate bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnFooterTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.PopupView != null && cPopup.PopupView.IsViewLoaded)
        {
            cPopup.PopupView?.UpdateFooterView();
        }
    }

    //
    // Summary:
    //     Delegate for ShowHeader bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnShowHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null)
        {
            cPopup.CalculatePopupViewHeight();
            if (cPopup.IsOpen)
            {
                cPopup.ResetPopupWidthHeight();
            }

            cPopup.PopupView?.PopupMessageView?.UpdatePadding();
            cPopup.PopupView?.InvalidateForceLayout();
        }
    }

    //
    // Summary:
    //     Delegate for ShowFooter bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnShowFooterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null)
        {
            if (cPopup.ShowFooter)
            {
                cPopup.PopupViewHeight = 240.0;
            }
            else
            {
                cPopup.PopupViewHeight = 176.0;
            }

            cPopup.CalculatePopupViewHeight();
            if (cPopup.IsOpen)
            {
                cPopup.ResetPopupWidthHeight();
            }

            cPopup.PopupView?.PopupMessageView?.UpdatePadding();
            cPopup.PopupView?.InvalidateForceLayout();
        }
    }

    //
    // Summary:
    //     Delegate for HeaderTitle bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnHeaderTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && newValue != null && newValue != oldValue)
        {
            cPopup.PopupView?.SetHeaderTitleText(newValue.ToString());
        }
        else
        {
            cPopup?.PopupView?.SetHeaderTitleText(CPopupResources.GetLocalizedString("Title"));
        }
    }

    //
    // Summary:
    //     Delegate for AcceptButtonText bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnAcceptButtonTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && newValue != null && newValue != oldValue)
        {
            cPopup.PopupView?.SetAcceptButtonText(newValue.ToString());
        }
        else
        {
            cPopup?.PopupView?.SetAcceptButtonText(CPopupResources.GetLocalizedString("AcceptButtonText"));
        }
    }

    //
    // Summary:
    //     Delegate for DeclineButtonText bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnDeclineButtonTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && newValue != null && newValue != oldValue)
        {
            cPopup.PopupView?.SetDeclineButtonText(newValue.ToString());
        }
        else
        {
            cPopup?.PopupView?.SetDeclineButtonText(CPopupResources.GetLocalizedString("DeclineButtonText"));
        }
    }

    //
    // Summary:
    //     Delegate for PopupStyle bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the PopupView class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnPopupStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is CPopup cPopup))
        {
            return;
        }

        if (newValue != null)
        {
            cPopup.PopupStyle.SetThemeProperties(cPopup);
            if (DeviceInfo.Platform != DevicePlatform.WinUI && cPopup.IsOpen)
            {
                Page mainPage = PopupExtension.GetMainPage();
                Application application = IPlatformApplication.Current?.Application as Application;
                if ((mainPage != null && mainPage.Window != null && mainPage.Handler != null) || (application != null && application.MainPage is Shell && application.MainPage.Handler != null))
                {
                    cPopup.UpdatePopupStyles();
                }
            }
        }
        else
        {
            PopupStyle popupStyle = new PopupStyle();
            popupStyle.SetThemeProperties(cPopup);
            cPopup.PopupStyle = popupStyle;
        }
    }

    //
    // Summary:
    //     Delegate for PopupStyle default value creator.
    //
    // Parameters:
    //   bindable:
    //     Instance of the CPopup class.
    //
    // Returns:
    //     Returns the popup style instance.
    private static object? CreatePopupStyle(BindableObject bindable)
    {
        CPopup cPopup = bindable as CPopup;
        PopupStyle popupStyle = new PopupStyle();
        if (cPopup != null)
        {
            popupStyle.SetThemeProperties(cPopup);
        }

        return popupStyle;
    }

    //
    // Summary:
    //     Delegate for AnimationMode bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the PopupView class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnAnimationModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
    }

    //
    // Summary:
    //     Delegate for AnimationEasing bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the PopupView class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnAnimationEasingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
    }

    //
    // Summary:
    //     Delegate for ShowCloseButton bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnShowCloseButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.PopupView != null && cPopup.PopupView.IsViewLoaded)
        {
            cPopup.PopupView?.UpdateHeaderView();
        }
    }

    //
    // Summary:
    //     Delegate for AnimationDuration bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the PopupView class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnAnimationDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
    }

    //
    // Summary:
    //     Delegate for HeaderHeight bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnHeaderHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null)
        {
            cPopup.CalculatePopupViewHeight();
            cPopup.PopupView?.InvalidateForceLayout();
        }
    }

    //
    // Summary:
    //     Delegate for FooterHeight bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnFooterHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null)
        {
            cPopup.CalculatePopupViewHeight();
            cPopup.PopupView?.InvalidateForceLayout();
        }
    }

    //
    // Summary:
    //     Delegate for MAUIToolkit.Core.Controls.Popup.CPopup.AutoSizeMode bindable property changed.
    //
    //
    // Parameters:
    //   bindable:
    //     Instance of the CPopupLayout class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnAutoSizeModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.IsOpen)
        {
            cPopup.UpdatePopupView();
            cPopup.PopupView?.InvalidateForceLayout();
        }
    }

    //
    // Summary:
    //     Delegate for Message bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     instance of the PopupView class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && newValue != null && newValue != oldValue)
        {
            cPopup.PopupView?.SetMessageText(newValue.ToString());
        }
        else
        {
            cPopup?.PopupView?.SetMessageText(CPopupResources.GetLocalizedString("Message"));
        }
    }

    //
    // Summary:
    //     Delegate for StartX bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     Instance of the MAUIToolkit.Core.Controls.Popup.CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnStartXPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.IsOpen && newValue != null && newValue != oldValue)
        {
            cPopup.PositionPopupView();
        }
    }

    //
    // Summary:
    //     Delegate for StartY bindable property changed.
    //
    // Parameters:
    //   bindable:
    //     Instance of the MAUIToolkit.Core.Controls.Popup.CPopup class.
    //
    //   oldValue:
    //     Old value in the property.
    //
    //   newValue:
    //     New value obtained in the property.
    private static void OnStartYPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CPopup cPopup = (CPopup)bindable;
        if (cPopup != null && cPopup.IsOpen && newValue != null && newValue != oldValue)
        {
            cPopup.PositionPopupView();
        }
    }

    //
    // Summary:
    //     Initializes the properties for MAUIToolkit.Core.Controls.Popup.CPopup.
    private void Initialize()
    {
        LocalizationResourceAccessor.InitializeDefaultResource("MAUIToolkit.Core.Controls.Popup.Resources.CPopup");
        PopupViewWidth = 313.0;
        if (ShowFooter)
        {
            PopupViewHeight = 240.0;
        }
        else
        {
            PopupViewHeight = 176.0;
        }

        positionXPoint = -1.0;
        positionYPoint = -1.0;
        showXPosition = -1.0;
        showYPosition = -1.0;
        PopupView = new PopupView(this);
        PopupOverlay = new Internals.Platforms.WindowOverlay();
        PopupOverlayContainer = new PopupOverlayContainer(this);
        PopupOverlay.SetWindowOverlayContainer(PopupOverlayContainer);
        SetPopupPositionBasedOnKeyboard();
        if (HeaderTitle != "Title")
        {
            PopupView.SetHeaderTitleText(HeaderTitle);
        }

        if (Message != "Popup Message")
        {
            PopupView.SetMessageText(Message);
        }

        if (AcceptButtonText != "ACCEPT")
        {
            PopupView.SetAcceptButtonText(AcceptButtonText);
        }

        if (DeclineButtonText != "DECLINE")
        {
            PopupView.SetDeclineButtonText(DeclineButtonText);
        }
    }

    //
    // Summary:
    //     Gets or sets the time delay in milliseconds for automatically closing the Popup.
    //     The Default value is 0.
    private async void AutoClosePopup()
    {
        if (RelativeView != null)
        {
            ShowRelativeToView(RelativeView, RelativePosition, AbsoluteX, AbsoluteY);
        }
        else
        {
            DisplayPopup();
        }

        await Task.Delay(AutoCloseDuration);
        IsOpen = false;
    }

    //
    // Summary:
    //     Used to set MAUIToolkit.Core.Controls.Popup.CPopup.IsOpen value.
    //
    // Parameters:
    //   open:
    //     The value to be set to the IsOpen property.
    private void OpenOrClosePopup(bool open)
    {
        IsOpen = open;
    }

    //
    // Summary:
    //     Used to trigger closed or opened event.
    private void RaisePopupEvent()
    {
        if (!IsOpen)
        {
            RaisePopupClosedEvent();
        }
        else
        {
            RaisePopupOpenedEvent();
        }
    }

    //
    // Summary:
    //     Displays the MAUIToolkit.Core.Controls.Popup.CPopup.PopupView in the view.
    private void DisplayPopup()
    {
        if (PopupView == null || relativeView != null)
        {
            return;
        }

        SetSemanticDescription();
        Page mainPage = PopupExtension.GetMainPage();
        Application application = IPlatformApplication.Current?.Application as Application;
        if ((mainPage != null && mainPage.Window != null && mainPage.Handler != null) || (application != null && application.MainPage is Shell shell && shell.IsLoaded && application.MainPage.Handler != null))
        {
            SetParent();
            if (PopupView.HeaderView != null)
            {
                PopupView.HeaderView.UpdateHeaderCloseButton();
            }

            CalculatePopupViewWidth();
            CalculatePopupViewHeight();
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                ApplyOverlayBackground();
            }

            PositionPopupView();
            if (DeviceInfo.Platform != DevicePlatform.Android)
            {
                ApplyOverlayBackground();
            }

            UpdatePopupView();
            if (!RaisePopupOpeningEvent())
            {
                ApplyContainerAnimation();
                ApplyPopupAnimation();
                WireEvents();
                WirePlatformSpecificEvents();
            }
            else
            {
                OpenOrClosePopup(open: false);
                RemovePopupViewAndResetValues();
            }
        }
        else
        {
            WireEvents();
        }
    }

    //
    // Summary:
    //     Used to update the PopupView while set the AutoSizeMode.
    private void UpdatePopupView()
    {
        if (AutoSizeMode != PopupAutoSizeMode.None && !CanShowPopupInFullScreen)
        {
            ResetPopupWidthHeight();
        }
        else
        {
            UpdatePopupStyles();
        }
    }

    //
    // Summary:
    //     Used to update the PopupStyles.
    private void UpdatePopupStyles()
    {
        PopupView.PopupMessageView.UpdateMessageViewStyle();
        PopupView?.HeaderView?.UpdateHeaderAppearance();
        PopupView?.FooterView?.UpdateFooterAppearance();
        PopupView?.ApplyShadowAndCornerRadius();
        ApplyOverlayBackground();
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            PopupView.Background = PopupStyle.GetPopupBackground();
        }
    }

    //
    // Summary:
    //     Used to update background for popup overlay.
    private void ApplyOverlayBackground()
    {
        if (PopupOverlayContainer == null)
        {
            return;
        }

        if (ShowOverlayAlways)
        {
            if (OverlayMode == PopupOverlayMode.Transparent)
            {
                PopupOverlayContainer.ApplyBackgroundColor(PopupStyle.GetOverlayColor());
            }
            else if (OverlayMode == PopupOverlayMode.Blur)
            {
                PopupExtension.Blur(PopupOverlayContainer, this, IsOpen);
            }
        }
        else
        {
            PopupOverlayContainer.ApplyBackgroundColor(Colors.Transparent);
        }

        PopupOverlayContainer.IsVisible = true;
    }

    //
    // Summary:
    //     Used this method to set the PopupView width and Height values to before applying
    //     padding Width and Height While screen orientation changes.
    private void ReAssignPopupViewWidthAndHeight()
    {
        if (popupViewWidthBeforePadding != PopupViewWidth)
        {
            PopupViewWidth = popupViewWidthBeforePadding;
        }

        if (popupViewHeightBeforePadding != PopupViewHeight)
        {
            PopupViewHeight = popupViewHeightBeforePadding;
        }
    }

    //
    // Summary:
    //     Used to apply padding for Popup.
    //
    // Parameters:
    //   x:
    //     X position value.
    //
    //   y:
    //     Y position value.
    private void ApplyPadding(ref double x, ref double y)
    {
        if (popupViewWidthBeforePadding == 0.0 || popupViewWidthBeforePadding != PopupViewWidth)
        {
            popupViewWidthBeforePadding = PopupViewWidth;
        }

        if (popupViewHeightBeforePadding == 0.0 || popupViewHeightBeforePadding != PopupViewHeight)
        {
            popupViewHeightBeforePadding = PopupViewHeight;
        }

        int num = ((!(DeviceInfo.Platform == DevicePlatform.Android) || OverlayMode != PopupOverlayMode.Blur) ? PopupExtension.GetStatusBarHeight() : 0);
        int screenWidth = PopupExtension.GetScreenWidth();
        int screenHeight = PopupExtension.GetScreenHeight();
        int actionBarHeight = PopupExtension.GetActionBarHeight();
        if (x <= (double)(minimalPadding + num))
        {
            x += base.Padding.Left;
        }

        if (y <= (double)(minimalPadding + PopupExtension.GetSafeAreaHeight("Top") + actionBarHeight + num))
        {
            y += base.Padding.Top;
        }

        if (screenWidth > 0 && x + PopupViewWidth >= (double)(screenWidth - minimalPadding))
        {
            PopupViewWidth = (double)screenWidth - x - base.Padding.Right;
        }

        if (screenHeight > 0 && y + PopupViewHeight >= (double)(screenHeight - minimalPadding))
        {
            PopupViewHeight = (double)screenHeight - y - base.Padding.Bottom;
        }

        AppliedBodyHeight = Math.Max(0.0, PopupViewHeight - (AppliedHeaderHeight + AppliedFooterHeight + (double)PopupStyle.StrokeThickness));
    }

    //
    // Summary:
    //     X and Y position of the PopupView is set here.
    private void PositionPopupView()
    {
        if (positionXPoint != -1.0 && positionYPoint != -1.0)
        {
            ApplyPadding(ref positionXPoint, ref positionYPoint);
            LayoutPopup(positionXPoint, positionYPoint);
            return;
        }

        int screenWidth = PopupExtension.GetScreenWidth();
        int screenHeight = PopupExtension.GetScreenHeight();
        int safeAreaHeight = PopupExtension.GetSafeAreaHeight("Left");
        int safeAreaHeight2 = PopupExtension.GetSafeAreaHeight("Right");
        int safeAreaHeight3 = PopupExtension.GetSafeAreaHeight("Top");
        int safeAreaHeight4 = PopupExtension.GetSafeAreaHeight("Bottom");
        int num = ((!(DeviceInfo.Platform == DevicePlatform.Android) || OverlayMode != PopupOverlayMode.Blur) ? PopupExtension.GetStatusBarHeight() : 0);
        int actionBarHeight = PopupExtension.GetActionBarHeight();
        double num2 = positionXPoint;
        double num3 = positionYPoint;
        num2 = ((StartX == -1) ? ((!(PopupViewWidth >= (double)(screenWidth - (safeAreaHeight + safeAreaHeight2)))) ? (((double)screenWidth - PopupViewWidth) / 2.0) : ((double)safeAreaHeight)) : ((!IsRTL) ? ValidatePopupPosition(StartX, PopupViewWidth, screenWidth) : ValidatePopupXPositionForRTL(StartX, PopupViewWidth, screenWidth)));
        num3 = ((StartY != -1) ? (ValidatePopupPosition(StartY, PopupViewHeight, screenHeight - num) + (double)num) : (CanShowPopupInFullScreen ? ((double)num) : ((PopupViewHeight >= (double)(screenHeight - (safeAreaHeight3 + safeAreaHeight4 + num + actionBarHeight))) ? ((double)(safeAreaHeight3 + num + actionBarHeight)) : ((!(PopupViewHeight >= (double)(screenHeight - (safeAreaHeight3 + safeAreaHeight4)))) ? (((double)screenHeight - PopupViewHeight) / 2.0) : ((double)safeAreaHeight3)))));
        ApplyPadding(ref num2, ref num3);
        LayoutPopup(num2, num3);
    }

    //
    // Summary:
    //     This method adds or updates the PopupView at the specified X and Y coordinates.
    //
    //
    // Parameters:
    //   x:
    //     The X-coordinate for positioning the popup.
    //
    //   y:
    //     The Y-coordinate for positioning the popup.
    private void LayoutPopup(double x, double y)
    {
        popupXPosition = x;
        popupYPosition = y;
        PopupOverlay?.AddOrUpdate(PopupView, x, y);
    }

    //
    // Summary:
    //     X and Y position of the PopupView is set here based on RelativePosition.
    //
    // Parameters:
    //   relativeView:
    //     The view relative to which popup should be layout.
    //
    //   position:
    //     The relative position from the view.
    //
    //   absoluteX:
    //     Absolute X Point where the popup should be positioned from the relative view.
    //
    //
    //   absoluteY:
    //     Absolute Y-Point where the popup should be positioned from the relative view.
    private void PositionPopupRelativeToView(Microsoft.Maui.Controls.View relativeView, PopupRelativePosition position, double absoluteX, double absoluteY)
    {
        PopupExtension.CalculateRelativePoint(PopupView, relativeView, position, absoluteX, absoluteY, ref positionXPoint, ref positionYPoint);
        if (CanShowPopupInFullScreen)
        {
            positionYPoint = ((!(DeviceInfo.Platform == DevicePlatform.Android) || OverlayMode != PopupOverlayMode.Blur) ? PopupExtension.GetStatusBarHeight() : 0);
        }

        ApplyPadding(ref positionXPoint, ref positionYPoint);
        LayoutPopup(positionXPoint, positionYPoint);
    }

    //
    // Summary:
    //     Width of the PopupView is set here.
    private void CalculatePopupViewWidth()
    {
        if (ContentTemplate == null)
        {
            goto IL_00b7;
        }

        if (!(base.WidthRequest > 0.0) && !(base.MinimumWidthRequest > 0.0))
        {
            PopupView? popupView = PopupView;
            if (popupView == null || !(popupView.PopupMessageView?.WidthRequest > 0.0))
            {
                goto IL_00b7;
            }
        }

        PopupViewWidth = Math.Max(GetFinalWidth(PopupView.PopupMessageView.Content), Math.Max(base.WidthRequest, base.MinimumWidthRequest));
        goto IL_014f;
    IL_014f:
        PopupViewWidth = Math.Min(PopupViewWidth, PopupExtension.GetScreenWidth() - (PopupExtension.GetSafeAreaHeight("Left") + PopupExtension.GetSafeAreaHeight("Right")));
        return;
    IL_00b7:
        if (base.WidthRequest >= 0.0 || base.MinimumWidthRequest >= 0.0)
        {
            PopupViewWidth = Math.Max(base.WidthRequest, base.MinimumWidthRequest);
        }
        else if (CanShowPopupInFullScreen)
        {
            PopupViewWidth = PopupExtension.GetScreenWidth();
        }
        else
        {
            if ((AutoSizeMode != PopupAutoSizeMode.Both && AutoSizeMode != 0) || ContentTemplate == null)
            {
                return;
            }

            CalculateAutoWidth();
        }

        goto IL_014f;
    }

    private int GetFinalWidth(Microsoft.Maui.Controls.View template)
    {
        if (template != null)
        {
            if (template.MinimumWidthRequest >= 0.0 && template.WidthRequest >= 0.0)
            {
                return (int)Math.Max(template.MinimumWidthRequest, template.WidthRequest);
            }

            if (template.WidthRequest >= 0.0)
            {
                return (int)template.WidthRequest;
            }

            if (template.MinimumWidthRequest >= 0.0)
            {
                return (int)template.MinimumWidthRequest;
            }

            return -1;
        }

        return -1;
    }

    private double GetFinalHeight(Microsoft.Maui.Controls.View template)
    {
        if (template != null)
        {
            if (template.MinimumHeightRequest >= 0.0 && template.HeightRequest >= 0.0)
            {
                return (int)Math.Max(template.MinimumHeightRequest, template.HeightRequest);
            }

            if (template.HeightRequest >= 0.0)
            {
                return (int)template.HeightRequest;
            }

            if (template.MinimumHeightRequest >= 0.0)
            {
                return (int)template.MinimumHeightRequest;
            }

            return -1.0;
        }

        return -1.0;
    }

    //
    // Summary:
    //     Height of the PopupView is set here.
    private void CalculatePopupViewHeight()
    {
        int screenHeight = PopupExtension.GetScreenHeight();
        int statusBarHeight = PopupExtension.GetStatusBarHeight();
        int actionBarHeight = PopupExtension.GetActionBarHeight();
        int safeAreaHeight = PopupExtension.GetSafeAreaHeight("Top");
        int safeAreaHeight2 = PopupExtension.GetSafeAreaHeight("Bottom");
        double strokeThickness = PopupStyle.GetStrokeThickness();
        ResetHeaderFooterHeight();
        if (ContentTemplate != null)
        {
            if (!(base.HeightRequest > 0.0) && !(base.MinimumHeightRequest > 0.0))
            {
                PopupView? popupView = PopupView;
                if (popupView == null || !(popupView.PopupMessageView?.HeightRequest > 0.0))
                {
                    goto IL_016f;
                }
            }

            PopupViewHeight = ((Math.Max(GetFinalHeight(PopupView.PopupMessageView.Content), Math.Max(base.HeightRequest, base.MinimumHeightRequest)) <= (double)(screenHeight - (safeAreaHeight + safeAreaHeight2 + statusBarHeight + actionBarHeight))) ? Math.Max(GetFinalHeight(PopupView?.PopupMessageView), Math.Max(base.HeightRequest, base.MinimumHeightRequest)) : ((double)(screenHeight - (safeAreaHeight + safeAreaHeight2 + statusBarHeight + actionBarHeight))));
            AppliedBodyHeight = Math.Max(0.0, PopupViewHeight - (AppliedHeaderHeight + AppliedFooterHeight + strokeThickness));
            return;
        }

        goto IL_016f;
    IL_016f:
        if (base.HeightRequest >= 0.0 || base.MinimumHeightRequest >= 0.0)
        {
            PopupViewHeight = Math.Min(screenHeight - (safeAreaHeight + safeAreaHeight2 + statusBarHeight + actionBarHeight), Math.Max(base.HeightRequest, base.MinimumHeightRequest));
            AppliedBodyHeight = Math.Max(0.0, PopupViewHeight - (AppliedHeaderHeight + AppliedFooterHeight + strokeThickness));
            return;
        }

        if (CanShowPopupInFullScreen)
        {
            PopupViewHeight = screenHeight - (safeAreaHeight2 + statusBarHeight);
            AppliedBodyHeight = PopupViewHeight - (AppliedHeaderHeight + AppliedFooterHeight + strokeThickness);
            return;
        }

        if (ContentTemplate != null && (AutoSizeMode == PopupAutoSizeMode.Both || AutoSizeMode == PopupAutoSizeMode.Height))
        {
            CalculateAutoHeight();
            if (AppliedHeaderHeight + AppliedBodyHeight + AppliedFooterHeight >= (double)(screenHeight - (safeAreaHeight + safeAreaHeight2 + statusBarHeight + actionBarHeight)))
            {
                AppliedBodyHeight = (int)((double)screenHeight - (AppliedHeaderHeight + AppliedFooterHeight + (double)safeAreaHeight + (double)safeAreaHeight2 + (double)statusBarHeight + (double)actionBarHeight));
            }

            PopupViewHeight = AppliedHeaderHeight + AppliedBodyHeight + AppliedFooterHeight;
        }

        if (!ShowHeader && !ShowFooter)
        {
            AppliedHeaderHeight = 0.0;
            AppliedFooterHeight = 0.0;
        }

        AppliedBodyHeight = Math.Max(0.0, PopupViewHeight - (AppliedHeaderHeight + AppliedFooterHeight + strokeThickness));
    }

    //
    // Summary:
    //     Calculate auto width based on content template content measured width.
    private void CalculateAutoWidth()
    {
        if (ContentTemplate != null)
        {
            Microsoft.Maui.Controls.View content = PopupView.PopupMessageView.Content;
            if (content != null)
            {
                PopupViewWidth = (int)CalculateSizeBasedOnAutoSizeMode().Width;
            }
        }
    }

    //
    // Summary:
    //     Calculate measured size for content template content. if auto size mode is width
    //     need to calculate based on ScreenWidtg,if auto size mode is height need to calculate
    //     based on ScreenHeight.
    private void CalculateAutoHeight()
    {
        if (ContentTemplate != null)
        {
            Microsoft.Maui.Controls.View content = PopupView.PopupMessageView.Content;
            if (content != null)
            {
                AppliedBodyHeight = (int)CalculateSizeBasedOnAutoSizeMode().Height;
            }
        }
    }

    //
    // Summary:
    //     Calculates and return content templtate content measured size using positive
    //     inifinity as width and Scren height as height if AutoSizeMode is Width. retrun
    //     measured size using positive inifinity as height and screnn width as width if
    //     AutoSizeMode is Height. For Both consider Scren Width and Scren Height.
    //
    // Returns:
    //     Return measured size.
    private Size CalculateSizeBasedOnAutoSizeMode()
    {
        if (ContentTemplate != null)
        {
            Microsoft.Maui.Controls.View content = PopupView.PopupMessageView.Content;
            if (content != null)
            {
                if (AutoSizeMode == PopupAutoSizeMode.Width)
                {
                    return ((IView)PopupView.PopupMessageView).Measure(double.PositiveInfinity, PopupViewHeight);
                }

                if (AutoSizeMode == PopupAutoSizeMode.Height)
                {
                    return ((IView)PopupView.PopupMessageView).Measure(PopupViewWidth, double.PositiveInfinity);
                }

                if (AutoSizeMode == PopupAutoSizeMode.Both)
                {
                    return ((IView)PopupView.PopupMessageView).Measure((double)PopupExtension.GetScreenWidth() - base.Padding.Left - base.Padding.Right, (double)PopupExtension.GetScreenHeight() - base.Padding.Top - base.Padding.Bottom);
                }
            }
        }

        return new Size(0.0, 0.0);
    }

    private void RemovePopupViewAndResetValues()
    {
        if (PopupView != null && PopupOverlay != null)
        {
            if (PopupOverlayContainer != null)
            {
                PopupOverlayContainer.IsVisible = false;
            }

            PopupOverlay?.Remove(PopupView);
            PopupOverlay?.RemoveFromWindow();
        }

        positionXPoint = -1.0;
        positionYPoint = -1.0;
        popupXPosition = -1.0;
        popupYPosition = -1.0;
        PopupViewWidth = 313.0;
        if (ShowFooter)
        {
            PopupViewHeight = 240.0;
        }
        else
        {
            PopupViewHeight = 176.0;
        }

        relativeView = null;
    }

    //
    // Summary:
    //     Calculate the height of the header and footer.
    private void ResetHeaderFooterHeight()
    {
        if (ShowHeader)
        {
            AppliedHeaderHeight = HeaderHeight;
        }
        else
        {
            AppliedHeaderHeight = 0.0;
        }

        if (ShowFooter)
        {
            AppliedFooterHeight = FooterHeight;
        }
        else
        {
            AppliedFooterHeight = 0.0;
        }
    }

    //
    // Summary:
    //     Used to adjust the PopupView height when PopupView height is exceeding the available
    //     size after keyboard comes to the view.
    //
    // Parameters:
    //   keyboardTopPoint:
    //     After keyboard comes to the view keyboard top point.
    private void ShrinkPopupToAvailableSize(double keyboardTopPoint)
    {
        if (popupViewHeightBeforeKeyboardInView == 0.0)
        {
            popupViewHeightBeforeKeyboardInView = PopupViewHeight;
        }

        int num = ((!(DeviceInfo.Platform == DevicePlatform.Android) || OverlayMode != PopupOverlayMode.Blur) ? PopupExtension.GetStatusBarHeight() : 0);
        if (CanShowPopupInFullScreen)
        {
            LayoutPopup(popupXPosition, num);
        }
        else
        {
            LayoutPopup(popupXPosition, num + PopupExtension.GetSafeAreaHeight("Top") + PopupExtension.GetActionBarHeight());
        }

        ResetHeaderFooterHeight();
        AdjustBodyHeightForAutoSizing(keyboardTopPoint);
        PopupView.ApplyShadowAndCornerRadius();
        PopupView.InvalidateForceLayout();
    }

    //
    // Summary:
    //     Adjust PopupView Y point and PopupViewHeight when keyboard comes to the View.
    private void SetPopupPositionBasedOnKeyboard()
    {
        PopupPositionBasedOnKeyboard();
    }

    //
    // Summary:
    //     Adjust PopupView body height for AutoSizing.
    //
    // Parameters:
    //   keyboardTopPoint:
    //     Keyboard's top point in the screen.
    private void AdjustBodyHeightForAutoSizing(double keyboardTopPoint)
    {
        double num = keyboardTopPoint - (double)PopupExtension.GetStatusBarHeight();
        if (!CanShowPopupInFullScreen)
        {
            num -= (double)(PopupExtension.GetActionBarHeight() + PopupExtension.GetSafeAreaHeight("Top"));
        }

        if (AppliedBodyHeight + AppliedHeaderHeight + AppliedFooterHeight >= num)
        {
            AppliedBodyHeight = num - (AppliedHeaderHeight + AppliedFooterHeight);
        }

        PopupViewHeight = AppliedHeaderHeight + AppliedBodyHeight + AppliedFooterHeight;
    }

    //
    // Summary:
    //     Sets the flow direction value to the popup view when it is applied to its parent.
    private void SetRTL()
    {
        if ((((IVisualElementController)this).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
        {
            IsRTL = true;
        }
        else
        {
            IsRTL = false;
        }
    }

    //
    // Summary:
    //     Used to wire common required events.
    private void WireEvents()
    {
        Page mainPage = PopupExtension.GetMainPage();
        Application application = IPlatformApplication.Current?.Application as Application;
        if (mainPage != null && mainPage.Handler == null && application != null && application.MainPage != null)
        {
            if (application.MainPage is Shell shell && shell != null)
            {
                if (shell.IsLoaded && application.MainPage.Handler != null)
                {
                    return;
                }

                if (!shell.IsLoaded)
                {
                    shell.Loaded -= ShellPage_Loaded;
                    shell.Loaded += ShellPage_Loaded;
                }
            }

            if ((DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst) && application != null && application.MainPage != null && application.MainPage.Navigation.ModalStack.LastOrDefault() != null)
            {
                mainPage.LayoutChanged -= OnPageLayoutChanged;
                mainPage.LayoutChanged += OnPageLayoutChanged;
            }
            else
            {
                mainPage.Loaded -= OnMainPageLoaded;
                mainPage.Loaded += OnMainPageLoaded;
            }
        }
        else if (mainPage != null && mainPage.Window == null && application != null)
        {
            application.PropertyChanged += OnCurrentPropertyChanged;
        }
        else if (application != null && application.MainPage != null)
        {
            application.MainPage.PropertyChanged += OnAppMainPagePropertyChanged;
            application.ModalPushed += OnAppCurrentModalPushed;
            application.ModalPopped += OnAppCurrentModalPopped;
        }
        else if (application == null && Application.Current != null)
        {
            Application.Current.PropertyChanged += OnCurrentPropertyChanged;
        }
    }

    //
    // Summary:
    //     Used to unwire the invoked events.
    private void UnWireEvents()
    {
        if (IPlatformApplication.Current?.Application is Application application && application.MainPage != null)
        {
            application.MainPage.Loaded -= OnMainPageLoaded;
            application.MainPage.PropertyChanged -= OnAppMainPagePropertyChanged;
            application.PropertyChanged -= OnCurrentPropertyChanged;
            application.ModalPushed -= OnAppCurrentModalPushed;
            application.ModalPopped -= OnAppCurrentModalPopped;
        }

        if (Application.Current != null && Application.Current.MainPage != null)
        {
            Application.Current.PropertyChanged -= OnCurrentPropertyChanged;
            Application.Current.MainPage.Loaded -= OnMainPageLoaded;
        }
    }

    //
    // Summary:
    //     Raise when the current page layout has changed.
    //
    // Parameters:
    //   sender:
    //     Represents current Page.
    //
    //   e:
    //     Corresponding propertychanged event args.
    private void OnPageLayoutChanged(object? sender, EventArgs e)
    {
        if (showXPosition != -1.0 && showYPosition != -1.0)
        {
            Show(showXPosition, showYPosition);
        }
        else
        {
            DisplayPopup();
        }

        (sender as Page).LayoutChanged -= OnPageLayoutChanged;
    }

    //
    // Summary:
    //     Raises on Application.Current property changes.
    //
    // Parameters:
    //   sender:
    //     Instance of Application.Current.
    //
    //   e:
    //     Corresponding propertychanged event args.
    private void OnCurrentPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Application application = IPlatformApplication.Current?.Application as Application;
        if (application != null && application.MainPage != null)
        {
            application.MainPage.Loaded += OnMainPageLoaded;
        }
        else if (application == null && Application.Current != null && Application.Current.MainPage != null)
        {
            Page mainPage = Application.Current.MainPage;
            mainPage.Loaded += OnMainPageLoaded;
        }
    }

    //
    // Summary:
    //     Raises on when MainPage is Loaded.
    //
    // Parameters:
    //   sender:
    //     Instance of Application.Cuurrent.MainPage.
    //
    //   e:
    //     Corresponding propertychanged event args.
    private void OnMainPageLoaded(object? sender, EventArgs e)
    {
        if (showXPosition != -1.0 && showYPosition != -1.0)
        {
            Show(showXPosition, showYPosition);
        }
        else
        {
            DisplayPopup();
        }
    }

    //
    // Summary:
    //     Raises on when ShellPage is Loaded.
    //
    // Parameters:
    //   sender:
    //     Instance of shell page.
    //
    //   e:
    //     Corresponding property changed event args.
    private void ShellPage_Loaded(object? sender, EventArgs e)
    {
        Page page = (sender as Shell)?.CurrentPage;
        if (page != null)
        {
            page.Loaded -= OnMainPageLoaded;
            page.Loaded += OnMainPageLoaded;
        }
    }

    //
    // Summary:
    //     Raises on Application.Current.MainPage property changes.
    //
    // Parameters:
    //   sender:
    //     Instance of Application.Current.MainPage.
    //
    //   e:
    //     Corresponding propertychanged event args.
    private void OnAppMainPagePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (IsOpen && (e.PropertyName == "Parent" || (sender != null && (((sender is NavigationPage || sender is TabbedPage) && e.PropertyName == "CurrentPage") || (sender is FlyoutPage && e.PropertyName == "Detail") || (sender is Shell && e.PropertyName == "CurrentState")))) && !CheckForParentPage(sender))
        {
            IsOpen = false;
            AbortPopupViewAnimation();
        }
    }

    //
    // Summary:
    //     Raised when a page has been popped modally.
    //
    // Parameters:
    //   sender:
    //     Current page of Application.
    //
    //   e:
    //     Event arguments for Microsoft.Maui.Controls.ModalPoppedEventArgs.
    private void OnAppCurrentModalPopped(object? sender, ModalPoppedEventArgs e)
    {
        IsOpen = false;
        AbortPopupViewAnimation();
    }

    //
    // Summary:
    //     Raised when a page has been pushed modally.
    //
    // Parameters:
    //   sender:
    //     Current page of Application.
    //
    //   e:
    //     Event arguments for Microsoft.Maui.Controls.ModalPushedEventArgs.
    private void OnAppCurrentModalPushed(object? sender, ModalPushedEventArgs e)
    {
        if (IsOpen && !CheckForParentPage(e.Modal))
        {
            IsOpen = false;
            AbortPopupViewAnimation();
        }
    }

    //
    // Summary:
    //     Gets the parent page of the view.
    //
    // Parameters:
    //   view:
    //     The view whose parent page has to be obtained.
    //
    // Returns:
    //     The parent page of the view.
    private Page? GetViewParentPage(Microsoft.Maui.Controls.View view)
    {
        if (!(view.Parent is Microsoft.Maui.Controls.View) && view.Parent is Page page && page != PopupExtension.GetMainPage())
        {
            view.Parent = PopupExtension.GetMainPage();
        }

        Microsoft.Maui.Controls.View view2 = view.Parent as Microsoft.Maui.Controls.View;
        if (view2 == null)
        {
            return view.Parent as Page;
        }

        while (view2 != null && view2.Parent != null && !(view2.Parent is Page))
        {
            view2 = view2.Parent as Microsoft.Maui.Controls.View;
        }

        return view2.Parent as Page;
    }

    //
    // Summary:
    //     Check whether popup parent page and pushed page is same or not.
    //
    // Parameters:
    //   sender:
    //     Instance of Application.Current.MainPage.
    //
    // Returns:
    //     Returns true when popup parent page and pushed page is same.
    private bool CheckForParentPage(object? sender)
    {
        bool result = false;
        Page viewParentPage = GetViewParentPage(this);
        if (viewParentPage != null)
        {
            result = ((sender is NavigationPage) ? (viewParentPage == (sender as NavigationPage)?.CurrentPage) : ((sender is TabbedPage) ? (viewParentPage == (sender as TabbedPage)?.CurrentPage) : ((sender is FlyoutPage) ? (viewParentPage == (sender as FlyoutPage)?.Detail) : ((!(sender is Shell)) ? (viewParentPage == sender as ContentPage) : (viewParentPage == (sender as Shell)?.CurrentPage)))));
        }

        return result;
    }

    //
    // Summary:
    //     Validates the position of the popup view.
    //
    // Parameters:
    //   point:
    //     The point at which the layout is expected.
    //
    //   actualSize:
    //     The actual size of the view.
    //
    //   availableSize:
    //     The available size of the view.
    //
    // Returns:
    //     Returns the validated position of the popup view.
    private double ValidatePopupXPositionForRTL(double point, double actualSize, double availableSize)
    {
        return Math.Max(availableSize - (double)PopupExtension.GetSafeAreaHeight("Right") - Math.Max(point, 0.0) - actualSize, 0.0);
    }

    //
    // Summary:
    //     Set parent page for sfpopup.
    private void SetParent()
    {
        Page mainPage = PopupExtension.GetMainPage();
        if (base.Parent == null && mainPage != null)
        {
            base.Parent = mainPage;
        }

        if ((PopupOverlayContainer.Parent == null || PopupView.Parent == null) && mainPage != null)
        {
            PopupOverlayContainer.Parent = mainPage;
            PopupView.Parent = PopupOverlayContainer;
        }
    }

    //
    // Summary:
    //     Applies animation easing effect to PopupView.
    //
    // Returns:
    //     returns easing effect of view.
    private Easing GetAnimationEasing()
    {
        return AnimationEasing switch
        {
            PopupAnimationEasing.SinIn => Easing.SinIn,
            PopupAnimationEasing.SinInOut => Easing.SinInOut,
            PopupAnimationEasing.SinOut => Easing.SinOut,
            _ => Easing.Linear,
        };
    }

    //
    // Summary:
    //     Applies animation to the MAUIToolkit.Core.Controls.Popup.CPopupOverlayContainer.
    private void ApplyContainerAnimation()
    {
        if (!IsContainerAnimationInProgress && PopupOverlayContainer != null && AnimationMode != PopupAnimationMode.None)
        {
            double num = 0.0;
            double num2 = 1.0;
            uint rate = 16u;
            Easing animationEasing = GetAnimationEasing();
            SetAnimationProgress(PopupOverlayContainer, animationInProgress: true);
            Animation animation = new Animation(delegate (double v)
            {
                PopupOverlayContainer.Opacity = v;
            }, IsOpen ? num : num2, IsOpen ? num2 : num);
            animation.Commit(PopupOverlayContainer, "ContainerFadeAnimation", rate, (uint)AnimationDuration, animationEasing, delegate
            {
                SetAnimationProgress(PopupOverlayContainer, animationInProgress: false);
            });
        }
    }

    //
    // Summary:
    //     Applies animation to the popup view.
    private void ApplyPopupAnimation()
    {
        if (IsPopupAnimationInProgress || PopupView == null)
        {
            return;
        }

        if ((DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst) && AnimationMode != PopupAnimationMode.None)
        {
            PopupView popupView = PopupView;
            ((IView)popupView).Frame = new Microsoft.Maui.Graphics.Rect(popupXPosition, popupYPosition, PopupViewHeight, PopupViewWidth);
        }

        double num = 0.0;
        double num2 = 1.0;
        float num3 = 0.75f;
        float num4 = 1f;
        switch (AnimationMode)
        {
            case PopupAnimationMode.Fade:
                FadeAnimate(PopupView, IsOpen ? num : num2, IsOpen ? num2 : num);
                break;
            case PopupAnimationMode.Zoom:
                ZoomAnimate(PopupView, IsOpen ? num3 : num4, IsOpen ? num4 : num3);
                break;
            case PopupAnimationMode.SlideOnLeft:
                {
                    double num5;
                    double num6;
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        num5 = 0.0 - PopupViewWidth;
                        num6 = popupXPosition;
                    }
                    else
                    {
                        num5 = 0.0 - (PopupViewWidth + popupXPosition);
                        if (IsRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
                        {
                            num5 = 0.0 - num5;
                        }

                        num6 = 0.0;
                    }

                    SlideAnimate(PopupView, IsOpen ? num5 : num6, IsOpen ? num6 : num5);
                    break;
                }
            case PopupAnimationMode.SlideOnRight:
                {
                    double num5;
                    double num6;
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        num5 = PopupExtension.GetScreenWidth();
                        num6 = popupXPosition;
                    }
                    else
                    {
                        num5 = (double)PopupExtension.GetScreenWidth() - popupXPosition;
                        if (IsRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
                        {
                            num5 = 0.0 - num5;
                        }

                        num6 = 0.0;
                    }

                    SlideAnimate(PopupView, IsOpen ? num5 : num6, IsOpen ? num6 : num5);
                    break;
                }
            case PopupAnimationMode.SlideOnTop:
                {
                    double num5;
                    double num6;
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        num5 = 0.0 - PopupViewHeight;
                        num6 = popupYPosition;
                    }
                    else
                    {
                        num5 = 0.0 - (PopupViewHeight + popupYPosition);
                        num6 = 0.0;
                    }

                    SlideAnimate(PopupView, IsOpen ? num5 : num6, IsOpen ? num6 : num5);
                    break;
                }
            case PopupAnimationMode.SlideOnBottom:
                {
                    double num5;
                    double num6;
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        num5 = PopupExtension.GetScreenHeight();
                        num6 = popupYPosition;
                    }
                    else
                    {
                        num5 = (double)PopupExtension.GetScreenHeight() - popupYPosition;
                        num6 = 0.0;
                    }

                    SlideAnimate(PopupView, IsOpen ? num5 : num6, IsOpen ? num6 : num5);
                    break;
                }
            case PopupAnimationMode.None:
                ProcessAnimationCompleted(PopupView);
                break;
        }
    }

    //
    // Summary:
    //     Animates the opacity of the popup view from a start value to an end value.
    //
    // Parameters:
    //   view:
    //     The view to which the animation to be applied.
    //
    //   startvalue:
    //     Start value of the animator.
    //
    //   endvalue:
    //     End value of the animator.
    private void FadeAnimate(Microsoft.Maui.Controls.View view, double startvalue, double endvalue)
    {
        Microsoft.Maui.Controls.View view2 = view;
        uint rate = 16u;
        Easing animationEasing = GetAnimationEasing();
        SetAnimationProgress(view2, animationInProgress: true);
        Animation animation = new Animation(delegate (double v)
        {
            view2.Opacity = v;
        }, startvalue, endvalue);
        animation.Commit(view2, "FadeAnimation", rate, (uint)AnimationDuration, animationEasing, delegate
        {
            ProcessAnimationCompleted(view2);
        });
    }

    //
    // Summary:
    //     Animates the scale of the popup view from a start value to an end value.
    //
    // Parameters:
    //   view:
    //     The view to which the animation to be applied.
    //
    //   startvalue:
    //     Start value of the animator.
    //
    //   endvalue:
    //     End value of the animator.
    private void ZoomAnimate(Microsoft.Maui.Controls.View view, float startvalue, float endvalue)
    {
        Microsoft.Maui.Controls.View view2 = view;
        uint rate = 16u;
        Easing animationEasing = GetAnimationEasing();
        SetAnimationProgress(view2, animationInProgress: true);
        Animation animation = new Animation(Zoom, startvalue, endvalue);
        animation.Commit(view2, "ZoomAnimation", rate, (uint)AnimationDuration, animationEasing, delegate
        {
            ProcessAnimationCompleted(view2);
        });
    }

    //
    // Summary:
    //     Animates the TranslationX and TranslationY of the popup view from a start value
    //     to an end value.
    //
    // Parameters:
    //   view:
    //     The view to which the animation to be applied.
    //
    //   startvalue:
    //     Start value of the animator.
    //
    //   endvalue:
    //     End value of the animator.
    private void SlideAnimate(Microsoft.Maui.Controls.View view, double startvalue, double endvalue)
    {
        Microsoft.Maui.Controls.View view2 = view;
        uint rate = 16u;
        Easing animationEasing = GetAnimationEasing();
        SetAnimationProgress(view2, animationInProgress: true);
        Animation animation = ((AnimationMode != PopupAnimationMode.SlideOnLeft && AnimationMode != PopupAnimationMode.SlideOnRight) ? new Animation(delegate (double v)
        {
            view2.TranslationY = v;
        }, startvalue, endvalue) : new Animation(delegate (double v)
        {
            view2.TranslationX = v;
        }, startvalue, endvalue));
        animation.Commit(view2, "SlideAnimation", rate, (uint)AnimationDuration, animationEasing, delegate
        {
            ProcessAnimationCompleted(view2);
        });
    }

    //
    // Summary:
    //     Set Scale value for PopupView.
    //
    // Parameters:
    //   scale:
    //     Scale value for PopupView.
    private void Zoom(double scale)
    {
        double num = popupXPosition + PopupViewWidth / 2.0;
        double num2 = popupYPosition + PopupViewHeight / 2.0;
        PopupView.TranslationX = num - PopupViewWidth * scale / 2.0;
        PopupView.TranslationY = num2 - PopupViewHeight * scale / 2.0;
        PopupView.Scale = scale;
    }

    //
    // Summary:
    //     Triggers after completion of the animation. Removes the popup view from the overlay
    //     and raise the popup events.
    //
    // Parameters:
    //   view:
    //     The view to which the animation progress to be set.
    private void ProcessAnimationCompleted(Microsoft.Maui.Controls.View view)
    {
        if (AnimationMode != PopupAnimationMode.None)
        {
            SetAnimationProgress(view, animationInProgress: false);
        }

        if (!IsOpen)
        {
            ResetAnimatedProperties();
            RemovePopupViewAndResetValues();
        }

        RaisePopupEvent();
    }

    //
    // Summary:
    //     Reset the PopupView Animated Properties.
    private void ResetAnimatedProperties()
    {
        PopupView.Scale = 1.0;
        PopupOverlayContainer.Opacity = 1.0;
        PopupView.Opacity = 1.0;
        PopupView.TranslationX = 0.0;
        PopupView.TranslationY = 0.0;
    }

    //
    // Summary:
    //     Abort the PopupView Animation.
    private void AbortPopupViewAnimation()
    {
        if (AnimationMode != PopupAnimationMode.None)
        {
            if (PopupView.AnimationIsRunning("ZoomAnimation"))
            {
                PopupView.AbortAnimation("ZoomAnimation");
            }
            else if (PopupView.AnimationIsRunning("FadeAnimation"))
            {
                PopupView.AbortAnimation("FadeAnimation");
            }
            else if (PopupView.AnimationIsRunning("SlideAnimation"))
            {
                PopupView.AbortAnimation("SlideAnimation");
            }

            if (PopupOverlayContainer.AnimationIsRunning("ContainerFadeAnimation"))
            {
                PopupOverlayContainer.AbortAnimation("ContainerFadeAnimation");
            }
        }
    }

    //
    // Summary:
    //     Used to set the value for Animation is progressing or not.
    //
    // Parameters:
    //   view:
    //     Animated view.
    //
    //   animationInProgress:
    //     Animation value.
    private void SetAnimationProgress(Microsoft.Maui.Controls.View view, bool animationInProgress)
    {
        if (view.GetType() == typeof(PopupOverlayContainer))
        {
            IsContainerAnimationInProgress = animationInProgress;
        }
        else
        {
            IsPopupAnimationInProgress = animationInProgress;
        }
    }
}
