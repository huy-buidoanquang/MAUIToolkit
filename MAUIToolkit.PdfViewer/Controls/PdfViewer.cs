//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Microsoft.Maui;
//using Microsoft.Maui.Controls;
//using Microsoft.Maui.Controls.Shapes;
//using Microsoft.Maui.Devices;
//using Microsoft.Maui.Graphics;
//using Microsoft.Maui.Media;
//using MAUIToolkit.Core.Drawing;
//using Syncfusion.Maui.Core;
//using Syncfusion.Maui.Core.Annotations;
//using MAUIToolkit.Core.Internals;
//using Syncfusion.Maui.Core.Localization;
//using Syncfusion.Maui.ListView;
//using Syncfusion.Maui.ListView.Helpers;
//using Syncfusion.Maui.PdfViewer;
//using MAUIToolkit.Core.Themes;
//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Interactive;
//using Syncfusion.Pdf.Parsing;

//namespace MAUIToolkit.PdfViewer;

////
//// Summary:
////     Represents a control that displays a PDF document.
//public class PdfViewer : MAUIToolkit.Core.Controls.View, ITouchListener
//{
//    private bool m_isDocumentLoaded;

//    private StickyNoteAnnotation? hoveredStickyNote;

//    private bool m_isControlLoaded;

//    private double? m_zoomToRequest = null;

//    private Microsoft.Maui.Graphics.Point? m_scrollToRequest = null;

//    private int? m_goToPageRequest = null;

//    private ViewportManager? m_viewportManager;

//    private PdfDocumentManager? m_documentManager;

//    private TextSearchManager? m_textSearchManager;

//    private TextInfoManager? m_textInfoManager;

//    private AnnotationsLoader? annotationsLoader;

//    private ChangeTracker? changeTracker;

//    private PasswordRequestedEventArgs? m_passwordRequestedEventArgs;

//    private KeyboardGestureManager? m_keyboardGestureManager;

//    private double? m_density;

//    private string? m_password = null;

//    private FlattenOptions? flattenOptions = FlattenOptions.None;

//    private CancellationTokenSource? m_cancellationTokenSource = null;

//    private bool? m_isAsynchronousLoadRequest = null;

//    private ObservableCollection<OutlineElement>? documentOutline;

//    private InkEraserView? eraserPointer;

//    private bool eraserPressed = false;

//    private List<InkAnnotation> BackUpAnnotations = new List<InkAnnotation>();

//    private DisplayOrientation previousDeviceOrientation;

//    private FreeTextAnnotation? freeText;

//    private Border? toast;

//    private Label? toastText;

//    private SfToolbar? previousToolbar;

//    private bool modificationMade = false;

//    private PrintService? printService;

//    private SignatureViewOverlay? signatureViewOverlay;

//    private PageLayoutMode m_pageLayoutModeRequest = PageLayoutMode.Continuous;

//    internal const string PrimaryBackgroundColor = "#F3EDF7";

//    internal const string PrimaryForegroundColor = "#1C1B1F";

//    private PasswordDialog? passwordDialog;

//    private MessageDialog? messageDialog;

//    private PageNavigationErrorDialog? pageNavigationErrorMessage;

//    private HyperlinkDialog? hyperlinkDialog;

//    private ListBoxFormFieldListDialog? listBoxFormFieldListDialog;

//    private ComboBoxFormFieldListDialog? comboBoxFormFieldListDialog;

//    private FreeTextDialog? freeTextDialog;

//    private static readonly BindableProperty ControlBackgroundColorProperty = BindableProperty.Create("ControlBackgroundColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#FFE7E0EC"));

//    private static readonly BindableProperty ToolbarDesktopBackgroundColorProperty = BindableProperty.Create("ToolbarDesktopBackgroundColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#F7F2FB"));

//    private static readonly BindableProperty ToolbarDesktopTextColorProperty = BindableProperty.Create("ToolbarDesktopTextColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#49454F"));

//    private static readonly BindableProperty ToolbarBorderColorProperty = BindableProperty.Create("ToolbarBorderColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#CAC4D0"));

//    private static readonly BindableProperty PrimaryToolbarViewDesktopEntryViewBackgroundColorProperty = BindableProperty.Create("PrimaryToolbarViewDesktopEntryViewBackgroundColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#F7F2FB"));

//    private static readonly BindableProperty PrimaryToolbarViewDesktopEntryViewTextColorProperty = BindableProperty.Create("PrimaryToolbarViewDesktopEntryViewTextColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#1C1B1F"));

//    private static readonly BindableProperty PrimaryToolbarViewDesktopEntryViewFocusedColorProperty = BindableProperty.Create("PrimaryToolbarViewDesktopEntryViewFocusedColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#6750A4"));

//    private static readonly BindableProperty PrimaryToolbarViewDesktopEntryViewUnFocusedColorProperty = BindableProperty.Create("PrimaryToolbarViewDesktopEntryViewUnFocusedColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#49454F"));

//    private static readonly BindableProperty PrimaryToolbarViewDesktopSeparatorColorProperty = BindableProperty.Create("PrimaryToolbarViewDesktopSeparatorColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#CAC4D0"));

//    private static readonly BindableProperty StampContextMenuSeparatorColorProperty = BindableProperty.Create("StampContextMenuSeparatorColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#CAC4D0"));

//    private static readonly BindableProperty PrimaryToolbarViewEntryViewStrokeColorProperty = BindableProperty.Create("PrimaryToolbarViewEntryViewStrokeColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#49454F"));

//    private static readonly BindableProperty DesktopToolbarStrokeSliderActiveFillColorProperty = BindableProperty.Create("DesktopToolbarStrokeSliderActiveFillColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#6750A4"));

//    private static readonly BindableProperty DesktopToolbarStrokeSliderInActiveFillColorProperty = BindableProperty.Create("DesktopToolbarStrokeSliderInActiveFillColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#1F000000"));

//    private static readonly BindableProperty DesktopToolbarStrokeSliderThumbStyleColorProperty = BindableProperty.Create("DesktopToolbarStrokeSliderThumbStyleColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#6750A4"));

//    private static readonly BindableProperty DesktopToolbarStrokeSliderThumbOverlayStyleColorProperty = BindableProperty.Create("DesktopToolbarStrokeSliderThumbOverlayStyleColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#DFD8F7"));

//    private static readonly BindableProperty DesktopToolbarIconIsPressedColorProperty = BindableProperty.Create("DesktopToolbarIconIsPressedColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(73, 69, 79, 31));

//    private static readonly BindableProperty DesktopToolbarIconIsHoveredColorProperty = BindableProperty.Create("DesktopToolbarIconIsHoveredColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(73, 69, 79, 20));

//    private static readonly BindableProperty StampContextMenuBackgroundColorProperty = BindableProperty.Create("StampContextMenuBackgroundColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#EEE8F4"));

//    private static readonly BindableProperty StampContextMenuIsPressedColorProperty = BindableProperty.Create("StampContextMenuIsPressedColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(73, 69, 79, 31));

//    private static readonly BindableProperty StampContextMenuIsHoveredColorProperty = BindableProperty.Create("StampContextMenuIsHoveredColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(73, 69, 79, 20));

//    private static readonly BindableProperty MobileToolbarIconIsPressedColorProperty = BindableProperty.Create("MobileToolbarIconIsPressedColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(73, 69, 79, 31));

//    private static readonly BindableProperty MobileToolbarIconIsHoveredColorProperty = BindableProperty.Create("MobileToolbarIconIsHoveredColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(73, 69, 79, 20));

//    private static readonly BindableProperty MobileToolbarBackgroundColorProperty = BindableProperty.Create("MobileToolbarBackgroundColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#F7F2FB"));

//    private static readonly BindableProperty MobileSearchBusyIndicatorColorProperty = BindableProperty.Create("MobileSearchBusyIndicatorColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#6750A4"));

//    private static readonly BindableProperty MobileSeparatorColorProperty = BindableProperty.Create("MobileSeparatorColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#CAC4D0"));

//    private static readonly BindableProperty ToastBackgroundColorProperty = BindableProperty.Create("ToastBackgroundColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#313033"));

//    private static readonly BindableProperty ToastTextColorProperty = BindableProperty.Create("ToastTextColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#F4EFF4"));

//    private static readonly BindableProperty MobileToolbarTextColorProperty = BindableProperty.Create("MobileToolbarTextColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#49454F"));

//    private static readonly BindableProperty MobileToolbarSliderActiveFillColorProperty = BindableProperty.Create("MobileToolbarSliderActiveFillColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#6750A4"));

//    private static readonly BindableProperty MobileToolbarSliderInActiveFillColorProperty = BindableProperty.Create("MobileToolbarSliderInActiveFillColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#1F000000"));

//    private static readonly BindableProperty MobileToolbarSliderThumbStyleColorProperty = BindableProperty.Create("MobileToolbarSliderThumbStyleColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromArgb("#6750A4"));

//    private static readonly BindableProperty PageLayoutModeSelectionColorProperty = BindableProperty.Create("PageLayoutModeSelectionColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(73, 69, 79, 31));

//    private static readonly BindableProperty ScrollableIndicatorColorProperty = BindableProperty.Create("ScrollableIndicatorColor", typeof(Microsoft.Maui.Graphics.Color), typeof(SfPdfViewer), Microsoft.Maui.Graphics.Color.FromRgba(247, 242, 251, 255));

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.IsOutlineViewVisible
//    //     property.
//    public static readonly BindableProperty IsOutlineViewVisibleProperty = BindableProperty.Create("IsOutlineViewVisible", typeof(bool), typeof(SfPdfViewer), false, BindingMode.TwoWay, null, OnIsOutlineViewVisibleChanged);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.ShowToolbars property.
//    public static readonly BindableProperty ShowToolbarsProperty = BindableProperty.Create("ShowToolbars", typeof(bool), typeof(SfPdfViewer), true, BindingMode.TwoWay, null, ShowToolbarsPropertyChanged);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.MinZoomFactor property.
//    public static readonly BindableProperty MinZoomFactorProperty = BindableProperty.Create("MinZoomFactor", typeof(double), typeof(SfPdfViewer), 0.25, BindingMode.OneWay, null, coerceValue: CoerceMinZoom, propertyChanged: OnMinZoomChanged);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.MaxZoomFactor property.
//    public static readonly BindableProperty MaxZoomFactorProperty = BindableProperty.Create("MaxZoomFactor", typeof(double?), typeof(SfPdfViewer), 4.0, BindingMode.OneWay, null, coerceValue: CoerceMaxZoom, propertyChanged: OnMaxZoomChanged);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.ZoomFactorProperty
//    //     property.
//    public static readonly BindableProperty ZoomFactorProperty = BindableProperty.Create("ZoomFactor", typeof(double), typeof(SfPdfViewer), 1.0, BindingMode.OneWay, null, coerceValue: CoerceZoom, propertyChanged: OnZoomChanged);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.PageNumber property.
//    public static readonly BindableProperty PageNumberProperty = BindableProperty.Create("PageNumber", typeof(int), typeof(SfPdfViewer), 0, BindingMode.OneWay, null, OnPageNumberChanged);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.PageCount property.
//    public static readonly BindableProperty PageCountProperty = BindableProperty.Create("PageCount", typeof(int), typeof(SfPdfViewer), 0);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.DocumentSource property.
//    public static readonly BindableProperty DocumentSourceProperty = BindableProperty.Create("DocumentSource", typeof(object), typeof(SfPdfViewer), null, BindingMode.OneWay, null, OnDocumentSourceChanged);

//    //
//    // Summary:
//    //     Backing store for the Syncfusion.Maui.PdfViewer.SfPdfViewer.AnnotationMode property.
//    public static readonly BindableProperty AnnotationModeProperty = BindableProperty.Create("AnnotationMode", typeof(AnnotationMode), typeof(SfPdfViewer), AnnotationMode.None, BindingMode.OneWay, null, OnAnnotationModeChanged);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.VerticalOffset property.
//    public static readonly BindableProperty VerticalOffsetProperty = BindableProperty.Create("VerticalOffset", typeof(double?), typeof(SfPdfViewer), 0.0);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.HorizontalOffset property.
//    public static readonly BindableProperty HorizontalOffsetProperty = BindableProperty.Create("HorizontalOffset", typeof(double?), typeof(SfPdfViewer), 0.0);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.ExtentHeight property.
//    public static readonly BindableProperty ExtentHeightProperty = BindableProperty.Create("ExtentHeight", typeof(double), typeof(SfPdfViewer), 0.0);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.ExtentWidth property.
//    public static readonly BindableProperty ExtentWidthProperty = BindableProperty.Create("ExtentWidth", typeof(double), typeof(SfPdfViewer), 0.0);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.ShowScrollHead property.
//    public static readonly BindableProperty ShowScrollHeadProperty = BindableProperty.Create("ShowScrollHead", typeof(bool), typeof(SfPdfViewer), true, BindingMode.OneWay, null, OnShowScrollHeadPropertyChanged);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.EnableDocumentLinkNavigation
//    //     property.
//    public static readonly BindableProperty EnableDocumentLinkNavigationProperty = BindableProperty.Create("EnableDocumentLinkNavigation", typeof(bool), typeof(SfPdfViewer), true, BindingMode.OneWay, null, OnEnableDocumentLinkAnnotationPropertyChanged);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.EnableHyperlinkNavigation
//    //     property.
//    public static readonly BindableProperty EnableHyperlinkNavigationProperty = BindableProperty.Create("EnableHyperlinkNavigation", typeof(bool), typeof(SfPdfViewer), true, BindingMode.OneWay, null, OnEnableHyperlinkNavigationPropertyChanged);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.TextSearchSettings property.
//    public static readonly BindableProperty TextSearchSettingsProperty = BindableProperty.Create("TextSearchSettings", typeof(TextSearchSettings), typeof(SfPdfViewer), new TextSearchSettings());

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.TextSelectionSettings
//    //     property
//    public static readonly BindableProperty TextSelectionSettingsProperty = BindableProperty.Create("TextSelectionSettings", typeof(TextSelectionSettings), typeof(SfPdfViewer), new TextSelectionSettings());

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.EnableTextSelection property
//    public static readonly BindableProperty EnableTextSelectionProperty = BindableProperty.Create("EnableTextSelection", typeof(bool), typeof(SfPdfViewer), true, BindingMode.OneWay, null, OnEnableTextSelectionPropertyChanged);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.PageLayoutMode property
//    public static readonly BindableProperty PageLayoutModeProperty = BindableProperty.Create("PageLayoutMode", typeof(PageLayoutMode), typeof(SfPdfViewer), PageLayoutMode.Continuous, BindingMode.OneWay, null, OnPageLayoutModeChanged);

//    //
//    // Summary:
//    //     Backing store for Syncfusion.Maui.PdfViewer.SfPdfViewer.ZoomMode property
//    public static readonly BindableProperty ZoomModeProperty = BindableProperty.Create("ZoomMode", typeof(ZoomMode), typeof(SfPdfViewer), ZoomMode.Default, BindingMode.OneWay, null, OnZoomModeChanged);

//    internal SfInteractiveScrollViewExt? ScrollView { get; set; }

//    internal BusyIndicator? LoadingIndicator { get; set; }

//    internal PdfDocumentView? DocumentView { get; set; }

//    internal OutlineView? OutlineView { get; set; }

//    internal Grid? ToolbarGridView { get; set; }

//    internal SinglePageViewContainer? SinglePageViewContainer { get; set; }

//    internal StandardStamps? CreatedCustomStamp { get; set; }

//    internal bool IsDialogCreated { get; set; } = false;


//    internal bool IsZoomModeChanging { get; set; }

//    internal Grid? ScrollableIndicatorEnd { get; set; }

//    internal Grid? ScrollableIndicatorStart { get; set; }

//    internal TopToolbarView? TopToolbarMobile { get; set; }

//    internal BottomToolbarView? BottomToolbarViewMobile { get; set; }

//    internal ScrollableViewContainer? ScrollViewGridMobile { get; set; }

//    internal Grid? TopToolbarGridViewMobile { get; set; }

//    internal Grid? BottomToolbarGridViewMobile { get; set; }

//    internal Border? MoreOptionToolbarLayoutViewMobile { get; set; }

//    internal Border? StampMoreOptionLayoutViewDesktop { get; set; }

//    internal ShapeAnnotationsToolbarView? ShapeAnnotationsToolbarViewMobile { get; set; }

//    internal StickyNoteIconsToolbarView? StickyNoteIconsViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? FreeTextEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? InkEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? EraserEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? LineEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? ArrowEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? RectangleEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? CircleEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? PolygonEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? PolylineEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? HighlightEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? StrikeOutEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? UnderlineEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? SquigglyEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? StickyNoteEditToolbarViewMobile { get; set; }

//    internal SecondaryAnnotationToolbarView? StampEditToolbarViewMobile { get; set; }

//    internal TextMarkupToolbarView? TextMarkupToolbarViewMobile { get; set; }

//    internal Toolbar? MoreOptionToolbarViewMobile { get; set; }

//    internal MoreOptionToolbarLayout? MatchCaseToolbarLayoutMobile { get; set; }

//    internal StampViewDialog? StampViewDialogMobile { get; set; }

//    internal CustomStampDialog? CustomStampViewDialog { get; set; }

//    internal CustomStampDialogMobile? CustomStampViewDialogMobile { get; set; }

//    internal Toolbar? SearchToolbarViewMobile { get; set; }

//    internal SliderToolbarView? SliderToolbarViewMobile { get; set; }

//    internal ColorToolbarView? ColorToolbarViewMobile { get; set; }

//    internal PageCountLabelView? PageCountLabelView { get; set; }

//    internal PageSettingsLayoutView? PageSettingsLayoutViewMobile { get; set; }

//    internal ZoomModeView? ZoomModeViewMobile { get; set; }

//    internal PageLayoutModeToolbar? PageSettingsLayoutMobile { get; set; }

//    internal ZoomToolbar? ZoomModeToolbarMobile { get; set; }

//    internal Grid? PrimaryToolbarGridViewDesktop { get; set; }

//    internal PrimaryToolbarViewDesktop? TopToolbarDesktop { get; set; }

//    internal Grid? SecondaryToolbarGridViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? AnnotationsToolbarViewDesktop { get; set; }

//    internal ScrollableViewContainer? ScrollViewGridDesktop { get; set; }

//    internal Border? MoreOptionToolbarLayoutViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? ShapeAnnotationsToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? FreeTextSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? InkSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? EraserSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? LineSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? ArrowSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? RectangleSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? CircleSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? PolygonSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? PolylineSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? HighlightAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? StrikeOutAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? UnderlineAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? SquigglyAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? StickyNoteSecondaryAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? StampAnnotationToolbarViewDesktop { get; set; }

//    internal AnnotationsToolbarViewDesktop? TextMarkupToolbarViewDesktop { get; set; }

//    internal Toolbar? MoreOptionToolbarViewDesktop { get; set; }

//    internal StampListView? StandardStampListView { get; set; }

//    internal CustomStampListView? CustomStampListView { get; set; }

//    internal StampContextMenu? StampContextMenu { get; set; }

//    internal TextMarkupAnnotationListView? TextMarkupAnnotationListView { get; set; }

//    internal ShapeAnnotationListView? ShapeAnnotationListView { get; set; }

//    internal SearchDialog? SearchDialog { get; set; }

//    internal FontSizeListView? FontSizeListView { get; set; }

//    internal ColorPalette? ColorPalette { get; set; }

//    internal OpacityPalette? OpacityPalette { get; set; }

//    internal FreeTextColorPalette? FreeTextColorPalette { get; set; }

//    internal ThicknessPalette? ThicknessPalette { get; set; }

//    internal StrokeAndFillColorPalette? StrokeAndFillColorPalette { get; set; }

//    internal FileOperationListView? FileOperationListView { get; set; }

//    internal ViewModeOptions? ViewModeOptionsDesktop { get; set; }

//    internal PageSizeListView? PageSizeListView { get; set; }

//    internal StickyNoteIconListView? StickyNoteIconListView { get; set; }

//    internal StickyNoteDialog? StickyNoteDialog { get; set; }

//    internal Microsoft.Maui.Graphics.Color ToolbarBorderColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(ToolbarBorderColorProperty);
//        }
//        set
//        {
//            SetValue(ToolbarBorderColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color ControlBackgroundColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(ControlBackgroundColorProperty);
//        }
//        set
//        {
//            SetValue(ControlBackgroundColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color ToolbarDesktopBackgroundColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(ToolbarDesktopBackgroundColorProperty);
//        }
//        set
//        {
//            SetValue(ToolbarDesktopBackgroundColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color ToolbarDesktopTextColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(ToolbarDesktopTextColorProperty);
//        }
//        set
//        {
//            SetValue(ToolbarDesktopTextColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color PrimaryToolbarViewDesktopEntryViewBackgroundColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PrimaryToolbarViewDesktopEntryViewBackgroundColorProperty);
//        }
//        set
//        {
//            SetValue(PrimaryToolbarViewDesktopEntryViewBackgroundColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color PrimaryToolbarViewDesktopEntryViewTextColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PrimaryToolbarViewDesktopEntryViewTextColorProperty);
//        }
//        set
//        {
//            SetValue(PrimaryToolbarViewDesktopEntryViewTextColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color PrimaryToolbarViewDesktopEntryViewFocusedColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PrimaryToolbarViewDesktopEntryViewFocusedColorProperty);
//        }
//        set
//        {
//            SetValue(PrimaryToolbarViewDesktopEntryViewFocusedColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color PrimaryToolbarViewDesktopEntryViewUnFocusedColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PrimaryToolbarViewDesktopEntryViewUnFocusedColorProperty);
//        }
//        set
//        {
//            SetValue(PrimaryToolbarViewDesktopEntryViewUnFocusedColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color PrimaryToolbarViewEntryViewStrokeColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PrimaryToolbarViewEntryViewStrokeColorProperty);
//        }
//        set
//        {
//            SetValue(PrimaryToolbarViewEntryViewStrokeColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color PrimaryToolbarViewDesktopSeparatorColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PrimaryToolbarViewDesktopSeparatorColorProperty);
//        }
//        set
//        {
//            SetValue(PrimaryToolbarViewDesktopSeparatorColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color StampContextMenuSeparatorColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(StampContextMenuSeparatorColorProperty);
//        }
//        set
//        {
//            SetValue(StampContextMenuSeparatorColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color DesktopToolbarStrokeSliderActiveFillColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(DesktopToolbarStrokeSliderActiveFillColorProperty);
//        }
//        set
//        {
//            SetValue(DesktopToolbarStrokeSliderActiveFillColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color DesktopToolbarStrokeSliderInActiveFillColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(DesktopToolbarStrokeSliderInActiveFillColorProperty);
//        }
//        set
//        {
//            SetValue(DesktopToolbarStrokeSliderInActiveFillColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color DesktopToolbarStrokeSliderThumbStyleColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(DesktopToolbarStrokeSliderThumbStyleColorProperty);
//        }
//        set
//        {
//            SetValue(DesktopToolbarStrokeSliderThumbStyleColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color DesktopToolbarStrokeSliderThumbOverlayStyleColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(DesktopToolbarStrokeSliderThumbOverlayStyleColorProperty);
//        }
//        set
//        {
//            SetValue(DesktopToolbarStrokeSliderThumbOverlayStyleColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color DesktopToolbarIconIsPressedColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(DesktopToolbarIconIsPressedColorProperty);
//        }
//        set
//        {
//            SetValue(DesktopToolbarIconIsPressedColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color DesktopToolbarIconIsHoveredColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(DesktopToolbarIconIsHoveredColorProperty);
//        }
//        set
//        {
//            SetValue(DesktopToolbarIconIsHoveredColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color StampContextMenuBackgroundColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(StampContextMenuBackgroundColorProperty);
//        }
//        set
//        {
//            SetValue(StampContextMenuBackgroundColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color StampContextMenuIsPressedColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(StampContextMenuIsPressedColorProperty);
//        }
//        set
//        {
//            SetValue(StampContextMenuIsPressedColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color StampContextMenuIsHoveredColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(StampContextMenuIsHoveredColorProperty);
//        }
//        set
//        {
//            SetValue(StampContextMenuIsHoveredColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileToolbarIconIsPressedColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileToolbarIconIsPressedColorProperty);
//        }
//        set
//        {
//            SetValue(MobileToolbarIconIsPressedColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileToolbarIconIsHoveredColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileToolbarIconIsHoveredColorProperty);
//        }
//        set
//        {
//            SetValue(MobileToolbarIconIsHoveredColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileToolbarBackgroundColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileToolbarBackgroundColorProperty);
//        }
//        set
//        {
//            SetValue(MobileToolbarBackgroundColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color ToastBackgroundColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(ToastBackgroundColorProperty);
//        }
//        set
//        {
//            SetValue(ToastBackgroundColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color ToastTextColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(ToastTextColorProperty);
//        }
//        set
//        {
//            SetValue(ToastTextColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileSearchBusyIndicatorColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileSearchBusyIndicatorColorProperty);
//        }
//        set
//        {
//            SetValue(MobileSearchBusyIndicatorColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileSeparatorColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileSeparatorColorProperty);
//        }
//        set
//        {
//            SetValue(MobileSeparatorColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileToolbarTextColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileToolbarTextColorProperty);
//        }
//        set
//        {
//            SetValue(MobileToolbarTextColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileToolbarSliderActiveFillColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileToolbarSliderActiveFillColorProperty);
//        }
//        set
//        {
//            SetValue(MobileToolbarSliderActiveFillColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileToolbarSliderInActiveFillColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileToolbarSliderInActiveFillColorProperty);
//        }
//        set
//        {
//            SetValue(MobileToolbarSliderInActiveFillColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color MobileToolbarSliderThumbStyleColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(MobileToolbarSliderThumbStyleColorProperty);
//        }
//        set
//        {
//            SetValue(MobileToolbarSliderThumbStyleColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color PageLayoutModeSelectionColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PageLayoutModeSelectionColorProperty);
//        }
//        set
//        {
//            SetValue(PageLayoutModeSelectionColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color ZoomModeSelectionColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(PageLayoutModeSelectionColorProperty);
//        }
//        set
//        {
//            SetValue(PageLayoutModeSelectionColorProperty, value);
//        }
//    }

//    internal Microsoft.Maui.Graphics.Color ScrollableIndicatorColor
//    {
//        get
//        {
//            return (Microsoft.Maui.Graphics.Color)GetValue(ScrollableIndicatorColorProperty);
//        }
//        set
//        {
//            SetValue(ScrollableIndicatorColorProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets the PDF document source as System.IO.Stream or byte array.
//    public object? DocumentSource
//    {
//        get
//        {
//            return GetValue(DocumentSourceProperty);
//        }
//        set
//        {
//            SetValue(DocumentSourceProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets a value indicating the type of annotation that should be drawn using
//    //     UI interaction on the PDF pages.
//    public AnnotationMode AnnotationMode
//    {
//        get
//        {
//            return (AnnotationMode)GetValue(AnnotationModeProperty);
//        }
//        set
//        {
//            SetValue(AnnotationModeProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets the layout mode in which the pages will be displayed .
//    public PageLayoutMode PageLayoutMode
//    {
//        get
//        {
//            return (PageLayoutMode)GetValue(PageLayoutModeProperty);
//        }
//        set
//        {
//            SetValue(PageLayoutModeProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets the zoom mode.
//    public ZoomMode ZoomMode
//    {
//        get
//        {
//            return (ZoomMode)GetValue(ZoomModeProperty);
//        }
//        set
//        {
//            SetValue(ZoomModeProperty, value);
//        }
//    }

//    internal List<Annotation> BackupAnnotations { get; set; } = new List<Annotation>();


//    //
//    // Summary:
//    //     Gets the list of annotations in the PDF.
//    public ReadOnlyObservableCollection<Annotation> Annotations
//    {
//        get
//        {
//            CheckAnnotationsLoaderReady();
//            m_documentManager?.AwaitAnnotationsLoadingTaskCompletion();
//            if (annotationsLoader != null)
//            {
//                return new ReadOnlyObservableCollection<Annotation>(annotationsLoader.Annotations);
//            }

//            return new ReadOnlyObservableCollection<Annotation>(new ObservableCollection<Annotation>());
//        }
//    }

//    //
//    // Summary:
//    //     Gets the list of form fields in the PDF document.
//    public ReadOnlyObservableCollection<FormField> FormFields
//    {
//        get
//        {
//            CheckAnnotationsLoaderReady();
//            if (annotationsLoader != null)
//            {
//                return new ReadOnlyObservableCollection<FormField>(annotationsLoader.FormFields);
//            }

//            return new ReadOnlyObservableCollection<FormField>(new ObservableCollection<FormField>());
//        }
//    }

//    //
//    // Summary:
//    //     Gets the collection of toolbars in the SfPdfViewer
//    public ToolbarCollection Toolbars { get; } = new ToolbarCollection();


//    //
//    // Summary:
//    //     Gets or sets the value that indicates whether the built-in toolbar is visible
//    public bool ShowToolbars
//    {
//        get
//        {
//            return (bool)GetValue(ShowToolbarsProperty);
//        }
//        set
//        {
//            SetValue(ShowToolbarsProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets the list of custom bookmarks in the PDF document.
//    public ObservableCollection<Bookmark> CustomBookmarks
//    {
//        get
//        {
//            if (annotationsLoader != null)
//            {
//                return annotationsLoader.CustomBookmarks;
//            }

//            return new ObservableCollection<Bookmark>();
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets a value indicating whether the outline view is visible.
//    public bool IsOutlineViewVisible
//    {
//        get
//        {
//            return (bool)GetValue(IsOutlineViewVisibleProperty);
//        }
//        set
//        {
//            if (m_isDocumentLoaded)
//            {
//                SetValue(IsOutlineViewVisibleProperty, value);
//            }
//        }
//    }

//    //
//    // Summary:
//    //     Returns the total number of pages in a document.
//    public int PageCount
//    {
//        get
//        {
//            return (int)GetValue(PageCountProperty);
//        }
//        private set
//        {
//            SetValue(PageCountProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Returns the current page number.
//    public int PageNumber
//    {
//        get
//        {
//            return (int)GetValue(PageNumberProperty);
//        }
//        internal set
//        {
//            SetValue(PageNumberProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets the zoom factor for the document loaded in the Syncfusion.Maui.PdfViewer.SfPdfViewer.
//    //
//    //
//    // Value:
//    //     The default value is 1. The value can be set between Syncfusion.Maui.PdfViewer.SfPdfViewer.MinZoomFactor
//    //     and Syncfusion.Maui.PdfViewer.SfPdfViewer.MaxZoomFactor. If anything set beyond
//    //     the limit, the value will be clamped.
//    public double ZoomFactor
//    {
//        get
//        {
//            return (double)GetValue(ZoomFactorProperty);
//        }
//        set
//        {
//            SetValue(ZoomFactorProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Get or sets the minimum allowed zoom factor.
//    //
//    // Value:
//    //     The default value is 0.25 and 1 for desktop and mobile platforms respectively.
//    //     The value should not exceed the Syncfusion.Maui.PdfViewer.SfPdfViewer.MaxZoomFactor.
//    public double MinZoomFactor
//    {
//        get
//        {
//            return (double)GetValue(MinZoomFactorProperty);
//        }
//        set
//        {
//            SetValue(MinZoomFactorProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Get or sets the maximum allowed zoom factor.
//    //
//    // Value:
//    //     The default value is 4. The value should not be lower than Syncfusion.Maui.PdfViewer.SfPdfViewer.MinZoomFactor.
//    public double MaxZoomFactor
//    {
//        get
//        {
//            return (double)GetValue(MaxZoomFactorProperty);
//        }
//        set
//        {
//            SetValue(MaxZoomFactorProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Returns the current vertical scrolled position of the Syncfusion.Maui.PdfViewer.SfPdfViewer.
//    public double? VerticalOffset
//    {
//        get
//        {
//            return (double)GetValue(VerticalOffsetProperty);
//        }
//        private set
//        {
//            SetValue(VerticalOffsetProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Returns the current horizontal scrolled position of the Syncfusion.Maui.PdfViewer.SfPdfViewer.
//    public double? HorizontalOffset
//    {
//        get
//        {
//            return (double)GetValue(HorizontalOffsetProperty);
//        }
//        private set
//        {
//            SetValue(HorizontalOffsetProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets the rectangle that represents the client area (viewport) of the Syncfusion.Maui.PdfViewer.SfPdfViewer
//    //     control. The client area is the visible portion of the PDF document within the
//    //     control.
//    public Rect ClientRectangle
//    {
//        get
//        {
//            if (ScrollView != null && ScrollView.ViewportHeight != 0.0)
//            {
//                return new Rect(0.0, 0.0, ScrollView.ViewportWidth, ScrollView.ViewportHeight);
//            }

//            if (ScrollView != null && ScrollView.ViewportHeight == 0.0)
//            {
//                return new Rect(0.0, 0.0, ScrollView.ViewportWidth, ScrollView.Height);
//            }

//            return Rect.Zero;
//        }
//    }

//    //
//    // Summary:
//    //     Gets the vertical size of the PDF document displayed by the Syncfusion.Maui.PdfViewer.SfPdfViewer.
//    //     The value is calculated considering the zoom factor, page spacing, and the overall
//    //     height of the pages.
//    public double ExtentHeight
//    {
//        get
//        {
//            return Math.Floor((double)GetValue(ExtentHeightProperty));
//        }
//        private set
//        {
//            SetValue(ExtentHeightProperty, value);
//            OnPropertyChanged("ExtentHeight");
//        }
//    }

//    //
//    // Summary:
//    //     Gets the horizontal size of the PDF document displayed by the Syncfusion.Maui.PdfViewer.SfPdfViewer.
//    //     The value is calculated considering the zoom factor, page spacing, and the overall
//    //     width of the pages.
//    public double ExtentWidth
//    {
//        get
//        {
//            return Math.Floor((double)GetValue(ExtentWidthProperty));
//        }
//        private set
//        {
//            SetValue(ExtentWidthProperty, value);
//            OnPropertyChanged("ExtentWidth");
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets the value indicates whether or not the scroll head can be shown.
//    //
//    //
//    // Value:
//    //     The default value is true.
//    //
//    // Remarks:
//    //     This property is applicable only for the mobile platforms.
//    public bool ShowScrollHead
//    {
//        get
//        {
//            return (bool)GetValue(ShowScrollHeadProperty);
//        }
//        set
//        {
//            SetValue(ShowScrollHeadProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Represents and allows for modification of the text search settings.
//    public TextSearchSettings TextSearchSettings => (TextSearchSettings)GetValue(TextSearchSettingsProperty);

