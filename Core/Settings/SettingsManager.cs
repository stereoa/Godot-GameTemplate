using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static GodotGameTemplate.Core.SettingsEnums;

namespace GodotGameTemplate.Core
{
    public class SettingsManager : Node
    {
        public const string CONFIG_FILE = "user://settings.save";

        private bool _settingsLoaded = false;

        [Signal]
        public delegate void Resized();
        [Signal]
        public delegate void ReTranslate();
        
        public override void _Ready()
        {
            SetFullscreen(OS.WindowFullscreen, OS.WindowBorderless);
            SetScale(3);
            Settings.VolumeMaster = 0;
            Settings.VolumeMusic = 0;
            Settings.VolumeSfx = 0;
            Settings.VolumeRange = 24 + 80;

            Settings.Language = TranslationServer.GetLocale();

            if (OS.GetName() == "HTML5")
            {
                Settings.Html5 = true;
            }

            _settingsLoaded = LoadSettings();
            UpdateResolution();
            UpdateVolumes();
            UpdateControls();
            SaveSettings();
        }

        public void SetLanguage(string language)
        {
            Settings.Language = language;
            TranslationServer.SetLocale(language);
            EmitSignal("ReTranslate");
        }

        public void SetScale(int scale)
        {
            Settings.Scale = Mathf.Clamp(scale, 1, Settings.MaxScale);
            if (Settings.Scale >= Settings.MaxScale)
            {
                OS.WindowFullscreen = true;
                Settings.Fullscreen = true;
            }
            else
            {
                //OS.WindowFullscreen = false;
                //Settings.Fullscreen = false;
                OS.WindowSize = Settings.GameResolution * Settings.Scale;
                OS.CenterWindow();
            }
            UpdateResolution();
            EmitSignal("Resized");
        }

        public void SetFullscreen(bool fullscreen, bool borderless)
        {
            Settings.Fullscreen = fullscreen;
            OS.WindowFullscreen = fullscreen;
            Settings.WindowResolution = OS.WindowSize;
            if (fullscreen)
            {
                Settings.Scale = Settings.MaxScale;
            }

            Settings.Borderless = borderless;
            OS.WindowBorderless = borderless;
        }

        public void SetBorder(bool fullscreen)
        {
            Settings.Fullscreen = fullscreen;
            OS.WindowFullscreen = fullscreen;
            Settings.WindowResolution = OS.WindowSize;
            if (fullscreen)
            {
                Settings.Scale = Settings.MaxScale;
            }
        }

        public void SetVolume(VolumeType type, float value)
        {
            string name = string.Empty;

            value = Mathf.Clamp(value, 0, 1);
            switch (type)
            {
                case VolumeType.Master:
                    name = "Master";
                    Settings.VolumeMaster = value;
                    break;
                case VolumeType.Music:
                    name = "Music";
                    Settings.VolumeMusic = value;
                    break;
                case VolumeType.SFX:
                    name = "SFX";
                    Settings.VolumeSfx = value;
                    break;
                case VolumeType.Range:
                    name = "Range";
                    Settings.VolumeRange = value;
                    break;
            }

            var level = Mathf.Lerp(-80, 24, value);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(name), level);
        }
        //RESOLUTION
        public void UpdateResolution()
        {
            Settings.View = GetViewport();
            Settings.ViewRect2 = Settings.View.GetVisibleRect();
            Settings.GameResolution = Settings.ViewRect2.Size;
            Settings.WindowResolution = OS.WindowSize;
            Settings.ScreenResolution = OS.GetScreenSize(OS.CurrentScreen);
            Settings.ScreenAspectRatio = Settings.ScreenResolution.x / Settings.ScreenResolution.y;
            Settings.MaxScale = (int)Mathf.Ceil(Settings.ScreenResolution.y / Settings.GameResolution.y);
        }

        //AUDIO
        public void UpdateVolumes()
        {
            var master = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master"));
            var music = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Music"));
            var sfx = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("SFX"));


            Settings.VolumeMaster = ((master + 80)) / Settings.VolumeRange;
            Settings.VolumeMusic = ((music + 80)) / Settings.VolumeRange;
            Settings.VolumeSfx = ((sfx + 80)) / Settings.VolumeRange;
        }

        //CONTROLS
        public void UpdateControls()
        {
            if (!_settingsLoaded)
            {
                DefaultControls();
            }
            UpdateActionsInfo();
        }

        //Reset to project settings controls
        public void DefaultControls()
        {
            InputMap.LoadFromGlobals();
            UpdateActionsInfo();
        }

        public void UpdateActionsInfo()
        {
            Settings.ActionControls.Clear();

            var actions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>();

            foreach (var action in actions)
            {
                Settings.ActionControls.Add(action, new List<InputBind>());

                //associated controls to the action
                var inputs = InputMap.GetActionList(action.ToString()).Cast<InputEvent>();
                foreach (var input in inputs)
                {
                    var bind = input.ToInputBind();
                    Settings.ActionControls[action].Add(bind);
                }
            }
        }

        //Save/Load
        public bool SaveSettings()
        {
            return StaticSerializer.Save(typeof(Settings), CONFIG_FILE);
        }

        public bool LoadSettings()
        {
            if (Settings.Html5)
            {
                return false;
            }

            if (System.IO.File.Exists(CONFIG_FILE))
            {
                //We don't have a save to load
                return false;
            }

            return StaticSerializer.Load(typeof(Settings), CONFIG_FILE);
        }

        public void SetInputMap()
        {
            var actions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>();
            foreach (var action in actions)
            {
                var name = action.ToString();
                InputMap.ActionEraseEvents(name);
                foreach (var bind in Settings.ActionControls[action])
                {
                    var inputEvent = bind.ToInputEvent();
                    InputMap.ActionAddEvent(name, inputEvent);
                }
            }
        }
    }
}