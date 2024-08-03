namespace MAUIToolkit.Core.Internals.Platforms;

public interface IRightTapGestureListener : IGestureListener
{
    //
    // Parameters:
    //   e:
    void OnRightTap(RightTapEventArgs e);

    //
    // Parameters:
    //   sender:
    //
    //   e:
    void OnRightTap(object sender, RightTapEventArgs e)
    {
    }
}