//    //
//    // Summary:
//    //     Gets or sets a value that indicates whether document link annotations (TOC) in
//    //     the PDF can be interacted with.
//    public bool EnableDocumentLinkNavigation
//    {
//        get
//        {
//            return (bool)GetValue(EnableDocumentLinkNavigationProperty);
//        }
//        set
//        {
//            SetValue(EnableDocumentLinkNavigationProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets the value that indicates whether the hyperlink navigation can be
//    //     performed or not.
//    //
//    // Value:
//    //     The default value is true.
//    public bool EnableHyperlinkNavigation
//    {
//        get
//        {
//            return (bool)GetValue(EnableHyperlinkNavigationProperty);
//        }
//        set
//        {
//            SetValue(EnableHyperlinkNavigationProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets the text selection settings
//    public TextSelectionSettings TextSelectionSettings
//    {
//        get
//        {
//            return (TextSelectionSettings)GetValue(TextSelectionSettingsProperty);
//        }
//        set
//        {
//            SetValue(TextSelectionSettingsProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets or sets a value that indicates whether the text selection is enabled or
//    //     not. The default value is true.
//    public bool EnableTextSelection
//    {
//        get
//        {
//            return (bool)GetValue(EnableTextSelectionProperty);
//        }
//        set
//        {
//            SetValue(EnableTextSelectionProperty, value);
//        }
//    }

//    //
//    // Summary:
//    //     Gets the oultline for the document.
//    //
//    // Value:
//    //     The outlines in the document.
//    public IReadOnlyList<OutlineElement> DocumentOutline
//    {
//        get
//        {
//            if (m_documentManager != null && m_documentManager.LoadedDocument != null && (documentOutline == null || documentOutline.Count == 0))
//            {
//                PopulateDocumentOutline(m_documentManager.LoadedDocument.Bookmarks);
//            }

//            return documentOutline;
//        }
//    }

//    //
//    // Summary:
//    //     Gets the value that represents the go to specific page command.
//    public ICommand? GoToPageCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the value that represents the go to previous page command.
//    public ICommand? GoToPreviousPageCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the value that represents the go to next page command.
//    public ICommand? GoToNextPageCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the value that represents the go to first page command.
//    public ICommand? GoToFirstPageCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the value that represents the go to last page command.
//    public ICommand? GoToLastPageCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the command to print the PDF document.
//    public ICommand? PrintDocumentCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the value that represents the scroll to specific offset command.
//    public ICommand? ScrollToOffsetCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the command that loads a PDF document when executed.
//    public ICommand? LoadDocumentCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the command that saves the PDF document.
//    public ICommand? SaveDocumentCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the command that performs the undo opertation.
//    public ICommand? UndoCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the command that performs the redo operation.
//    public ICommand? RedoCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the command that removes all annotations from the PDF document.
//    public ICommand? RemoveAllAnnotationsCommand { get; private set; }

//    //
//    // Summary:
//    //     Gets the command to search a text in the PDF document asynchronously.
//    public ICommand? SearchTextCommand { get; private set; }

//    private Microsoft.Maui.Graphics.Point LastTouchPoint { get; set; }

//    //
//    // Summary:
//    //     Gets or sets a value indicating whether the page navigation dialog should be
//    //     shown when the scroll head is tapped.
//    public bool EnablePageNavigationDialog { get; set; } = true;


//    //
//    // Summary:
//    //     Gets or sets the default annotation settings.
//    public AnnotationSettings AnnotationSettings { get; private set; } = new AnnotationSettings();


//    //
//    // Summary:
//    //     Gets the annotation that is currently selected.
//    //
//    // Remarks:
//    //     The property will be null if no annotation is currently selected.
//    internal Annotation? SelectedAnnotation { get; private set; }

//    //
//    // Summary:
//    //     Occurs when a PDF document is unloaded.
//    public event EventHandler<EventArgs?>? DocumentUnloaded;

//    //
//    // Summary:
//    //     Occurs when an annotation is selected.
//    public event EventHandler<AnnotationEventArgs>? AnnotationSelected;

//    //
//    // Summary:
//    //     Occurs when a selected annotation gets deselected.
//    public event EventHandler<AnnotationEventArgs>? AnnotationDeselected;

//    //
//    // Summary:
//    //     Occurs when annotation is modified.
//    public event EventHandler<AnnotationEventArgs>? AnnotationEdited;

//    //
//    // Summary:
//    //     Occurs when an annotation is added to a page.
//    public event EventHandler<AnnotationEventArgs>? AnnotationAdded;

//    //
//    // Summary:
//    //     Occurs when an annotation is removed from the page.
//    public event EventHandler<AnnotationEventArgs>? AnnotationRemoved;

//    //
//    // Summary:
//    //     Occurs when a PDF document is loaded.
//    public event EventHandler<EventArgs?>? DocumentLoaded;

//    //
//    // Summary:
//    //     Occurs when the PDF Viewer fails to load a PDF document.
//    public event EventHandler<DocumentLoadFailedEventArgs?>? DocumentLoadFailed;

//    //
//    // Summary:
//    //     Occurs when a security password is required to open a PDF document.
//    public event EventHandler<PasswordRequestedEventArgs?>? PasswordRequested;

//    //
//    // Summary:
//    //     Occurs when hyperlink is clicked or tapped.
//    public event EventHandler<HyperlinkClickedEventArgs?>? HyperlinkClicked;

//    //
//    // Summary:
//    //     Occurs when the text search is in progress.
//    public event EventHandler<TextSearchProgressEventArgs?>? TextSearchProgress;

//    //
//    // Summary:
//    //     Occurs when the text is selected or when the selected text is changed.
//    public event EventHandler<TextSelectionChangedEventArgs?>? TextSelectionChanged;

//    //
//    // Summary:
//    //     Occurs when the Syncfusion.Maui.PdfViewer.SfPdfViewer is tapped.
//    public event EventHandler<GestureEventArgs?>? Tapped;

//    //
//    // Summary:
//    //     Occurs when form field value changes.
//    public event EventHandler<FormFieldValueChangedEventArgs?>? FormFieldValueChanged;

//    //
//    // Summary:
//    //     Occurs when a form field receives or loses focus.
//    //
//    // Remarks:
//    //     The event will occur only for text and signature fields.
//    public event EventHandler<FormFieldFocusChangedEvenArgs?>? FormFieldFocusChanged;

//    //
//    // Summary:
//    //     Occurs when a signature is created using the built-in signature pad.
//    public event EventHandler<SignatureCreatedEventArgs>? SignatureCreated;

//    //
//    // Summary:
//    //     Occurs when the free text modal view is about to appear.
//    public event EventHandler<AnnotationModalViewAppearingEventArgs>? FreeTextModalViewAppearing;

//    //
//    // Summary:
//    //     Occurs when the free text modal view is about to disappear after being dismissed.
//    public event EventHandler<EventArgs>? FreeTextModalViewDisappearing;

//    //
//    // Summary:
//    //     Occurs when the sticky note modal view is about to appear.
//    public event EventHandler<AnnotationModalViewAppearingEventArgs>? StickyNoteModalViewAppearing;

//    //
//    // Summary:
//    //     Occurs when the sticky note modal view is about to disappear after being dismissed.
//    public event EventHandler<EventArgs>? StickyNoteModalViewDisappearing;

//    //
//    // Summary:
//    //     Occurs when the custom stamp modal view is about to appear.
//    public event EventHandler<AnnotationModalViewAppearingEventArgs>? CustomStampModalViewAppearing;

//    //
//    // Summary:
//    //     Occurs when the custom stamp modal view is about to disappear after being dismissed.
//    public event EventHandler<EventArgs>? CustomStampModalViewDisappearing;

//    //
//    // Summary:
//    //     Occurs when the signature modal view is about to appear.
//    public event EventHandler<FormFieldModalViewAppearingEventArgs>? SignatureModalViewAppearing;

//    //
//    // Summary:
//    //     Occurs when the signature modal view is about to disappear after being dismissed.
//    public event EventHandler<EventArgs>? SignatureModalViewDisappearing;

//    //
//    // Summary:
//    //     Initializes a new instance of the Syncfusion.Maui.PdfViewer.SfPdfViewer class.
//    public SfPdfViewer()
//    {
//        InitializeComponent();
//        previousDeviceOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
//        this.ValidateLicense();
//        LocalizationResourceAccessor.InitializeDefaultResource("Syncfusion.Maui.PdfViewer.Resources.SfPdfViewer");
//        this.AddTouchListener(this);
//    }

//    private void InitializeComponent()
//    {
//        ApplyDynamicStyles();
//        base.BackgroundColor = ControlBackgroundColor;
//        AddScrollView();
//        base.PropertyChanged += OnPdfViewerPropertyChanged;
//        base.SizeChanged += SfPdfViewer_SizeChanged;
//        m_keyboardGestureManager = new KeyboardGestureManager(this);
//        ToolbarGridView = new Grid();
//        ToolbarGridView.RowDefinitions.Add(new RowDefinition
//        {
//            Height = new GridLength(56.0)
//        });
//        ToolbarGridView.RowDefinitions.Add(new RowDefinition
//        {
//            Height = new GridLength(0.0)
//        });
//        ToolbarGridView.RowDefinitions.Add(new RowDefinition
//        {
//            Height = new GridLength(1.0, GridUnitType.Star)
//        });
//        InitializeDesktopToolbarLayout();
//        AddLoadingIndicator();
//        InitializeCommands();
//        AnnotationSettings.Ink.PropertyChanged += InkSettings_PropertyChanged;
//        AnnotationSettings.InkEraser.PropertyChanged += InkEraserSettings_PropertyChanged;
//        CreateToast();
//    }

//    private void SfPdfViewer_SizeChanged(object? sender, EventArgs e)
//    {
//        if (CustomStampListView != null && CustomStampListView.IsVisible && (CustomStampListView.HeightRequest >= base.Bounds.Height / 1.5 || (CustomStampListView?.Content as SfListView)?.GetVisualContainer().Bounds.Height > base.Bounds.Height / 1.5))
//        {
//            CustomStampListView.HeightRequest = base.Bounds.Height / 1.5;
//        }

//        if (StandardStampListView != null && StandardStampListView.IsVisible)
//        {
//            StandardStampListView.HeightRequest = base.Bounds.Height / 1.5;
//        }
//    }

//    private void ApplyDynamicStyles()
//    {
//        ThemeElement.InitializeThemeResources(this, "SfPdfViewerTheme");
//    }

//    private async void InitializeDesktopToolbarLayout()
//    {
//        PrimaryToolbarGridViewDesktop = new Grid();
//        TopToolbarDesktop = new PrimaryToolbarViewDesktop(this);
//        Toolbars.Add(TopToolbarDesktop);
//        Grid.SetRow((BindableObject)PrimaryToolbarGridViewDesktop, 0);
//        TopToolbarDesktop.IsAddedInView = true;
//        BoxView primaryToolbarBorder = new BoxView
//        {
//            HeightRequest = 1.0,
//            BackgroundColor = ToolbarBorderColor,
//            VerticalOptions = LayoutOptions.End,
//            ZIndex = 1
//        };
//        ToolbarGridView?.Children.Add(PrimaryToolbarGridViewDesktop);
//        ScrollViewGridDesktop = new ScrollableViewContainer();
//        Grid.SetRow((BindableObject)ScrollViewGridDesktop, 2);
//        ScrollViewGridDesktop.Children.Add(ScrollView);
//        ToolbarGridView?.Children.Add(ScrollViewGridDesktop);
//        base.Children.Add(ToolbarGridView);
//        await Task.Delay(10);
//        PrimaryToolbarGridViewDesktop.Children.Add(TopToolbarDesktop.ScrollableToolbar);
//        PrimaryToolbarGridViewDesktop.Children.Add(primaryToolbarBorder);
//    }

//    internal void RenderToolbarView(Toolbar? pdfViewerToolbar, bool IsVisible)
//    {
//        if ((pdfViewerToolbar != null && !pdfViewerToolbar.IsVisible) || !ShowToolbars)
//        {
//            return;
//        }

//        if (previousToolbar != null)
//        {
//            previousToolbar.IsVisible = false;
//        }

//        if (pdfViewerToolbar != null && SecondaryToolbarGridViewDesktop != null && AnnotationsToolbarViewDesktop != null && !pdfViewerToolbar.IsAddedInView)
//        {
//            pdfViewerToolbar.IsAddedInView = true;
//            SecondaryToolbarGridViewDesktop.IsVisible = true;
//            AnnotationsToolbarViewDesktop.ScrollableToolbar.IsVisible = true;
//            if (ToolbarGridView?.RowDefinitions[1] != null && ToolbarGridView.RowDefinitions[1].Height.Value == 0.0)
//            {
//                ToolbarGridView.RowDefinitions[1].Height = new GridLength(56.0);
//            }

//            SecondaryToolbarGridViewDesktop?.Children.Add(pdfViewerToolbar.ScrollableToolbar);
//        }
//        else if (pdfViewerToolbar != null && SecondaryToolbarGridViewDesktop != null && AnnotationsToolbarViewDesktop != null)
//        {
//            SecondaryToolbarGridViewDesktop.IsVisible = true;
//            AnnotationsToolbarViewDesktop.ScrollableToolbar.IsVisible = true;
//            if (ToolbarGridView?.RowDefinitions[1] != null && ToolbarGridView.RowDefinitions[1].Height.Value == 0.0)
//            {
//                ToolbarGridView.RowDefinitions[1].Height = new GridLength(56.0);
//            }

//            pdfViewerToolbar.ScrollableToolbar.IsVisible = IsVisible;
//        }

//        if (SelectedAnnotation != null)
//        {
//            if (SelectedAnnotation is StampAnnotation)
//            {
//                View view = pdfViewerToolbar?.ScrollableToolbar?.Items.FirstOrDefault((SfToolbarItem i) => i.Name == "ColorPickerSeparator")?.View;
//                if (view != null)
//                {
//                    view.IsVisible = true;
//                }

//                if (AnnotationsToolbarViewDesktop != null)
//                {
//                    View view2 = pdfViewerToolbar?.ScrollableToolbar?.Items.FirstOrDefault((SfToolbarItem i) => i.Name == AnnotationsToolbarViewDesktop.ColorPickerText)?.View;
//                    if (view2 != null)
//                    {
//                        view2.IsVisible = true;
//                    }
//                }
//            }

//            if (AnnotationsToolbarViewDesktop != null)
//            {
//                View view3 = pdfViewerToolbar?.ScrollableToolbar?.Items.FirstOrDefault((SfToolbarItem i) => i.Name == AnnotationsToolbarViewDesktop.DeleteText)?.View;
//                if (view3 != null)
//                {
//                    view3.IsVisible = true;
//                }
//            }
//        }
//        else if (AnnotationsToolbarViewDesktop != null)
//        {
//            View view4 = pdfViewerToolbar?.ScrollableToolbar?.Items.FirstOrDefault((SfToolbarItem i) => i.Name == AnnotationsToolbarViewDesktop.DeleteText)?.View;
//            if (view4 != null)
//            {
//                view4.IsVisible = false;
//            }
//        }

//        if (pdfViewerToolbar?.OverlayToolbar != null)
//        {
//            previousToolbar = pdfViewerToolbar?.OverlayToolbar;
//        }
//        else
//        {
//            previousToolbar = pdfViewerToolbar?.ScrollableToolbar;
//        }
//    }

//    private void VisibleOrInVisibleBorderStyleIcon(SecondaryAnnotationToolbarView pdfViewerToolbar)
//    {
//        View view = pdfViewerToolbar.ScrollableToolbar.LeadingItems?.FirstOrDefault((SfToolbarItem i) => i.Name == "AnnotationType")?.View;
//        View view2 = pdfViewerToolbar.ScrollableToolbar.LeadingItems?.FirstOrDefault((SfToolbarItem i) => i.Name == "BorderStyle")?.View;
//        if ((SelectedAnnotation != null && SelectedAnnotation is PolygonAnnotation polygonAnnotation && polygonAnnotation.BorderStyle == BorderStyle.Cloudy) || AnnotationSettings.Polygon.BorderStyle == BorderStyle.Cloudy)
//        {
//            if (view != null)
//            {
//                view.IsVisible = false;
//            }

//            if (view2 != null)
//            {
//                view2.IsVisible = true;
//            }
//        }
//        else
//        {
//            if (view != null)
//            {
//                view.IsVisible = true;
//            }

//            if (view2 != null)
//            {
//                view2.IsVisible = false;
//            }
//        }
//    }

//    private void InkEraserSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            DocumentView?.InkEraserThicknessChanged(PageNumber);
//        }
//        else
//        {
//            SinglePageViewContainer?.InkEraserThicknessChanged(PageNumber);
//        }

//        if (eraserPointer != null)
//        {
//            float x = (float)eraserPointer.Bounds.X;
//            float y = (float)eraserPointer.Bounds.Y;
//            float thickness = AnnotationSettings.InkEraser.Thickness;
//            float thickness2 = AnnotationSettings.InkEraser.Thickness;
//            AbsoluteLayout.SetLayoutBounds((BindableObject)eraserPointer, (Rect)new RectF(x, y, thickness, thickness2));
//            eraserPointer.Thickness = AnnotationSettings.InkEraser.Thickness;
//            eraserPointer.InvalidateDrawable();
//        }
//    }

//    private void InkSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        if (AnnotationMode != AnnotationMode.Ink)
//        {
//            return;
//        }

//        if (e.PropertyName == "IsStylusModeWithStylusTouch" || e.PropertyName == "TouchScreenInputMode")
//        {
//            if (AnnotationSettings.Ink.IsStylusModeWithStylusTouch || AnnotationSettings.Ink.TouchScreenInputMode == TouchScreenInputMode.FingerAndStylus)
//            {
//                if (ScrollView != null)
//                {
//                    ScrollView.AllowZoom = false;
//                }
//            }
//            else if (ScrollView != null)
//            {
//                ScrollView.AllowZoom = true;
//            }
//        }
//        else
//        {
//            if (annotationsLoader == null || annotationsLoader.UnconfirmedInks == null)
//            {
//                return;
//            }

//            foreach (InkAnnotation unconfirmedInk in annotationsLoader.UnconfirmedInks)
//            {
//                switch (e.PropertyName)
//                {
//                    case "Color":
//                        unconfirmedInk.Color = AnnotationSettings.Ink.Color;
//                        break;
//                    case "BorderWidth":
//                        unconfirmedInk.BorderWidth = AnnotationSettings.Ink.BorderWidth;
//                        break;
//                    case "Opacity":
//                        unconfirmedInk.Opacity = AnnotationSettings.Ink.Opacity;
//                        break;
//                }
//            }
//        }
//    }

//    private static object? CoerceMinZoom(BindableObject bindable, object? value)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer)
//        {
//            value = sfPdfViewer.ScrollView?.GetCoercedMinZoom(value);
//        }

//        return value;
//    }

//    private static object? CoerceZoom(BindableObject bindable, object? value)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer)
//        {
//            value = sfPdfViewer.ScrollView?.GetCoercedZoom(value);
//        }

//        return value;
//    }

//    private static object? CoerceMaxZoom(BindableObject bindable, object? value)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer)
//        {
//            value = sfPdfViewer.ScrollView?.GetCoercedMaxZoom(value);
//        }

//        return value;
//    }

//    private static void OnMinZoomChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (!(bindable is SfPdfViewer sfPdfViewer))
//        {
//            return;
//        }

//        if (sfPdfViewer.ScrollView != null)
//        {
//            sfPdfViewer.ScrollView.MinZoomFactor = (double)newValue;
//        }
//        else
//        {
//            if (sfPdfViewer.SinglePageViewContainer == null)
//            {
//                return;
//            }

//            for (int i = 0; i < sfPdfViewer.SinglePageViewContainer.Pages.Length; i++)
//            {
//                SinglePageScrollView singlePageScrollView = sfPdfViewer.SinglePageViewContainer.Pages[i];
//                if (singlePageScrollView != null)
//                {
//                    singlePageScrollView.MinZoomFactor = (double)newValue;
//                }
//            }
//        }
//    }

//    private static void OnIsOutlineViewVisibleChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (!(bindable is SfPdfViewer sfPdfViewer) || !(newValue is bool))
//        {
//            return;
//        }

//        bool flag = (bool)newValue;
//        if (true)
//        {
//            if (flag)
//            {
//                sfPdfViewer.ShowOutlineView();
//            }
//            else
//            {
//                sfPdfViewer.OutlineView?.Close();
//            }
//        }
//    }

//    private static void ShowToolbarsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (!(bindable is SfPdfViewer sfPdfViewer) || !(newValue is bool))
//        {
//            return;
//        }

//        bool flag = (bool)newValue;
//        if (true)
//        {
//            if (flag)
//            {
//                sfPdfViewer.ShowToolbar();
//            }
//            else
//            {
//                sfPdfViewer.HideToolbar();
//            }
//        }
//    }

//    private static void OnMaxZoomChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (!(bindable is SfPdfViewer sfPdfViewer))
//        {
//            return;
//        }

//        if (sfPdfViewer.ScrollView != null)
//        {
//            sfPdfViewer.ScrollView.MaxZoomFactor = (double)newValue;
//        }
//        else
//        {
//            if (sfPdfViewer.SinglePageViewContainer == null)
//            {
//                return;
//            }

//            for (int i = 0; i < sfPdfViewer.SinglePageViewContainer.Pages.Length; i++)
//            {
//                SinglePageScrollView singlePageScrollView = sfPdfViewer.SinglePageViewContainer.Pages[i];
//                if (singlePageScrollView != null)
//                {
//                    singlePageScrollView.MaxZoomFactor = (double)newValue;
//                }
//            }
//        }
//    }

//    private static void OnAnnotationModeChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer)
//        {
//            sfPdfViewer.OnAnnotationModeChanged((AnnotationMode)newValue);
//            sfPdfViewer.CloseOverlayToolbar();
//            sfPdfViewer.CloseDialog(sfPdfViewer.StickyNoteDialog);
//        }
//    }

//    private static void OnDocumentSourceChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (!(bindable is SfPdfViewer sfPdfViewer))
//        {
//            return;
//        }

//        if (newValue != null)
//        {
//            if (sfPdfViewer.m_isControlLoaded)
//            {
//                sfPdfViewer.m_isAsynchronousLoadRequest = true;
//                sfPdfViewer.LoadDocument(newValue, sfPdfViewer.m_password, sfPdfViewer.flattenOptions);
//            }
//        }
//        else
//        {
//            sfPdfViewer.Unload();
//        }
//    }

//    private static void OnShowScrollHeadPropertyChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//    }

//    private static void OnEnableDocumentLinkAnnotationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer && sfPdfViewer.annotationsLoader != null)
//        {
//            sfPdfViewer.annotationsLoader.EnableDocumentLinkNavigation = (bool)newValue;
//        }
//    }

//    private static void OnEnableHyperlinkNavigationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer && sfPdfViewer.annotationsLoader != null)
//        {
//            sfPdfViewer.annotationsLoader.EnableHyperlinkNavigation = (bool)newValue;
//        }
//    }

//    private static void OnEnableTextSelectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer && sfPdfViewer.m_textInfoManager != null)
//        {
//            sfPdfViewer.m_textInfoManager.EnableTextSelection = (bool)newValue;
//            sfPdfViewer.SetTextInfoManager();
//        }
//    }

//    private static void OnPageNumberChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//    }

//    private static void OnZoomChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (!(bindable is SfPdfViewer sfPdfViewer))
//        {
//            return;
//        }

//        double num = (double)newValue;
//        if (sfPdfViewer.PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            if (sfPdfViewer.ScrollView != null)
//            {
//                if (sfPdfViewer.ScrollView.ZoomFactor != num)
//                {
//                    sfPdfViewer.ZoomTo(num);
//                }
//            }
//            else
//            {
//                sfPdfViewer.ZoomTo(num);
//            }
//        }
//        else if (sfPdfViewer.SinglePageViewContainer != null)
//        {
//            SinglePageScrollView currentSinglePageView = sfPdfViewer.GetCurrentSinglePageView(sfPdfViewer.PageNumber);
//            if (currentSinglePageView != null && currentSinglePageView.ZoomFactor != num)
//            {
//                sfPdfViewer.ZoomTo(num);
//            }
//        }
//        else
//        {
//            sfPdfViewer.ZoomTo(num);
//        }

//        AnnotationConstants.ZoomFactor = num;
//        sfPdfViewer?.annotationsLoader?.RefreshAnnotations();
//        sfPdfViewer?.PositionStickyNoteOnZoom();
//        if (sfPdfViewer != null && !sfPdfViewer.IsZoomModeChanging)
//        {
//            sfPdfViewer.ZoomMode = ZoomMode.Default;
//        }

//        if (sfPdfViewer != null && sfPdfViewer.TopToolbarMobile != null && sfPdfViewer.TopToolbarMobile.ZoomView != null && sfPdfViewer.ZoomMode == ZoomMode.Default)
//        {
//            sfPdfViewer.TopToolbarMobile.ZoomView.IsVisible = true;
//        }

//        if (sfPdfViewer != null && sfPdfViewer.TopToolbarMobile != null && sfPdfViewer.TopToolbarMobile.ZoomView != null && sfPdfViewer.ZoomMode != 0)
//        {
//            sfPdfViewer.TopToolbarMobile.ZoomView.IsVisible = false;
//        }
//    }

//    private void OnScrollViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        string propertyName = e.PropertyName;
//        string text = propertyName;
//        if (!(text == "ViewportHeight"))
//        {
//            if (text == "ZoomFactor")
//            {
//                ZoomFactor = ScrollView.ZoomFactor;
//            }
//        }
//        else
//        {
//            if (!m_isDocumentLoaded)
//            {
//                return;
//            }

//            m_viewportManager?.UpdateScrollData(ScrollView.ScrollX, ScrollView.ScrollY, ScrollView.ViewportWidth, ScrollView.ViewportHeight, isIntermediate: false);
//            AdjustZoomForZoomMode(ZoomMode);
//        }

//        if (TopToolbarDesktop != null && TopToolbarDesktop.PreviousPage != null && PageNumber == 1)
//        {
//            TopToolbarDesktop.PreviousPage.IsEnabled = false;
//        }

//        if (TopToolbarDesktop != null && TopToolbarDesktop.NextPage != null && PageNumber == PageCount)
//        {
//            TopToolbarDesktop.NextPage.IsEnabled = false;
//        }
//    }

//    private void OnViewportPropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        if (!(e.PropertyName == "CurrentPageNumber"))
//        {
//            return;
//        }

//        PageNumber = m_viewportManager.CurrentPageNumber;
//        OutlineView?.UpdateOutlineViewPageNumber(PageNumber);
//        RefreshPageCommandCanExecutes();
//        if (TopToolbarDesktop == null || TopToolbarDesktop.PreviousPage == null || TopToolbarDesktop.NextPage == null)
//        {
//            return;
//        }

//        if (PageNumber == 1)
//        {
//            TopToolbarDesktop.PreviousPage.IsEnabled = false;
//        }
//        else if (PageNumber == PageCount)
//        {
//            TopToolbarDesktop.NextPage.IsEnabled = false;
//        }
//        else if (PageNumber > 1 && PageNumber < PageCount)
//        {
//            DispatcherUtils.Dispatch(this, delegate
//            {
//                TopToolbarDesktop.PreviousPage.IsEnabled = true;
//                TopToolbarDesktop.NextPage.IsEnabled = true;
//            });
//        }
//    }

//    private void OnDocumentPropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        if (e.PropertyName == "PageCount")
//        {
//            PageCount = m_documentManager.PageCount;
//        }
//    }

//    private static void OnPageLayoutModeChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer)
//        {
//            sfPdfViewer.OnPageLayoutModeChanged((PageLayoutMode)newValue);
//        }
//    }

//    private static void OnZoomModeChanged(BindableObject bindable, object oldValue, object newValue)
//    {
//        if (bindable is SfPdfViewer sfPdfViewer)
//        {
//            sfPdfViewer.OnZoomModeChanged((ZoomMode)newValue);
//        }
//    }

//    private void AnnotationsLoader_FormFieldTapped(object? sender, FormFieldEventArgs e)
//    {
//        if (e.FormField is ButtonFormField buttonFormField && buttonFormField.Destination != null)
//        {
//            int pageIndex = buttonFormField.Destination.PageIndex;
//            Microsoft.Maui.Graphics.PointF destination = new Microsoft.Maui.Graphics.PointF(buttonFormField.Destination.Location.X, buttonFormField.Destination.Location.Y);
//            ScrollToLocation(pageIndex, destination);
//        }

//        if (e.FormField is SignatureFormField signatureFormField && signatureFormField.Signature == null)
//        {
//            ShowSignatureDialog(signatureFormField);
//        }
//    }

//    private async void AnnotationsLoader_LinkTapped(object? sender, LinkTappedEventArgs e)
//    {
//        bool isStickyNoteDialogVisible = StickyNoteDialog != null && StickyNoteDialog.Parent != null;
//        if (e.LinkView == null || isStickyNoteDialogVisible)
//        {
//            return;
//        }

//        LinkView linkView = e.LinkView;
//        if (string.IsNullOrEmpty(linkView.Uri) && EnableDocumentLinkNavigation)
//        {
//            if (linkView.Destination.HasValue && m_documentManager != null && m_documentManager.PagesBoundsInfo != null)
//            {
//                linkView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromArgb("#D0BCFF");
//                await Task.Delay(50);
//                double zoomFactor = ZoomFactor * m_documentManager.PageSizeMultiplier;
//                Rect bounds = Rect.Zero;
//                if (m_documentManager.PagesBoundsInfo.ContainsKey(linkView.DestinationPageNumber - 1))
//                {
//                    bounds = m_documentManager.PagesBoundsInfo[linkView.DestinationPageNumber - 1];
//                }

//                double top = bounds.Top * ZoomFactor;
//                double left = bounds.Left * ZoomFactor;
//                double x2 = linkView.Destination.Value.X * zoomFactor;
//                double y2 = linkView.Destination.Value.Y * zoomFactor;
//                x2 = ((x2 < 0.0) ? 0.0 : x2);
//                y2 = ((y2 < 0.0) ? 0.0 : y2);
//                if (PageLayoutMode == PageLayoutMode.Continuous)
//                {
//                    ScrollToOffset(x2 + left, y2 + top);
//                }
//                else if (SinglePageViewContainer != null)
//                {
//                    GoToPage(linkView.DestinationPageNumber);
//                }

//                linkView.BackgroundColor = Colors.Transparent;
//            }
//        }
//        else if (EnableHyperlinkNavigation && !string.IsNullOrEmpty(linkView.Uri))
//        {
//            HyperlinkClickedEventArgs eventArgs = new HyperlinkClickedEventArgs(linkView.Uri);
//            this.HyperlinkClicked?.Invoke(this, eventArgs);
//            if (!eventArgs.Handled)
//            {
//                ShowHyperlinkDialog(SfPdfViewerResources.GetLocalizedString("OpenWebPage"), eventArgs.Uri);
//            }
//        }
//    }

//    private void OnScrollViewLoaded(object? sender, EventArgs e)
//    {
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            m_density = DeviceDisplay.MainDisplayInfo.Density;
//        });
//        m_isControlLoaded = true;
//        if (DocumentSource != null)
//        {
//            LoadDocument(DocumentSource, m_password, flattenOptions);
//        }

//        ScrollView.Loaded -= OnScrollViewLoaded;
//    }

//    private void OnZoomChangingCompleted(object? sender, EventArgs? e)
//    {
//        DocumentView?.RefreshPages();
//    }

//    private void OnDocumentLoadingFailed(object? sender, DocumentLoadFailedEventArgs? e)
//    {
//        Unload();
//        LoadingIndicator?.CanShowBusy(value: false);
//        if (e != null && e.Message == "Can't open an encrypted document. The password is invalid." && passwordDialog != null && passwordDialog.IsOpen)
//        {
//            passwordDialog.NotifyInvalidPassword();
//            return;
//        }

//        UnloadPasswordEventArgs();
//        ClearBackupLoadInputs();
//        if (e != null && e.Message == "Document loading has been cancelled")
//        {
//            NotifyDocumentLoadingFailed(null, e.Message);
//        }
//        else
//        {
//            InvokeDocumentLoadFailed(e);
//        }
//    }

//    private void OnPasswordVerified(object? sender, EventArgs? e)
//    {
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            passwordDialog?.Reset();
//            ClosePasswordDialog();
//        });
//    }

//    private void PasswordDialog_CloseRequested(object? sender, EventArgs e)
//    {
//        ClosePasswordDialog();
//        NotifyDocumentLoadingFailed(null, "Document loading has been cancelled");
//    }

//    private void PasswordDialog_OnSubmitted(object? sender, EventArgs e)
//    {
//        if (m_passwordRequestedEventArgs != null && passwordDialog != null)
//        {
//            LoadDocument(m_passwordRequestedEventArgs.DocumentSource, passwordDialog.Password, flattenOptions);
//        }
//    }

//    internal void UpdatePageNumberLabel()
//    {
//        PageCountLabelView?.SetPageNumberLabelText(PageNumber);
//    }

//    internal void OnDocumentLoaded(DocumentLoadedEventArgs e)
//    {
//        UnloadPasswordEventArgs();
//        if (m_viewportManager != null && ScrollView != null)
//        {
//            if (PageLayoutMode == PageLayoutMode.Single)
//            {
//                if (m_pageLayoutModeRequest == PageLayoutMode.Single)
//                {
//                    SwitchToSinglePageViewMode();
//                    m_pageLayoutModeRequest = PageLayoutMode.Continuous;
//                }
//                else
//                {
//                    SinglePageViewContainer?.Intialize(m_viewportManager.CurrentPageNumber);
//                }
//            }

//            if (!m_goToPageRequest.HasValue && !m_scrollToRequest.HasValue)
//            {
//                m_viewportManager.UpdateScrollData(0.0, 0.0, ScrollView.ViewportWidth, ScrollView.ViewportHeight, isIntermediate: false);
//            }
//            else
//            {
//                m_goToPageRequest = null;
//                m_scrollToRequest = null;
//            }
//        }

//        LoadingIndicator?.CanShowBusy(value: false);
//        if (m_cancellationTokenSource == null || !m_cancellationTokenSource.IsCancellationRequested)
//        {
//            this.DocumentLoaded?.Invoke(this, EventArgs.Empty);
//        }
//        else
//        {
//            Unload();
//            NotifyDocumentLoadingFailed(null, "Document loading has been cancelled");
//        }

//        if (TopToolbarDesktop != null)
//        {
//            TopToolbarDesktop.EnablePrimaryToolbarIcons();
//        }

//        if (TopToolbarDesktop?.UndoView?.View != null)
//        {
//            TopToolbarDesktop.UndoView.View.IsVisible = false;
//        }

//        if (TopToolbarDesktop?.RedoView?.View != null)
//        {
//            TopToolbarDesktop.RedoView.View.IsVisible = false;
//        }
//    }

