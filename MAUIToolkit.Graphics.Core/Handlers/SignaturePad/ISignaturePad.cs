namespace MAUIToolkit.Graphics.Core.SignaturePad;

public interface ISignaturePad : IView, IElement, ITransform
{
    //
    // Summary:
    //     Gets or sets the maximum stroke thickness of the signature.
    double MaximumStrokeThickness { get; set; }

    //
    // Summary:
    //     Gets or sets the minimum stroke thickness of the signature.
    double MinimumStrokeThickness { get; set; }

    //
    // Summary:
    //     Gets or sets the stroke color of the signature.
    Color StrokeColor { get; set; }

    //
    // Summary:
    //     Gets or sets the background of the signature.
    new Brush Background { get; set; }

    //
    // Summary:
    //     Calls to convert the drawn signature as an image.
    ImageSource? ToImageSource();

    //
    // Summary:
    //     Calls to clear the drawn signature.
    void Clear();

    //
    // Summary:
    //     Calls when the drawing starts.
    bool StartInteraction();

    //
    // Summary:
    //     Calls when the drawing ends.
    void EndInteraction();
}
