using Godot;
using GodotGameTemplate.Core;

namespace GodotGameTemplate.Gui
{
	public class OptionsMenu : CanvasLayer
	{
		private EventManager _eventManager;
		public override void _Ready()
		{
			//show main section and hide controls
			_eventManager = GetNode<EventManager>("/root/EventManager");
			_eventManager.Connect("Options", this, "OnShowOptions");
			_eventManager.ControlsShown = false;
		}
		public void OnShowOptions(bool value)
		{
			GetNode<Control>("Control").Visible = value;
			_eventManager.ControlsShown = false;
		}

	}
}