//    private void UpdateUndoRedoButtons()
//    {
//        if (!modificationMade)
//        {
//            return;
//        }

//        if (TopToolbarMobile?.UndoView?.View != null && TopToolbarMobile?.RedoView?.View != null)
//        {
//            DispatcherUtils.Dispatch(this, delegate
//            {
//                TopToolbarMobile.UndoView.View.IsVisible = true;
//                TopToolbarMobile.RedoView.View.IsVisible = true;
//                TopToolbarMobile.UndoView.View.IsEnabled = true;
//                TopToolbarMobile.RedoView.View.IsEnabled = false;
//            });
//        }

//        if (TopToolbarDesktop?.UndoView?.View != null && TopToolbarDesktop.RedoView?.View != null)
//        {
//            DispatcherUtils.Dispatch(this, delegate
//            {
//                TopToolbarDesktop.UndoView.View.IsVisible = true;
//                TopToolbarDesktop.RedoView.View.IsVisible = true;
//                TopToolbarDesktop.UndoView.View.IsEnabled = true;
//                TopToolbarDesktop.RedoView.View.IsEnabled = false;
//            });
//        }
//    }

//    internal void CreateViewModeListViewDesktop()
//    {
//        ViewModeOptionsDesktop = new ViewModeOptions(this);
//        base.Children.Add(ViewModeOptionsDesktop);
//        ViewModeOptionsDesktop.IsVisible = false;
//    }

//    internal void CloseViewModeListViewDesktop()
//    {
//        if (ViewModeOptionsDesktop != null)
//        {
//            ViewModeOptionsDesktop.IsVisible = false;
//        }
//    }

//    internal void CreatePageSizeListView()
//    {
//        PageSizeListView = new PageSizeListView(this);
//        base.Children.Add(PageSizeListView);
//        PageSizeListView.IsVisible = false;
//    }

//    internal void ClosePageSizeListView()
//    {
//        if (PageSizeListView != null)
//        {
//            PageSizeListView.IsVisible = false;
//        }
//    }

//    internal void CreateFileOperationListView()
//    {
//        FileOperationListView = new FileOperationListView(this);
//        base.Children.Add(FileOperationListView);
//        FileOperationListView.IsVisible = false;
//    }

//    internal void CloseFileOperationListView()
//    {
//        if (FileOperationListView != null)
//        {
//            FileOperationListView.IsVisible = false;
//        }
//    }

//    internal void CreateColorPalette()
//    {
//        ColorPalette = new ColorPalette(this);
//        base.Children.Add(ColorPalette);
//        ColorPalette.IsVisible = false;
//    }

//    internal void CreateFontSizeListView()
//    {
//        FontSizeListView = new FontSizeListView(this);
//        base.Children.Add(FontSizeListView);
//        FontSizeListView.IsVisible = false;
//    }

//    internal void CloseFontSizeListView()
//    {
//        if (FontSizeListView != null)
//        {
//            FontSizeListView.IsVisible = false;
//        }
//    }

//    internal void CreateOpacityPalette()
//    {
//        OpacityPalette = new OpacityPalette(this);
//        base.Children.Add(OpacityPalette);
//        OpacityPalette.IsVisible = false;
//    }

//    internal void CreateFreeTextColorPalette()
//    {
//        FreeTextColorPalette = new FreeTextColorPalette(this);
//        base.Children.Add(FreeTextColorPalette);
//        FreeTextColorPalette.IsVisible = false;
//    }

//    internal void CreateThicknessPalette()
//    {
//        ThicknessPalette = new ThicknessPalette(this);
//        base.Children.Add(ThicknessPalette);
//        ThicknessPalette.IsVisible = false;
//    }

//    internal void CreateStickyNoteIconListViewDesktop()
//    {
//        StickyNoteIconListView = new StickyNoteIconListView(this);
//        StickyNoteIconListView.IsVisible = false;
//    }

//    internal void CreateCustomStampDialog()
//    {
//        if (CustomStampViewDialog == null)
//        {
//            CustomStampViewDialog = new CustomStampDialog(this);
//            CustomStampViewDialog.CustomStampCreated += OnCustomStampCreated;
//        }
//    }

//    internal void CloseStickyNoteIconsListViewDesktop()
//    {
//        if (StickyNoteIconListView != null)
//        {
//            StickyNoteIconListView.IsVisible = false;
//        }
//    }

//    internal void ShowColorPalette()
//    {
//        if (ColorPalette != null)
//        {
//            ColorPalette.IsVisible = true;
//        }
//    }

//    internal void CloseColorPalette()
//    {
//        if (ColorPalette != null)
//        {
//            ColorPalette.IsVisible = false;
//        }
//    }

//    internal void CloseOpacityPalette()
//    {
//        if (OpacityPalette != null)
//        {
//            OpacityPalette.IsVisible = false;
//        }
//    }

//    internal void CloseFreeTextColorPalette()
//    {
//        if (FreeTextColorPalette != null)
//        {
//            FreeTextColorPalette.IsVisible = false;
//        }
//    }

//    internal void CloseThicknessPalette()
//    {
//        if (ThicknessPalette != null)
//        {
//            ThicknessPalette.IsVisible = false;
//        }
//    }

//    internal void CreateStrokeAndFillColorPalette()
//    {
//        StrokeAndFillColorPalette = new StrokeAndFillColorPalette(this);
//        base.Children.Add(StrokeAndFillColorPalette);
//        StrokeAndFillColorPalette.IsVisible = false;
//    }

//    internal void ShowStrokeAndFillColorPalette()
//    {
//        if (StrokeAndFillColorPalette != null)
//        {
//            StrokeAndFillColorPalette.IsVisible = true;
//        }
//    }

//    internal void CloseStrokeAndFillColorPalette()
//    {
//        if (StrokeAndFillColorPalette != null)
//        {
//            StrokeAndFillColorPalette.IsVisible = false;
//        }
//    }

//    internal void CreateTextMarkupAnnotationDialog()
//    {
//        TextMarkupAnnotationListView = new TextMarkupAnnotationListView(this);
//        base.Children.Add(TextMarkupAnnotationListView);
//        TextMarkupAnnotationListView.IsVisible = false;
//    }

//    internal void ShowTextMarkupAnnotationDialog()
//    {
//        if (TextMarkupAnnotationListView != null)
//        {
//            TextMarkupAnnotationListView.IsVisible = true;
//        }

//        TextMarkupAnnotationListView?.DisappearHighlight();
//    }

//    internal void HideTextMarkupAnnotationDialog()
//    {
//        if (TextMarkupAnnotationListView != null)
//        {
//            TextMarkupAnnotationListView.IsVisible = false;
//        }
//    }

//    internal void CreateShapeAnnotationDialog()
//    {
//        ShapeAnnotationListView = new ShapeAnnotationListView(this);
//        base.Children.Add(ShapeAnnotationListView);
//        ShapeAnnotationListView.IsVisible = false;
//    }

//    internal void ShowShapeAnnotationDialog()
//    {
//        if (ShapeAnnotationListView != null)
//        {
//            ShapeAnnotationListView.IsVisible = true;
//        }

//        ShapeAnnotationListView?.DisappearHighlight();
//    }

//    internal void HideShapeAnnotationDialog()
//    {
//        if (ShapeAnnotationListView != null)
//        {
//            ShapeAnnotationListView.IsVisible = false;
//        }
//    }

//    internal void CreateSearchDialog()
//    {
//        SearchDialog = new SearchDialog();
//        base.Children.Add(SearchDialog);
//        if (SearchDialog.SearchCount != null && SearchDialog.SearchView != null)
//        {
//            SearchDialog.SearchView.FlowDirection = FlowDirection.LeftToRight;
//            SearchDialog.SearchCount.FlowDirection = FlowDirection.LeftToRight;
//        }

//        SearchDialog.IsVisible = false;
//        SearchDialog.NoMatchesFound += SearchDialog_NoMatchesFound;
//        SearchDialog.SearchHelper = this;
//    }

//    internal void ShowSearchDialog()
//    {
//        if (SearchDialog != null)
//        {
//            SearchDialog.IsVisible = true;
//        }
//    }

//    internal void CloseSearchDialog()
//    {
//        SearchDialog?.Close();
//    }

//    internal void ShowPageCountLabelView()
//    {
//        if (PageCountLabelView != null)
//        {
//            PageCountLabelView.IsVisible = true;
//        }
//    }

//    internal void HidePageCountLabelView()
//    {
//        if (PageCountLabelView != null)
//        {
//            PageCountLabelView.IsVisible = false;
//        }
//    }

//    internal void CloseStampDialogs()
//    {
//        if (CustomStampViewDialog != null)
//        {
//            CustomStampViewDialog.IsVisible = false;
//        }

//        if (StampContextMenu != null)
//        {
//            StampContextMenu.IsVisible = false;
//        }

//        if (StandardStampListView != null)
//        {
//            StandardStampListView.IsVisible = false;
//        }

//        if (CustomStampListView != null)
//        {
//            CustomStampListView.IsVisible = false;
//        }

//        if (StampMoreOptionLayoutViewDesktop != null)
//        {
//            StampMoreOptionLayoutViewDesktop.IsVisible = false;
//        }
//    }

//    private void SearchDialog_NoMatchesFound(object? sender, EventArgs e)
//    {
//        SearchDialog?.NotifyNoMatchesFound();
//    }

//    private void OnPasswordRequested(object? sender, PasswordRequestedEventArgs? e)
//    {
//        Unload();
//        LoadingIndicator?.CanShowBusy(value: false);
//        this.PasswordRequested?.Invoke(this, e);
//        m_passwordRequestedEventArgs = e;
//        if (e != null)
//        {
//            if (m_documentManager != null)
//            {
//                m_documentManager.IsLoadingInProgress = false;
//            }

//            if (!e.Handled)
//            {
//                DispatcherUtils.Dispatch(this, ShowPasswordDialog);
//            }
//            else if (!string.IsNullOrEmpty(e.Password))
//            {
//                LoadDocument(e.DocumentSource, e.Password, flattenOptions);
//            }
//        }
//    }

//    private void OnDocumentLoadingCompleted(object? sender, DocumentLoadedEventArgs? e)
//    {
//        DocumentLoadedEventArgs e2 = e;
//        if (e2 == null || m_viewportManager == null || m_documentManager == null)
//        {
//            return;
//        }

//        m_viewportManager.PageCount = e2.TotalPagesParsed;
//        if (m_viewportManager.PagesBoundsInfo == null)
//        {
//            m_viewportManager.PagesBoundsInfo = m_documentManager.PagesBoundsInfo;
//        }

//        DispatcherUtils.Dispatch(this, delegate
//        {
//            DocumentView?.SizeTo(e2.DocumentWidth, e2.DocumentHeight);
//            ScrollView?.UpdateContentSize(e2.DocumentWidth, e2.DocumentHeight);
//            UpdateExtentDimensions();
//            m_isDocumentLoaded = true;
//            ExecutePreloadRequests();
//            annotationsLoader?.GetLoadedCustomBookmarks();
//            OnDocumentLoaded(e2);
//            if (m_documentManager.LoadedDocument != null && OutlineView != null)
//            {
//                OutlineView.PopulateInitialBookmarks(m_documentManager.LoadedDocument.Bookmarks);
//                OutlineView.PopulateCustomBookmarks();
//            }

//            ClearBackupLoadInputs();
//        });
//    }

//    void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
//    {
//        SetDynamicResource(ControlBackgroundColorProperty, "SfPdfViewerControlBackgroundColor");
//        SetDynamicResource(ToolbarDesktopBackgroundColorProperty, "SfPdfViewerToolbarBackgroundColor");
//        SetDynamicResource(ToolbarDesktopTextColorProperty, "SfPdfViewerToolbarTextColor");
//        SetDynamicResource(PrimaryToolbarViewDesktopEntryViewBackgroundColorProperty, "SfPdfViewerPageNumberEntryViewBackgroundColor");
//        SetDynamicResource(PrimaryToolbarViewDesktopEntryViewTextColorProperty, "SfPdfViewerPageNumberEntryViewTextColor");
//        SetDynamicResource(PrimaryToolbarViewDesktopEntryViewFocusedColorProperty, "SfPdfViewerPageNumberEntryViewFocusedStrokeColor");
//        SetDynamicResource(PrimaryToolbarViewDesktopEntryViewUnFocusedColorProperty, "SfPdfViewerPageNumberEntryViewUnFocusedStrokeColor");
//        SetDynamicResource(PrimaryToolbarViewDesktopSeparatorColorProperty, "SfPdfViewerSeparatorColor");
//        SetDynamicResource(StampContextMenuSeparatorColorProperty, "SfPdfViewerSeparatorColor");
//        SetDynamicResource(PrimaryToolbarViewEntryViewStrokeColorProperty, "SfPdfViewerPageNumberEntryViewUnFocusedStrokeColor");
//        SetDynamicResource(DesktopToolbarStrokeSliderActiveFillColorProperty, "SfPdfViewerSliderTrackStyleActiveFillColor");
//        SetDynamicResource(DesktopToolbarStrokeSliderInActiveFillColorProperty, "SfPdfViewerSliderTrackStyleInActiveFillColor");
//        SetDynamicResource(DesktopToolbarStrokeSliderThumbStyleColorProperty, "SfPdfViewerSliderThumbStyleColor");
//        SetDynamicResource(DesktopToolbarStrokeSliderThumbOverlayStyleColorProperty, "SfPdfViewerSliderThumbOverlayStyleColor");
//        SetDynamicResource(DesktopToolbarIconIsPressedColorProperty, "SfPdfViewerToolbarIconIsPressedColor");
//        SetDynamicResource(DesktopToolbarIconIsHoveredColorProperty, "SfPdfViewerToolbarIconIsHoveredColor");
//        SetDynamicResource(StampContextMenuBackgroundColorProperty, "SfPdfViewerStampContextMenuBackgroundColor");
//        SetDynamicResource(StampContextMenuIsPressedColorProperty, "SfPdfViewerStampContextMenuItemIsPressedColor");
//        SetDynamicResource(StampContextMenuIsHoveredColorProperty, "SfPdfViewerStampContextMenuItemIsHoveredColor");
//        SetDynamicResource(MobileToolbarIconIsPressedColorProperty, "SfPdfViewerToolbarIconIsPressedColor");
//        SetDynamicResource(MobileToolbarIconIsHoveredColorProperty, "SfPdfViewerToolbarIconIsHoveredColor");
//        SetDynamicResource(MobileToolbarBackgroundColorProperty, "SfPdfViewerToolbarBackgroundColor");
//        SetDynamicResource(MobileToolbarTextColorProperty, "SfPdfViewerToolbarTextColor");
//        SetDynamicResource(MobileToolbarSliderActiveFillColorProperty, "SfPdfViewerSliderTrackStyleActiveFillColor");
//        SetDynamicResource(MobileToolbarSliderInActiveFillColorProperty, "SfPdfViewerSliderTrackStyleInActiveFillColor");
//        SetDynamicResource(MobileToolbarSliderThumbStyleColorProperty, "SfPdfViewerSliderThumbStyleColor");
//        SetDynamicResource(PageLayoutModeSelectionColorProperty, "SfPdfViewerPageLayoutModeSelectionColor");
//        SetDynamicResource(MobileSearchBusyIndicatorColorProperty, "SfPdfViewerSearchDialogSearchBusyIndicatorColor");
//        SetDynamicResource(MobileSeparatorColorProperty, "SfPdfViewerSeparatorColor");
//        SetDynamicResource(ToastBackgroundColorProperty, "SfPdfViewerToastBackgroundColor");
//        SetDynamicResource(ToastTextColorProperty, "SfPdfViewerToastTextColor");
//        SetDynamicResource(ScrollableIndicatorColorProperty, "SfPdfViewerScrollableIndicatorColor");
//        SetDynamicResource(ToolbarBorderColorProperty, "SfPdfViewerSeparatorColor");
//    }

//    void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
//    {
//    }

//    private void UpdateExtentDimensions()
//    {
//        if (ScrollView != null && ScrollView.ExtentSize.HasValue)
//        {
//            ExtentWidth = Math.Floor(ScrollView.ExtentSize.Value.Width);
//            ExtentHeight = Math.Floor(ScrollView.ExtentSize.Value.Height);
//        }
//    }

//    private void InvokeDocumentLoadFailed(DocumentLoadFailedEventArgs? e)
//    {
//        DocumentLoadFailedEventArgs e2 = e;
//        this.DocumentLoadFailed?.Invoke(this, e2);
//        if (e2 == null || e2.Handled)
//        {
//            return;
//        }

//        DispatcherUtils.Dispatch(this, delegate
//        {
//            if (e2.Message == "Invalid cross reference table.")
//            {
//                ShowMessageDialog(SfPdfViewerResources.GetLocalizedString("Error"), SfPdfViewerResources.GetLocalizedString("DocumentLoadFailed"));
//            }
//            else
//            {
//                ShowMessageDialog(SfPdfViewerResources.GetLocalizedString("Error"), e2.Message);
//            }
//        });
//    }

//    internal void PopulateDocumentOutline(PdfBookmarkBase bookmarkBase)
//    {
//        documentOutline = new ObservableCollection<OutlineElement>();
//        for (int i = 0; i < bookmarkBase.Count; i++)
//        {
//            PdfBookmark bookmark = bookmarkBase[i];
//            OutlineElement item = new OutlineElement(bookmark);
//            documentOutline?.Add(item);
//        }
//    }

//    private void OnDialogVisibilityChange(bool isOpening)
//    {
//        DialogBase.IsAnyDialogOpen = isOpening;
//        if (DocumentView != null)
//        {
//            AutomationProperties.SetIsInAccessibleTree(DocumentView, !isOpening);
//        }
//    }

//    internal void ShowDialog(DialogBase? dialog)
//    {
//        if (dialog != null && !dialog.IsOpen)
//        {
//            OnDialogVisibilityChange(isOpening: true);
//            ScrollViewGridDesktop?.Children.Add(dialog);
//        }
//    }

//    private void ShowDialog(ComboBoxFormFieldListDialog? dialog)
//    {
//        if (dialog != null)
//        {
//            OnDialogVisibilityChange(isOpening: true);
//            ScrollViewGridDesktop?.Children.Add(dialog);
//        }
//    }

//    private void ShowDialog(ListBoxFormFieldListDialog? dialog)
//    {
//        if (dialog != null)
//        {
//            OnDialogVisibilityChange(isOpening: true);
//            ScrollViewGridDesktop?.Children.Add(dialog);
//        }
//    }

//    private void ShowDialog(SignaturePadDialog? dialog)
//    {
//        if (dialog != null)
//        {
//            OnDialogVisibilityChange(isOpening: true);
//            ScrollViewGridDesktop?.Children.Add(dialog);
//            this.RemoveTouchListener(this);
//        }
//    }

//    private void ShowDialog(StickyNoteDialog? dialog)
//    {
//        if (dialog != null && !dialog.IsOpen)
//        {
//            OnDialogVisibilityChange(isOpening: true);
//            ScrollViewGridDesktop?.Children.Add(dialog);
//        }
//    }

//    internal void CloseDialog(DialogBase? dialog)
//    {
//        DialogBase dialog2 = dialog;
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            if (dialog2 != null && dialog2.IsOpen)
//            {
//                ScrollViewGridDesktop?.Children.Remove(dialog2);
//                OnDialogVisibilityChange(isOpening: false);
//            }
//        });
//    }

//    private void CloseDialog(ComboBoxFormFieldListDialog? dialog)
//    {
//        ComboBoxFormFieldListDialog dialog2 = dialog;
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            if (dialog2 != null)
//            {
//                ScrollViewGridDesktop?.Children.Remove(dialog2);
//                OnDialogVisibilityChange(isOpening: false);
//            }
//        });
//    }

//    private void CloseDialog(ListBoxFormFieldListDialog? dialog)
//    {
//        ListBoxFormFieldListDialog dialog2 = dialog;
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            if (dialog2 != null)
//            {
//                ScrollViewGridDesktop?.Children.Remove(dialog2);
//                OnDialogVisibilityChange(isOpening: false);
//            }
//        });
//    }

//    private void CloseDialog(SignaturePadDialog? dialog)
//    {
//        SignaturePadDialog dialog2 = dialog;
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            if (dialog2 != null)
//            {
//                ScrollViewGridDesktop?.Children.Remove(dialog2);
//                OnDialogVisibilityChange(isOpening: false);
//                this.AddTouchListener(this);
//            }
//        });
//    }

//    private void CloseDialog(StickyNoteDialog? dialog)
//    {
//        StickyNoteDialog dialog2 = dialog;
//        hoveredStickyNote?.Unfocus();
//        hoveredStickyNote = null;
//        if (SelectedAnnotation != null)
//        {
//            DeselectAnnotation(SelectedAnnotation);
//        }

//        DispatcherUtils.Dispatch(this, delegate
//        {
//            if (dialog2 != null && dialog2.IsOpen)
//            {
//                if (dialog2.StickyNoteEditor != null)
//                {
//                    dialog2.StickyNoteEditor.IsEnabled = false;
//                }

//                if (dialog2.CloseLabel != null)
//                {
//                    dialog2.CloseLabel.IsVisible = false;
//                }

//                if (dialog2.SaveLabel != null)
//                {
//                    dialog2.SaveLabel.IsVisible = false;
//                }

//                if (dialog2.EditLabel != null)
//                {
//                    dialog2.EditLabel.IsVisible = false;
//                }

//                ScrollViewGridDesktop?.Children.Remove(dialog2);
//                base.Children.Remove(dialog2);
//                OnDialogVisibilityChange(isOpening: false);
//            }
//        });
//    }

//    private void CreateMessageDialog()
//    {
//        messageDialog = new MessageDialog();
//        messageDialog.MessageDialogSettings = new MessageDialogThemeSettings();
//        messageDialog.ApplyStyleForButtons();
//        messageDialog.CloseRequested += MessageDialog_CloseRequested;
//    }

//    private void CreatePageNavigationErrorDialog()
//    {
//        pageNavigationErrorMessage = new PageNavigationErrorDialog();
//        pageNavigationErrorMessage.MessageDialogSettings = new MessageDialogThemeSettings();
//        pageNavigationErrorMessage.ApplyStyleForButtons();
//        pageNavigationErrorMessage.CloseRequested += PageNavigationErrorMessageDialog_CloseRequested;
//    }

//    private void CreateListBoxDialog(ListBoxFormField listBoxField)
//    {
//        listBoxFormFieldListDialog = new ListBoxFormFieldListDialog(listBoxField, base.Height);
//        listBoxFormFieldListDialog.CloseRequested += ListBoxItemMobile_CloseRequested;
//    }

//    private void CreateComboBoxListDialog(ComboBoxFormField comboBoxField)
//    {
//        comboBoxFormFieldListDialog = new ComboBoxFormFieldListDialog(comboBoxField, base.Height);
//        comboBoxFormFieldListDialog.CloseRequested += ComboBoxItemMobile_CloseRequested;
//    }

//    internal void SwitchToContinousPageViewMode()
//    {
//        if (m_viewportManager != null && DocumentView != null)
//        {
//            m_viewportManager.IsSinglePageView = false;
//            m_viewportManager.UpdateZoomData(1.0);
//            ZoomFactor = m_viewportManager.ZoomFactor;
//            m_textInfoManager?.ClearSelectionInfo();
//            DocumentView.RemoveTextSelection();
//            AddExistingScrollView();
//            RenderCurrentPage();
//            DocumentView?.WireSelectionEvents();
//            SinglePageViewContainer?.UnwireSelectionAndTextSearchEvents();
//            PreserveSearchedTextInContinousLayout();
//        }
//    }

//    private void AddExistingScrollView()
//    {
//        if (SinglePageViewContainer != null && ScrollView != null && DocumentView != null)
//        {
//            ScrollViewGridDesktop?.Remove(SinglePageViewContainer);
//            ScrollViewGridDesktop?.Children.Add(ScrollView);
//            UnLoadAndReloadScrollView();
//        }
//    }

//    private void UnLoadAndReloadScrollView()
//    {
//        DocumentView?.Unload();
//        Microsoft.Maui.Graphics.Size documentSize = default(Microsoft.Maui.Graphics.Size);
//        if (m_documentManager != null)
//        {
//            documentSize = m_documentManager.GetDocumentSize();
//        }

//        DispatcherUtils.Dispatch(this, delegate
//        {
//            DocumentView?.SizeTo(documentSize.Width, documentSize.Height);
//            ScrollView?.UpdateContentSize(documentSize.Width, documentSize.Height);
//            UpdateExtentDimensions();
//        });
//    }

//    private void RenderCurrentPage()
//    {
//        if (m_viewportManager != null)
//        {
//            if (PageNumber == 0)
//            {
//                GoToPage(PageNumber + 1);
//            }
//            else
//            {
//                GoToPage(PageNumber);
//            }

//            if (ScrollView != null)
//            {
//                ScrollView.ScrollChanged += ScrollView.OnScrollChanged;
//            }
//        }
//    }

//    private void PreserveSearchedTextInContinousLayout()
//    {
//        if (m_textSearchManager == null)
//        {
//            return;
//        }

//        DocumentView?.AssignTextSearchManager(m_textSearchManager);
//        if (m_textSearchManager.SearchResults == null)
//        {
//            return;
//        }

//        DocumentView?.InvokeMatchingTextChanged();
//        for (int i = 0; i < DocumentView?.Children.Count; i++)
//        {
//            if (DocumentView.Children[i] is PdfPageView pdfPageView)
//            {
//                pdfPageView.InvokeContentRendered();
//            }
//        }
//    }

//    private void TriggerEvents()
//    {
//        if (SinglePageViewContainer == null)
//        {
//            return;
//        }

//        for (int i = 0; i < SinglePageViewContainer.Pages.Length; i++)
//        {
//            SinglePageScrollView singlePageScrollView = SinglePageViewContainer.Pages[i];
//            if (singlePageScrollView != null && singlePageScrollView.PresentedContent != null)
//            {
//                singlePageScrollView.PresentedContent.Tapped += OnScrollViewTapped;
//            }
//        }
//    }

//    private void SwitchToSinglePageViewMode()
//    {
//        if (m_isControlLoaded)
//        {
//            if (ScrollView != null && DocumentView != null && SinglePageViewContainer == null)
//            {
//                m_textInfoManager?.ClearSelectionInfo();
//                DocumentView.RemoveTextSelection();
//                AddSinglePageViewContainer();
//                TriggerEvents();
//                InitiateSinglePage();
//                RequestPagesAndRenderContent();
//            }
//            else if (SinglePageViewContainer != null && m_viewportManager != null)
//            {
//                m_viewportManager.IsSinglePageView = true;
//                AddSinglePageViewContainer();
//                RequestPagesAndRenderContent();
//            }

//            DocumentView?.UnwireSelectionAndTextSearchEvents();
//            SinglePageViewContainer?.WireSelectionEvents();
//            PreserveSearchedTextInSinglePageLayout();
//        }
//        else
//        {
//            m_pageLayoutModeRequest = PageLayoutMode.Single;
//        }
//    }

//    private void RequestPagesAndRenderContent()
//    {
//        if (m_viewportManager != null)
//        {
//            if (ScrollView != null)
//            {
//                m_viewportManager.UpdateScrollData(0.0, 0.0, ScrollView.ViewportWidth, ScrollView.ViewportHeight, isIntermediate: false);
//            }
//            else
//            {
//                m_viewportManager.SetScrollDataForSinglePageView();
//            }

//            SinglePageViewContainer?.Intialize(m_viewportManager.CurrentPageNumber);
//        }
//    }

//    private void PreserveSearchedTextInSinglePageLayout()
//    {
//        if (m_textSearchManager == null)
//        {
//            return;
//        }

//        SinglePageViewContainer?.AssignTextSearchManager(m_textSearchManager);
//        if (m_textSearchManager.SearchResults == null)
//        {
//            return;
//        }

//        SinglePageViewContainer?.InvokeMatchingTextChanged();
//        for (int i = 0; i < SinglePageViewContainer?.Pages.Length; i++)
//        {
//            SinglePageScrollView singlePageScrollView = SinglePageViewContainer.Pages[i];
//            if (singlePageScrollView != null && singlePageScrollView.Content is PdfPageView pdfPageView)
//            {
//                pdfPageView.InvokeContentRendered();
//            }
//        }
//    }

//    private void AddScrollHeadForSinglePageLayout()
//    {
//    }

//    private void AddSinglePageViewContainer()
//    {
//        if (ScrollView != null)
//        {
//            ScrollView.ScrollChanged -= ScrollView.OnScrollChanged;
//            ScrollViewGridDesktop?.Children?.Remove(ScrollView);
//            if (SinglePageViewContainer == null)
//            {
//                SinglePageViewContainer = new SinglePageViewContainer(ScrollView.Width, ScrollView.Height, MinZoomFactor, MaxZoomFactor, this);
//                ScrollViewGridDesktop?.Children?.Add(SinglePageViewContainer);
//            }
//            else
//            {
//                ScrollViewGridDesktop?.Children?.Add(SinglePageViewContainer);
//            }

//            WirePanZoomListernerOnTouch();
//        }
//    }

//    private void WirePanZoomListernerOnTouch()
//    {
//        if (SinglePageViewContainer == null)
//        {
//            return;
//        }

//        SinglePageScrollView[] pages = SinglePageViewContainer.Pages;
//        foreach (SinglePageScrollView singlePageScrollView in pages)
//        {
//            if (singlePageScrollView.m_panZoomListener != null)
//            {
//                singlePageScrollView.m_panZoomListener.OnTouch += M_panZoomListener_OnTouch;
//            }
//        }
//    }

//    private void InitiateSinglePage()
//    {
//        if (SinglePageViewContainer != null && DocumentView != null)
//        {
//            SinglePageViewContainer.AssignViewportManager(m_viewportManager);
//            SinglePageViewContainer.AssignDocumentManager(m_documentManager);
//            SinglePageViewContainer?.AssignTextInfoManager(m_textInfoManager);
//            SinglePageViewContainer?.Activate();
//        }
//    }

//    private void ComboBoxItemMobile_CloseRequested(object? sender, EventArgs e)
//    {
//        CloseDialog(comboBoxFormFieldListDialog);
//    }

//    private void MessageDialog_CloseRequested(object? sender, EventArgs e)
//    {
//        CloseDialog(messageDialog);
//    }

//    private void PageNavigationErrorMessageDialog_CloseRequested(object? sender, EventArgs e)
//    {
//        CloseDialog(pageNavigationErrorMessage);
//    }

//    private void ListBoxItemMobile_CloseRequested(object? sender, EventArgs e)
//    {
//        CloseDialog(listBoxFormFieldListDialog);
//    }

//    private SfInteractiveScrollViewExt CreateScrollView()
//    {
//        SfInteractiveScrollViewExt sfInteractiveScrollViewExt = new SfInteractiveScrollViewExt
//        {
//            MinZoomFactor = MinZoomFactor,
//            MaxZoomFactor = MaxZoomFactor
//        };
//        BindingBase binding = new Binding("ScrollY", BindingMode.Default, null, null, null, sfInteractiveScrollViewExt);
//        SetBinding(VerticalOffsetProperty, binding);
//        BindingBase binding2 = new Binding("ScrollX", BindingMode.Default, null, null, null, sfInteractiveScrollViewExt);
//        SetBinding(HorizontalOffsetProperty, binding2);
//        BindingBase binding3 = new Binding("BackgroundColor", BindingMode.Default, null, null, null, this);
//        sfInteractiveScrollViewExt.SetBinding(VisualElement.BackgroundColorProperty, binding3);
//        AbsoluteLayout.SetLayoutBounds((BindableObject)sfInteractiveScrollViewExt, new Rect(0.0, 0.0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
//        return sfInteractiveScrollViewExt;
//    }

//    internal void OnAnnotationModeChanged(AnnotationMode annotationMode)
//    {
//        CloseOverlayToolbar();
//        if (annotationMode == AnnotationMode.None)
//        {
//            if (AnnotationsToolbarViewDesktop != null)
//            {
//                AnnotationsToolbarViewDesktop.ScrollableToolbar.IsVisible = true;
//            }

//            if (CustomStampListView != null)
//            {
//                CustomStampListView.CustomStamp = false;
//            }

//            CloseOverlayToolbar();
//            if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.Items != null)
//            {
//                for (int i = 0; i < AnnotationsToolbarViewDesktop.Items.Count; i++)
//                {
//                    ToolbarItem toolbarItem = AnnotationsToolbarViewDesktop.Items[i];
//                    if (toolbarItem != null && toolbarItem.View != null)
//                    {
//                        toolbarItem.View.Background = Colors.Transparent;
//                    }
//                }
//            }
//        }

