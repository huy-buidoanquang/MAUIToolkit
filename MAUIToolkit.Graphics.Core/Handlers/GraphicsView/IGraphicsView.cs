namespace MAUIToolkit.Graphics.Core
{
    public interface IGraphicsView : IView, IDrawable
    {
        event EventHandler Loaded;
        event EventHandler Unloaded;

        event EventHandler TouchDown;
        event EventHandler TouchMove;
        event EventHandler TouchUp;

        event EventHandler<MultiTouchEventArgs> StartHoverInteraction;
        event EventHandler<MultiTouchEventArgs> MoveHoverInteraction;
        event EventHandler EndHoverInteraction;
        event EventHandler<MultiTouchEventArgs> StartInteraction;
        event EventHandler<MultiTouchEventArgs> DragInteraction;
        event EventHandler<MultiTouchEventArgs> EndInteraction;
        event EventHandler CancelInteraction;

        /// <summary>
        /// Informs the canvas that it needs to redraw itself.
        /// </summary>
        void Invalidate();

        void Load();
        void Unload();

        void OnTouchDown(Point point);
        void OnTouchMove(Point point);
        void OnTouchUp(Point point);

        /// <summary>
        /// Raised when a pointer enters the hit test area of the GraphicsView.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void OnStartHoverInteraction(PointF[] points);

        /// <summary>
        /// Raised when a pointer moves while the pointer remains within the hit test 
        /// area of the GraphicsView.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void OnMoveHoverInteraction(PointF[] points);

        /// <summary>
        /// Raised when a pointer leaves the hit test area of the GraphicsView.
        /// </summary>
        void OnEndHoverInteraction();

        /// <summary>
        /// Raised when the GraphicsView is pressed.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void OnStartInteraction(PointF[] points);

        /// <summary>
        /// Raised when the GraphicsView is dragged.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void OnDragInteraction(PointF[] points);

        /// <summary>
        /// Raised when the press that raised the StartInteraction event is released.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        /// <param name="isInsideBounds">a boolean that indicates if the interaction takes place within the bounds.</param>
        void OnEndInteraction(PointF[] points, bool isInsideBounds);

        /// <summary>
        /// Raised when the press that made contact with the GraphicsView loses contact.
        /// </summary>
        void OnCancelInteraction();
    }
}