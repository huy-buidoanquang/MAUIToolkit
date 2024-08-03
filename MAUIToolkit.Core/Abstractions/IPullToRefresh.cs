namespace MAUIToolkit.Core.Abstractions;

//
// Summary:
//     Defines methods and properties that must be implemented to get notification of
//     the pull to refresh actions.
internal interface IPullToRefresh
{
    //
    // Summary:
    //     Gets or sets a value indicating whether to bounce SfDataGrid body region only
    //     with TransitionMode-Push or SfDataGrid to bounce with HeaderRow also.
    bool CanCustomizeContentLayout { get; set; }

    //
    // Summary:
    //     Provides notifications regarding the pulling action with the progress value and
    //     an out parameter to cancel the pulling event.
    //
    // Parameters:
    //   progress:
    //     Progress value of the pulling action.
    //
    //   pullToRefresh:
    //     Instance of PullToRefresh.
    //
    //   cancel:
    //     If cancel is set to true, pulling will be cancelled.
    void Pulling(double progress, object pullToRefresh, out bool cancel);

    //
    // Summary:
    //     Invoked during the refreshing action.
    //
    // Parameters:
    //   pullToRefresh:
    //     Instance of PullToRefresh.
    void Refreshing(object pullToRefresh);

    //
    // Summary:
    //     Invoked when the view refreshing action is completed.
    //
    // Parameters:
    //   pullToRefresh:
    //     Instance of PullToRefresh.
    void Refreshed(object pullToRefresh);

    //
    // Summary:
    //     Invoked when the pulling action is cancelled before the progress meeting 100.
    //
    //
    // Parameters:
    //   pullToRefresh:
    //     Instance of PullToRefresh.
    void PullingCancelled(object pullToRefresh);

    //
    // Summary:
    //     Method that determines whether the gesture should be handled by the child elements
    //     or not.
    //
    // Parameters:
    //   pullToRefresh:
    //     Instance of PullToRefresh.
    //
    // Returns:
    //     Returns a boolean value indicating whether the child elements can handle gestures.
    bool CanHandleGesture(object pullToRefresh);
}