//        if (annotationMode == AnnotationMode.Square)
//        {
//            RenderToolbarView(RectangleSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (RectangleSecondaryAnnotationToolbarViewDesktop != null && RectangleSecondaryAnnotationToolbarViewDesktop.RectangleButton != null && RectangleSecondaryAnnotationToolbarViewDesktop.RectangleButton.IsEnabled)
//            {
//                RectangleSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                RectangleSecondaryAnnotationToolbarViewDesktop.RectangleButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Circle)
//        {
//            RenderToolbarView(CircleSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (CircleSecondaryAnnotationToolbarViewDesktop != null && CircleSecondaryAnnotationToolbarViewDesktop.CircleButton != null && CircleSecondaryAnnotationToolbarViewDesktop.CircleButton.IsEnabled)
//            {
//                CircleSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                CircleSecondaryAnnotationToolbarViewDesktop.CircleButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Line)
//        {
//            RenderToolbarView(LineSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (LineSecondaryAnnotationToolbarViewDesktop != null && LineSecondaryAnnotationToolbarViewDesktop.LineButton != null && LineSecondaryAnnotationToolbarViewDesktop.LineButton.IsEnabled)
//            {
//                LineSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                LineSecondaryAnnotationToolbarViewDesktop.LineButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Arrow)
//        {
//            RenderToolbarView(ArrowSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (ArrowSecondaryAnnotationToolbarViewDesktop != null && ArrowSecondaryAnnotationToolbarViewDesktop.ArrowButton != null && ArrowSecondaryAnnotationToolbarViewDesktop.ArrowButton.IsEnabled)
//            {
//                ArrowSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                ArrowSecondaryAnnotationToolbarViewDesktop.ArrowButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Ink)
//        {
//            RenderToolbarView(InkSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (InkSecondaryAnnotationToolbarViewDesktop != null && InkSecondaryAnnotationToolbarViewDesktop.InkButton != null && InkSecondaryAnnotationToolbarViewDesktop.InkButton.IsEnabled)
//            {
//                InkSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                InkSecondaryAnnotationToolbarViewDesktop.InkButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.InkEraser)
//        {
//            RenderToolbarView(EraserSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (EraserSecondaryAnnotationToolbarViewDesktop != null && EraserSecondaryAnnotationToolbarViewDesktop.EraserButton != null && EraserSecondaryAnnotationToolbarViewDesktop.EraserButton.IsEnabled)
//            {
//                EraserSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                EraserSecondaryAnnotationToolbarViewDesktop.EraserButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Highlight)
//        {
//            RenderToolbarView(HighlightAnnotationToolbarViewDesktop, IsVisible: true);
//            if (HighlightAnnotationToolbarViewDesktop != null && HighlightAnnotationToolbarViewDesktop.HighlightButton != null && HighlightAnnotationToolbarViewDesktop.HighlightButton.IsEnabled)
//            {
//                HighlightAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                HighlightAnnotationToolbarViewDesktop.HighlightButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Underline)
//        {
//            RenderToolbarView(UnderlineAnnotationToolbarViewDesktop, IsVisible: true);
//            if (UnderlineAnnotationToolbarViewDesktop != null && UnderlineAnnotationToolbarViewDesktop.UnderlineButton != null && UnderlineAnnotationToolbarViewDesktop.UnderlineButton.IsEnabled)
//            {
//                UnderlineAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                UnderlineAnnotationToolbarViewDesktop.UnderlineButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.StrikeOut)
//        {
//            RenderToolbarView(StrikeOutAnnotationToolbarViewDesktop, IsVisible: true);
//            if (StrikeOutAnnotationToolbarViewDesktop != null && StrikeOutAnnotationToolbarViewDesktop.StrikeoutButton != null && StrikeOutAnnotationToolbarViewDesktop.StrikeoutButton.IsEnabled)
//            {
//                StrikeOutAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                StrikeOutAnnotationToolbarViewDesktop.StrikeoutButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Squiggly)
//        {
//            RenderToolbarView(SquigglyAnnotationToolbarViewDesktop, IsVisible: true);
//            if (SquigglyAnnotationToolbarViewDesktop != null && SquigglyAnnotationToolbarViewDesktop.SquigglyButton != null && SquigglyAnnotationToolbarViewDesktop.SquigglyButton.IsEnabled)
//            {
//                SquigglyAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                SquigglyAnnotationToolbarViewDesktop.SquigglyButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Polyline)
//        {
//            RenderToolbarView(PolylineSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (PolylineSecondaryAnnotationToolbarViewDesktop != null && PolylineSecondaryAnnotationToolbarViewDesktop.PolylineButton != null && PolylineSecondaryAnnotationToolbarViewDesktop.PolylineButton.IsEnabled)
//            {
//                PolylineSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                PolylineSecondaryAnnotationToolbarViewDesktop.PolylineButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.Polygon)
//        {
//            RenderToolbarView(PolygonSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (PolygonSecondaryAnnotationToolbarViewDesktop != null && PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton != null && PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton.IsEnabled && PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton != null && AnnotationSettings.Polygon.BorderStyle == BorderStyle.Solid)
//            {
//                PolygonSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton.IsVisible = true;
//                PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton.IsVisible = false;
//                PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton.Background = DesktopToolbarIconIsPressedColor;
//            }

//            if (PolygonSecondaryAnnotationToolbarViewDesktop != null && PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton != null && PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton.IsEnabled && PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton != null && AnnotationSettings.Polygon.BorderStyle == BorderStyle.Cloudy)
//            {
//                PolygonSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton.IsVisible = true;
//                PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton.IsVisible = false;
//                PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton.Background = DesktopToolbarIconIsPressedColor;
//            }
//        }

//        if (annotationMode == AnnotationMode.FreeText)
//        {
//            RenderToolbarView(FreeTextSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (FreeTextSecondaryAnnotationToolbarViewDesktop != null && FreeTextSecondaryAnnotationToolbarViewDesktop.FreeTextButton != null && FreeTextSecondaryAnnotationToolbarViewDesktop.FreeTextButton.IsEnabled)
//            {
//                FreeTextSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                FreeTextSecondaryAnnotationToolbarViewDesktop.FreeTextButton.Background = DesktopToolbarIconIsPressedColor;
//            }

//            ShowToast(SfPdfViewerResources.GetLocalizedString("AddFreeTextToastMessage"), LayoutOptions.Center);
//        }

//        if (annotationMode == AnnotationMode.StickyNote)
//        {
//            RenderToolbarView(StickyNoteSecondaryAnnotationToolbarViewDesktop, IsVisible: true);
//            if (StickyNoteSecondaryAnnotationToolbarViewDesktop != null && StickyNoteSecondaryAnnotationToolbarViewDesktop.StickyNoteButton != null && StickyNoteSecondaryAnnotationToolbarViewDesktop.StickyNoteButton.IsEnabled)
//            {
//                StickyNoteSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                StickyNoteSecondaryAnnotationToolbarViewDesktop.StickyNoteButton.Background = DesktopToolbarIconIsPressedColor;
//            }

//            ShowToast(SfPdfViewerResources.GetLocalizedString("AddStickyNoteToastMessage"), LayoutOptions.Center);
//        }

//        if (annotationMode == AnnotationMode.Stamp && StampAnnotationToolbarViewDesktop == null)
//        {
//            StampAnnotationToolbarViewDesktop = new StampAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(StampAnnotationToolbarViewDesktop);
//        }

//        if (annotationMode == AnnotationMode.Signature)
//        {
//            if (AnnotationsToolbarViewDesktop != null)
//            {
//                AnnotationsToolbarViewDesktop.ScrollableToolbar.IsVisible = true;
//            }

//            if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.SignatureButton != null && AnnotationsToolbarViewDesktop.SignatureButton.IsEnabled)
//            {
//                AnnotationsToolbarViewDesktop.SignatureButton.Background = DesktopToolbarIconIsPressedColor;
//            }

//            ShowSignatureDialog();
//        }
//        else
//        {
//            CloseSignatureDialog();
//            if (annotationMode != 0)
//            {
//                SignatureHelper.CurrentSignatureItem = null;
//            }
//        }

//        if (StickyNoteDialog != null && StickyNoteDialog.IsOpen)
//        {
//            CloseDialog(StickyNoteDialog);
//        }

//        CloseComboBoxListView();
//        UnfocusFormFields();
//        if (annotationMode == AnnotationMode.InkEraser)
//        {
//            eraserPointer = new InkEraserView(AnnotationSettings.InkEraser.Thickness);
//            eraserPointer.InputTransparent = true;
//            eraserPointer.IsVisible = false;
//            ScrollViewGridDesktop?.Add(eraserPointer);
//            EnableTextSelection = false;
//            DisableOrEnableSinglePagePanMode(enable: false);
//        }
//        else if (annotationMode != AnnotationMode.InkEraser)
//        {
//            AnnotationsLoader? obj = annotationsLoader;
//            if (obj != null && obj.AnnotationMode == AnnotationMode.InkEraser)
//            {
//                ScrollViewGridDesktop?.Remove(eraserPointer);
//                eraserPointer = null;
//                EnableTextSelection = true;
//                DisableOrEnableSinglePagePanMode(enable: true);
//            }
//        }

//        DeselectAnnotation(SelectedAnnotation);
//        if (annotationsLoader == null)
//        {
//            return;
//        }

//        DispatcherUtils.Dispatch(this, delegate
//        {
//            annotationsLoader.AnnotationMode = annotationMode;
//            if (annotationsLoader.AnnotationMode != 0 && !annotationsLoader.IsTextMarkupAnnotationMode && annotationsLoader.AnnotationMode != AnnotationMode.InkEraser && annotationsLoader.AnnotationMode != AnnotationMode.FreeText && !annotationsLoader.IsInkModeWithStylus && annotationsLoader.AnnotationMode != AnnotationMode.Stamp && annotationsLoader.AnnotationMode != AnnotationMode.StickyNote)
//            {
//                RestrictOrAllowZoom(allow: false);
//            }
//            else
//            {
//                RestrictOrAllowZoom(allow: true);
//                if (AnnotationSettings.Ink.IsStylusModeWithStylusTouch)
//                {
//                    AnnotationSettings.Ink.IsStylusModeWithStylusTouch = false;
//                }
//            }
//        });
//    }

//    internal void ToggleSignatureViewVisibility()
//    {
//        if (signatureViewOverlay == null || signatureViewOverlay.Parent == null)
//        {
//            ShowSignatureListDialog();
//        }
//        else
//        {
//            CloseSignatureDialog();
//        }
//    }

//    internal void OnPageLayoutModeChanged(PageLayoutMode pageViewMode)
//    {
//        switch (pageViewMode)
//        {
//            case PageLayoutMode.Single:
//                SwitchToSinglePageViewMode();
//                break;
//            case PageLayoutMode.Continuous:
//                SwitchToContinousPageViewMode();
//                break;
//        }

//        if (annotationsLoader != null)
//        {
//            annotationsLoader.PageLayoutMode = pageViewMode;
//        }
//    }

//    internal void AdjustZoomForZoomMode(ZoomMode zoomMode)
//    {
//        IsZoomModeChanging = true;
//        Rect currentPageBounds = GetCurrentPageBounds(PageNumber);
//        double num = currentPageBounds.Width / currentPageBounds.Height;
//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            if (ScrollView != null && DocumentView != null)
//            {
//                double num2 = ScrollView.ViewportWidth / ScrollView.ViewportHeight;
//                switch (zoomMode)
//                {
//                    case ZoomMode.FitToPage:
//                        if (num > num2)
//                        {
//                            ZoomFactor = ScrollView.ViewportWidth / currentPageBounds.Width;
//                        }
//                        else
//                        {
//                            ZoomFactor = ScrollView.ViewportHeight / currentPageBounds.Height;
//                        }

//                        GoToPage(PageNumber);
//                        break;
//                    case ZoomMode.FitToWidth:
//                        ZoomFactor = ScrollView.ViewportWidth / DocumentView.Width;
//                        break;
//                }
//            }
//        }
//        else
//        {
//            SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//            if (currentSinglePageView != null)
//            {
//                double num3 = currentSinglePageView.ViewportWidth / currentSinglePageView.ViewportHeight;
//                switch (zoomMode)
//                {
//                    case ZoomMode.FitToPage:
//                        if (num > num3)
//                        {
//                            ZoomFactor = currentSinglePageView.ViewportWidth / currentPageBounds.Width;
//                        }
//                        else
//                        {
//                            ZoomFactor = currentSinglePageView.ViewportHeight / currentPageBounds.Height;
//                        }

//                        break;
//                    case ZoomMode.FitToWidth:
//                        ZoomFactor = currentSinglePageView.ViewportWidth / currentPageBounds.Width;
//                        break;
//                }
//            }
//        }

//        IsZoomModeChanging = false;
//    }

//    internal void OnZoomModeChanged(ZoomMode zoomMode)
//    {
//        AdjustZoomForZoomMode(zoomMode);
//    }

//    private void RestrictOrAllowZoom(bool allow)
//    {
//        if (ScrollView == null)
//        {
//            return;
//        }

//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            ScrollView.AllowZoom = (allow ? true : false);
//            return;
//        }

//        for (int i = 0; i < SinglePageViewContainer?.Pages.Length; i++)
//        {
//            SinglePageScrollView singlePageScrollView = SinglePageViewContainer.Pages[i];
//            if (singlePageScrollView != null)
//            {
//                singlePageScrollView.AllowZoom = (allow ? true : false);
//            }
//        }
//    }

//    internal void SetTextInfoManager()
//    {
//        if (DocumentView != null)
//        {
//            DocumentView?.SetTextInfoManager();
//        }
//    }

//    internal void RefreshPageCommandCanExecutes()
//    {
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            RefreshCommand(GoToPreviousPageCommand);
//            RefreshCommand(GoToNextPageCommand);
//            RefreshCommand(GoToFirstPageCommand);
//            RefreshCommand(GoToLastPageCommand);
//        });
//    }

//    private void RefreshCommand(ICommand? inputCommand)
//    {
//        if (inputCommand is Command command)
//        {
//            command.ChangeCanExecute();
//        }
//    }

//    private void CreatePasswordDialog()
//    {
//        passwordDialog = new PasswordDialog();
//        passwordDialog.PasswordDialogSettings = new PasswordDialogThemeSettings();
//        passwordDialog.ApplyStyleForButtons();
//        passwordDialog.PasswordSubmitted += PasswordDialog_OnSubmitted;
//        passwordDialog.CloseRequested += PasswordDialog_CloseRequested;
//        if (m_documentManager != null)
//        {
//            m_documentManager.PasswordVerified += OnPasswordVerified;
//        }
//    }

//    private void CreateStickyNoteDialog()
//    {
//        StickyNoteDialog = new StickyNoteDialog();
//        StickyNoteDialog.StickNoteTextSubmitted += StickyNoteDialog_StickNoteTextSubmitted;
//        StickyNoteDialog.CloseRequested += StickyNoteDialog_CloseRequested;
//    }

//    private void CreateFreeTextDialog(FreeTextAnnotation freeText)
//    {
//        freeTextDialog = new FreeTextDialog(freeText);
//        freeTextDialog.CancelClicked += FreeTextDialog_CancelClicked;
//        freeTextDialog.OkClicked += FreeTextDialog_OkClicked;
//    }

//    private void FreeTextDialog_OkClicked(object? sender, string e)
//    {
//        if (freeText != null && DocumentView != null)
//        {
//            freeText.Text = e;
//            freeText.IsNewlyAdded = false;
//        }

//        OnFreeTextModalViewDisappearing(EventArgs.Empty);
//        CloseDialog(freeTextDialog);
//        if (AnnotationMode == AnnotationMode.FreeText)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }
//    }

//    private void FreeTextDialog_CancelClicked(object? sender, EventArgs e)
//    {
//        if (freeText != null)
//        {
//            freeText.IsNewlyAdded = false;
//        }

//        OnFreeTextModalViewDisappearing(EventArgs.Empty);
//        CloseDialog(freeTextDialog);
//        if (AnnotationMode == AnnotationMode.FreeText)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }
//    }

//    private void StickyNoteDialog_StickNoteTextSubmitted(object? sender, EventArgs e)
//    {
//        StickyNoteDialog?.Reset();
//        CloseDialog(StickyNoteDialog);
//        if (AnnotationMode == AnnotationMode.StickyNote)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }
//    }

//    private void StickyNoteDialog_CloseRequested(object? sender, EventArgs e)
//    {
//        CloseDialog(StickyNoteDialog);
//        DeselectAnnotation(SelectedAnnotation);
//        if (AnnotationMode == AnnotationMode.StickyNote)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }
//    }

//    private void CreateHyperlinkDialog()
//    {
//        hyperlinkDialog = new HyperlinkDialog();
//        hyperlinkDialog.HyperlinkSettings = new HyperlinkThemeSettings();
//        hyperlinkDialog.ApplyStyleForButtons();
//        hyperlinkDialog.CloseRequested += HyperlinkDialog_CloseRequested;
//    }

//    private void HyperlinkDialog_CloseRequested(object? sender, EventArgs e)
//    {
//        CloseDialog(hyperlinkDialog);
//    }

//    private void ShowMessageDialog(string? title, string? description)
//    {
//        if (messageDialog == null)
//        {
//            CreateMessageDialog();
//        }

//        messageDialog?.SetContent(title, description);
//        ShowDialog(messageDialog);
//    }

//    internal void ShowPageNagivationErrorDialog(string? title, string? description)
//    {
//        if (messageDialog == null)
//        {
//            CreatePageNavigationErrorDialog();
//        }

//        pageNavigationErrorMessage?.SetContent(title, description);
//        ShowDialog(pageNavigationErrorMessage);
//    }

//    private void ShowStickyNoteDialog(Annotation annotation, bool isHovered = false, bool isSelected = false)
//    {
//        if (!(annotation is StickyNoteAnnotation stickyNoteAnnotation) || stickyNoteAnnotation.IsLocked)
//        {
//            return;
//        }

//        if (StickyNoteDialog == null)
//        {
//            CreateStickyNoteDialog();
//        }

//        StickyNoteDialog?.SetAnnotationAndStickyText(stickyNoteAnnotation);
//        if (StickyNoteDialog?.StickyNoteEditor != null && StickyNoteDialog.CloseLabel != null && StickyNoteDialog.SaveLabel != null && StickyNoteDialog.EditLabel != null)
//        {
//            if (isHovered)
//            {
//                StickyNoteDialog.StickyNoteEditor.IsEnabled = false;
//                StickyNoteDialog.CloseLabel.IsVisible = false;
//                StickyNoteDialog.SaveLabel.IsVisible = false;
//                StickyNoteDialog.EditLabel.IsVisible = false;
//            }
//            else
//            {
//                StickyNoteDialog.StickyNoteEditor.IsEnabled = true;
//                StickyNoteDialog.CloseLabel.IsVisible = true;
//                StickyNoteDialog.SaveLabel.IsVisible = true;
//                StickyNoteDialog.EditLabel.IsVisible = false;
//            }

//            if (isSelected)
//            {
//                StickyNoteDialog.StickyNoteEditor.IsEnabled = false;
//                StickyNoteDialog.CloseLabel.IsVisible = true;
//                StickyNoteDialog.SaveLabel.IsVisible = false;
//                StickyNoteDialog.EditLabel.IsVisible = true;
//            }
//        }

//        UpdateStickyNoteDialogPosition(stickyNoteAnnotation);
//        ShowDialog(StickyNoteDialog);
//        DialogBase.IsAnyDialogOpen = false;
//    }

//    internal void PositionStickyNoteOnZoom()
//    {
//        if (hoveredStickyNote != null)
//        {
//            UpdateStickyNoteDialogPosition(hoveredStickyNote);
//        }
//    }

//    private void UpdateStickyNoteDialogPosition(StickyNoteAnnotation stickyNoteAnnotation)
//    {
//        if (ScrollView != null && m_documentManager != null && DocumentView != null && m_viewportManager != null)
//        {
//            Microsoft.Maui.Graphics.Point point = ConversionUtil.ConvertPagePointToClientPoint(new Microsoft.Maui.Graphics.Point(stickyNoteAnnotation.Bounds.X, stickyNoteAnnotation.Bounds.Y), stickyNoteAnnotation.PageNumber, ScrollView.ContentSize, DocumentView.Height, m_viewportManager, m_documentManager.PageSizeMultiplier);
//            double width = 254.0;
//            double height = 180.0;
//            double pageSizeMultiplier = m_documentManager.PageSizeMultiplier;
//            double x = (double)stickyNoteAnnotation.Bounds.X * pageSizeMultiplier;
//            double y = (double)stickyNoteAnnotation.Bounds.Y * pageSizeMultiplier;
//            double width2 = (double)stickyNoteAnnotation.Bounds.Width * pageSizeMultiplier;
//            double height2 = (double)stickyNoteAnnotation.Bounds.Height * pageSizeMultiplier;
//            Rect rect = new Rect(x, y, width2, height2);
//            double num = (rect.Width * ZoomFactor - rect.Width) / 2.0 - (double)(AnnotationConstants.SelectionBorderMargin / 2);
//            double num2 = (rect.Height * ZoomFactor - rect.Height) / 2.0 + (double)AnnotationConstants.SelectionBorderThickness;
//            AbsoluteLayout.SetLayoutBounds(bounds: new Rect(new Microsoft.Maui.Graphics.Point(point.X + num, point.Y + rect.Height + num2), new Microsoft.Maui.Graphics.Size(width, height)), bindable: StickyNoteDialog);
//        }
//    }

//    private void ShowFreeTextDialog(Annotation annotation)
//    {
//        if (annotation is FreeTextAnnotation freeTextAnnotation && !freeTextAnnotation.IsLocked)
//        {
//            freeText = freeTextAnnotation;
//            if (freeTextDialog == null)
//            {
//                CreateFreeTextDialog(freeText);
//            }

//            if (freeTextDialog != null && freeTextDialog.FreeTextEditor != null)
//            {
//                freeTextDialog.FreeTextEditor.Text = freeTextAnnotation.Text;
//                ShowDialog(freeTextDialog);
//            }
//        }
//    }

//    internal void ShowListBoxDialog(ListBoxFormField listBoxField)
//    {
//        CreateListBoxDialog(listBoxField);
//        ShowDialog(listBoxFormFieldListDialog);
//    }

//    internal void ShowComboBoxlistDialog(ComboBoxFormField comboBoxField)
//    {
//        CreateComboBoxListDialog(comboBoxField);
//        ShowDialog(comboBoxFormFieldListDialog);
//    }

//    internal void ShowSignatureListDialog()
//    {
//        CheckSignatureViewOverlayReady();
//        if (signatureViewOverlay != null)
//        {
//            if (signatureViewOverlay.Parent == null)
//            {
//                base.Children.Add(signatureViewOverlay);
//            }

//            signatureViewOverlay.ShowSignatureListDialog();
//        }
//    }

//    internal void ShowSignatureDialog(SignatureFormField? signatureField = null)
//    {
//        CheckSignatureViewOverlayReady();
//        if (signatureViewOverlay == null)
//        {
//            return;
//        }

//        signatureViewOverlay.SetSignatureFieldToSign(signatureField);
//        if (!signatureViewOverlay.ShowSignaturePad())
//        {
//            if (signatureViewOverlay.Parent == null)
//            {
//                base.Children.Add(signatureViewOverlay);
//            }

//            this.RemoveTouchListener(this);
//        }
//        else
//        {
//            AnnotationMode = AnnotationMode.None;
//        }
//    }

//    private void SignatureViewOverlay_SignatureDialogModalViewAppearning(object? sender, FormFieldModalViewAppearingEventArgs e)
//    {
//        OnSignatureModalViewAppearing(e);
//    }

//    internal void SetSignatureListDialogMargin(double marginLeftValue)
//    {
//        signatureViewOverlay?.SetSignatureListDialogMargin(marginLeftValue);
//    }

//    private void SignatureViewOverlay_SignatureCreated(object? sender, SignatureCreationCompletedEventArgs e)
//    {
//        this.SignatureCreated?.Invoke(this, new SignatureCreatedEventArgs(e.Signature));
//        if (e.CanShowToast)
//        {
//            ShowToast(SfPdfViewerResources.GetLocalizedString("AddSignatureToastMessage") ?? "Tap to add the signature", LayoutOptions.Center);
//        }
//    }

//    private void SignatureViewOverlay_CloseRequested(object? sender, EventArgs e)
//    {
//        if (AnnotationMode == AnnotationMode.Signature)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }

//        OnSignatureModalViewDisappearing(EventArgs.Empty);
//        CloseSignatureDialog();
//    }

//    internal void CloseSignatureDialog()
//    {
//        signatureViewOverlay?.Hide();
//        this.AddTouchListener(this);
//    }

//    private void ShowPasswordDialog()
//    {
//        if (passwordDialog == null)
//        {
//            CreatePasswordDialog();
//        }

//        ShowDialog(passwordDialog);
//    }

//    private void ShowHyperlinkDialog(string? title, string? uri)
//    {
//        string description = "\n" + uri + "?";
//        if (hyperlinkDialog == null)
//        {
//            CreateHyperlinkDialog();
//        }

//        if (hyperlinkDialog != null)
//        {
//            hyperlinkDialog.Uri = uri;
//        }

//        hyperlinkDialog?.SetContent(title, description);
//        ShowDialog(hyperlinkDialog);
//    }

//    private void ClosePasswordDialog()
//    {
//        CloseDialog(passwordDialog);
//        UnloadPasswordEventArgs();
//    }

//    private void AddLoadingIndicator()
//    {
//        LoadingIndicator = new BusyIndicator();
//        base.Children.Add(LoadingIndicator);
//    }

//    private void AddScrollView()
//    {
//        ScrollView = CreateScrollView();
//        ScrollView.Loaded += OnScrollViewLoaded;
//        if (ScrollView.PresentedContent != null)
//        {
//            ScrollView.PresentedContent.Tapped += OnScrollViewTapped;
//        }

//        ScrollView.ZoomChangingCompleted += OnZoomChangingCompleted;
//        ScrollView.PropertyChanged += OnScrollViewPropertyChanged;
//        if (ScrollView.m_panZoomListener != null)
//        {
//            ScrollView.m_panZoomListener.OnTouch += M_panZoomListener_OnTouch;
//        }
//    }

//    private async void OnCustomStampCreated(object? sender, CustomStampEventArgs? e)
//    {
//        Label customStampLabel = e?.CustomStampLabel ?? new Label();
//        Stream memoryStream = new MemoryStream();
//        StampImage image = new StampImage();
//        RoundRectangle roundRectangle = new RoundRectangle
//        {
//            CornerRadius = 0.0
//        };
//        if (e?.StampView != null)
//        {
//            e.StampView.StrokeShape = roundRectangle;
//        }

//        IScreenshotResult imageStream = await (e?.StampView?.CaptureAsync());
//        await imageStream.CopyToAsync(memoryStream);
//        memoryStream.Position = 0L;
//        image.ImageStream = memoryStream;
//        Stream streamFromScreenshot = await imageStream.OpenReadAsync();
//        image.Source = ImageSource.FromStream(() => streamFromScreenshot);
//        image.HeightRequest = 50.0;
//        image.Aspect = Aspect.AspectFit;
//        image.HorizontalOptions = LayoutOptions.Start;
//        image.Margin = new Thickness(5.0);
//        CreatedCustomStamp = new StandardStamps
//        {
//            BorderBackground = e?.StampView?.BackgroundColor,
//            LabelText = customStampLabel.Text,
//            LabelTextColor = customStampLabel.TextColor,
//            BorderHeight = 30.0,
//            BorderColor = customStampLabel.TextColor,
//            BorderWidth = e?.StampView?.Width,
//            StampImageStream = streamFromScreenshot
//        };
//        StampViewDialogMobile?.StampsData?.CustomStampItems.Add(CreatedCustomStamp);
//        if (StampViewDialogMobile != null)
//        {
//            StampViewDialogMobile.CustomStamp = true;
//        }

//        CustomStampListView?.StampsData?.CustomStampItems.Add(CreatedCustomStamp);
//        if (CustomStampListView != null)
//        {
//            CustomStampListView.CustomStamp = true;
//        }

//        if (sender is View view)
//        {
//            OnCustomStampModalViewDisappearing(EventArgs.Empty);
//            view.IsVisible = false;
//        }

//        AnnotationMode = AnnotationMode.Stamp;
//        ShowToast(SfPdfViewerResources.GetLocalizedString("AddStampToastMessage") ?? "Tap to add the stamp", LayoutOptions.Center);
//    }

//    private void OnScrollViewTapped(object? sender, TapEventArgs e)
//    {
//        Microsoft.Maui.Graphics.PointF pointF = e.TapPoint;
//        GestureEventArgs gestureEventArgs = new GestureEventArgs();
//        if (HorizontalOffset.HasValue && VerticalOffset.HasValue)
//        {
//            double num = HorizontalOffset.Value;
//            double num2 = VerticalOffset.Value;
//            Microsoft.Maui.Graphics.Size size = default(Microsoft.Maui.Graphics.Size);
//            if (PageLayoutMode == PageLayoutMode.Single)
//            {
//                SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//                if (currentSinglePageView != null && currentSinglePageView.ExtentSize.HasValue && currentSinglePageView.ViewportManager != null && m_documentManager != null)
//                {
//                    size = currentSinglePageView.ExtentSize.Value;
//                    if (ScrollViewGridMobile != null && ScrollViewGridMobile.Bounds.Height > size.Height)
//                    {
//                        double num3 = (ScrollViewGridMobile.Bounds.Height - size.Height) / 2.0;
//                        pointF.Y = Math.Abs(pointF.Y - (float)num3);
//                    }

//                    num = currentSinglePageView.ViewportManager.HorizontalOffset;
//                    num2 = currentSinglePageView.ViewportManager.VerticalOffset;
//                }

//                if (num2 > size.Height)
//                {
//                    num2 = 0.0;
//                }
//            }

//            Microsoft.Maui.Graphics.Point position = new Microsoft.Maui.Graphics.Point((double)pointF.X - num, (double)pointF.Y - num2);
//            gestureEventArgs.Position = position;
//        }

//        Microsoft.Maui.Graphics.Point clientPagePoint;
//        int pageIndexFromScrollPoint = GetPageIndexFromScrollPoint(pointF, out clientPagePoint);
//        if (pageIndexFromScrollPoint != -1 && m_documentManager != null)
//        {
//            MAUIToolkit.Core.Drawing.PointF pagePosition = new MAUIToolkit.Core.Drawing.PointF((float)(clientPagePoint.X / m_documentManager.PageSizeMultiplier), (float)(clientPagePoint.Y / m_documentManager.PageSizeMultiplier));
//            gestureEventArgs.PagePosition = pagePosition;
//            gestureEventArgs.PageNumber = pageIndexFromScrollPoint + 1;
//        }

//        if (this.Tapped != null)
//        {
//            this.Tapped(this, gestureEventArgs);
//        }

//        if (AnnotationMode == AnnotationMode.FreeText)
//        {
//            FreeTextAnnotation freeTextAnnotation = new FreeTextAnnotation("", gestureEventArgs.PageNumber, new RectF(gestureEventArgs.PagePosition.X, gestureEventArgs.PagePosition.Y, 100f, 28f));
//            freeTextAnnotation.IsNewlyAdded = true;
//            AddAnnotation(freeTextAnnotation);
//        }

//        if (AnnotationMode == AnnotationMode.StickyNote)
//        {
//            StickyNoteAnnotation stickyNoteAnnotation = new StickyNoteAnnotation(AnnotationSettings.StickyNote.Icon, "", gestureEventArgs.PageNumber, new Microsoft.Maui.Graphics.Point(gestureEventArgs.PagePosition.X, gestureEventArgs.PagePosition.Y));
//            stickyNoteAnnotation.Author = AnnotationSettings.Author;
//            AddAnnotation(stickyNoteAnnotation);
//            if (ColorToolbarViewMobile?.FixedToolbarView != null)
//            {
//                ColorToolbarViewMobile.FixedToolbarView.IsVisible = false;
//                ColorToolbarViewMobile.IsStrokeColorToolbarVisible = false;
//                ColorToolbarViewMobile.IsFillColorToolbarVisible = false;
//            }

//            if (SliderToolbarViewMobile?.FixedToolbarView != null)
//            {
//                SliderToolbarViewMobile.FixedToolbarView.IsVisible = false;
//                SliderToolbarViewMobile.IsThicknessToolbarVisible = false;
//                SliderToolbarViewMobile.IsFillOpacityToolbarVisible = false;
//            }

//            if (StickyNoteIconsViewMobile?.ScrollableToolbar != null)
//            {
//                StickyNoteIconsViewMobile.ScrollableToolbar.IsVisible = false;
//            }
//        }

//        if (AnnotationMode == AnnotationMode.Stamp)
//        {
//            Microsoft.Maui.Graphics.PointF position2 = new Microsoft.Maui.Graphics.PointF(gestureEventArgs.PagePosition.X, gestureEventArgs.PagePosition.Y);
//            StampListView? standardStampListView = StandardStampListView;
//            if (standardStampListView != null)
//            {
//                _ = standardStampListView.StampType;
//                if (true && CustomStampListView != null && !CustomStampListView.CustomStamp)
//                {
//                    StampType stampType = StandardStampListView.StampType;
//                    StampAnnotation stampAnnotation = new StampAnnotation(stampType, gestureEventArgs.PageNumber, position2);
//                    RectF bounds = new RectF(stampAnnotation.Bounds.X - stampAnnotation.Bounds.Width / 2f, stampAnnotation.Bounds.Y - stampAnnotation.Bounds.Height / 2f, stampAnnotation.Bounds.Width, stampAnnotation.Bounds.Height);
//                    stampAnnotation.Bounds = bounds;
//                    AddAnnotation(stampAnnotation);
//                    goto IL_06dd;
//                }
//            }

//            if (CustomStampListView != null && (CustomStampListView.CustomStampData?.StampImageStream != null || CreatedCustomStamp != null) && CustomStampListView.CustomStamp)
//            {
//                if (CustomStampListView.CustomStampData != null)
//                {
//                    CreatedCustomStamp = CustomStampListView.CustomStampData;
//                    CustomStampListView.CustomStampData = null;
//                }

//                StampAnnotation annotation = new StampAnnotation(CreatedCustomStamp?.StampImageStream, gestureEventArgs.PageNumber, new RectF((float)((double)position2.X - CreatedCustomStamp.BorderWidth / 2.0).Value, (float)((double)position2.Y - (CreatedCustomStamp.BorderHeight + 18.0) / 2.0).Value, (float)CreatedCustomStamp.BorderWidth.Value, (float)CreatedCustomStamp.BorderHeight.Value + 18f));
//                AddAnnotation(annotation);
//                CustomStampListView.CustomStamp = false;
//                CreatedCustomStamp = null;
//            }

//            goto IL_06dd;
//        }

//        goto IL_06e6;
//    IL_06e6:
//        if (StampMoreOptionLayoutViewDesktop != null && StampMoreOptionLayoutViewDesktop.IsVisible)
//        {
//            StampMoreOptionLayoutViewDesktop.IsVisible = false;
//        }

//        if (StandardStampListView != null && StandardStampListView.IsVisible)
//        {
//            StandardStampListView.IsVisible = false;
//        }

//        if (CustomStampListView != null && CustomStampListView.IsVisible)
//        {
//            CustomStampListView.IsVisible = false;
//        }

//        CloseOverlayToolbar();
//        return;
//    IL_06dd:
//        AnnotationMode = AnnotationMode.None;
//        goto IL_06e6;
//    }

//    internal SinglePageScrollView? GetCurrentSinglePageView(int pageNumber)
//    {
//        SinglePageScrollView result = null;
//        if (PageLayoutMode == PageLayoutMode.Single)
//        {
//            for (int i = 0; i < SinglePageViewContainer?.Pages.Length; i++)
//            {
//                SinglePageScrollView singlePageScrollView = SinglePageViewContainer.Pages[i];
//                if (singlePageScrollView != null && singlePageScrollView.Content is PdfPageView pdfPageView && pdfPageView.PageNumber == pageNumber)
//                {
//                    return singlePageScrollView;
//                }
//            }
//        }

//        return result;
//    }

//    private int GetPageIndexFromScrollPoint(Microsoft.Maui.Graphics.Point scrollPoint, out Microsoft.Maui.Graphics.Point clientPagePoint)
//    {
//        clientPagePoint = new Microsoft.Maui.Graphics.Point(-1.0, -1.0);
//        if (m_viewportManager != null)
//        {
//            Microsoft.Maui.Graphics.Point pt = scrollPoint;
//            if (m_viewportManager.PagesBoundsInfo != null && m_viewportManager.CurrentViewportInfo != null && m_viewportManager.CurrentViewportInfo.Count > 0)
//            {
//                if (PageLayoutMode == PageLayoutMode.Single)
//                {
//                    Microsoft.Maui.Graphics.Point tappedPointForCurrentSinglePageView = GetTappedPointForCurrentSinglePageView(scrollPoint, clientPagePoint);
//                    clientPagePoint = tappedPointForCurrentSinglePageView;
//                    return PageNumber - 1;
//                }

