using Godot;
using GodotGameTemplate.Core;

namespace GodotGameTemplate.Gui
{
	public class Languages : Panel
	{
		[Signal]

		public delegate void LanguageChosen();

		private EventManager _eventManager;
		private SettingsManager _settingsManager;
		PackedScene _button = ResourceLoader.Load("res://GUI/Buttons/DefaultButton.tscn") as PackedScene;
		HBoxContainer _parent;

		public override void _Ready()
		{
			_parent = GetNode<HBoxContainer>("VBoxContainer/MarginContainer/HBoxContainer");
			_eventManager = GetNode<EventManager>("/root/EventManager");
			_settingsManager = GetNode<SettingsManager>("/root/SettingsManager");
			_eventManager.Connect("Languages", this, "OnShowLanguages");

			foreach (var language in Settings.Languages)
			{
				var newButton = (Button)_button.Instance();
				_parent.AddChild(newButton);
				newButton.Text = "\"" + language.Value + "\"";
				newButton.Connect("pressed", this, "OnButtonPressed", new Godot.Collections.Array { language.Value });
			}
		}

		public void OnShowLanguages(bool visible)
		{
			Visible = visible;
		}

		public void OnButtonPressed(string language)
		{
			_settingsManager.SetLanguage(language);
		}
	}
}
