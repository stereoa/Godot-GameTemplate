using Godot;
using GodotGameTemplate.Core;
using System;

namespace GodotGameTemplate.Gui
{
	public class OptionsMain : VBoxContainer
	{

		private HSlider _masterSlider;
		private HSlider _musicSlider;
		private HSlider _sfxSlider;
		private Panel ResolutionPanel;
		private Panel VolumePanel;
		private Panel LanguagePanel;
		private bool _setUp = true;
		private EventManager _eventManager;
		private SettingsManager _settingsManager;
		private AudioStream _testBeepSound;

		public override void _Ready()
		{
			_eventManager = GetNode<EventManager>("/root/EventManager");
			_settingsManager = GetNode<SettingsManager>("/root/SettingsManager");

			_testBeepSound = ResourceLoader.Load<AudioStream>("res://Assets/Sounds/TestBeep.wav");

			_masterSlider = FindNode("Master").GetNode<HSlider>("HSlider");
			_musicSlider = FindNode("Music").GetNode<HSlider>("HSlider");
			_sfxSlider = FindNode("SFX").GetNode<HSlider>("HSlider");

			ResolutionPanel = (Panel)FindNode("ResolutionPanel");
			VolumePanel = (Panel)FindNode("VolumePanel");
			LanguagePanel = (Panel)FindNode("LanguagePanel");

			//Set up toggles and sliders
			if (Settings.Html5)
			{
				var borderlessNode = (CheckButton)FindNode("Borderless");
				borderlessNode.Visible = false;

				var scaleNode = (HSlider)FindNode("Scale");
				scaleNode.Visible = false;
			}
			SetResolution();
			SetVolumeSliders();
			SetScaleLabel();

			_eventManager.LanguagesShown = false;

			_setUp = false;

			_eventManager.Connect("Controls", this, "on_show_controls");
			_eventManager.Connect("Languages", this, "on_show_languages");

			_settingsManager.Connect("Resized", this, "_on_Resized");
			_settingsManager.Connect("ReTranslate", this, "Retranslate");

			Retranslate();
		}
		public void SetResolution()
		{
			var fullscreenNode = (CheckButton)FindNode("Fullscreen");
			fullscreenNode.Pressed = Settings.Fullscreen;


			var borderlessNode = (CheckButton)FindNode("Borderless");
			borderlessNode.Pressed = Settings.Borderless;
		}
		public void SetVolumeSliders()
		{
			_masterSlider.Value = Settings.VolumeMaster * 100;
			_musicSlider.Value = Settings.VolumeMusic * 100;
			_sfxSlider.Value = Settings.VolumeSfx * 100;
		}

		public void _on_Master_value_changed(float value)
		{
			if (_setUp)
			{
				return;
			}
			
			_settingsManager.SetVolume(SettingsEnums.VolumeType.Master, value / 100);

			var player = FindNode("Master").GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			player.Stream = _testBeepSound;
			player.Play();
		}

		public void _on_Music_value_changed(float value)
		{
			if (_setUp)
			{
				return;
			}

			_settingsManager.SetVolume(SettingsEnums.VolumeType.Music, value / 100);

			var player = FindNode("Music").GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			player.Stream = _testBeepSound;
			player.Play();
		}

		public void _on_SFX_value_changed(float value)
		{
			if (_setUp)
			{
				return;
			}

			_settingsManager.SetVolume(SettingsEnums.VolumeType.SFX, value / 100);

			var player = FindNode("SFX").GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			player.Stream = _testBeepSound;
			player.Play();
		}

		public void _on_Fullscreen_pressed(bool value)
		{
			if (_setUp)
			{
				return;
			}

			_settingsManager.SetFullscreen(value, Settings.Borderless);
		}

		public void _on_Borderless_pressed(bool value)
		{
			if (_setUp)
			{
				return;
			}
			_settingsManager.SetFullscreen(Settings.Fullscreen, value);
		}

		public void _on_ScaleUp_pressed()
		{
			_settingsManager.SetScale(Settings.Scale + 1);
			SetScaleLabel();
		}

		public void _on_ScaleDown_pressed()
		{
			_settingsManager.SetScale(Settings.Scale - 1);
			SetScaleLabel();
		}

		private void SetScaleLabel()
		{
			var scaleLabel = (Label)FindNode("Scale");
			scaleLabel.Text = $"{OS.WindowSize.x} x {OS.WindowSize.y}";
		}

		public void _on_Resized()
		{
			SetResolution();
		}

		public void _on_Controls_pressed()
		{
			_eventManager.ControlsShown = true;
		}

		public void _on_Back_pressed()
		{
			_settingsManager.SaveSettings();
			_eventManager.OptionsShown = false;
		}

		public async void _on_Languages_pressed()
		{
			_eventManager.LanguagesShown = !_eventManager.LanguagesShown;

			if (!_eventManager.LanguagesShown)
			{
				return;
			}
			await ToSignal(_settingsManager, "ReTranslate");

			GD.Print("Language chosen.");

			_eventManager.LanguagesShown = !_eventManager.LanguagesShown;
		}

		//EVENT SIGNALS
		public void on_show_controls(bool visible)
		{
			Visible = !visible;    //because showing controls
		}

		public void on_show_languages(bool visible)
		{
			ResolutionPanel.Visible = !visible;

			VolumePanel.Visible = !visible;
		}

		//Localization
		public void Retranslate()
		{
			var resolutionLabel = (Label)FindNode("Resolution");
			resolutionLabel.Text = Tr("RESOLUTION");

			var volumeLabel = (Label)FindNode("Volume");
			volumeLabel.Text = Tr("VOLUME");

			var languagesLabel = GetNode<Label>("HBoxContainer/LanguagePanel/VBoxContainer/Languages");
			languagesLabel.Text = Tr("LANGUAGES");

			var fullscreenLabel = (CheckButton)FindNode("Fullscreen");
			fullscreenLabel.Text = Tr("FULLSCREEN");

			var borderlessLabel = (CheckButton)FindNode("Borderless");
			borderlessLabel.Text = Tr("BORDERLESS");

			FindNode("Master").GetNode<Label>("ScaleName").Text = Tr("MASTER");

			FindNode("Music").GetNode<Label>("ScaleName").Text = Tr("MUSIC");

			FindNode("SFX").GetNode<Label>("ScaleName").Text = Tr("SFX");

			GetNode<Button>("MarginContainer/VBoxContainer/Languages").Text = Tr("LANGUAGES");

			var controlsButton = (Button)FindNode("Controls");
			controlsButton.Text = Tr("CONTROLS");

			var backButton = (Button)FindNode("Back");
			backButton.Text = Tr("BACK");
		}

		public void SetNodeInFocus()
		{
			var FocusGroup = GetGroups();
		}
	}
}