//                if (m_viewportManager.StartingPageNumber > 0)
//                {
//                    int num = m_viewportManager.StartingPageNumber - 1;
//                    for (int i = num; i < num + m_viewportManager.CurrentViewportInfo.Count; i++)
//                    {
//                        Rect rect = m_viewportManager.PagesBoundsInfo[i];
//                        Microsoft.Maui.Graphics.Size? size = ScrollView?.ContentSize;
//                        if (size.HasValue)
//                        {
//                            rect.Width *= ZoomFactor;
//                            rect.Height *= ZoomFactor;
//                            rect.X = (size.Value.Width - rect.Width) / 2.0;
//                            if (DocumentView != null && ScrollView != null && DocumentView.Height * ZoomFactor < ScrollView.Height)
//                            {
//                                double num2 = (ScrollView.Height - DocumentView.Height * ZoomFactor) / 2.0;
//                                rect.Y += num2;
//                            }
//                            else
//                            {
//                                rect.Y *= ZoomFactor;
//                            }

//                            if (rect.Contains(pt))
//                            {
//                                clientPagePoint.Y = (scrollPoint.Y - rect.Y) / ZoomFactor;
//                                clientPagePoint.X = (scrollPoint.X - rect.X) / ZoomFactor;
//                                return i;
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        return -1;
//    }

//    private Microsoft.Maui.Graphics.Point GetTappedPointForCurrentSinglePageView(Microsoft.Maui.Graphics.Point scrollPoint, Microsoft.Maui.Graphics.Point clientPagePoint)
//    {
//        if (m_viewportManager?.PagesBoundsInfo != null && m_viewportManager.CurrentViewportInfo != null && m_viewportManager.CurrentViewportInfo.Count > 0)
//        {
//            Microsoft.Maui.Graphics.Point pt = scrollPoint;
//            Rect rect = m_viewportManager.PagesBoundsInfo[PageNumber - 1];
//            SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//            Microsoft.Maui.Graphics.Size? size = currentSinglePageView?.ContentSize;
//            rect.Y = 0.0;
//            if (size.HasValue)
//            {
//                rect.Width *= ZoomFactor;
//                rect.Height *= ZoomFactor;
//                rect.X = (size.Value.Width - rect.Width) / 2.0;
//                if (currentSinglePageView != null && rect.Height < currentSinglePageView.Height)
//                {
//                    rect.Y = (currentSinglePageView.Height - rect.Height) / 2.0;
//                }

//                if (rect.Contains(pt))
//                {
//                    clientPagePoint.Y = (scrollPoint.Y - rect.Y) / ZoomFactor;
//                    clientPagePoint.X = (scrollPoint.X - rect.X) / ZoomFactor;
//                    return clientPagePoint;
//                }
//            }
//        }

//        return new Microsoft.Maui.Graphics.Point(-1.0, -1.0);
//    }

//    private void InitializeCommands()
//    {
//        GoToPageCommand = new Command(OnGoToPageCommand);
//        GoToPreviousPageCommand = new Command(GoToPreviousPage, () => m_isDocumentLoaded && PageNumber > 1);
//        GoToNextPageCommand = new Command(GoToNextPage, () => m_isDocumentLoaded && PageNumber < PageCount);
//        GoToFirstPageCommand = new Command(GoToFirstPage, () => m_isDocumentLoaded && PageNumber > 1);
//        GoToLastPageCommand = new Command(GoToLastPage, () => m_isDocumentLoaded && PageNumber < PageCount);
//        PrintDocumentCommand = new Command(PrintDocument);
//        ScrollToOffsetCommand = new Command(OnScrollToOffsetCommand);
//        LoadDocumentCommand = new Command(OnLoadDocumentCommand);
//        SaveDocumentCommand = new Command(OnSaveDocumentCommand);
//        SearchTextCommand = new Command(OnSearchTextCommand);
//        UndoCommand = new Command(Undo, () => changeTracker != null && changeTracker.CanUndo);
//        RedoCommand = new Command(Redo, () => changeTracker != null && changeTracker.CanRedo);
//        RemoveAllAnnotationsCommand = new Command(RemoveAllAnnotations, () => Annotations.Count > 0);
//    }

//    private void UnloadPasswordEventArgs()
//    {
//        if (m_passwordRequestedEventArgs != null)
//        {
//            m_passwordRequestedEventArgs.Unload();
//            m_passwordRequestedEventArgs = null;
//        }
//    }

//    private void NotifyDocumentLoadingFailed(Exception? exception, string? message)
//    {
//        DocumentLoadFailedEventArgs documentLoadFailedEventArgs = new DocumentLoadFailedEventArgs();
//        documentLoadFailedEventArgs.SetData(exception, message);
//        this.DocumentLoadFailed?.Invoke(this, documentLoadFailedEventArgs);
//    }

//    private void OnGoToPageCommand(object destinationPage)
//    {
//        if (int.TryParse(destinationPage.ToString(), out var result))
//        {
//            GoToPage(result);
//        }
//    }

//    private void OnScrollToOffsetCommand(object parameter)
//    {
//        if (parameter is double[] array && array.Length == 2)
//        {
//            double horizontalOffset = array[0];
//            double verticalOffset = array[1];
//            ScrollToOffset(horizontalOffset, verticalOffset);
//        }
//    }

//    private void OnSaveDocumentCommand(object parameter)
//    {
//        if (parameter != null && parameter is Stream outputStream)
//        {
//            SaveDocument(outputStream);
//        }
//    }

//    private void OnLoadDocumentCommand(object parameter)
//    {
//        Stream stream = null;
//        string text = null;
//        FlattenOptions? flattenOptions = null;
//        if (parameter != null && parameter is object[])
//        {
//            object[] array = (object[])parameter;
//            foreach (object obj in array)
//            {
//                if (obj is Stream)
//                {
//                    stream = (Stream)obj;
//                }
//                else if (obj is string)
//                {
//                    text = (string)obj;
//                }
//                else if (obj is FlattenOptions)
//                {
//                    flattenOptions = (FlattenOptions)obj;
//                }
//            }
//        }
//        else if (parameter is Stream)
//        {
//            stream = (Stream)parameter;
//        }

//        if (stream != null && text == null && !flattenOptions.HasValue)
//        {
//            LoadDocument(stream, null, FlattenOptions.None);
//        }
//        else if (stream != null && text != null && flattenOptions.HasValue)
//        {
//            LoadDocument(stream, text, flattenOptions);
//        }
//        else if (stream != null && text != null && !flattenOptions.HasValue)
//        {
//            LoadDocument(stream, text, FlattenOptions.None);
//        }
//        else if (stream != null && text == null && flattenOptions.HasValue)
//        {
//            LoadDocument(stream, null, flattenOptions);
//        }
//    }

//    private async void OnSearchTextCommand(object text)
//    {
//        string searchText = text as string;
//        if (searchText != null && !string.IsNullOrEmpty(searchText))
//        {
//            await SearchTextAsync(searchText);
//        }
//    }

//    private void UnloadDocumentResources()
//    {
//        m_viewportManager?.Unload();
//        annotationsLoader?.Unload();
//        m_documentManager?.UnloadCurrentDocument();
//    }

//    private void CheckViewportManagerReady()
//    {
//        if (m_viewportManager == null)
//        {
//            m_viewportManager = new ViewportManager();
//            m_viewportManager.PropertyChanged += OnViewportPropertyChanged;
//            m_viewportManager.ScrollChanged += M_viewportManager_ScrollChanged;
//            ScrollView?.AssignViewportManager(m_viewportManager);
//        }
//    }

//    private void M_viewportManager_ScrollChanged(object? sender, EventArgs? e)
//    {
//        if (StickyNoteDialog != null && StickyNoteDialog.IsOpen && StickyNoteDialog.StickyNoteEditor != null && StickyNoteDialog.StickyNoteEditor.IsEnabled && hoveredStickyNote != null && ScrollView != null && m_documentManager != null && DocumentView != null && m_viewportManager != null)
//        {
//            Microsoft.Maui.Graphics.Point point = ConversionUtil.ConvertPagePointToClientPoint(new Microsoft.Maui.Graphics.Point(hoveredStickyNote.Bounds.X, hoveredStickyNote.Bounds.Y), hoveredStickyNote.PageNumber, ScrollView.ContentSize, DocumentView.Height, m_viewportManager, m_documentManager.PageSizeMultiplier);
//            double width = 254.0;
//            double height = 180.0;
//            double pageSizeMultiplier = m_documentManager.PageSizeMultiplier;
//            double x = (double)hoveredStickyNote.Bounds.X * pageSizeMultiplier;
//            double y = (double)hoveredStickyNote.Bounds.Y * pageSizeMultiplier;
//            double width2 = (double)hoveredStickyNote.Bounds.Width * pageSizeMultiplier;
//            double height2 = (double)hoveredStickyNote.Bounds.Height * pageSizeMultiplier;
//            Rect rect = new Rect(x, y, width2, height2);
//            double num = (rect.Width * ZoomFactor - rect.Width) / 2.0 - (double)(AnnotationConstants.SelectionBorderMargin / 2);
//            double num2 = (rect.Height * ZoomFactor - rect.Height) / 2.0 + (double)AnnotationConstants.SelectionBorderThickness;
//            double num3 = point.X + num;
//            double num4 = point.Y + rect.Height + num2;
//            if (num3 < 0.0 || num4 < 0.0 || num3 > base.Width || num4 > base.Height)
//            {
//                hoveredStickyNote.Unfocus();
//                DeselectAnnotation(hoveredStickyNote);
//                CloseDialog(StickyNoteDialog);
//            }
//            else
//            {
//                AbsoluteLayout.SetLayoutBounds(bounds: new Rect(new Microsoft.Maui.Graphics.Point(num3, num4), new Microsoft.Maui.Graphics.Size(width, height)), bindable: StickyNoteDialog);
//            }
//        }
//    }

//    private void CheckDocumentManagerReady()
//    {
//        if (m_documentManager == null)
//        {
//            m_documentManager = new PdfDocumentManager();
//            m_documentManager.PropertyChanged += OnDocumentPropertyChanged;
//            m_documentManager.DocumentLoadingCompleted += OnDocumentLoadingCompleted;
//            m_documentManager.DocumentLoadingFailed += OnDocumentLoadingFailed;
//            m_documentManager.PasswordRequested += OnPasswordRequested;
//        }
//    }

//    private void CheckAnnotationsLoaderReady()
//    {
//        if (annotationsLoader == null)
//        {
//            annotationsLoader = new AnnotationsLoader();
//            annotationsLoader.EnableDocumentLinkNavigation = EnableDocumentLinkNavigation;
//            annotationsLoader.EnableHyperlinkNavigation = EnableHyperlinkNavigation;
//            annotationsLoader.PanModeChangeRequested += AnnotationsLoader_PanModeChangeRequested;
//            annotationsLoader.AnnotationSelected += AnnotationsLoader_AnnotationSelected;
//            annotationsLoader.AnnotationAdded += AnnotationsLoader_AnnotationAdded;
//            annotationsLoader.AnnotationPropertyChanged += AnnotationsLoader_AnnotationPropertyChanged;
//            annotationsLoader.FormFieldPropertyChanged += AnnotationsLoader_FormFieldPropertyChanged;
//            annotationsLoader.InkErased += AnnotationsLoader_InkErased;
//            annotationsLoader.AnnotationPropertyChanging += AnnotationsLoader_AnnotationPropertyChanging;
//            annotationsLoader.FormFieldFocusChanged += AnnotationsLoader_FormFieldFocusChanged;
//            annotationsLoader.AnnotationDeselected += AnnotationsLoader_AnnotationDeselected;
//            annotationsLoader.PageTapped += AnnotationsLoader_PageTapped;
//            annotationsLoader.PageTappedToAddSignature += AnnotationsLoader_PageTappedToAddSignature;
//            annotationsLoader.InkSessionEnded += AnnotationsLoader_InkSessionEnded;
//            annotationsLoader.Settings = AnnotationSettings;
//            CheckDocumentManagerReady();
//            annotationsLoader.SetDocumentManager(m_documentManager);
//            annotationsLoader.LinkTapped += AnnotationsLoader_LinkTapped;
//            annotationsLoader.FormFieldTapped += AnnotationsLoader_FormFieldTapped;
//            annotationsLoader.StickyNoteDoubleTapped += AnnotationsLoader_StickyNoteDoubleTapped;
//            annotationsLoader.StickyNoteSelected += AnnotationsLoader_StickyNoteSelected;
//            annotationsLoader.StickyNotePointerEntered += AnnotationsLoader_StickyNotePointerEntered;
//            annotationsLoader.StickyNotePointerExited += AnnotationsLoader_StickyNotePointerExited;
//            annotationsLoader.AnnotationContainerTapped += AnnotationsLoader_AnnotationContainerTapped;
//            annotationsLoader.BackupAnnotation = BackupAnnotations;
//        }
//    }

//    private void AnnotationsLoader_AnnotationContainerTapped(object? sender, EventArgs e)
//    {
//        CloseOverlayToolbar();
//        CloseSignatureDialog();
//        if (AnnotationMode == AnnotationMode.Signature)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }

//        if (AnnotationsToolbarViewDesktop != null)
//        {
//            AnnotationsToolbarViewDesktop.ClearButtonHighlights();
//        }
//    }

//    private void AnnotationsLoader_PageTappedToAddSignature(object? sender, AnnotationEventArgs e)
//    {
//        if (e.Annotation != null)
//        {
//            AddAnnotation(e.Annotation);
//        }
//    }

//    private void AnnotationsLoader_StickyNoteSelected(object? sender, AnnotationEventArgs e)
//    {
//        if (e.Annotation != null && AnnotationSettings.CanEdit(e.Annotation))
//        {
//            ShowStickyNoteDialog(e.Annotation, isHovered: false, isSelected: true);
//        }
//    }

//    private void AnnotationsLoader_StickyNotePointerExited(object? sender, AnnotationEventArgs e)
//    {
//        if (StickyNoteDialog != null && (StickyNoteDialog.StickyNoteEditor == null || !StickyNoteDialog.StickyNoteEditor.IsEnabled) && (StickyNoteDialog.EditLabel == null || !StickyNoteDialog.EditLabel.IsVisible))
//        {
//            CloseDialog(StickyNoteDialog);
//            if (e.Annotation is StickyNoteAnnotation stickyNoteAnnotation)
//            {
//                stickyNoteAnnotation.Unfocus();
//            }
//        }
//    }

//    private void AnnotationsLoader_StickyNotePointerEntered(object? sender, AnnotationEventArgs e)
//    {
//        if (e.Annotation != null && hoveredStickyNote == null && e.Annotation is StickyNoteAnnotation stickyNoteAnnotation && !string.IsNullOrEmpty(stickyNoteAnnotation.Text))
//        {
//            ShowStickyNoteDialog(e.Annotation, isHovered: true);
//            stickyNoteAnnotation.Focus();
//            hoveredStickyNote = stickyNoteAnnotation;
//        }
//    }

//    private void AnnotationsLoader_FreeTextAnnotationDoubleTapped(object? sender, AnnotationEventArgs e)
//    {
//        if (e.Annotation != null)
//        {
//            AnnotationModalViewAppearingEventArgs annotationModalViewAppearingEventArgs = new AnnotationModalViewAppearingEventArgs(e.Annotation);
//            OnFreeTextModalViewAppearing(annotationModalViewAppearingEventArgs);
//            if (!annotationModalViewAppearingEventArgs.Cancel)
//            {
//                ShowFreeTextDialog(e.Annotation);
//            }
//        }
//    }

//    private void AnnotationsLoader_FormFieldFocusChanged(object? sender, FormFieldFocusChangedEvenArgs e)
//    {
//        if (e.FormField is TextFormField)
//        {
//            CloseComboBoxListView();
//        }

//        this.FormFieldFocusChanged?.Invoke(this, e);
//    }

//    internal void StickyNoteEditLayout()
//    {
//        if (SelectedAnnotation != null)
//        {
//            ShowStickyNoteDialog(SelectedAnnotation);
//        }
//    }

//    private void AnnotationsLoader_StickyNoteDoubleTapped(object? sender, AnnotationEventArgs e)
//    {
//        if (e.Annotation != null && AnnotationSettings.CanEdit(e.Annotation))
//        {
//            ShowStickyNoteDialog(e.Annotation);
//        }
//    }

//    private void AnnotationsLoader_InkErased(object? sender, InkErasedEventArgs e)
//    {
//        InkErased(sender, e);
//    }

//    private void AnnotationsLoader_AnnotationPropertyChanging(object? sender, AnnotationPropertyChangingEventArgs e)
//    {
//        OnAnnotationPropertyChanging(sender, e);
//    }

//    private void AnnotationsLoader_AnnotationPropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        OnAnnotationPropertyChanged(sender, e);
//    }

//    private void AnnotationsLoader_FormFieldPropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        if (sender is SignatureFormField signatureFormField && e.PropertyName == "Signature")
//        {
//            if (signatureFormField.Signature != null)
//            {
//                if (signatureFormField.Signature is Annotation annotation && !Annotations.Contains(annotation))
//                {
//                    AddAnnotation(annotation);
//                }
//            }
//            else if (signatureFormField.AssociatedSignature != null && signatureFormField.AssociatedSignature is Annotation annotation2 && signatureFormField.FormWidgets[0].Bounds.IntersectsWith(annotation2.BoundingBox) && Annotations.Contains(annotation2))
//            {
//                RemoveAnnotation(annotation2);
//            }
//        }

//        if (e is FormFieldPropertyChangedEventArgs formFieldPropertyChangedEventArgs && sender is FormField formField)
//        {
//            object oldValue = formFieldPropertyChangedEventArgs.OldValue;
//            object newValue = formFieldPropertyChangedEventArgs.NewValue;
//            if ((changeTracker == null || !changeTracker.IsBulkFormChangeInProgress) && formFieldPropertyChangedEventArgs.PropertyName != "Signature")
//            {
//                CheckChangeTrackerReady();
//                FormFieldChangeRecord formFieldChange = new FormFieldChangeRecord(formField, oldValue, newValue);
//                FormFieldEditCommand command = new FormFieldEditCommand(formFieldChange);
//                changeTracker?.RegisterChange(command);
//            }

//            FormFieldValueChangedEventArgs e2 = new FormFieldValueChangedEventArgs(formField, oldValue, newValue);
//            this.FormFieldValueChanged?.Invoke(this, e2);
//        }

//        modificationMade = true;
//        UpdateUndoRedoButtons();
//    }

//    private void OnAnnotationPropertyChanging(object? sender, AnnotationPropertyChangingEventArgs e)
//    {
//        if (sender is Annotation annotation && (e.PropertyName == "Color" || e.PropertyName == "FillColor" || e.PropertyName == "Opacity" || e.PropertyName == "BorderWidth" || e.PropertyName == "BorderWidth" || e.PropertyName == "Name" || e.PropertyName == "BoundingBox" || e.PropertyName == "IntermediateBounds" || e.PropertyName == "Hidden" || (annotation is LineAnnotation && e.PropertyName == "Points") || (annotation is InkAnnotation && e.PropertyName == "PointsCollection") || annotation is StampAnnotation || (annotation is StickyNoteAnnotation && e.PropertyName == "Position") || (annotation is PolylineAnnotation && e.PropertyName == "Points") || (annotation is PolygonAnnotation && e.PropertyName == "Points") || (annotation is FreeTextAnnotation && e.PropertyName == "BorderColor") || (annotation is FreeTextAnnotation && e.PropertyName == "BorderWidth") || (annotation is FreeTextAnnotation && e.PropertyName == "FontSize")))
//        {
//            e.CancelChange = !AnnotationSettings.CanEdit(annotation);
//        }
//    }

//    private void InkErased(object? sender, InkErasedEventArgs e)
//    {
//        CheckChangeTrackerReady();
//        List<InkAnnotation> list = new List<InkAnnotation>();
//        foreach (InkDrawableView erasedInkView in e.ErasedInkViews)
//        {
//            if (erasedInkView is IAnnotationView annotationView && annotationView.Annotation is InkAnnotation inkAnnotation)
//            {
//                list.Add(inkAnnotation);
//                if (inkAnnotation.PointsCollection.Count == 0)
//                {
//                    RemoveErasedInkAnnotation(inkAnnotation);
//                    continue;
//                }

//                AnnotationEventArgs e2 = new AnnotationEventArgs(inkAnnotation);
//                this.AnnotationEdited?.Invoke(this, e2);
//            }
//        }

//        InkErasedCommand command = new InkErasedCommand(list, e.OldValue, e.NewValue, AddEraseredInkAnnotion, RemoveErasedInkAnnotation);
//        changeTracker?.RegisterChange(command);
//    }

//    private void OnAnnotationPropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        if (!(sender is Annotation annotation))
//        {
//            return;
//        }

//        InkAnnotation inkSignature = annotation as InkAnnotation;
//        if (inkSignature != null && e.PropertyName == "Bounds")
//        {
//            FormField formField = FormFields.Where((FormField field) => field is SignatureFormField signatureFormField2 && signatureFormField2.AssociatedSignature == inkSignature).FirstOrDefault();
//            if (formField is SignatureFormField signatureFormField)
//            {
//                if (signatureFormField.FormWidgets[0].Bounds.IntersectsWith(inkSignature.Bounds))
//                {
//                    signatureFormField.Signature = inkSignature;
//                }
//                else
//                {
//                    signatureFormField.Signature = null;
//                }
//            }
//        }

//        if (e.PropertyName == "BorderWidth" && annotation is InkAnnotation inkAnnotation)
//        {
//            AnnotationPropertyChangedEventArgs annotationPropertyChangedEventArgs = e as AnnotationPropertyChangedEventArgs;
//            float oldThickness = (float)annotationPropertyChangedEventArgs.OldValue;
//            float newThickness = (float)annotationPropertyChangedEventArgs.NewValue;
//            inkAnnotation.UpdateBoundsWithThickness(oldThickness, newThickness);
//        }

//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            DocumentView?.UpdateAnnotationView(annotation, e.PropertyName);
//        }
//        else
//        {
//            SinglePageViewContainer?.UpdateAnnotationView(annotation, e.PropertyName);
//        }

//        if (AnnotationMode == AnnotationMode.Ink && annotation is InkAnnotation inkAnnotation2 && inkAnnotation2.State == InkState.Wet)
//        {
//            if (e is AnnotationPropertyChangedEventArgs annotationPropertyChangedEventArgs2 && annotationPropertyChangedEventArgs2.PropertyName == "PointsCollection")
//            {
//                CheckChangeTrackerReady();
//                AnnotationEditCommand command = new AnnotationEditCommand(annotation, annotationPropertyChangedEventArgs2.PropertyName, annotationPropertyChangedEventArgs2.OldValue, annotationPropertyChangedEventArgs2.NewValue);
//                changeTracker?.RegisterChange(command);
//            }
//        }
//        else if (e.PropertyName != "MarkAnnotationViewDirty" && e.PropertyName != "BoundingBox" && e.PropertyName != "SignatureBounds")
//        {
//            OnAnnotationEdited(annotation, e as AnnotationPropertyChangedEventArgs);
//        }
//    }

//    private void AnnotationsLoader_InkSessionEnded(object? sender, InkSessionEndedEventArgs e)
//    {
//        List<InkAnnotation> list = new List<InkAnnotation>();
//        foreach (InkAnnotation unconfirmedInk in e.UnconfirmedInks)
//        {
//            if (e.PageNumberToSkip == -1 || unconfirmedInk.PageNumber != e.PageNumberToSkip)
//            {
//                unconfirmedInk.State = InkState.Dry;
//                InkAnnotation inkAnnotation = unconfirmedInk;
//                this.AnnotationAdded?.Invoke(this, new AnnotationEventArgs(inkAnnotation));
//                list.Add(inkAnnotation);
//            }
//        }

//        foreach (InkAnnotation item in list)
//        {
//            annotationsLoader?.UnconfirmedInks.Remove(item);
//        }

//        for (int i = 0; i < DocumentView.Children.Count; i++)
//        {
//            if (DocumentView.Children[i] is PdfPageView pdfPageView && pdfPageView.PageNumber != e.PageNumberToSkip)
//            {
//                pdfPageView.ResetInkCanvas();
//            }
//        }
//    }

//    internal async void ShowToast(string? message, LayoutOptions layoutOptions)
//    {
//        if (toast != null && toastText != null)
//        {
//            if (toast.Parent != null)
//            {
//                base.Children.Remove(toast);
//            }

//            toast.HorizontalOptions = LayoutOptions.Center;
//            toast.VerticalOptions = layoutOptions;
//            if (toast.VerticalOptions.Equals(LayoutOptions.End) && BottomToolbarGridViewMobile != null)
//            {
//                toast.Margin = new Thickness(0.0, BottomToolbarGridViewMobile.Height + 12.0);
//            }

//            base.Children.Add(toast);
//            toast.Opacity = 1.0;
//            toastText.Text = message;
//            toast.InputTransparent = true;
//            await toast.FadeTo(0.0, 2000u, Easing.SinIn);
//        }
//    }

//    internal void CreateToast()
//    {
//        toast = new Border
//        {
//            AutomationId = "toast",
//            BackgroundColor = ToastBackgroundColor,
//            Padding = new Thickness(8.0),
//            VerticalOptions = LayoutOptions.Center,
//            HorizontalOptions = LayoutOptions.Center,
//            Opacity = 0.0
//        };
//        toast.StrokeShape = new RoundRectangle
//        {
//            CornerRadius = 4.0
//        };
//        toastText = new Label
//        {
//            TextColor = ToastTextColor,
//            VerticalOptions = LayoutOptions.Center,
//            VerticalTextAlignment = TextAlignment.Center,
//            HorizontalOptions = LayoutOptions.Center,
//            AutomationId = "toastText"
//        };
//        toast.Content = toastText;
//    }

//    private void AnnotationsLoader_PageTapped(object? sender, EventArgs e)
//    {
//        DeselectAnnotation(SelectedAnnotation);
//    }

//    private void CheckChangeTrackerReady()
//    {
//        if (changeTracker == null)
//        {
//            changeTracker = new ChangeTracker();
//            changeTracker.PropertyChanged += ChangeTracker_PropertyChanged;
//        }
//    }

//    private void DisableOrEnableSinglePagePanMode(bool enable)
//    {
//        for (int i = 0; i < SinglePageViewContainer?.Pages.Length; i++)
//        {
//            SinglePageScrollView singlePageScrollView = SinglePageViewContainer.Pages[i];
//            if (singlePageScrollView != null && singlePageScrollView.m_panZoomListener != null)
//            {
//                singlePageScrollView.m_panZoomListener.PanMode = (enable ? PanMode.Both : PanMode.None);
//            }
//        }
//    }

//    private void DisableOrEnableScrollOrientation(bool enable)
//    {
//        if (PageLayoutMode == PageLayoutMode.Continuous && ScrollView != null)
//        {
//            ScrollView.Orientation = (enable ? ScrollOrientation.Both : ScrollOrientation.Neither);
//            return;
//        }

//        for (int i = 0; i < SinglePageViewContainer?.Pages.Length; i++)
//        {
//            SinglePageScrollView singlePageScrollView = SinglePageViewContainer.Pages[i];
//            if (singlePageScrollView != null && singlePageScrollView.m_panZoomListener != null)
//            {
//                singlePageScrollView.Orientation = (enable ? ScrollOrientation.Both : ScrollOrientation.Neither);
//            }
//        }
//    }

//    private void ChangeTracker_PropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        PropertyChangedEventArgs e2 = e;
//        ChangeTracker changeTracker = sender as ChangeTracker;
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            if (e2.PropertyName == "CanUndo" && UndoCommand is Command command)
//            {
//                command.ChangeCanExecute();
//            }
//            else if (e2.PropertyName == "CanRedo" && RedoCommand is Command command2)
//            {
//                command2.ChangeCanExecute();
//            }
//        });
//    }

//    private bool isErasedInkAnnotation(Annotation annotation, AnnotationPropertyChangedEventArgs? e)
//    {
//        if (annotation is InkAnnotation)
//        {
//            if (AnnotationMode == AnnotationMode.InkEraser && "PointsCollection" == e.PropertyName)
//            {
//                return false;
//            }

//            return true;
//        }

//        return true;
//    }

//    private void OnAnnotationEdited(Annotation? annotation, AnnotationPropertyChangedEventArgs? e)
//    {
//        if (isErasedInkAnnotation(annotation, e))
//        {
//            CheckChangeTrackerReady();
//            AnnotationEditCommand command = new AnnotationEditCommand(annotation, e.PropertyName, e.OldValue, e.NewValue);
//            changeTracker?.RegisterChange(command);
//            AnnotationEventArgs e2 = new AnnotationEventArgs(annotation);
//            this.AnnotationEdited?.Invoke(this, e2);
//        }

//        modificationMade = true;
//        UpdateUndoRedoButtons();
//    }

//    internal void Undo()
//    {
//        changeTracker?.Undo();
//        CloseAllDialogs();
//        if (TopToolbarMobile?.UndoView?.View != null)
//        {
//            if (changeTracker != null && changeTracker.UndoStack.Count > 0)
//            {
//                TopToolbarMobile.UndoView.View.IsEnabled = true;
//            }
//            else
//            {
//                TopToolbarMobile.UndoView.View.IsEnabled = false;
//            }
//        }

//        if (TopToolbarMobile?.RedoView?.View != null)
//        {
//            TopToolbarMobile.RedoView.View.IsEnabled = true;
//        }

//        if (TopToolbarDesktop?.UndoView?.View != null)
//        {
//            if (changeTracker != null && changeTracker.UndoStack.Count > 0)
//            {
//                TopToolbarDesktop.UndoView.View.IsEnabled = true;
//            }
//            else
//            {
//                TopToolbarDesktop.UndoView.View.IsEnabled = false;
//            }
//        }

//        if (TopToolbarDesktop?.RedoView?.View != null)
//        {
//            TopToolbarDesktop.RedoView.View.IsEnabled = true;
//        }
//    }

//    internal void Redo()
//    {
//        changeTracker?.Redo();
//        CloseAllDialogs();
//        if (TopToolbarMobile?.RedoView?.View != null)
//        {
//            if (changeTracker != null && changeTracker.RedoStack.Count > 0)
//            {
//                TopToolbarMobile.RedoView.View.IsEnabled = true;
//            }
//            else
//            {
//                TopToolbarMobile.RedoView.View.IsEnabled = false;
//            }
//        }

//        if (TopToolbarDesktop?.RedoView?.View != null)
//        {
//            if (changeTracker != null && changeTracker.RedoStack.Count > 0)
//            {
//                TopToolbarDesktop.RedoView.View.IsEnabled = true;
//            }
//            else
//            {
//                TopToolbarDesktop.RedoView.View.IsEnabled = false;
//            }
//        }
//    }

//    private void AnnotationsLoader_AnnotationDeselected(object? sender, AnnotationEventArgs e)
//    {
//        if (ScrollView != null && ScrollView.m_panZoomListener != null && !ScrollView.m_panZoomListener.AllowDoubleTapZoom)
//        {
//            ScrollView.m_panZoomListener.AllowDoubleTapZoom = true;
//        }

//        DisableOrEnableSinglePagePanMode(enable: true);
//        SelectedAnnotation = null;
//        CloseDialog(StickyNoteDialog);
//        EnableAnnotationsEditToolbarDesktopItems();
//        if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.SignatureButton != null && AnnotationsToolbarViewDesktop.SignatureButton.IsEnabled)
//        {
//            AnnotationsToolbarViewDesktop.SignatureButton.Background = Colors.Transparent;
//        }

//        if (AnnotationsToolbarViewDesktop != null)
//        {
//            View view = AnnotationsToolbarViewDesktop.ScrollableToolbar?.Items.FirstOrDefault((SfToolbarItem i) => i.Name == "ColorPickerSeparator")?.View;
//            if (view != null)
//            {
//                view.IsVisible = false;
//                view.IsEnabled = true;
//            }

//            if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.DeleteButton != null)
//            {
//                AnnotationsToolbarViewDesktop.DeleteButton.IsVisible = false;
//            }
//        }

//        this.AnnotationDeselected?.Invoke(this, e);
//    }

//    internal void EnableAnnotationsEditToolbarDesktopItems()
//    {
//        if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.Items != null)
//        {
//            for (int i = 0; i < AnnotationsToolbarViewDesktop.Items.Count; i++)
//            {
//                AnnotationsToolbarViewDesktop.Items[i].View.IsEnabled = true;
//            }
//        }

//        if (InkSecondaryAnnotationToolbarViewDesktop != null && InkSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int j = 0; j < InkSecondaryAnnotationToolbarViewDesktop.Items.Count; j++)
//            {
//                InkSecondaryAnnotationToolbarViewDesktop.Items[j].View.IsEnabled = true;
//            }
//        }

//        if (FreeTextSecondaryAnnotationToolbarViewDesktop != null && FreeTextSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int k = 0; k < FreeTextSecondaryAnnotationToolbarViewDesktop.Items.Count; k++)
//            {
//                FreeTextSecondaryAnnotationToolbarViewDesktop.Items[k].View.IsEnabled = true;
//            }
//        }

//        if (CircleSecondaryAnnotationToolbarViewDesktop != null && CircleSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int l = 0; l < CircleSecondaryAnnotationToolbarViewDesktop.Items.Count; l++)
//            {
//                CircleSecondaryAnnotationToolbarViewDesktop.Items[l].View.IsEnabled = true;
//            }
//        }

//        if (RectangleSecondaryAnnotationToolbarViewDesktop != null && RectangleSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int m = 0; m < RectangleSecondaryAnnotationToolbarViewDesktop.Items.Count; m++)
//            {
//                RectangleSecondaryAnnotationToolbarViewDesktop.Items[m].View.IsEnabled = true;
//            }
//        }

//        if (LineSecondaryAnnotationToolbarViewDesktop != null && LineSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int n = 0; n < LineSecondaryAnnotationToolbarViewDesktop.Items.Count; n++)
//            {
//                LineSecondaryAnnotationToolbarViewDesktop.Items[n].View.IsEnabled = true;
//            }
//        }

//        if (ArrowSecondaryAnnotationToolbarViewDesktop != null && ArrowSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int num = 0; num < ArrowSecondaryAnnotationToolbarViewDesktop.Items.Count; num++)
//            {
//                ArrowSecondaryAnnotationToolbarViewDesktop.Items[num].View.IsEnabled = true;
//            }
//        }

//        if (PolygonSecondaryAnnotationToolbarViewDesktop != null && PolygonSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int num2 = 0; num2 < PolygonSecondaryAnnotationToolbarViewDesktop.Items.Count; num2++)
//            {
//                PolygonSecondaryAnnotationToolbarViewDesktop.Items[num2].View.IsEnabled = true;
//            }
//        }

//        if (StickyNoteSecondaryAnnotationToolbarViewDesktop != null && StickyNoteSecondaryAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int num3 = 0; num3 < StickyNoteSecondaryAnnotationToolbarViewDesktop.Items.Count; num3++)
//            {
//                StickyNoteSecondaryAnnotationToolbarViewDesktop.Items[num3].View.IsEnabled = true;
//            }
//        }

