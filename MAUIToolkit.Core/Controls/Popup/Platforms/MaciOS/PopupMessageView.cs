using MAUIToolkit.Core.ContentView;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     The message view of the PopupView, which is the content of the PopupView, when
//     the MAUIToolkit.Core.Controls.Popup.PopupMessageView.ContentView is null.
internal class PopupMessageView : CContentView
{
    //
    // Summary:
    //     Gets PopupView's instance.
    internal PopupView? PopupView;

    //
    // Summary:
    //     Gets or sets the CustomView to be loaded in the message view of the PopupView.
    //
    //
    // Value:
    //     The CustomView to be loaded in the content of the PopupView.
    internal View? ContentView;

    //
    // Summary:
    //     Gets or sets the text view in the message of the PopupView.
    //
    // Value:
    //     The text view in the content of the PopupView.
    internal PopupLabel? MessageView;

    //
    // Summary:
    //     Default padding for MAUIToolkit.Core.Controls.Popup.PopupMessageView.ContentView.
    private int contentPadding = 24;

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupMessageView class.
    public PopupMessageView()
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupMessageView class.
    //
    //
    // Parameters:
    //   popupView:
    //     The instance of PopupView.
    public PopupMessageView(PopupView popupView)
    {
        PopupView = popupView;
        Initialize();
    }

    //
    // Summary:
    //     Update the child views of the message view.
    internal void UpdateChildViews()
    {
        if (base.Content != null)
        {
            base.Content = null;
        }

        AddChildViews();
        UpdateMessageViewStyle();
    }

    //
    // Summary:
    //     Updates the popup message view style.
    internal void UpdateMessageViewStyle()
    {
        base.Background = PopupView.Popup.PopupStyle.GetMessageBackground();
        if (PopupView.Popup.ContentTemplate == null)
        {
            MessageView.TextColor = PopupView.Popup.PopupStyle.GetMessageTextColor();
            MessageView.FontFamily = PopupView.Popup.PopupStyle.MessageFontFamily;
            MessageView.FontSize = PopupView.Popup.PopupStyle.GetMessageFontSize();
            if (PopupView.Popup.PopupStyle.MessageFontAttribute == FontAttributes.Bold)
            {
                MessageView.FontAttributes = FontAttributes.Bold;
            }
            else if (PopupView.Popup.PopupStyle.MessageFontAttribute == FontAttributes.Italic)
            {
                MessageView.FontAttributes = FontAttributes.Italic;
            }
            else
            {
                MessageView.FontAttributes = FontAttributes.None;
            }

            if (PopupView.Popup.PopupStyle.MessageTextAlignment == TextAlignment.Start)
            {
                MessageView.VerticalTextAlignment = TextAlignment.Start;
                MessageView.HorizontalTextAlignment = TextAlignment.Start;
            }
            else if (PopupView.Popup.PopupStyle.MessageTextAlignment == TextAlignment.Center)
            {
                MessageView.VerticalTextAlignment = TextAlignment.Start;
                MessageView.HorizontalTextAlignment = TextAlignment.Center;
            }
            else
            {
                MessageView.VerticalTextAlignment = TextAlignment.Start;
                MessageView.HorizontalTextAlignment = TextAlignment.End;
            }
        }
    }

    //
    // Summary:
    //     Update message view padding.
    internal void UpdatePadding()
    {
        if (PopupView?.Popup.ContentTemplate == null && base.Content != null)
        {
            base.Content.Margin = new Thickness(contentPadding, (!PopupView.Popup.ShowHeader) ? contentPadding : 0, contentPadding, (!PopupView.Popup.ShowFooter) ? contentPadding : 0);
        }
    }

    //
    // Summary:
    //     Add the child views of the message view.
    internal void AddChildViews()
    {
        if (base.Content == null)
        {
            if (PopupView?.Popup.ContentTemplate == null)
            {
                base.Content = MessageView;
                UpdatePadding();
            }
            else
            {
                View content = (View)PopupView.Popup.GetTemplate(PopupView.Popup.ContentTemplate).CreateContent();
                base.Content = content;
            }
        }
    }

    //
    // Summary:
    //     Used to initialize the popup message view.
    private void Initialize()
    {
        MessageView = new PopupLabel
        {
            Text = PopupResources.GetLocalizedString("Message"),
            Style = new Style(typeof(PopupLabel))
        };
    }
}

