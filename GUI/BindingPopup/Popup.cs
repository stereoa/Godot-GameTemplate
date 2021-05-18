using Godot;
using GodotGameTemplate.Core;

namespace GodotGameTemplate.Gui
{
    public class Popup : Godot.Popup
    {

        [Signal]
        public delegate void NewControl();
        private SettingsManager _settingsManager;

        public InputEvent NewEvent;

        public override void _Ready()
        {
            _settingsManager = GetNode<SettingsManager>("/root/SettingsManager");

            PopupExclusive = true;
            SetProcessInput(false);
            Connect("about_to_show", this, "ReceiveInput");

            //Localization
            _settingsManager = GetNode<SettingsManager>("/root/SettingsManager");
            _settingsManager.Connect("Retranslate", this, "Retranslate");
            Retranslate();
        }
        public void ReceiveInput()
        {
            SetProcessInput(true);
            GetFocusOwner().ReleaseFocus();
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (!(inputEvent is InputEventKey) &&
                !(inputEvent is InputEventJoypadButton) &&
                !(inputEvent is InputEventJoypadMotion))
            {
                //only continue if one of those
                return;
            }
            if (!inputEvent.IsPressed())
            {
                return;
            }
            NewEvent = inputEvent;
            EmitSignal("NewControl");
            SetProcessInput(false);
            Visible = false;
        }

        public void OnButtonPressed()
        {
            NewEvent = null;
            EmitSignal("NewControl");
            SetProcessInput(false);
            Visible = false;
        }

        //Localization
        public void Retranslate()
        {
            var cancelButton = (Button)FindNode("Cancel");
            cancelButton.Text = Tr("CANCEL");
            var messageLabel = (Label)FindNode("Message");
            messageLabel.Text = Tr("USE_NEW_CONTROLS");
        }

    }
}