//        if (StampAnnotationToolbarViewDesktop != null && StampAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int num4 = 0; num4 < StampAnnotationToolbarViewDesktop.Items.Count; num4++)
//            {
//                StampAnnotationToolbarViewDesktop.Items[num4].View.IsEnabled = true;
//            }
//        }
//    }

//    private void OnAnnotationRemoved(Annotation annotation)
//    {
//        Annotation annotation2 = annotation;
//        FormField formField = FormFields.Where((FormField field) => field is SignatureFormField signatureFormField2 && signatureFormField2.Signature == annotation2).FirstOrDefault();
//        if (formField is SignatureFormField signatureFormField)
//        {
//            signatureFormField.Signature = null;
//        }

//        CheckChangeTrackerReady();
//        IChangeCommand command = new AnnotationAddRemoveCommand(annotation2, AddAnnotation, RemoveAnnotation);
//        changeTracker?.RegisterChange(command);
//        this.AnnotationRemoved?.Invoke(this, new AnnotationEventArgs(annotation2));
//        modificationMade = true;
//        UpdateUndoRedoButtons();
//        if (ColorToolbarViewMobile?.FixedToolbarView != null)
//        {
//            ColorToolbarViewMobile.FixedToolbarView.IsVisible = false;
//            ColorToolbarViewMobile.IsStrokeColorToolbarVisible = false;
//            ColorToolbarViewMobile.IsFillColorToolbarVisible = false;
//        }

//        if (SliderToolbarViewMobile?.FixedToolbarView != null)
//        {
//            SliderToolbarViewMobile.FixedToolbarView.IsVisible = false;
//            SliderToolbarViewMobile.IsThicknessToolbarVisible = false;
//            SliderToolbarViewMobile.IsFillOpacityToolbarVisible = false;
//        }

//        if (StickyNoteIconsViewMobile?.ScrollableToolbar != null)
//        {
//            StickyNoteIconsViewMobile.ScrollableToolbar.IsVisible = false;
//        }
//    }

//    internal void OnFreeTextModalViewAppearing(AnnotationModalViewAppearingEventArgs e)
//    {
//        this.FreeTextModalViewAppearing?.Invoke(this, e);
//    }

//    internal void OnStickyNoteModalViewAppearing(AnnotationModalViewAppearingEventArgs e)
//    {
//        this.StickyNoteModalViewAppearing?.Invoke(this, e);
//    }

//    internal void OnSignatureModalViewAppearing(FormFieldModalViewAppearingEventArgs e)
//    {
//        this.SignatureModalViewAppearing?.Invoke(this, e);
//    }

//    internal void OnCustomStampModalViewAppearing(AnnotationModalViewAppearingEventArgs e)
//    {
//        this.CustomStampModalViewAppearing?.Invoke(this, e);
//    }

//    internal void OnFreeTextModalViewDisappearing(EventArgs e)
//    {
//        this.FreeTextModalViewDisappearing?.Invoke(this, e);
//    }

//    internal void OnStickyNoteModalViewDisappearing(EventArgs e)
//    {
//        this.StickyNoteModalViewDisappearing?.Invoke(this, e);
//    }

//    internal void OnSignatureModalViewDisappearing(EventArgs e)
//    {
//        this.SignatureModalViewDisappearing?.Invoke(this, e);
//    }

//    internal void OnCustomStampModalViewDisappearing(EventArgs e)
//    {
//        this.CustomStampModalViewDisappearing?.Invoke(this, e);
//    }

//    private void OnAnnotationAdded(Annotation annotation)
//    {
//        Annotation annotation2 = annotation;
//        FormField formField = FormFields.Where((FormField field) => field is SignatureFormField signatureFormField2 && signatureFormField2.AssociatedSignature == annotation2).FirstOrDefault();
//        if (formField is SignatureFormField signatureFormField)
//        {
//            signatureFormField.Signature = annotation2;
//        }

//        CheckChangeTrackerReady();
//        IChangeCommand command = new AnnotationAddRemoveCommand(annotation2, RemoveAnnotation, AddAnnotation);
//        changeTracker?.RegisterChange(command);
//        this.AnnotationAdded?.Invoke(this, new AnnotationEventArgs(annotation2));
//        modificationMade = true;
//        UpdateUndoRedoButtons();
//        if (annotation2 is StickyNoteAnnotation && AnnotationMode == AnnotationMode.StickyNote)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }
//        else if (annotation2 is FreeTextAnnotation && AnnotationMode == AnnotationMode.FreeText)
//        {
//            AnnotationMode = AnnotationMode.None;
//        }
//    }

//    private void AnnotationsLoader_AnnotationAdded(object? sender, AnnotationEventArgs e)
//    {
//        CheckChangeTrackerReady();
//        modificationMade = true;
//        UpdateUndoRedoButtons();
//        IChangeCommand command = new AnnotationAddRemoveCommand(e.Annotation, RemoveAnnotation, AddAnnotation);
//        changeTracker?.RegisterChange(command);
//        if (!(e.Annotation is InkAnnotation) || AnnotationMode != AnnotationMode.Ink || !AnnotationSettings.Ink.AggregateInkStrokes)
//        {
//            this.AnnotationAdded?.Invoke(this, e);
//        }
//    }

//    private void AnnotationsLoader_AnnotationSelected(object? sender, AnnotationEventArgs e)
//    {
//        if (SelectedAnnotation != null && SelectedAnnotation != e.Annotation)
//        {
//            DeselectAnnotation(SelectedAnnotation);
//        }

//        SelectedAnnotation = e.Annotation;
//        DisableOrEnableSinglePagePanMode(enable: false);
//        if ((SelectedAnnotation is StickyNoteAnnotation || SelectedAnnotation is FreeTextAnnotation) && ScrollView != null && ScrollView.m_panZoomListener != null)
//        {
//            ScrollView.m_panZoomListener.AllowDoubleTapZoom = false;
//            for (int j = 0; j < SinglePageViewContainer?.Pages.Length; j++)
//            {
//                SinglePageScrollView singlePageScrollView = SinglePageViewContainer.Pages[j];
//                if (singlePageScrollView != null && singlePageScrollView.m_panZoomListener != null)
//                {
//                    singlePageScrollView.m_panZoomListener.AllowDoubleTapZoom = false;
//                }
//            }
//        }

//        InitializeAnnotationToolbarViewDesktop();
//        if (SelectedAnnotation != null)
//        {
//            if (SelectedAnnotation is PolygonAnnotation polygonAnnotation && polygonAnnotation.BorderStyle == BorderStyle.Solid && PolygonSecondaryAnnotationToolbarViewDesktop != null && PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton != null && PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton.IsEnabled && PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton != null)
//            {
//                PolygonSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton.IsVisible = true;
//                PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton.IsVisible = false;
//            }

//            if (SelectedAnnotation is PolygonAnnotation polygonAnnotation2 && polygonAnnotation2.BorderStyle == BorderStyle.Cloudy && PolygonSecondaryAnnotationToolbarViewDesktop != null && PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton != null && PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton.IsEnabled && PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton != null)
//            {
//                PolygonSecondaryAnnotationToolbarViewDesktop.ClearButtonHighlights();
//                PolygonSecondaryAnnotationToolbarViewDesktop.PolygonButton.IsVisible = false;
//                PolygonSecondaryAnnotationToolbarViewDesktop.CloudButton.IsVisible = true;
//            }

//            if (((SelectedAnnotation is InkAnnotation inkAnnotation && inkAnnotation.IsSignature) || (SelectedAnnotation is StampAnnotation stampAnnotation && stampAnnotation.IsSignature)) && AnnotationsToolbarViewDesktop != null)
//            {
//                View view = AnnotationsToolbarViewDesktop.ScrollableToolbar?.Items.FirstOrDefault((SfToolbarItem i) => i.Name == "ColorPickerSeparator")?.View;
//                if (view != null)
//                {
//                    view.IsVisible = true;
//                }

//                if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.DeleteButton != null)
//                {
//                    AnnotationsToolbarViewDesktop.DeleteButton.IsVisible = true;
//                }
//            }

//            if (SelectedAnnotation != null && SelectedAnnotation is FreeTextAnnotation freeTextAnnotation && FreeTextSecondaryAnnotationToolbarViewDesktop?.FreeTextFontSizeButton is ToolbarIconWithDropdownforZoomPercentage toolbarIconWithDropdownforZoomPercentage)
//            {
//                toolbarIconWithDropdownforZoomPercentage.TextLabel.Text = freeTextAnnotation.FontSize + "px";
//            }
//        }

//        if (SelectedAnnotation.IsLocked)
//        {
//            DisableAnnotationsEditToolbarDesktop();
//        }

//        if (SelectedAnnotation is StickyNoteAnnotation)
//        {
//            if ((StickyNoteDialog == null || !StickyNoteDialog.IsOpen) && AnnotationSettings.CanEdit(SelectedAnnotation))
//            {
//                ShowStickyNoteDialog(SelectedAnnotation, isHovered: false, isSelected: true);
//            }

//            if (StickyNoteDialog != null)
//            {
//                if (StickyNoteDialog.StickyNoteEditor != null)
//                {
//                    StickyNoteDialog.StickyNoteEditor.IsEnabled = false;
//                }

//                if (StickyNoteDialog.CloseLabel != null)
//                {
//                    StickyNoteDialog.CloseLabel.IsVisible = true;
//                }

//                if (StickyNoteDialog.SaveLabel != null)
//                {
//                    StickyNoteDialog.SaveLabel.IsVisible = false;
//                }

//                if (StickyNoteDialog.EditLabel != null && AnnotationSettings.CanEdit(SelectedAnnotation))
//                {
//                    StickyNoteDialog.EditLabel.IsVisible = true;
//                }
//            }
//        }

//        OpenOrCloseAnnotationEditToolbar(SelectedAnnotation, IsOpen: true);
//        this.AnnotationSelected?.Invoke(this, e);
//    }

//    private void InitializeAnnotationToolbarViewDesktop()
//    {
//        if (SecondaryToolbarGridViewDesktop == null && AnnotationsToolbarViewDesktop == null)
//        {
//            SecondaryToolbarGridViewDesktop = new Grid();
//            SecondaryToolbarGridViewDesktop.ZIndex = 1;
//            AnnotationsToolbarViewDesktop = new AnnotationsToolbarViewDesktop(this);
//            Toolbars.Add(AnnotationsToolbarViewDesktop);
//            AnnotationsToolbarViewDesktop.IsAddedInView = true;
//            AnnotationsToolbarViewDesktop.ScrollableToolbar.VerticalOptions = LayoutOptions.End;
//            BoxView item = new BoxView
//            {
//                HeightRequest = 1.0,
//                BackgroundColor = ToolbarBorderColor,
//                VerticalOptions = LayoutOptions.End,
//                ZIndex = 1
//            };
//            SecondaryToolbarGridViewDesktop.Children.Add(AnnotationsToolbarViewDesktop.ScrollableToolbar);
//            SecondaryToolbarGridViewDesktop.Children.Add(item);
//            Grid.SetRow((BindableObject)SecondaryToolbarGridViewDesktop, 1);
//            ToolbarGridView?.Children.Add(SecondaryToolbarGridViewDesktop);
//            SecondaryToolbarGridViewDesktop.IsVisible = false;
//            AnnotationsToolbarViewDesktop.ScrollableToolbar.IsVisible = false;
//        }

//        if (LineSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            LineSecondaryAnnotationToolbarViewDesktop = new LineSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(LineSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (ArrowSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            ArrowSecondaryAnnotationToolbarViewDesktop = new ArrowSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(ArrowSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (RectangleSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            RectangleSecondaryAnnotationToolbarViewDesktop = new RectangleSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(RectangleSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (CircleSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            CircleSecondaryAnnotationToolbarViewDesktop = new CircleSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(CircleSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (PolygonSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            PolygonSecondaryAnnotationToolbarViewDesktop = new PolygonSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(PolygonSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (PolylineSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            PolylineSecondaryAnnotationToolbarViewDesktop = new PolylineSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(PolylineSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (FreeTextSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            FreeTextSecondaryAnnotationToolbarViewDesktop = new FreeTextSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(FreeTextSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (InkSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            InkSecondaryAnnotationToolbarViewDesktop = new InkSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(InkSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (EraserSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            EraserSecondaryAnnotationToolbarViewDesktop = new EraserSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(EraserSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (StickyNoteSecondaryAnnotationToolbarViewDesktop == null)
//        {
//            StickyNoteSecondaryAnnotationToolbarViewDesktop = new StickyNoteSecondaryAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(StickyNoteSecondaryAnnotationToolbarViewDesktop);
//        }

//        if (StampAnnotationToolbarViewDesktop == null)
//        {
//            StampAnnotationToolbarViewDesktop = new StampAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(StampAnnotationToolbarViewDesktop);
//        }

//        if (HighlightAnnotationToolbarViewDesktop == null)
//        {
//            HighlightAnnotationToolbarViewDesktop = new HighlightAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(HighlightAnnotationToolbarViewDesktop);
//        }

//        if (UnderlineAnnotationToolbarViewDesktop == null)
//        {
//            UnderlineAnnotationToolbarViewDesktop = new UnderlineAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(UnderlineAnnotationToolbarViewDesktop);
//        }

//        if (StrikeOutAnnotationToolbarViewDesktop == null)
//        {
//            StrikeOutAnnotationToolbarViewDesktop = new StrikeOutAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(StrikeOutAnnotationToolbarViewDesktop);
//        }

//        if (SquigglyAnnotationToolbarViewDesktop == null)
//        {
//            SquigglyAnnotationToolbarViewDesktop = new SquigglyAnnotationToolbarViewDesktop(this);
//            Toolbars?.Add(SquigglyAnnotationToolbarViewDesktop);
//        }
//    }

//    internal void DisableAnnotationsEditToolbarDesktop()
//    {
//        if (SelectedAnnotation is InkAnnotation)
//        {
//            if (InkSecondaryAnnotationToolbarViewDesktop != null && InkSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int j = 0; j < InkSecondaryAnnotationToolbarViewDesktop.Items.Count; j++)
//                {
//                    InkSecondaryAnnotationToolbarViewDesktop.Items[j].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is FreeTextAnnotation)
//        {
//            if (FreeTextSecondaryAnnotationToolbarViewDesktop != null && FreeTextSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int k = 0; k < FreeTextSecondaryAnnotationToolbarViewDesktop.Items.Count; k++)
//                {
//                    FreeTextSecondaryAnnotationToolbarViewDesktop.Items[k].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is CircleAnnotation)
//        {
//            if (CircleSecondaryAnnotationToolbarViewDesktop != null && CircleSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int l = 0; l < CircleSecondaryAnnotationToolbarViewDesktop.Items.Count; l++)
//                {
//                    CircleSecondaryAnnotationToolbarViewDesktop.Items[l].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is SquareAnnotation)
//        {
//            if (RectangleSecondaryAnnotationToolbarViewDesktop != null && RectangleSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int m = 0; m < RectangleSecondaryAnnotationToolbarViewDesktop.Items.Count; m++)
//                {
//                    RectangleSecondaryAnnotationToolbarViewDesktop.Items[m].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is LineAnnotation)
//        {
//            if (LineSecondaryAnnotationToolbarViewDesktop != null && LineSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int n = 0; n < LineSecondaryAnnotationToolbarViewDesktop.Items.Count; n++)
//                {
//                    LineSecondaryAnnotationToolbarViewDesktop.Items[n].View.IsEnabled = false;
//                }
//            }

//            if (ArrowSecondaryAnnotationToolbarViewDesktop != null && ArrowSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int num = 0; num < ArrowSecondaryAnnotationToolbarViewDesktop.Items.Count; num++)
//                {
//                    ArrowSecondaryAnnotationToolbarViewDesktop.Items[num].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is PolygonAnnotation)
//        {
//            if (PolygonSecondaryAnnotationToolbarViewDesktop != null && PolygonSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int num2 = 0; num2 < PolygonSecondaryAnnotationToolbarViewDesktop.Items.Count; num2++)
//                {
//                    PolygonSecondaryAnnotationToolbarViewDesktop.Items[num2].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is StickyNoteAnnotation)
//        {
//            if (StickyNoteSecondaryAnnotationToolbarViewDesktop != null && StickyNoteSecondaryAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int num3 = 0; num3 < StickyNoteSecondaryAnnotationToolbarViewDesktop.Items.Count; num3++)
//                {
//                    StickyNoteSecondaryAnnotationToolbarViewDesktop.Items[num3].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is StampAnnotation)
//        {
//            if (StampAnnotationToolbarViewDesktop != null && StampAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int num4 = 0; num4 < StampAnnotationToolbarViewDesktop.Items.Count; num4++)
//                {
//                    StampAnnotationToolbarViewDesktop.Items[num4].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is HighlightAnnotation)
//        {
//            if (HighlightAnnotationToolbarViewDesktop != null && HighlightAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int num5 = 0; num5 < HighlightAnnotationToolbarViewDesktop.Items.Count; num5++)
//                {
//                    HighlightAnnotationToolbarViewDesktop.Items[num5].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is UnderlineAnnotation)
//        {
//            if (UnderlineAnnotationToolbarViewDesktop != null && UnderlineAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int num6 = 0; num6 < UnderlineAnnotationToolbarViewDesktop.Items.Count; num6++)
//                {
//                    UnderlineAnnotationToolbarViewDesktop.Items[num6].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is StrikeOutAnnotation)
//        {
//            if (StrikeOutAnnotationToolbarViewDesktop != null && StrikeOutAnnotationToolbarViewDesktop.Items != null)
//            {
//                for (int num7 = 0; num7 < StrikeOutAnnotationToolbarViewDesktop.Items.Count; num7++)
//                {
//                    StrikeOutAnnotationToolbarViewDesktop.Items[num7].View.IsEnabled = false;
//                }
//            }
//        }
//        else if (SelectedAnnotation is SquigglyAnnotation && SquigglyAnnotationToolbarViewDesktop != null && SquigglyAnnotationToolbarViewDesktop.Items != null)
//        {
//            for (int num8 = 0; num8 < SquigglyAnnotationToolbarViewDesktop.Items.Count; num8++)
//            {
//                SquigglyAnnotationToolbarViewDesktop.Items[num8].View.IsEnabled = false;
//            }
//        }

//        if ((!(SelectedAnnotation is InkAnnotation inkAnnotation2) || !inkAnnotation2.IsSignature) && (!(SelectedAnnotation is StampAnnotation stampAnnotation2) || !stampAnnotation2.IsSignature))
//        {
//            return;
//        }

//        if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.Items != null)
//        {
//            for (int num9 = 0; num9 < AnnotationsToolbarViewDesktop.Items.Count; num9++)
//            {
//                AnnotationsToolbarViewDesktop.Items[num9].View.IsEnabled = false;
//            }
//        }

//        if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.SignatureButton != null && AnnotationsToolbarViewDesktop.SignatureButton.IsEnabled)
//        {
//            AnnotationsToolbarViewDesktop.SignatureButton.Background = DesktopToolbarIconIsPressedColor;
//            View view = AnnotationsToolbarViewDesktop.ScrollableToolbar?.Items.FirstOrDefault((SfToolbarItem i) => i.Name == "ColorPickerSeparator")?.View;
//            if (view != null)
//            {
//                view.IsVisible = true;
//                view.IsEnabled = false;
//            }

//            if (AnnotationsToolbarViewDesktop != null && AnnotationsToolbarViewDesktop.DeleteButton != null)
//            {
//                AnnotationsToolbarViewDesktop.DeleteButton.IsVisible = true;
//                AnnotationsToolbarViewDesktop.DeleteButton.IsEnabled = false;
//            }
//        }
//    }

//    private void AnnotationsLoader_PanModeChangeRequested(object? sender, PanModeChangeRequestedEventArgs e)
//    {
//        if (ScrollView != null)
//        {
//            if (e.EnablePan)
//            {
//                ExecutePanChnageRequest(e);
//            }
//            else
//            {
//                ExecutePanChnageRequest(e);
//            }
//        }

//        if (m_textInfoManager != null)
//        {
//            m_textInfoManager.EnableTextSelection = e.EnablePan;
//        }
//    }

//    //
//    // Summary:
//    //     Checks the prerequisites are available to load. If not it will assign the values
//    //     are assigned.
//    private void CheckReadyToLoad(string? password = null)
//    {
//        if (password == null)
//        {
//            ClosePasswordDialog();
//        }

//        if (m_isDocumentLoaded)
//        {
//            Unload();
//        }

//        CheckViewportManagerReady();
//        CheckDocumentManagerReady();
//        CheckAnnotationsLoaderReady();
//        if (m_textInfoManager == null)
//        {
//            m_textInfoManager = new TextInfoManager();
//            m_textInfoManager.AssignDocumentManager(m_documentManager);
//            m_textInfoManager.Settings = TextSelectionSettings;
//            m_textInfoManager.EnableTextSelection = EnableTextSelection;
//            m_textInfoManager.SelectionChanged += OnSelectionChanged;
//            m_textInfoManager.TextMarkupRequested += OnTextMarkupRequested;
//        }

//        m_documentManager.annotationsLoader = annotationsLoader;
//        DispatcherUtils.Dispatch(this, CheckViewsAreReady);
//        if (m_density.HasValue)
//        {
//            m_documentManager.PageSizeMultiplier = m_density.Value;
//            m_documentManager.DeviceDensity = m_density.Value;
//        }

//        m_documentManager.CancellationTokenSource = m_cancellationTokenSource;
//    }

//    private void ExecutePanChnageRequest(PanModeChangeRequestedEventArgs e)
//    {
//        if (e.PointerType != 0)
//        {
//            if (PageLayoutMode == PageLayoutMode.Continuous && ScrollView?.m_panZoomListener != null)
//            {
//                ScrollView.m_panZoomListener.PanMode = (e.EnablePan ? PanMode.Both : PanMode.None);
//            }
//            else
//            {
//                DisableOrEnableSinglePagePanMode(e.EnablePan);
//            }
//        }
//        else
//        {
//            DisableOrEnableScrollOrientation(e.EnablePan);
//        }
//    }

//    private void OnTextMarkupRequested(object? sender, TextMarkupRequestedEventArgs e)
//    {
//        if (e.TextMarkupAnnotationType == SfPdfViewerResources.GetLocalizedString("Highlight"))
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.Highlight);
//        }
//        else if (e.TextMarkupAnnotationType == SfPdfViewerResources.GetLocalizedString("Underline"))
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.Underline);
//        }
//        else if (e.TextMarkupAnnotationType == SfPdfViewerResources.GetLocalizedString("Strikeout"))
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.StrikeOut);
//        }
//        else if (e.TextMarkupAnnotationType == SfPdfViewerResources.GetLocalizedString("Squiggly"))
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.Squiggly);
//        }
//    }

//    private void AddTextMarkupAnnotation(TextMarkupAnnotationType textMarkupType)
//    {
//        if (m_textInfoManager != null)
//        {
//            switch (textMarkupType)
//            {
//                case TextMarkupAnnotationType.Highlight:
//                    AddTextMarkupAnnotation(m_textInfoManager.SelectedTextLineBoundsInPixels, m_textInfoManager.GetBoundingBox(), m_textInfoManager.CurrentSelectionPageNumber, textMarkupType, AnnotationSettings.Highlight.Color, AnnotationSettings.Highlight.Opacity);
//                    break;
//                case TextMarkupAnnotationType.Underline:
//                    AddTextMarkupAnnotation(m_textInfoManager.SelectedTextLineBoundsInPixels, m_textInfoManager.GetBoundingBox(), m_textInfoManager.CurrentSelectionPageNumber, textMarkupType, AnnotationSettings.Underline.Color, AnnotationSettings.Underline.Opacity);
//                    break;
//                case TextMarkupAnnotationType.Squiggly:
//                    AddTextMarkupAnnotation(m_textInfoManager.SelectedTextLineBoundsInPixels, m_textInfoManager.GetBoundingBox(), m_textInfoManager.CurrentSelectionPageNumber, textMarkupType, AnnotationSettings.Squiggly.Color, AnnotationSettings.Squiggly.Opacity);
//                    break;
//                case TextMarkupAnnotationType.StrikeOut:
//                    AddTextMarkupAnnotation(m_textInfoManager.SelectedTextLineBoundsInPixels, m_textInfoManager.GetBoundingBox(), m_textInfoManager.CurrentSelectionPageNumber, textMarkupType, AnnotationSettings.StrikeOut.Color, AnnotationSettings.StrikeOut.Opacity);
//                    break;
//            }
//        }
//    }

//    private void CheckViewsAreReady()
//    {
//        ScrollView?.Activate();
//        if (ScrollView != null)
//        {
//            ScrollView.ZoomEnded += OnZoomEnded;
//        }

//        CloseDialog(messageDialog);
//        CheckDocumentViewIsReady();
//    }

//    private void OnZoomEnded(object? sender, ZoomEventArgs e)
//    {
//        UpdateExtentDimensions();
//    }

//    private void CheckDocumentViewIsReady()
//    {
//        if (DocumentView == null)
//        {
//            DocumentView = new PdfDocumentView();
//            if (ScrollView != null)
//            {
//                ScrollView.Content = DocumentView;
//            }

//            DocumentView.AssignViewportManager(m_viewportManager);
//            DocumentView.AssignDocumentManager(m_documentManager);
//            DocumentView?.AssignAnnotationsLoader(annotationsLoader);
//            DocumentView?.AssignTextInfoManager(m_textInfoManager);
//        }
//    }

//    private void CheckSignatureViewOverlayReady()
//    {
//        if (signatureViewOverlay == null)
//        {
//            signatureViewOverlay = new SignatureViewOverlay();
//            signatureViewOverlay.CloseRequested += SignatureViewOverlay_CloseRequested;
//            signatureViewOverlay.SignatureCreated += SignatureViewOverlay_SignatureCreated;
//            signatureViewOverlay.IntegratedSignatureViewAppearning += SignatureViewOverlay_SignatureDialogModalViewAppearning;
//        }
//    }

//    private void ExecutePreloadRequests()
//    {
//        if (m_zoomToRequest.HasValue)
//        {
//            ZoomTo(m_zoomToRequest.Value);
//            m_zoomToRequest = null;
//        }

//        if (m_goToPageRequest.HasValue)
//        {
//            GoToPage(m_goToPageRequest.Value);
//        }
//        else if (m_scrollToRequest.HasValue)
//        {
//            ScrollToOffset(m_scrollToRequest.Value.X, m_scrollToRequest.Value.Y);
//        }
//    }

//    private void BackupLoadInputs(object? documentSource, string? password = null, FlattenOptions? flattenOptions = FlattenOptions.None, CancellationTokenSource? cancellationTokenSource = null, bool isAsynchronousRequest = false)
//    {
//        DocumentSource = documentSource;
//        m_password = password;
//        this.flattenOptions = flattenOptions;
//        m_cancellationTokenSource = cancellationTokenSource;
//        m_isAsynchronousLoadRequest = isAsynchronousRequest;
//    }

//    private void BackupAnnotation(Annotation annotation)
//    {
//        BackupAnnotations.Add(annotation);
//    }

//    private void ClearBackupLoadInputs()
//    {
//        m_password = null;
//        m_cancellationTokenSource = null;
//        m_isAsynchronousLoadRequest = false;
//    }

//    private async void LoadDocument(object? documentSource, string? password = null, FlattenOptions? flattenOptions = FlattenOptions.None)
//    {
//        if (documentSource == null)
//        {
//            Unload();
//        }
//        else if (!m_isAsynchronousLoadRequest.HasValue || m_isAsynchronousLoadRequest.Value)
//        {
//            if (documentSource is Stream documentStream2)
//            {
//                await LoadDocumentAsync(documentStream2, password, flattenOptions);
//            }
//            else if (documentSource is byte[] documentBytes2)
//            {
//                await LoadDocumentAsync(documentBytes2, password, flattenOptions);
//            }
//        }
//        else if (documentSource is Stream documentStream)
//        {
//            LoadDocument(documentStream, password, flattenOptions);
//        }
//        else if (documentSource is byte[] documentBytes)
//        {
//            LoadDocument(documentBytes, password, flattenOptions);
//        }
//    }

//    private void ZoomTo(double zoomFactor)
//    {
//        if (m_isDocumentLoaded)
//        {
//            if (PageLayoutMode == PageLayoutMode.Continuous)
//            {
//                ScrollView?.ZoomTo(zoomFactor, new Microsoft.Maui.Graphics.Point(ScrollView.ScrollX, ScrollView.ScrollY));
//            }
//            else if (SinglePageViewContainer != null)
//            {
//                SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//                if (currentSinglePageView != null && currentSinglePageView.ZoomFactor != zoomFactor)
//                {
//                    currentSinglePageView.ZoomTo(zoomFactor, new Microsoft.Maui.Graphics.Point(currentSinglePageView.ScrollX, currentSinglePageView.ScrollY));
//                }
//            }
//        }
//        else
//        {
//            m_zoomToRequest = zoomFactor;
//        }
//    }

//    private void OnPdfViewerPropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        if (e.PropertyName == "FlowDirection")
//        {
//            if (ScrollView != null)
//            {
//                ScrollView.FlowDirection = base.FlowDirection;
//            }

//            if (OutlineView != null)
//            {
//                OutlineView.FlowDirection = base.FlowDirection;
//            }
//        }
//    }

//    //
//    // Summary:
//    //     Called when the control size is allocated
//    //
//    // Parameters:
//    //   width:
//    //     Control's new width
//    //
//    //   height:
//    //     Control's new height
//    protected override void OnSizeAllocated(double width, double height)
//    {
//        base.OnSizeAllocated(width, height);
//        if (SinglePageViewContainer != null)
//        {
//            SinglePageViewContainer.PdfViewerSizeAllocated(width, height);
//        }

//        DisplayOrientation displayOrientation = ((!(base.Width > base.Height)) ? DisplayOrientation.Portrait : DisplayOrientation.Landscape);
//        if (previousDeviceOrientation != displayOrientation && DocumentView != null)
//        {
//            DocumentView.RerenderRequiredOnOrientaitonChange = true;
//        }

//        previousDeviceOrientation = displayOrientation;
//    }

//    //
//    // Summary:
//    //     Finds and highlights all occurrences of the given text in the PDF document asynchronously.
//    //
//    //
//    // Parameters:
//    //   text:
//    //     The text or keyword to be searched.
//    //
//    //   searchOption:
//    //     The constant to specify the text search options.
//    //
//    //   cancellationTokenSource:
//    //     The cancellation token to cancel the current search progress.
//    //
//    // Returns:
//    //     Provides the search result obtained from searching a given text.
//    public async Task<TextSearchResult?> SearchTextAsync(string text, TextSearchOptions searchOption = TextSearchOptions.None, CancellationTokenSource? cancellationTokenSource = null)
//    {
//        CancellationTokenSource cancellationTokenSource2 = cancellationTokenSource;
//        string text2 = text;
//        m_textInfoManager?.ClearSelectionInfo();
//        DocumentView.RemoveTextSelection();
//        TextSearchResult results = null;
//        if (m_isDocumentLoaded)
//        {
//            await Task.Run(delegate
//            {
//                if (m_textSearchManager == null)
//                {
//                    m_textSearchManager = new TextSearchManager();
//                    m_textSearchManager.SearchProgress += OnSearchProgress;
//                    m_textSearchManager.AssignDocumentManager(m_documentManager);
//                    if (PageLayoutMode == PageLayoutMode.Continuous)
//                    {
//                        DocumentView?.AssignTextSearchManager(m_textSearchManager);
//                    }
//                    else
//                    {
//                        SinglePageViewContainer?.AssignTextSearchManager(m_textSearchManager);
//                    }
//                }

//                m_textSearchManager.Settings = TextSearchSettings;
//                if (m_viewportManager != null)
//                {
//                    m_textSearchManager.StartingPageIndex = m_viewportManager.StartingPageNumber - 1;
//                }

//                if (cancellationTokenSource2 == null)
//                {
//                    cancellationTokenSource2 = new CancellationTokenSource();
//                }

//                m_textSearchManager.SearchText(text2, cancellationTokenSource2, searchOption);
//                results = m_textSearchManager.SearchResults;
//            });
//        }

//        return results;
//    }

//    private void OnSearchProgress(object? sender, TextSearchProgressEventArgs? e)
//    {
//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            DocumentView?.OnSearchProgress(e);
//        }
//        else
//        {
//            SinglePageViewContainer?.OnSearchProgress(e);
//        }

//        this.TextSearchProgress?.Invoke(this, e);
//    }

//    private void AddTextMarkupAnnotation(List<Rect> lineBounds, Rect boundingBox, int pageNumber, TextMarkupAnnotationType annotationType, Microsoft.Maui.Graphics.Color color, float opacity)
//    {
//        RectF bounds = new RectF((float)(boundingBox.X / AnnotationConstants.PageSizeMultiplier), (float)(boundingBox.Y / AnnotationConstants.PageSizeMultiplier), (float)(boundingBox.Width / AnnotationConstants.PageSizeMultiplier), (float)(boundingBox.Height / AnnotationConstants.PageSizeMultiplier));
//        if (m_documentManager != null && m_documentManager.LoadedDocument != null)
//        {
//            PdfPageBase pdfPageBase = m_documentManager.LoadedDocument.Pages[pageNumber - 1];
//            List<RectF> list = new List<RectF>();
//            for (int i = 0; i < lineBounds.Count; i++)
//            {
//                RectF item = new RectF((float)(lineBounds[i].X / AnnotationConstants.PageSizeMultiplier), (float)(lineBounds[i].Y / AnnotationConstants.PageSizeMultiplier), (float)(lineBounds[i].Width / AnnotationConstants.PageSizeMultiplier), (float)(lineBounds[i].Height / AnnotationConstants.PageSizeMultiplier));
//                list.Add(item);
//            }

//            Annotation annotation = null;
//            switch (annotationType)
//            {
//                case TextMarkupAnnotationType.Highlight:
//                    {
//                        HighlightAnnotation highlightAnnotation = new HighlightAnnotation(pageNumber, list, bounds);
//                        highlightAnnotation.Rotation = pdfPageBase.Rotation;
//                        annotation = highlightAnnotation;
//                        break;
//                    }
//                case TextMarkupAnnotationType.Underline:
//                    {
//                        UnderlineAnnotation underlineAnnotation = new UnderlineAnnotation(pageNumber, list, bounds);
//                        underlineAnnotation.Rotation = pdfPageBase.Rotation;
//                        annotation = underlineAnnotation;
//                        break;
//                    }
//                case TextMarkupAnnotationType.Squiggly:
//                    {
//                        SquigglyAnnotation squigglyAnnotation = new SquigglyAnnotation(pageNumber, list, bounds);
//                        squigglyAnnotation.Rotation = pdfPageBase.Rotation;
//                        annotation = squigglyAnnotation;
//                        break;
//                    }
//                case TextMarkupAnnotationType.StrikeOut:
//                    {
//                        StrikeOutAnnotation strikeOutAnnotation = new StrikeOutAnnotation(pageNumber, list, bounds);
//                        strikeOutAnnotation.Rotation = pdfPageBase.Rotation;
//                        annotation = strikeOutAnnotation;
//                        break;
//                    }
//            }

