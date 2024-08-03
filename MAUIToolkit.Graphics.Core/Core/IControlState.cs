namespace MAUIToolkit.Graphics.Core;

	public interface IControlState
	{
		ControlState CurrentState { get; set; }
		Action<ControlState> StateChanged { get; }
	}