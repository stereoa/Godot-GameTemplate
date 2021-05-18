using Godot;
using GodotGameTemplate.Core;

namespace GodotGameTemplate.Gui
{
	public class HtmlFocus : CanvasLayer
	{
		public override void _Ready()
		{
			if (!Settings.Html5)
			{
				QueueFree();
				return;
			}

			GetNode<Panel>("Panel").Show();
			GetNode<Button>("Button").Show();
		}
		public void OnButtonPressed()
		{
			QueueFree();
		}
	}
}