//            if (annotation != null)
//            {
//                annotation.SetColor(color);
//                annotation.SetOpacity(opacity);
//                AddAnnotation(annotation);
//            }
//        }
//    }

//    //
//    // Summary:
//    //     Adds the given annotation to the page represented by the annotation’s PageNumber
//    //     property.
//    //
//    // Parameters:
//    //   annotation:
//    //     The annotation to be added.
//    public void AddAnnotation(Annotation annotation)
//    {
//        if (annotation == null)
//        {
//            return;
//        }

//        SetDefaultProperties(annotation);
//        annotation.PropertyChanged += OnAnnotationPropertyChanged;
//        annotation.PropertyChanging += OnAnnotationPropertyChanging;
//        AddAnnotationWithoutNotification(annotation);
//        OnAnnotationAdded(annotation);
//        if (annotation is StickyNoteAnnotation stickyNoteAnnotation && stickyNoteAnnotation.IsNewlyAdded)
//        {
//            hoveredStickyNote?.Unfocus();
//            stickyNoteAnnotation.Focus();
//            if (string.IsNullOrEmpty(stickyNoteAnnotation.Text))
//            {
//                ShowStickyNoteDialog(stickyNoteAnnotation);
//                hoveredStickyNote = stickyNoteAnnotation;
//            }

//            stickyNoteAnnotation.IsNewlyAdded = false;
//        }

//        modificationMade = true;
//        UpdateUndoRedoButtons();
//    }

//    //
//    // Summary:
//    //     Add the bookmark to the specified page number with the given name.
//    //
//    // Parameters:
//    //   pageNumber:
//    //
//    //   name:
//    public void Bookmark(int pageNumber, string name)
//    {
//        if (annotationsLoader != null)
//        {
//            annotationsLoader.CustomBookmarks.Add(new Bookmark(name, pageNumber));
//        }
//    }

//    private void AddEraseredInkAnnotion(Annotation annotation)
//    {
//        if (annotation != null)
//        {
//            SetDefaultProperties(annotation);
//            annotation.PropertyChanged += OnAnnotationPropertyChanged;
//            annotation.PropertyChanging += OnAnnotationPropertyChanging;
//            AddAnnotationWithoutNotification(annotation);
//            this.AnnotationAdded?.Invoke(this, new AnnotationEventArgs(annotation));
//        }
//    }

//    private void SetDefaultProperties(Annotation annotation)
//    {
//        if (annotation is SquareAnnotation squareAnnotation)
//        {
//            if (squareAnnotation.Opacity == -1f)
//            {
//                squareAnnotation.SetOpacity(AnnotationSettings.Square.Opacity);
//            }

//            if (squareAnnotation.BorderWidth == -1f)
//            {
//                squareAnnotation.SetThickness(AnnotationSettings.Square.BorderWidth);
//            }

//            if (squareAnnotation.Color == Colors.Transparent)
//            {
//                squareAnnotation.SetColor(AnnotationSettings.Square.Color);
//            }

//            if (squareAnnotation.FillColor == Colors.Transparent)
//            {
//                squareAnnotation.SetFillColor(AnnotationSettings.Square.FillColor);
//            }
//        }
//        else if (annotation is CircleAnnotation circleAnnotation)
//        {
//            if (circleAnnotation.Opacity == -1f)
//            {
//                circleAnnotation.SetOpacity(AnnotationSettings.Circle.Opacity);
//            }

//            if (circleAnnotation.BorderWidth == -1f)
//            {
//                circleAnnotation.SetThickness(AnnotationSettings.Circle.BorderWidth);
//            }

//            if (circleAnnotation.Color == Colors.Transparent)
//            {
//                circleAnnotation.SetColor(AnnotationSettings.Circle.Color);
//            }

//            if (circleAnnotation.FillColor == Colors.Transparent)
//            {
//                circleAnnotation.SetFillColor(AnnotationSettings.Circle.FillColor);
//            }
//        }
//        else if (annotation is LineAnnotation lineAnnotation)
//        {
//            if (lineAnnotation.Opacity == -1f)
//            {
//                lineAnnotation.SetOpacity(AnnotationSettings.Line.Opacity);
//            }

//            if (lineAnnotation.BorderWidth == -1f)
//            {
//                lineAnnotation.SetThickness(AnnotationSettings.Line.BorderWidth);
//            }

//            if (lineAnnotation.Color == Colors.Transparent)
//            {
//                lineAnnotation.SetColor(AnnotationSettings.Line.Color);
//            }
//        }
//        else if (annotation is InkAnnotation inkAnnotation)
//        {
//            if (inkAnnotation.Opacity == -1f)
//            {
//                inkAnnotation.SetOpacity(AnnotationSettings.Ink.Opacity);
//            }

//            if (inkAnnotation.BorderWidth == -1f)
//            {
//                inkAnnotation.SetThickness(AnnotationSettings.Ink.BorderWidth);
//            }

//            if (inkAnnotation.Color == Colors.Transparent)
//            {
//                inkAnnotation.SetColor(AnnotationSettings.Ink.Color);
//            }
//        }
//        else if (annotation is PolylineAnnotation polylineAnnotation)
//        {
//            if (polylineAnnotation.Opacity == -1f)
//            {
//                polylineAnnotation.SetOpacity(AnnotationSettings.Polyline.Opacity);
//            }

//            if (polylineAnnotation.BorderWidth == -1f)
//            {
//                polylineAnnotation.SetThickness(AnnotationSettings.Polyline.BorderWidth);
//            }

//            if (polylineAnnotation.Color == Colors.Transparent)
//            {
//                polylineAnnotation.SetColor(AnnotationSettings.Polyline.Color);
//            }
//        }
//        else if (annotation is PolygonAnnotation polygonAnnotation)
//        {
//            if (polygonAnnotation.Opacity == -1f)
//            {
//                polygonAnnotation.SetOpacity(AnnotationSettings.Polygon.Opacity);
//            }

//            if (polygonAnnotation.BorderWidth == -1f)
//            {
//                polygonAnnotation.SetThickness(AnnotationSettings.Polygon.BorderWidth);
//            }

//            if (polygonAnnotation.Color == Colors.Transparent)
//            {
//                polygonAnnotation.SetColor(AnnotationSettings.Polygon.Color);
//            }

//            if (polygonAnnotation.FillColor == Colors.Transparent)
//            {
//                polygonAnnotation.SetFillColor(AnnotationSettings.Polygon.FillColor);
//            }
//        }
//        else if (annotation is StampAnnotation stampAnnotation)
//        {
//            if (stampAnnotation.Opacity == -1f)
//            {
//                stampAnnotation.SetOpacity(AnnotationSettings.Stamp.Opacity);
//            }
//        }
//        else if (annotation is HighlightAnnotation highlightAnnotation)
//        {
//            if (highlightAnnotation.Opacity == -1f)
//            {
//                highlightAnnotation.SetOpacity(AnnotationSettings.Highlight.Opacity);
//            }

//            if (highlightAnnotation.Color == Colors.Transparent)
//            {
//                highlightAnnotation.SetColor(AnnotationSettings.Highlight.Color);
//            }
//        }
//        else if (annotation is UnderlineAnnotation underlineAnnotation)
//        {
//            if (underlineAnnotation.Opacity == -1f)
//            {
//                underlineAnnotation.SetOpacity(AnnotationSettings.Underline.Opacity);
//            }

//            if (underlineAnnotation.Color == Colors.Transparent)
//            {
//                underlineAnnotation.SetColor(AnnotationSettings.Underline.Color);
//            }
//        }
//        else if (annotation is SquigglyAnnotation squigglyAnnotation)
//        {
//            if (squigglyAnnotation.Opacity == -1f)
//            {
//                squigglyAnnotation.SetOpacity(AnnotationSettings.Squiggly.Opacity);
//            }

//            if (squigglyAnnotation.Color == Colors.Transparent)
//            {
//                squigglyAnnotation.SetColor(AnnotationSettings.Squiggly.Color);
//            }
//        }
//        else if (annotation is StrikeOutAnnotation strikeOutAnnotation)
//        {
//            if (strikeOutAnnotation.Opacity == -1f)
//            {
//                strikeOutAnnotation.SetOpacity(AnnotationSettings.StrikeOut.Opacity);
//            }

//            if (strikeOutAnnotation.Color == Colors.Transparent)
//            {
//                strikeOutAnnotation.SetColor(AnnotationSettings.StrikeOut.Color);
//            }
//        }
//        else if (annotation is StickyNoteAnnotation stickyNoteAnnotation)
//        {
//            if (stickyNoteAnnotation.Opacity == -1f)
//            {
//                stickyNoteAnnotation.SetOpacity(AnnotationSettings.StickyNote.Opacity);
//            }

//            if (stickyNoteAnnotation.Color == Colors.Transparent)
//            {
//                stickyNoteAnnotation.SetColor(AnnotationSettings.StickyNote.Color);
//            }

//            if (stickyNoteAnnotation.DateTime == string.Empty)
//            {
//                stickyNoteAnnotation.DateTime = DateTime.Now.ToString("MMM dd ; hh:mm tt");
//            }
//        }
//        else if (annotation is FreeTextAnnotation freeTextAnnotation)
//        {
//            if (freeTextAnnotation.Opacity == -1f)
//            {
//                freeTextAnnotation.SetOpacity(AnnotationSettings.FreeText.Opacity);
//            }

//            if (freeTextAnnotation.Color == Colors.Transparent)
//            {
//                freeTextAnnotation.Color = AnnotationSettings.FreeText.Color;
//            }

//            if (freeTextAnnotation.FontSize == 0.0)
//            {
//                freeTextAnnotation.FontSize = AnnotationSettings.FreeText.FontSize;
//            }

//            if (freeTextAnnotation.FillColor == Colors.Transparent)
//            {
//                freeTextAnnotation.SetFillColor(AnnotationSettings.FreeText.FillColor);
//            }

//            if (freeTextAnnotation.BorderColor == Colors.Transparent)
//            {
//                freeTextAnnotation.BorderColor = AnnotationSettings.FreeText.BorderColor;
//            }

//            if (freeTextAnnotation.BorderWidth == 0f)
//            {
//                freeTextAnnotation.BorderWidth = AnnotationSettings.FreeText.BorderWidth;
//            }
//        }
//    }

//    //
//    // Summary:
//    //     Removes the given annotation from the page.
//    //
//    // Parameters:
//    //   annotation:
//    //     The annotation to be removed.
//    public void RemoveAnnotation(Annotation annotation)
//    {
//        if (annotation != null && AnnotationSettings.CanEdit(annotation))
//        {
//            RemoveAnnotationWithoutNotification(annotation);
//            OnAnnotationRemoved(annotation);
//            if (RemoveAllAnnotationsCommand is Command command)
//            {
//                command.ChangeCanExecute();
//            }
//        }
//    }

//    private void RemoveErasedInkAnnotation(Annotation annotation)
//    {
//        RemoveAnnotationWithoutNotification(annotation);
//        this.AnnotationRemoved?.Invoke(this, new AnnotationEventArgs(annotation));
//    }

//    private void RemoveAnnotationWithoutNotification(Annotation annotation)
//    {
//        if (annotation.IsSelected)
//        {
//            DeselectAnnotation(annotation);
//        }

//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            DocumentView?.RemoveAnnotation(annotation);
//        }
//        else
//        {
//            SinglePageViewContainer?.RemoveAnnotation(annotation);
//        }
//    }

//    private void AddAnnotationWithoutNotification(Annotation annotation)
//    {
//        if (DocumentView == null)
//        {
//            BackupAnnotation(annotation);
//        }

//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            DocumentView?.AddAnnotation(annotation);
//        }
//        else
//        {
//            SinglePageViewContainer?.AddAnnotation(annotation);
//        }
//    }

//    //
//    // Summary:
//    //     Removes all annotations from the PDF document.
//    public void RemoveAllAnnotations()
//    {
//        if (AnnotationSettings.IsLocked)
//        {
//            return;
//        }

//        List<Annotation> list = Annotations.Where((Annotation annot) => AnnotationSettings.CanEdit(annot)).ToList();
//        if (list.Count <= 0)
//        {
//            return;
//        }

//        foreach (Annotation item in list)
//        {
//            RemoveAnnotationWithoutNotification(item);
//            this.AnnotationRemoved?.Invoke(this, new AnnotationEventArgs(item));
//        }

//        CheckChangeTrackerReady();
//        AnnotationAddRemoveCommand command = new AnnotationAddRemoveCommand(list, AddAnnotations, RemoveAnnotations);
//        changeTracker?.RegisterChange(command);
//    }

//    private void AddAnnotations(List<Annotation> annotations)
//    {
//        foreach (Annotation annotation in annotations)
//        {
//            AddAnnotationWithoutNotification(annotation);
//        }

//        CheckChangeTrackerReady();
//        AnnotationAddRemoveCommand command = new AnnotationAddRemoveCommand(annotations, RemoveAnnotations, AddAnnotations);
//        changeTracker?.RegisterChange(command);
//    }

//    private void RemoveAnnotations(List<Annotation> annotations)
//    {
//        if (AnnotationSettings.IsLocked)
//        {
//            return;
//        }

//        foreach (Annotation annotation in annotations)
//        {
//            RemoveAnnotation(annotation);
//        }

//        CheckChangeTrackerReady();
//        AnnotationAddRemoveCommand command = new AnnotationAddRemoveCommand(annotations, AddAnnotations, RemoveAnnotations);
//        changeTracker?.RegisterChange(command);
//    }

//    //
//    // Summary:
//    //     Selects the given annotation.
//    //
//    // Parameters:
//    //   annotation:
//    //     The annotation to be selected.
//    public void SelectAnnotation(Annotation annotation)
//    {
//        if (annotation != SelectedAnnotation)
//        {
//            if (SelectedAnnotation != null)
//            {
//                DocumentView?.DeselectAnnotation(SelectedAnnotation);
//            }

//            GoToPage(annotation.PageNumber);
//            double num = (double)annotation.BoundingBox.Y * AnnotationConstants.PageSizeMultiplier - (double)AnnotationConstants.SelectionBorderMargin;
//            if (HorizontalOffset.HasValue && VerticalOffset.HasValue)
//            {
//                ScrollToOffset(HorizontalOffset.Value, (VerticalOffset + num).Value);
//            }

//            if (PageLayoutMode == PageLayoutMode.Continuous)
//            {
//                DocumentView?.SelectAnnotation(annotation);
//            }
//            else
//            {
//                SinglePageViewContainer?.SelectAnnotation(annotation);
//            }

//            SelectedAnnotation = annotation;
//        }
//    }

//    internal void OpenOrCloseAnnotationEditToolbar(Annotation? selectedAnnotation, bool IsOpen)
//    {
//        if (selectedAnnotation is SquareAnnotation)
//        {
//            RenderToolbarView(RectangleSecondaryAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is CircleAnnotation)
//        {
//            RenderToolbarView(CircleSecondaryAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is LineAnnotation lineAnnotation)
//        {
//            if (lineAnnotation.LineEndingStyle == LineEndingStyle.None)
//            {
//                RenderToolbarView(LineSecondaryAnnotationToolbarViewDesktop, IsOpen);
//            }
//            else if (lineAnnotation.LineEndingStyle == LineEndingStyle.Open)
//            {
//                RenderToolbarView(ArrowSecondaryAnnotationToolbarViewDesktop, IsOpen);
//            }
//        }
//        else if (selectedAnnotation is PolygonAnnotation)
//        {
//            RenderToolbarView(PolygonSecondaryAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is PolylineAnnotation)
//        {
//            RenderToolbarView(PolylineSecondaryAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is FreeTextAnnotation)
//        {
//            RenderToolbarView(FreeTextSecondaryAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is InkAnnotation inkAnnotation && !inkAnnotation.IsSignature)
//        {
//            RenderToolbarView(InkSecondaryAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is HighlightAnnotation)
//        {
//            RenderToolbarView(HighlightAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is UnderlineAnnotation)
//        {
//            RenderToolbarView(UnderlineAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is StrikeOutAnnotation)
//        {
//            RenderToolbarView(StrikeOutAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is SquigglyAnnotation)
//        {
//            RenderToolbarView(SquigglyAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is StickyNoteAnnotation)
//        {
//            RenderToolbarView(StickyNoteSecondaryAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation is StampAnnotation stampAnnotation && !stampAnnotation.IsSignature)
//        {
//            RenderToolbarView(StampAnnotationToolbarViewDesktop, IsOpen);
//        }
//        else if (selectedAnnotation == null && previousToolbar != null && (AnnotationMode == AnnotationMode.None || AnnotationMode == AnnotationMode.Signature))
//        {
//            previousToolbar.IsVisible = false;
//        }
//    }

//    internal void OnAnnotationSelected(Annotation annotation)
//    {
//        this.AnnotationSelected(this, new AnnotationEventArgs(annotation));
//    }

//    //
//    // Summary:
//    //     Deselects the given annotation.
//    //
//    // Parameters:
//    //   annotation:
//    //     Deselects the given annotation.
//    public void DeselectAnnotation(Annotation? annotation)
//    {
//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            DocumentView?.DeselectAnnotation(annotation);
//        }
//        else
//        {
//            SinglePageViewContainer?.DeselectAnnotation(annotation);
//        }

//        OpenOrCloseAnnotationEditToolbar(annotation, IsOpen: false);
//        if (annotation == SelectedAnnotation)
//        {
//            SelectedAnnotation = null;
//        }
//    }

//    private void OnSelectionChanged(object? sender, EventArgs e)
//    {
//        if (m_textInfoManager == null || !(sender is TextInfoManager textInfoManager))
//        {
//            return;
//        }

//        TextSelectionChangedEventArgs textSelectionChangedEventArgs = new TextSelectionChangedEventArgs(textInfoManager.SelectedText.ToString(), textInfoManager.CurrentSelectionPageNumber);
//        if (this.TextSelectionChanged != null)
//        {
//            List<Rect> selectedTextLineBoundsInPixels = textInfoManager.SelectedTextLineBoundsInPixels;
//            if (selectedTextLineBoundsInPixels != null && selectedTextLineBoundsInPixels.Count > 0 && m_documentManager != null)
//            {
//                for (int i = 0; i < selectedTextLineBoundsInPixels.Count; i++)
//                {
//                    textSelectionChangedEventArgs.SelectedTextLineBounds.Add(m_documentManager.ConvertClientPageRectToPdfPageRect(selectedTextLineBoundsInPixels[i], textInfoManager.CurrentSelectionPageNumber - 1));
//                }
//            }

//            this.TextSelectionChanged(this, textSelectionChangedEventArgs);
//        }

//        if (AnnotationMode == AnnotationMode.Highlight)
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.Highlight);
//            textSelectionChangedEventArgs.Handled = true;
//        }
//        else if (AnnotationMode == AnnotationMode.Underline)
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.Underline);
//            textSelectionChangedEventArgs.Handled = true;
//        }
//        else if (AnnotationMode == AnnotationMode.StrikeOut)
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.StrikeOut);
//            textSelectionChangedEventArgs.Handled = true;
//        }
//        else if (AnnotationMode == AnnotationMode.Squiggly)
//        {
//            AddTextMarkupAnnotation(TextMarkupAnnotationType.Squiggly);
//            textSelectionChangedEventArgs.Handled = true;
//        }

//        if (PageLayoutMode.Equals(PageLayoutMode.Continuous))
//        {
//            DocumentView?.SetFlowDirection(base.FlowDirection);
//            DocumentView?.ShowContextMenu(textSelectionChangedEventArgs);
//        }
//        else
//        {
//            SinglePageViewContainer?.SetFlowDirection(base.FlowDirection);
//            SinglePageViewContainer?.ShowContextMenu(textSelectionChangedEventArgs);
//        }
//    }

//    //
//    // Summary:
//    //     Jumps to the first page of a document.
//    public void GoToFirstPage()
//    {
//        GoToPage(1);
//    }

//    //
//    // Summary:
//    //     Jumps to the last page of a document.
//    public void GoToLastPage()
//    {
//        GoToPage(PageCount);
//    }

//    //
//    // Summary:
//    //     Jumps to the previous page of a document.
//    public void GoToPreviousPage()
//    {
//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            GoToPage(PageNumber - 1);
//        }
//        else if (PageLayoutMode == PageLayoutMode.Single)
//        {
//            GoToPreviousPageInSinglePageViewMode();
//        }
//    }

//    //
//    // Summary:
//    //     Jumps to the next page of a document.
//    public void GoToNextPage()
//    {
//        if (PageLayoutMode == PageLayoutMode.Continuous)
//        {
//            GoToPage(PageNumber + 1);
//        }
//        else if (PageLayoutMode == PageLayoutMode.Single)
//        {
//            GoToNextPageInSinglePageViewMode();
//        }
//    }

//    private async void GoToNextPageInSinglePageViewMode()
//    {
//        if (SinglePageViewContainer != null)
//        {
//            await SinglePageViewContainer.GoToNextPage((float)SinglePageViewContainer.TranslationX);
//        }
//    }

//    private async void GoToPreviousPageInSinglePageViewMode()
//    {
//        if (SinglePageViewContainer != null)
//        {
//            await SinglePageViewContainer.GoToPreviousPage((float)SinglePageViewContainer.TranslationX);
//        }
//    }

//    //
//    // Summary:
//    //     Jumps to the specified bookmark of a document.
//    //
//    // Parameters:
//    //   customBookmark:
//    public void GoToBookmark(Bookmark customBookmark)
//    {
//        GoToPage(customBookmark.PageNumber);
//    }

//    //
//    // Summary:
//    //     Prints the PDF document.
//    public void PrintDocument()
//    {
//        MemoryStream memoryStream = new MemoryStream();
//        annotationsLoader?.SaveDocumentForPrinting(memoryStream);
//        if (memoryStream.Length != 0)
//        {
//            if (printService == null)
//            {
//                printService = new PrintService();
//            }

//            printService.Print(memoryStream, base.Window.Handler, PageCount);
//        }
//    }

//    //
//    // Summary:
//    //     Saves the PDF document and writes the final document into the given output stream.
//    //
//    //
//    // Parameters:
//    //   outputStream:
//    //     The stream to which the PDF document is saved.
//    public void SaveDocument(Stream outputStream)
//    {
//        annotationsLoader?.Save(outputStream);
//    }

//    //
//    // Summary:
//    //     Saves the PDF document and writes the final document into the given output stream.
//    //
//    //
//    // Parameters:
//    //   outputStream:
//    //     The stream to which the PDF document is saved.
//    //
//    //   cancellationToken:
//    //     The token to receive notice of the cancellation.
//    public async Task SaveDocumentAsync(Stream outputStream, CancellationToken cancellationToken = default(CancellationToken))
//    {
//        Stream outputStream2 = outputStream;
//        await Task.Run(delegate
//        {
//            annotationsLoader?.Save(outputStream2, cancellationToken);
//        });
//    }

//    //
//    // Summary:
//    //     Imports annotation data into the PDF document.
//    //
//    // Parameters:
//    //   annotationDataStream:
//    //     The stream to which the PDF document is saved.
//    //
//    //   annotationDataFormat:
//    //     The data format of the input stream.
//    //
//    //   cancellationToken:
//    //     The token to receive notice of the cancellation.
//    public async Task ImportAnnotationsAsync(Stream annotationDataStream, AnnotationDataFormat annotationDataFormat, CancellationToken cancellationToken = default(CancellationToken))
//    {
//        Stream annotationDataStream2 = annotationDataStream;
//        await Task.Run(delegate
//        {
//            List<Annotation> list = annotationsLoader?.ImportAndGetAnnotations(annotationDataStream2, annotationDataFormat, cancellationToken);
//            if (list != null && list.Any() && !cancellationToken.IsCancellationRequested)
//            {
//                AddAnnotations(list);
//            }
//        });
//    }

//    //
//    // Summary:
//    //     Exports the annotations in the PDF document to the specified format and writes
//    //     the exported data to the given output stream.
//    //
//    // Parameters:
//    //   stream:
//    //     The stream that contains the exported annotation data.
//    //
//    //   dataFormat:
//    //     The data format in which the annotations should be exported.
//    //
//    //   annotations:
//    //     The annotations to be exported.
//    //
//    // Remarks:
//    //     If the annotations parameter is not null, only the annotations specified in the
//    //     collection will be exported and other annotations will be ignored.
//    public void ExportAnnotations(Stream stream, AnnotationDataFormat dataFormat, List<Annotation>? annotations = null)
//    {
//        annotationsLoader?.ExportAnnotations(stream, dataFormat, CancellationToken.None, annotations);
//    }

//    //
//    // Summary:
//    //     Exports the annotations in the PDF document to the specified format and writes
//    //     the exported data to the given output stream.
//    //
//    // Parameters:
//    //   stream:
//    //     The stream that contains the exported annotation data.
//    //
//    //   dataFormat:
//    //     The data format in which the annotations should be exported.
//    //
//    //   annotations:
//    //     The annotations to be exported.
//    //
//    //   cancellationToken:
//    //     The token to receive notice of the cancellation.
//    //
//    // Remarks:
//    //     If the annotations parameter is not null, only the annotations specified in the
//    //     collection will be exported and other annotations will be ignored.
//    public async Task ExportAnnotationsAsync(Stream stream, AnnotationDataFormat dataFormat, List<Annotation>? annotations = null, CancellationToken cancellationToken = default(CancellationToken))
//    {
//        Stream stream2 = stream;
//        List<Annotation> annotations2 = annotations;
//        await Task.Run(delegate
//        {
//            annotationsLoader?.ExportAnnotations(stream2, dataFormat, cancellationToken, annotations2);
//        });
//    }

//    //
//    // Summary:
//    //     Exports the form data in the PDF document to the specified format and writes
//    //     the exported data to the given output stream.
//    //
//    // Parameters:
//    //   stream:
//    //     The stream that contains the exported form data.
//    //
//    //   dataFormat:
//    //     The data format in which the form data should be exported.
//    public void ExportFormData(Stream stream, DataFormat dataFormat)
//    {
//        annotationsLoader?.ExportFormData(stream, dataFormat, CancellationToken.None);
//    }

//    //
//    // Summary:
//    //     Exports the form data in the PDF document to the specified format and writes
//    //     the exported data to the given output stream.
//    //
//    // Parameters:
//    //   stream:
//    //
//    //   dataFormat:
//    //
//    //   cancellationToken:
//    public async Task ExportFormDataAsync(Stream stream, DataFormat dataFormat, CancellationToken cancellationToken = default(CancellationToken))
//    {
//        Stream stream2 = stream;
//        await Task.Run(delegate
//        {
//            annotationsLoader?.ExportFormData(stream2, dataFormat, cancellationToken);
//        });
//    }

//    //
//    // Summary:
//    //     Imports annotation data into the PDF document.
//    //
//    // Parameters:
//    //   stream:
//    //     The stream that contains the annotation data to be imported.
//    //
//    //   annotationDataFormat:
//    //     The data format of the input stream.
//    public void ImportAnnotations(Stream stream, AnnotationDataFormat annotationDataFormat)
//    {
//        List<Annotation> list = annotationsLoader?.ImportAndGetAnnotations(stream, annotationDataFormat, default(CancellationToken));
//        if (list != null && list.Any())
//        {
//            AddAnnotations(list);
//        }
//    }

//    //
//    // Summary:
//    //     Imports form data into the PDF document.
//    //
//    // Parameters:
//    //   stream:
//    //
//    //   dataFormat:
//    //
//    //   continueImportOnError:
//    public void ImportFormData(Stream stream, DataFormat dataFormat, bool continueImportOnError = false)
//    {
//        ImportFormData(stream, dataFormat, continueImportOnError, CancellationToken.None);
//    }

//    private void ImportFormData(Stream stream, DataFormat dataFormat, bool continueImportOnError, CancellationToken cancellationToken)
//    {
//        if (!FormFields.Any())
//        {
//            return;
//        }

//        List<FormFieldChangeRecord> list = new List<FormFieldChangeRecord>();
//        foreach (FormField formField in FormFields)
//        {
//            object formFieldValue = GetFormFieldValue(formField);
//            if (!(formField is SignatureFormField))
//            {
//                FormFieldChangeRecord item = new FormFieldChangeRecord(formField)
//                {
//                    OldValue = formFieldValue
//                };
//                list.Add(item);
//            }
//        }

//        annotationsLoader?.ImportFormData(stream, dataFormat, continueImportOnError, cancellationToken);
//        foreach (FormFieldChangeRecord item2 in list)
//        {
//            item2.NewValue = GetFormFieldValue(item2.FormField);
//        }

//        CheckChangeTrackerReady();
//        FormFieldEditCommand command = new FormFieldEditCommand(list);
//        changeTracker?.RegisterChange(command);
//    }

//    //
//    // Summary:
//    //     Imports form data into the PDF document.
//    //
//    // Parameters:
//    //   stream:
//    //
//    //   dataFormat:
//    //
//    //   continueImportOnError:
//    //
//    //   cancellationToken:
//    public async Task ImportFormDataAsync(Stream stream, DataFormat dataFormat, bool continueImportOnError = false, CancellationToken cancellationToken = default(CancellationToken))
//    {
//        Stream stream2 = stream;
//        await Task.Run(delegate
//        {
//            ImportFormData(stream2, dataFormat, continueImportOnError, cancellationToken);
//        });
//    }

//    //
//    // Summary:
//    //     Unloads the current PDF document asynchronously.
//    public async Task UnloadDocumentAsync()
//    {
//        await Task.Run(delegate
//        {
//            Unload();
//        });
//    }

//    //
//    // Summary:
//    //     Loads the PDF document asynchronously from the specified stream.
//    //
//    // Parameters:
//    //   documentStream:
//    //     The document stream.
//    //
//    //   password:
//    //     Password for the document. It is required to open a password protected document.
//    //
//    //
//    //   flattenOptions:
//    //     The option that represents whether annotations or form fields should be flattened.
//    //
//    //
//    //   cancellationTokenSource:
//    //     The cancellation token source for cancelling the task.
//    //
//    // Returns:
//    //     Returns the task
//    public async Task LoadDocumentAsync(Stream? documentStream, string? password = null, FlattenOptions? flattenOptions = FlattenOptions.None, CancellationTokenSource? cancellationTokenSource = null)
//    {
//        Stream documentStream2 = documentStream;
//        string password2 = password;
//        if (m_isControlLoaded)
//        {
//            CancelCurrentProgress();
//            while (m_documentManager != null && m_documentManager.IsLoadingInProgress)
//            {
//            }

//            m_cancellationTokenSource = ((cancellationTokenSource != null) ? cancellationTokenSource : new CancellationTokenSource());
//            await Task.Run(delegate
//            {
//                LoadDocument(documentStream2, password2, flattenOptions);
//            });
//        }
//        else
//        {
//            BackupLoadInputs(documentStream2, password2, flattenOptions, cancellationTokenSource, isAsynchronousRequest: true);
//        }
//    }

//    //
//    // Summary:
//    //     Loads the PDF document from the specified stream.
//    //
//    // Parameters:
//    //   documentStream:
//    //     The document stream.
//    //
//    //   password:
//    //     Password for the document. It is required to open a password protected document.
//    //
//    //
//    //   flattenOptions:
//    //     The option that represents whether annotations or form fields should be flattened.
//    public void LoadDocument(Stream? documentStream, string? password = null, FlattenOptions? flattenOptions = FlattenOptions.None)
//    {
//        LoadingIndicator?.CanShowBusy(value: true);
//        if (m_isControlLoaded)
//        {
//            CheckReadyToLoad(password);
//            m_documentManager?.LoadNewDocument(documentStream, password, flattenOptions);
//        }
//        else
//        {
//            BackupLoadInputs(documentStream, password, flattenOptions);
//        }
//    }

//    //
//    // Summary:
//    //     Loads the PDF document asynchronously from the specified bytes.
//    //
//    // Parameters:
//    //   documentBytes:
//    //     The document as byte array.
//    //
//    //   password:
//    //     Password for the document. It is required to open a password protected document.
//    //
//    //
//    //   flattenOptions:
//    //     The option that represents whether annotations or form fields should be flattened.
//    //
//    //
//    //   cancellationTokenSource:
//    //     The cancellation token source for cancelling the task.
//    //
//    // Returns:
//    //     Returns the task
//    public async Task LoadDocumentAsync(byte[] documentBytes, string? password = null, FlattenOptions? flattenOptions = FlattenOptions.None, CancellationTokenSource? cancellationTokenSource = null)
//    {
//        byte[] documentBytes2 = documentBytes;
//        string password2 = password;
//        if (m_isControlLoaded)
//        {
//            CancelCurrentProgress();
//            while (m_documentManager != null && m_documentManager.IsLoadingInProgress)
//            {
//            }

//            m_cancellationTokenSource = ((cancellationTokenSource != null) ? cancellationTokenSource : new CancellationTokenSource());
//            await Task.Run(delegate
//            {
//                LoadDocument(documentBytes2, password2, flattenOptions);
//            });
//        }
//        else
//        {
//            BackupLoadInputs(documentBytes2, password2, flattenOptions, cancellationTokenSource, isAsynchronousRequest: true);
//        }
//    }

//    //
//    // Summary:
//    //     Loads the PDF document from the specified bytes.
//    //
//    // Parameters:
//    //   documentBytes:
//    //     The document as byte array
//    //
//    //   password:
//    //     Password for the document. It is required to open a password protected document.
//    //
//    //
//    //   flattenOptions:
//    //     The option that represents whether annotations or form fields should be flattened.
//    public void LoadDocument(byte[] documentBytes, string? password = null, FlattenOptions? flattenOptions = FlattenOptions.None)
//    {
//        LoadingIndicator?.CanShowBusy(value: true);
//        if (m_isControlLoaded)
//        {
//            CheckReadyToLoad(password);
//            m_documentManager?.LoadNewDocument(documentBytes, password, flattenOptions);
//        }
//        else
//        {
//            BackupLoadInputs(documentBytes, password, flattenOptions);
//        }
//    }

//    //
//    // Summary:
//    //     Clears all form data.
//    //
//    // Parameters:
//    //   pageNumber:
//    //     The page number in which all form fields should be cleared and the fields in
//    //     the other pages will be left unchanged. This parameter ranges from 1 to the total
//    //     page count of the document.
//    public void ClearFormData(int pageNumber = 0)
//    {
//        if (!FormFields.Any())
//        {
//            return;
//        }

//        List<FormFieldChangeRecord> list = new List<FormFieldChangeRecord>();
//        List<FormField> list2 = FormFields.ToList();
//        if (pageNumber > 0 && pageNumber <= PageCount)
//        {
//            list2 = list2.Where((FormField x) => x.PageNumber == pageNumber).ToList();
//        }

