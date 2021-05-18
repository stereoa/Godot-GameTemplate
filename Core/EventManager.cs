using Godot;

namespace GodotGameTemplate.Core
{
    public class EventManager : Node
    {
        [Signal]
        public delegate void ChangeScene();

        [Signal]
        public delegate void MainMenu();

        [Signal]
        public delegate void NewGame();

        [Signal]
        public delegate void Continue();

        [Signal]
        public delegate void Resume();

        [Signal]
        public delegate void Restart();

        [Signal]
        public delegate void Options();

        [Signal]
        public delegate void Controls();

        [Signal]
        public delegate void Languages();

        [Signal]
        public delegate void Paused();

        [Signal]
        public delegate void Exit();

        [Signal]
        public delegate void Refocus();

        //For section tracking
        private bool _mainMenuShown;
        public bool MainMenuShown
        {
            get
            {
                return _mainMenuShown;
            }
            set
            {
                _mainMenuShown = value;
                EmitSignal("MainMenu", _mainMenuShown);
            }
        }

        private bool _optionsShown;
        public bool OptionsShown
        {
            get
            {
                return _optionsShown;
            }
            set
            {
                _optionsShown = value;
                EmitSignal("Options", _optionsShown);
            }
        }

        private bool _controlsShown;
        public bool ControlsShown
        {
            get
            {
                return _controlsShown;
            }
            set
            {
                _controlsShown = value;
                EmitSignal("Controls", _controlsShown);
            }
        }

        private bool _languagesShown;
        public bool LanguagesShown
        {
            get
            {
                return _languagesShown;
            }
            set
            {
                _languagesShown = value;
                EmitSignal("Languages", _languagesShown);
            }
        }

        private bool _isPaused;
        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;
                EmitSignal("Paused", _isPaused);
            }
        }

        public EventManager()
        {
            MainMenuShown = false;
            OptionsShown = false;
            ControlsShown = false;
            LanguagesShown = false;
            IsPaused = false;
        }

    }
}