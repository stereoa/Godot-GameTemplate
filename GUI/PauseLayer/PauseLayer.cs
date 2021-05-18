using Godot;
using GodotGameTemplate.Core;

namespace GodotGameTemplate.Gui
{
    public class PauseLayer : CanvasLayer
    {
        [Export(PropertyHint.File, "*.tscn")]
        string MainMenu;
        private EventManager _eventManager;
        private SettingsManager _settingsManager;

        public override void _Ready()
        {
            _eventManager = GetNode<EventManager>("/root/EventManager");
            _settingsManager = GetNode<SettingsManager>("/root/SettingsManager");

            _eventManager.Connect("Paused", this, "on_show_paused");
            _eventManager.Connect("Options", this, "on_show_options");
            _eventManager.IsPaused = false;

            //Localization
            _settingsManager.Connect("ReTranslate", this, "retranslate");
        }

        public void on_show_paused(bool visible)
        {
            //Signals allow each module have it's own response logic
            GetNode<Control>("Control").Visible = visible;
            GetTree().Paused = visible;
        }

        public void on_show_options(bool visible)
        {
            if (!_eventManager.MainMenuShown)
            {
                GetNode<Control>("Control").Visible = !visible;
            }
        }

        public void _on_Resume_pressed()
        {
            _eventManager.IsPaused = false;
        }

        public void _on_Restart_pressed()
        {
            _eventManager.EmitSignal("Restart");
            _eventManager.IsPaused = false;
        }

        public void _on_Options_pressed()
        {
            _eventManager.OptionsShown = true;
        }

        public void _on_MainMenu_pressed()
        {

            _eventManager.EmitSignal("ChangeScene", MainMenu);

            _eventManager.IsPaused = false;
        }

        public void _on_Exit_pressed()
        {
            _eventManager.EmitSignal("Exit");
        }

        public void Retranslate()
        {
            var resumeButton = (Button)FindNode("Resume");
            resumeButton.Text = Tr("RESUME");
            var restartButton = (Button)FindNode("Restart");
            restartButton.Text = Tr("RESTART");
            var optionsButton = (Button)FindNode("Options");
            optionsButton.Text = Tr("OPTIONS");
            var mainMenuButton = (Button)FindNode("MainMenu");
            mainMenuButton.Text = Tr("MAIN_MENU");
            var exitButton = (Button)FindNode("Exit");
            exitButton.Text = Tr("EXIT");
        }
    }
}