//        foreach (FormField item2 in list2)
//        {
//            object formFieldValue = GetFormFieldValue(item2);
//            item2.Reset();
//            object formFieldValue2 = GetFormFieldValue(item2);
//            if (!(item2 is SignatureFormField))
//            {
//                FormFieldChangeRecord item = new FormFieldChangeRecord(item2, formFieldValue, formFieldValue2);
//                list.Add(item);
//            }
//        }

//        CheckChangeTrackerReady();
//        FormFieldEditCommand command = new FormFieldEditCommand(list);
//        changeTracker?.RegisterChange(command);
//    }

//    private object? GetFormFieldValue(FormField formField)
//    {
//        if (formField is TextFormField textFormField)
//        {
//            return textFormField.Text;
//        }

//        if (formField is CheckboxFormField checkboxFormField)
//        {
//            return checkboxFormField.IsChecked;
//        }

//        if (formField is RadioButtonFormField radioButtonFormField)
//        {
//            return radioButtonFormField.SelectedItem;
//        }

//        if (formField is ComboBoxFormField comboBoxFormField)
//        {
//            return comboBoxFormField.SelectedItem;
//        }

//        if (formField is ListBoxFormField listBoxFormField)
//        {
//            return listBoxFormField.SelectedItems;
//        }

//        return null;
//    }

//    //
//    // Summary:
//    //     Scrolls the content to the specified offset.
//    //
//    // Parameters:
//    //   horizontalOffset:
//    //     The value to which the control scrolls horizontally. The value is measured in
//    //     device-independent units.
//    //
//    //   verticalOffset:
//    //     The value to which the control scrolls vertically. The value is measured in device-independent
//    //     units.
//    public void ScrollToOffset(double horizontalOffset, double verticalOffset)
//    {
//        if (m_isDocumentLoaded)
//        {
//            if (PageLayoutMode == PageLayoutMode.Continuous)
//            {
//                ScrollView?.ScrollToAsync(horizontalOffset, verticalOffset, animated: false);
//            }
//            else
//            {
//                GetCurrentSinglePageView(PageNumber)?.ScrollByAsync(horizontalOffset, verticalOffset, animated: false);
//            }
//        }
//        else
//        {
//            m_scrollToRequest = new Microsoft.Maui.Graphics.Point(horizontalOffset, verticalOffset);
//        }
//    }

//    //
//    // Summary:
//    //     Jumps to the specified page in a document.
//    //
//    // Parameters:
//    //   pageNumber:
//    //     The number of the page to which the control should go.
//    public void GoToPage(int pageNumber)
//    {
//        if (m_isDocumentLoaded)
//        {
//            if (PageLayoutMode == PageLayoutMode.Continuous)
//            {
//                m_viewportManager?.GoToPage(pageNumber - 1);
//            }
//            else if (SinglePageViewContainer != null)
//            {
//                SinglePageViewContainer.Intialize(pageNumber);
//                m_viewportManager?.GoToPage(pageNumber - 1);
//            }
//        }
//        else
//        {
//            m_goToPageRequest = pageNumber;
//        }
//    }

//    //
//    // Summary:
//    //     Navigates to the location of the given outline element.
//    public void GoToOutlineElement(OutlineElement? outlineElement)
//    {
//        if (outlineElement != null)
//        {
//            int pageIndex = outlineElement.PageIndex;
//            Microsoft.Maui.Graphics.PointF destination = new Microsoft.Maui.Graphics.PointF(outlineElement.Destination.X, outlineElement.Destination.Y);
//            ScrollToLocation(pageIndex, destination);
//        }
//    }

//    internal void ScrollToLocation(int pageIndex, Microsoft.Maui.Graphics.PointF destination)
//    {
//        if (m_documentManager != null && m_documentManager.PagesBoundsInfo != null)
//        {
//            double num = m_documentManager.PagesBoundsInfo[pageIndex].Top * ZoomFactor;
//            double num2 = m_documentManager.PagesBoundsInfo[pageIndex].Left * ZoomFactor;
//            double num3 = (double)destination.X * m_documentManager.PageSizeMultiplier * ZoomFactor;
//            double num4 = (double)destination.Y * m_documentManager.PageSizeMultiplier * ZoomFactor;
//            if (PageLayoutMode == PageLayoutMode.Continuous)
//            {
//                ScrollToOffset(num3 + num2, num4 + num);
//                return;
//            }

//            GoToPage(pageIndex + 1);
//            ScrollToOffset(num3, num4);
//        }
//    }

//    private void HideToolbar()
//    {
//        if (ToolbarGridView?.RowDefinitions != null && BottomToolbarGridViewMobile != null && TopToolbarGridViewMobile != null)
//        {
//            ToolbarGridView.RowDefinitions[0].Height = new GridLength(0.0);
//            ToolbarGridView.RowDefinitions[2].Height = new GridLength(0.0);
//            BottomToolbarGridViewMobile.IsVisible = false;
//            TopToolbarGridViewMobile.IsVisible = false;
//            if (ColorToolbarViewMobile?.FixedToolbarView != null)
//            {
//                ColorToolbarViewMobile.FixedToolbarView.IsVisible = false;
//                ColorToolbarViewMobile.IsStrokeColorToolbarVisible = false;
//                ColorToolbarViewMobile.IsFillColorToolbarVisible = false;
//            }

//            if (SliderToolbarViewMobile?.FixedToolbarView != null)
//            {
//                SliderToolbarViewMobile.FixedToolbarView.IsVisible = false;
//                SliderToolbarViewMobile.IsThicknessToolbarVisible = false;
//                SliderToolbarViewMobile.IsFillOpacityToolbarVisible = false;
//            }

//            StickyNoteIconsViewMobile.ScrollableToolbar.IsVisible = false;
//            if (MoreOptionToolbarLayoutViewMobile != null)
//            {
//                MoreOptionToolbarLayoutViewMobile.IsVisible = false;
//            }

//            if (PageSettingsLayoutViewMobile != null)
//            {
//                PageSettingsLayoutViewMobile.IsVisible = false;
//            }

//            if (ZoomModeViewMobile != null)
//            {
//                ZoomModeViewMobile.IsVisible = false;
//            }

//            if (MatchCaseToolbarLayoutMobile != null)
//            {
//                MatchCaseToolbarLayoutMobile.IsVisible = false;
//            }

//            if (ScrollableIndicatorEnd != null && ScrollableIndicatorStart != null)
//            {
//                ScrollableIndicatorEnd.IsVisible = false;
//                ScrollableIndicatorStart.IsVisible = false;
//            }

//            if (ShapeAnnotationsToolbarViewMobile != null && ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorEnd != null && ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorStart != null)
//            {
//                ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorEnd.IsVisible = false;
//                ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorStart.IsVisible = false;
//            }
//        }

//        if (ToolbarGridView?.RowDefinitions != null && PrimaryToolbarGridViewDesktop != null && SecondaryToolbarGridViewDesktop != null)
//        {
//            ToolbarGridView.RowDefinitions[0].Height = new GridLength(0.0);
//            ToolbarGridView.RowDefinitions[1].Height = new GridLength(0.0);
//            PrimaryToolbarGridViewDesktop.IsVisible = false;
//            SecondaryToolbarGridViewDesktop.IsVisible = false;
//            AnnotationsToolbarViewDesktop.ScrollableToolbar.IsVisible = false;
//            CloseOverlayToolbar();
//            if (AnnotationMode == AnnotationMode.Signature)
//            {
//                AnnotationMode = AnnotationMode.None;
//            }
//        }

//        if (ToolbarGridView?.RowDefinitions != null && PrimaryToolbarGridViewDesktop != null && SecondaryToolbarGridViewDesktop == null)
//        {
//            ToolbarGridView.RowDefinitions[0].Height = new GridLength(0.0);
//            ToolbarGridView.RowDefinitions[1].Height = new GridLength(0.0);
//            PrimaryToolbarGridViewDesktop.IsVisible = false;
//        }
//    }

//    private void ShowToolbar()
//    {
//        if (ToolbarGridView?.RowDefinitions != null && BottomToolbarGridViewMobile != null && TopToolbarGridViewMobile != null)
//        {
//            ToolbarGridView.RowDefinitions[0].Height = new GridLength(64.0);
//            ToolbarGridView.RowDefinitions[2].Height = new GridLength(64.0);
//            BottomToolbarGridViewMobile.IsVisible = true;
//            TopToolbarGridViewMobile.IsVisible = true;
//            if (ScrollableIndicatorEnd != null && ScrollableIndicatorStart != null)
//            {
//                ScrollableIndicatorEnd.IsVisible = !ScrollableIndicatorEnd.IsVisible;
//            }

//            if (ShapeAnnotationsToolbarViewMobile != null && ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorEnd != null && ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorStart != null)
//            {
//                ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorEnd.IsVisible = !ShapeAnnotationsToolbarViewMobile.ScrollableIndicatorEnd.IsVisible;
//            }
//        }

//        if (ToolbarGridView?.RowDefinitions != null && PrimaryToolbarGridViewDesktop != null && SecondaryToolbarGridViewDesktop != null)
//        {
//            ToolbarGridView.RowDefinitions[0].Height = new GridLength(56.0);
//            ToolbarGridView.RowDefinitions[1].Height = new GridLength(0.0);
//            PrimaryToolbarGridViewDesktop.IsVisible = true;
//        }

//        if (ToolbarGridView?.RowDefinitions != null && PrimaryToolbarGridViewDesktop != null && SecondaryToolbarGridViewDesktop == null)
//        {
//            ToolbarGridView.RowDefinitions[0].Height = new GridLength(56.0);
//            ToolbarGridView.RowDefinitions[1].Height = new GridLength(0.0);
//            PrimaryToolbarGridViewDesktop.IsVisible = true;
//        }
//    }

//    private void ShowOutlineView()
//    {
//        CloseAllDialogs();
//        if (OutlineView == null && m_isDocumentLoaded && m_documentManager != null && m_documentManager.LoadedDocument != null)
//        {
//            OutlineView = new OutlineView();
//            OutlineView.AssignCustomBookmarkCollection(annotationsLoader.CustomBookmarks);
//            OutlineView.UpdateOutlineViewPageNumber(PageNumber);
//            if (base.FlowDirection == FlowDirection.RightToLeft)
//            {
//                OutlineView.FlowDirection = FlowDirection.RightToLeft;
//            }

//            OutlineView.PopulateInitialBookmarks(m_documentManager.LoadedDocument.Bookmarks);
//            OutlineView.PopulateCustomBookmarks();
//            OutlineView.OutlineListViewItemTapped += OutlineView_OutlineListViewItemTapped;
//            OutlineView.CustomBookmarkItemTapped += OutlineView_CustomBookmarkItemTapped;
//            OutlineView.AddCustomBookmarkPressed += OutlineView_AddCustomBookmarkPressed;
//            DispatcherUtils.Dispatch(this, delegate
//            {
//                ScrollViewGridDesktop?.Add(OutlineView);
//            });
//        }
//        else if (OutlineView != null && OutlineView.Parent == null && m_isDocumentLoaded)
//        {
//            OutlineView.ClearSelection();
//            DispatcherUtils.Dispatch(this, delegate
//            {
//                ScrollViewGridDesktop?.Add(OutlineView);
//            });
//        }

//        if (DocumentView != null)
//        {
//            AutomationProperties.SetIsInAccessibleTree(DocumentView, false);
//        }
//    }

//    private void OutlineView_OutlineListViewItemTapped(object? sender, EventArgs? e)
//    {
//        if (sender != null && m_documentManager != null && m_documentManager.PagesBoundsInfo != null)
//        {
//            BookmarkPaneBindingContext bookmarkPaneBindingContext = (BookmarkPaneBindingContext)sender;
//            int pageIndex = bookmarkPaneBindingContext.PageIndex;
//            Microsoft.Maui.Graphics.PointF destination = new Microsoft.Maui.Graphics.PointF(bookmarkPaneBindingContext.Destination.X, bookmarkPaneBindingContext.Destination.Y);
//            ScrollToLocation(pageIndex, destination);
//        }
//    }

//    private void OutlineView_AddCustomBookmarkPressed(object? sender, EventArgs? e)
//    {
//        if (sender != null)
//        {
//            string name = "Page " + PageNumber;
//            annotationsLoader.CustomBookmarks.Add(new Bookmark(name, PageNumber));
//        }
//    }

//    private void OutlineView_CustomBookmarkItemTapped(object? sender, EventArgs? e)
//    {
//        if (sender != null && sender is CustomBookmarkBindingContext customBookmarkBindingContext)
//        {
//            GoToBookmark(customBookmarkBindingContext.Bookmark);
//        }
//    }

//    internal void CopySelectedText()
//    {
//        DocumentView?.CopySelectedText();
//    }

//    private void CancelCurrentProgress()
//    {
//        if (m_cancellationTokenSource != null)
//        {
//            m_cancellationTokenSource?.Cancel();
//            m_cancellationTokenSource = null;
//        }
//    }

//    internal void UnloadViews()
//    {
//        ScrollView?.Unload();
//        UpdateExtentDimensions();
//        DocumentView?.Unload();
//        SinglePageViewContainer?.Unload();
//        CloseDialog(messageDialog);
//        CloseDialog(hyperlinkDialog);
//        CloseDialog(StickyNoteDialog);
//        CloseDialog(comboBoxFormFieldListDialog);
//        CloseDialog(listBoxFormFieldListDialog);
//        CloseDialog(freeTextDialog);
//        UnloadOutlineView();
//    }

//    private void CloseComboBoxListView()
//    {
//        DocumentView?.CloseComboBoxListView();
//    }

//    internal void CloseAllDialogs()
//    {
//        CloseDialog(passwordDialog);
//        CloseDialog(messageDialog);
//        CloseDialog(hyperlinkDialog);
//        CloseDialog(StickyNoteDialog);
//        CloseDialog(comboBoxFormFieldListDialog);
//        CloseDialog(listBoxFormFieldListDialog);
//        CloseSignatureDialog();
//    }

//    internal void CloseRenameEntry()
//    {
//        if (OutlineView != null)
//        {
//            OutlineView.CloseRenameEntry();
//        }
//    }

//    internal void CloseOverlayToolbar()
//    {
//        HideTextMarkupAnnotationDialog();
//        HideShapeAnnotationDialog();
//        CloseColorPalette();
//        CloseOpacityPalette();
//        CloseFreeTextColorPalette();
//        CloseThicknessPalette();
//        CloseStrokeAndFillColorPalette();
//        CloseSearchDialog();
//        CloseStampDialogs();
//        CloseStickyNoteIconsListViewDesktop();
//        CloseViewModeListViewDesktop();
//        ClosePageSizeListView();
//        CloseFontSizeListView();
//    }

//    internal void UnfocusFormFields()
//    {
//        CloseComboBoxListView();
//        DocumentView?.UnfocusFormFields();
//        SinglePageViewContainer?.UnfocusFormFields();
//    }

//    private void UnloadOutlineView()
//    {
//        documentOutline?.Clear();
//        if (OutlineView != null)
//        {
//            OutlineView.Unload();
//        }
//    }

//    //
//    // Summary:
//    //     Unloads the current PDF document.
//    public void UnloadDocument()
//    {
//        Unload();
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            passwordDialog?.Reset();
//        });
//    }

//    internal void ResetState()
//    {
//        CloseComboBoxListView();
//        changeTracker?.ClearStacks();
//        SelectedAnnotation = null;
//        AnnotationMode = AnnotationMode.None;
//    }

//    internal void Unload()
//    {
//        ResetState();
//        m_isDocumentLoaded = false;
//        UnloadDocumentResources();
//        m_textSearchManager?.Clear();
//        m_textSearchManager = null;
//        hoveredStickyNote = null;
//        SignatureHelper.CurrentSignatureItem = null;
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            UnloadViews();
//            this.DocumentUnloaded?.Invoke(this, EventArgs.Empty);
//        });
//        DispatcherUtils.Dispatch(this, delegate
//        {
//            TopToolbarDesktop?.DisablePrimaryToolbarIcons();
//        });
//    }

//    private Rect GetCurrentPageBounds(int pageNumber)
//    {
//        if (m_documentManager != null && m_documentManager.PagesBoundsInfo != null && pageNumber != 0)
//        {
//            Dictionary<int, Rect> pagesBoundsInfo = m_documentManager.PagesBoundsInfo;
//            Rect result = pagesBoundsInfo[pageNumber - 1];
//            if (PageLayoutMode == PageLayoutMode.Single)
//            {
//                result.Y = 0.0;
//            }

//            return result;
//        }

//        return new Rect(0.0, 0.0, -1.0, -1.0);
//    }

//    private Microsoft.Maui.Graphics.Size GetCurrentContentSize()
//    {
//        if (ScrollView != null)
//        {
//            Microsoft.Maui.Graphics.Size contentSize = ScrollView.ContentSize;
//            if (PageLayoutMode == PageLayoutMode.Single)
//            {
//                SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//                if (currentSinglePageView != null)
//                {
//                    _ = currentSinglePageView.ContentSize;
//                    if (true)
//                    {
//                        contentSize = currentSinglePageView.ContentSize;
//                    }
//                }
//            }

//            return contentSize;
//        }

//        return new Microsoft.Maui.Graphics.Size(-1.0, -1.0);
//    }

//    private Microsoft.Maui.Graphics.Size GetCurrentExtentSize()
//    {
//        if (ScrollView != null)
//        {
//            Microsoft.Maui.Graphics.Size? size = ScrollView.ExtentSize;
//            if (PageLayoutMode == PageLayoutMode.Single)
//            {
//                SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//                if (currentSinglePageView != null && currentSinglePageView.ExtentSize.HasValue)
//                {
//                    size = currentSinglePageView.ExtentSize.Value;
//                }
//            }

//            return size ?? new Microsoft.Maui.Graphics.Size(-1.0, -1.0);
//        }

//        return new Microsoft.Maui.Graphics.Size(-1.0, -1.0);
//    }

//    private double GetScrollYforSinglePageView(double scrollY)
//    {
//        if (PageLayoutMode == PageLayoutMode.Single)
//        {
//            SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//            if (currentSinglePageView?.ViewportManager != null)
//            {
//                scrollY = currentSinglePageView.ViewportManager.VerticalOffset;
//                if (currentSinglePageView.ExtentSize.HasValue && scrollY > currentSinglePageView.ExtentSize.Value.Height)
//                {
//                    scrollY = 0.0;
//                }
//            }
//        }

//        return scrollY;
//    }

//    //
//    // Summary:
//    //     Converts a point from the client rectangle (viewport) area coordinates to PDF
//    //     page coordinates.
//    //
//    // Parameters:
//    //   clientPoint:
//    //     The client point to convert. The client point coordinates start from the top-left
//    //     corner of the client rectangle (viewport) area of the PDF Viewer.
//    //
//    //   pageNumber:
//    //     The number of the page on which the point conversion will be computed. The required
//    //     page number on the client position can also be obtained from the Syncfusion.Maui.PdfViewer.SfPdfViewer.GetPageNumberFromClientPoint(Microsoft.Maui.Graphics.Point)
//    //     method.
//    public Microsoft.Maui.Graphics.Point ConvertClientPointToPagePoint(Microsoft.Maui.Graphics.Point clientPoint, int pageNumber)
//    {
//        if (m_documentManager != null && ScrollView != null && DocumentView != null)
//        {
//            if (pageNumber <= 0 || m_documentManager.PagesBoundsInfo == null)
//            {
//                return Microsoft.Maui.Graphics.Point.Zero;
//            }

//            double num = 0.0;
//            double num2 = 0.0;
//            if (HorizontalOffset.HasValue)
//            {
//                num = HorizontalOffset.Value;
//            }

//            if (VerticalOffset.HasValue)
//            {
//                num2 = VerticalOffset.Value;
//            }

//            Dictionary<int, Rect> pagesBoundsInfo = m_documentManager.PagesBoundsInfo;
//            Rect rect = pagesBoundsInfo[pageNumber - 1];
//            double num3 = rect.Top / m_documentManager.PageSizeMultiplier;
//            double num4 = (ScrollView.ContentSize.Width - rect.Width * ZoomFactor) / 2.0;
//            double num5 = (ScrollView.ContentSize.Height - DocumentView.Height * ZoomFactor) / 2.0;
//            double x = (clientPoint.X - num4 + num) / (m_documentManager.PageSizeMultiplier * ZoomFactor);
//            double y = (clientPoint.Y - num5 + num2) / (m_documentManager.PageSizeMultiplier * ZoomFactor) - num3;
//            return new Microsoft.Maui.Graphics.Point(x, y);
//        }

//        return Microsoft.Maui.Graphics.Point.Zero;
//    }

//    //
//    // Summary:
//    //     Converts a point from PDF page coordinates to client rectangle (viewport) area
//    //     coordinates.
//    //
//    // Parameters:
//    //   pagePoint:
//    //     The PDF page point to convert. The page point coordinates start from the top-left
//    //     corner of the page corresponding to the given page number.
//    //
//    //   pageNumber:
//    //     The number of the page on which the point conversion will be computed.
//    public Microsoft.Maui.Graphics.Point ConvertPagePointToClientPoint(Microsoft.Maui.Graphics.Point pagePoint, int pageNumber)
//    {
//        if (m_documentManager != null && ScrollView != null && ScrollView.ExtentSize.HasValue)
//        {
//            if (pageNumber <= 0 || m_documentManager.PagesBoundsInfo == null)
//            {
//                return Microsoft.Maui.Graphics.Point.Zero;
//            }

//            double num = (HorizontalOffset.HasValue ? HorizontalOffset.Value : 0.0);
//            double scrollY = (VerticalOffset.HasValue ? VerticalOffset.Value : 0.0);
//            Rect currentPageBounds = GetCurrentPageBounds(pageNumber);
//            Microsoft.Maui.Graphics.Size currentContentSize = GetCurrentContentSize();
//            Microsoft.Maui.Graphics.Size currentExtentSize = GetCurrentExtentSize();
//            scrollY = GetScrollYforSinglePageView(scrollY);
//            double num2 = currentPageBounds.Top * ZoomFactor;
//            double num3 = (currentContentSize.Width - currentPageBounds.Width * ZoomFactor) / 2.0;
//            double num4 = (currentContentSize.Height - currentExtentSize.Height) / 2.0;
//            double x = pagePoint.X * m_documentManager.PageSizeMultiplier * ZoomFactor - num + num3;
//            double y = pagePoint.Y * m_documentManager.PageSizeMultiplier * ZoomFactor + num2 - scrollY + num4;
//            return new Microsoft.Maui.Graphics.Point(x, y);
//        }

//        return Microsoft.Maui.Graphics.Point.Zero;
//    }

//    //
//    // Summary:
//    //     Converts a point from the PDF page coordinates to the scroll area coordinates
//    //     of the PDF Viewer.
//    //
//    // Parameters:
//    //   pagePoint:
//    //     The PDF page point to convert. The page point coordinates start from the top-left
//    //     corner of the page corresponding to the given page number.
//    //
//    //   pageNumber:
//    //     The number of the PDF page on which the point will be computed.
//    //
//    // Remarks:
//    //     The scroll coordinates start at the top-left of the first page and end at the
//    //     bottom-right of the last page. The scroll coordinates are subject to Zoom changes.
//    public Microsoft.Maui.Graphics.Point ConvertPagePointToScrollPoint(Microsoft.Maui.Graphics.Point pagePoint, int pageNumber)
//    {
//        if (pageNumber > 0)
//        {
//            double num = 0.0;
//            double num2 = 0.0;
//            if (HorizontalOffset.HasValue)
//            {
//                num = HorizontalOffset.Value;
//            }

//            if (VerticalOffset.HasValue)
//            {
//                num2 = VerticalOffset.Value;
//            }

//            Microsoft.Maui.Graphics.Point point = ConvertPagePointToClientPoint(pagePoint, pageNumber);
//            return new Microsoft.Maui.Graphics.Point(point.X + num, point.Y + num2);
//        }

//        return Microsoft.Maui.Graphics.Point.Zero;
//    }

//    //
//    // Summary:
//    //     Gets the page number corresponding to the client point.
//    //
//    // Parameters:
//    //   clientPoint:
//    //     The client point on which the page number need to be computed. The client point
//    //     coordinates start from the top-left corner of the client rectangle (viewport)
//    //     area of the PDF Viewer.
//    //
//    // Remarks:
//    //     It returns -1, when computed from an invalid area such as page gaps.
//    public int GetPageNumberFromClientPoint(Microsoft.Maui.Graphics.Point clientPoint)
//    {
//        if (m_viewportManager != null && m_documentManager != null && m_viewportManager.CurrentViewportInfo != null && m_documentManager.PagesBoundsInfo != null)
//        {
//            double num = 0.0;
//            double num2 = 0.0;
//            if (HorizontalOffset.HasValue)
//            {
//                num = HorizontalOffset.Value;
//            }

//            if (VerticalOffset.HasValue)
//            {
//                num2 = VerticalOffset.Value;
//            }

//            Microsoft.Maui.Graphics.Point pt = new Microsoft.Maui.Graphics.Point(clientPoint.X + num, clientPoint.Y + num2);
//            int num3 = m_viewportManager.StartingPageNumber - 1;
//            for (int i = num3; i < num3 + m_viewportManager.CurrentViewportInfo.Count; i++)
//            {
//                Rect rect = m_documentManager.PagesBoundsInfo[i];
//                Microsoft.Maui.Graphics.Size? size = ScrollView?.ContentSize;
//                if (size.HasValue)
//                {
//                    rect.Width *= ZoomFactor;
//                    rect.Height *= ZoomFactor;
//                    rect.X = (size.Value.Width - rect.Width) / 2.0;
//                    rect.Y *= ZoomFactor;
//                    if (rect.Contains(pt))
//                    {
//                        return i + 1;
//                    }
//                }
//            }
//        }

//        return -1;
//    }

//    //
//    // Summary:
//    //     Serves event for the mouse action action on the view in the ink eraser.
//    //
//    // Parameters:
//    //   e:
//    //     MAUIToolkit.Core.Internals.PointerEventArgs
//    void ITouchListener.OnTouch(MAUIToolkit.Core.Internals.PointerEventArgs e)
//    {
//        bool flag = true;
//        Microsoft.Maui.Graphics.Point point = new Microsoft.Maui.Graphics.Point(e.TouchPoint.X, e.TouchPoint.Y - ToolbarGridView.RowDefinitions[0].Height.Value - ToolbarGridView.RowDefinitions[1].Height.Value);
//        if (AnnotationSettings.Ink.TouchScreenInputMode == TouchScreenInputMode.Stylus && e.PointerDeviceType != PointerDeviceType.Stylus)
//        {
//            flag = false;
//            PointerActions action = e.Action;
//            PointerActions pointerActions = action;
//            if (pointerActions == PointerActions.Moved && eraserPointer != null)
//            {
//                if (point.Y < eraserPointer.Bounds.Height / 4.0)
//                {
//                    eraserPointer.IsVisible = false;
//                }
//                else
//                {
//                    eraserPointer.IsVisible = true;
//                }

//                float x2 = (float)point.X - AnnotationSettings.InkEraser.Thickness / 2f;
//                float y = (float)point.Y - AnnotationSettings.InkEraser.Thickness / 2f;
//                float thickness = AnnotationSettings.InkEraser.Thickness;
//                float thickness2 = AnnotationSettings.InkEraser.Thickness;
//                AbsoluteLayout.SetLayoutBounds((BindableObject)eraserPointer, (Rect)new RectF(x2, y, thickness, thickness2));
//                eraserPressed = false;
//            }
//        }

//        if (!(eraserPointer != null && flag))
//        {
//            return;
//        }

//        switch (e.Action)
//        {
//            case PointerActions.Pressed:
//                {
//                    eraserPressed = true;
//                    if (eraserPointer == null)
//                    {
//                        break;
//                    }

//                    ScrollView.IsScrolling = true;
//                    int pageNumber = ConversionUtil.GetPageNumberFromClientPoint(point, m_viewportManager);
//                    List<Annotation> list2 = Annotations.Where((Annotation x) => x is InkAnnotation && x.PageNumber == pageNumber).ToList();
//                    foreach (InkAnnotation item2 in list2)
//                    {
//                        InkAnnotation item = new InkAnnotation(item2.PointsCollection, pageNumber);
//                        BackUpAnnotations.Add(item);
//                    }

//                    Microsoft.Maui.Graphics.PointF pagePoint = ConversionUtil.ConvertClientPointToPagePoint(point, pageNumber, ScrollView.ContentSize, DocumentView.Height, m_viewportManager, m_documentManager.PageSizeMultiplier, PageLayoutMode);
//                    if (PageLayoutMode == PageLayoutMode.Single)
//                    {
//                        SinglePageScrollView currentSinglePageView2 = GetCurrentSinglePageView(PageNumber);
//                        if (currentSinglePageView2 != null && currentSinglePageView2.Content != null && m_viewportManager != null)
//                        {
//                            pagePoint = ConversionUtil.ConvertClientPointToPagePoint(point, PageNumber, currentSinglePageView2.ContentSize, currentSinglePageView2.Content.Height, m_viewportManager, m_documentManager.PageSizeMultiplier, PageLayoutMode);
//                        }
//                    }

//                    List<Annotation> list3 = Annotations.Where((Annotation x) => x is InkAnnotation inkAnnotation4 && x.PageNumber == pageNumber && inkAnnotation4.Bounds.Contains(pagePoint)).ToList();
//                    if (list3.Count <= 0)
//                    {
//                        break;
//                    }

//                    {
//                        foreach (Annotation item3 in list3)
//                        {
//                            if (item3 is InkAnnotation inkAnnotation3 && !inkAnnotation3.IsSignature)
//                            {
//                                if (PageLayoutMode == PageLayoutMode.Continuous)
//                                {
//                                    DocumentView?.EraseInkPoints(item3, pagePoint, eraserPointer.Thickness);
//                                }
//                                else
//                                {
//                                    SinglePageViewContainer?.EraseInkPoints(item3, pagePoint, eraserPointer.Thickness);
//                                }
//                            }
//                        }

//                        break;
//                    }
//                }
//            case PointerActions.Moved:
//                {
//                    if (point.Y < eraserPointer.Bounds.Height / 4.0)
//                    {
//                        eraserPointer.IsVisible = false;
//                    }
//                    else
//                    {
//                        eraserPointer.IsVisible = true;
//                    }

//                    float x3 = (float)point.X - AnnotationSettings.InkEraser.Thickness / 2f;
//                    float y2 = (float)point.Y - AnnotationSettings.InkEraser.Thickness / 2f;
//                    float thickness3 = AnnotationSettings.InkEraser.Thickness;
//                    float thickness4 = AnnotationSettings.InkEraser.Thickness;
//                    AbsoluteLayout.SetLayoutBounds((BindableObject)eraserPointer, (Rect)new RectF(x3, y2, thickness3, thickness4));
//                    if (!eraserPressed)
//                    {
//                        break;
//                    }

//                    int pageNumber2 = ConversionUtil.GetPageNumberFromClientPoint(point, m_viewportManager);
//                    Microsoft.Maui.Graphics.PointF pagePoint2 = ConversionUtil.ConvertClientPointToPagePoint(point, pageNumber2, ScrollView.ContentSize, DocumentView.Height, m_viewportManager, m_documentManager.PageSizeMultiplier, PageLayoutMode);
//                    if (PageLayoutMode == PageLayoutMode.Single)
//                    {
//                        pageNumber2 = PageNumber;
//                        SinglePageScrollView currentSinglePageView = GetCurrentSinglePageView(PageNumber);
//                        if (currentSinglePageView != null && currentSinglePageView.Content != null && m_viewportManager != null)
//                        {
//                            pagePoint2 = ConversionUtil.ConvertClientPointToPagePoint(point, pageNumber2, currentSinglePageView.ContentSize, currentSinglePageView.Content.Height, m_viewportManager, m_documentManager.PageSizeMultiplier, PageLayoutMode);
//                        }
//                    }

//                    List<Annotation> list = Annotations.Where((Annotation x) => x is InkAnnotation inkAnnotation6 && x.PageNumber == pageNumber2 && inkAnnotation6.Bounds.Contains(pagePoint2)).ToList();
//                    if (list.Count <= 0)
//                    {
//                        break;
//                    }

//                    {
//                        foreach (Annotation item4 in list)
//                        {
//                            if (item4 is InkAnnotation inkAnnotation && !inkAnnotation.IsSignature)
//                            {
//                                if (PageLayoutMode == PageLayoutMode.Continuous)
//                                {
//                                    DocumentView?.EraseInkPoints(item4, pagePoint2, eraserPointer.Thickness);
//                                }
//                                else
//                                {
//                                    SinglePageViewContainer?.EraseInkPoints(item4, pagePoint2, eraserPointer.Thickness);
//                                }

//                                LastTouchPoint = point;
//                            }
//                        }

//                        break;
//                    }
//                }
//        }
//    }

//    private void M_panZoomListener_OnTouch(object? sender, MAUIToolkit.Core.Internals.PointerEventArgs e)
//    {
//        bool flag = true;
//        if (AnnotationSettings.Ink.TouchScreenInputMode == TouchScreenInputMode.Stylus && e.PointerDeviceType != PointerDeviceType.Stylus)
//        {
//            flag = false;
//            eraserPressed = false;
//        }

//        if (!(AnnotationMode == AnnotationMode.InkEraser && flag) || e.Action != PointerActions.Released)
//        {
//            return;
//        }

//        eraserPressed = false;
//        ScrollView.IsScrolling = false;
//        int pageNumber = ConversionUtil.GetPageNumberFromClientPoint(LastTouchPoint, m_viewportManager);
//        List<Annotation> list = Annotations.Where((Annotation x) => x is InkAnnotation && x.PageNumber == pageNumber).ToList();
//        int num = 0;
//        List<InkAnnotation> list2 = new List<InkAnnotation>();
//        List<List<List<float>>> list3 = new List<List<List<float>>>();
//        List<List<List<float>>> list4 = new List<List<List<float>>>();
//        foreach (InkAnnotation item in list)
//        {
//            if (item.PointsCollection != BackUpAnnotations[num].PointsCollection)
//            {
//                list2.Add(item);
//                list3.Add(BackUpAnnotations[num].PointsCollection);
//                list4.Add(item.PointsCollection);
//                if (item.PointsCollection.Count == 0)
//                {
//                    RemoveErasedInkAnnotation(item);
//                }
//                else
//                {
//                    AnnotationEventArgs e2 = new AnnotationEventArgs(item);
//                    this.AnnotationEdited?.Invoke(this, e2);
//                }
//            }

//            num++;
//        }

//        if (list2.Count > 0)
//        {
//            CheckChangeTrackerReady();
//            InkErasedCommand command = new InkErasedCommand(list2, list3, list4, AddEraseredInkAnnotion, RemoveErasedInkAnnotation);
//            changeTracker?.RegisterChange(command);
//        }

//        BackUpAnnotations.Clear();
//    }
//}