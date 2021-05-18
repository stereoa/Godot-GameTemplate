using Godot;
using System.Collections.Generic;

namespace GodotGameTemplate.Core
{
    public class GuiBrain : Node
    {
        [Signal]
        public delegate void NewScrollContainerButton();

        ////Probably all GUI controlling functions will be there to separate mixing functions
        private Control _focusDetect = new Control();
        private Godot.Collections.Array _focusGroup = new Godot.Collections.Array();
        private Dictionary<string, Button> _buttonsSections = new Dictionary<string, Button>();
        private EventManager _eventManager;
        public override void _Ready()
        {
            _eventManager = GetNode<EventManager>("/root/EventManager");

            //Use to detect if no button in focus
            var _focusDetect = new Control();

            //Without this it can't detect buttons in focus
            AddChild(_focusDetect);

            PauseMode = PauseModeEnum.Process;

            SetProcessUnhandledKeyInput(true);


            _eventManager.Connect("Refocus", this, "ForceFocus");
        }

        //Workaround to get initial focus
        public void GuiCollectFocusGroup()
        {
            _focusGroup.Clear();
            _focusGroup = GetTree().GetNodesInGroup("FocusGroup");

            //Save references to call main buttons in sections
            foreach (Button button in _focusGroup)
            {

                var groups = button.GetGroups();

                if (groups.Contains("MainMenu"))
                {
                    _buttonsSections["MainMenu"] = button;
                }

                if (groups.Contains("Pause"))
                {
                    _buttonsSections["Pause"] = button;
                }

                if (groups.Contains("OptionsMain"))
                {
                    _buttonsSections["OptionsMain"] = button;
                }

                if (groups.Contains("OptionsControls"))
                {
                    _buttonsSections["OptionsControls"] = button;
                }
            }
        }
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_cancel"))
            {
                //not in main menu
                if (!_eventManager.MainMenuShown)
                {
                    if (!_eventManager.IsPaused)
                    {
                        _eventManager.IsPaused = true;
                    }
                    else if (!_eventManager.OptionsShown)
                    {
                        _eventManager.IsPaused = false;

                    }
                }
            }
            else if (_focusDetect.GetFocusOwner() != null)
            {
                //There's already button in focus
                return;
            }
            else if (@event.IsActionPressed("ui_right") ||
                     @event.IsActionPressed("ui_left") ||
                     @event.IsActionPressed("ui_up") ||
                     @event.IsActionPressed("ui_down"))
            {
                _eventManager.EmitSignal("Refocus");
            }
        }

        public void ForceFocus()
        {
            Button button = null;

            if (_eventManager.MainMenuShown)
            {
                if (_eventManager.OptionsShown)
                {
                    if (_eventManager.ControlsShown)
                    {
                        button = _buttonsSections["OptionsControls"];
                    }
                    else
                    {
                        button = _buttonsSections["OptionsMain"];
                    }
                }
                else
                {
                    button = _buttonsSections["MainMenu"];
                }
            }
            else
            {
                if (_eventManager.OptionsShown)
                {
                    if (_eventManager.ControlsShown)
                    {
                        button = _buttonsSections["OptionsControls"];
                    }
                    else
                    {
                        button = _buttonsSections["OptionsMain"];
                    }
                }
                else
                {
                    if (_eventManager.IsPaused)
                    {
                        button = _buttonsSections["Pause"];
                    }
                }
            }

            if (button != null)
            {
                button.GrabFocus();
            }
        }
    }
}