using Godot;
using GodotGameTemplate.Core;

namespace GodotGameTemplate.Gui
{
	public class MainMenu : CanvasLayer
	{

		[Export(PropertyHint.File, "*.tscn")]
		private string FirstLevel;
		private EventManager _eventManager;
		private SettingsManager _settingsManager;
		private GuiBrain _guiBrain;
		public override void _Ready()
		{
			_eventManager = GetNode<EventManager>("/root/EventManager");
			_settingsManager = GetNode<SettingsManager>("/root/SettingsManager");
			_guiBrain = GetNode<GuiBrain>("/root/GuiBrain");

			_eventManager.MainMenuShown = true;

			_guiBrain.GuiCollectFocusGroup();

			if (Settings.Html5)
			{
				GetNode<Button>("BG/MarginContainer/VBoxMain/HBoxContainer/ButtonContainer/Exit").Visible = false;
			}
			//Localization
			_settingsManager.Connect("ReTranslate", this, "Retranslate");

			Retranslate();
		}

		public override void _Process(float delta)
		{
			GetNode<Panel>("BG").Visible = !_eventManager.OptionsShown;
		}

		public void ExitTree()
		{
			_eventManager.MainMenuShown = false;

			_guiBrain.GuiCollectFocusGroup();
		}

		public void OnNewGamePressed()
		{
			_eventManager.EmitSignal("NewGame");

			_eventManager.EmitSignal("ChangeScene", FirstLevel);
		}

		public void OnOptionsPressed()
		{
			_eventManager.OptionsShown = true;
		}

		public void OnExitPressed()
		{
			_eventManager.EmitSignal("Exit");
		}

		public void Retranslate()
		{
			var newGameLabel = (Button)FindNode("NewGame");
			newGameLabel.Text = Tr("NEW_GAME");

			var optionsLabel = (Button)FindNode("Options");
			optionsLabel.Text = Tr("OPTIONS");

			var exitLabel = (Button)FindNode("Exit");
			exitLabel.Text = Tr("EXIT");
		}
	}
}
