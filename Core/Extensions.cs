using Godot;
using static GodotGameTemplate.Core.SettingsEnums;

namespace GodotGameTemplate.Core
{
    public static class Extensions
    {
        public static InputBind ToInputBind(this InputEvent inputEvent)
        {
            if (inputEvent is InputEventKey)
            {
                var key = (InputEventKey)inputEvent;
                return new InputBind
                {
                    Index = (int)key.Scancode
                };
            }
            else if (inputEvent is InputEventJoypadButton)
            {
                var joyPadButton = (InputEventJoypadButton)inputEvent;
                return new InputBind
                {
                    Device = joyPadButton.Device,
                    Axis = joyPadButton.ButtonIndex
                }; 
            }
            else if (inputEvent is InputEventJoypadMotion)
            {
                var joyPadMotion = (InputEventJoypadMotion)inputEvent;
                return new InputBind
                {
                    Device = joyPadMotion.Device,
                    Axis = joyPadMotion.Axis,
                    AxisValue = joyPadMotion.AxisValue
                };
            }
            return null;
        }

        public static InputEvent ToInputEvent(this InputBind inputBind)
        {
            if (inputBind.EventType == InputEventType.InputEventKey)
            {
                return new InputEventKey
                {
                    Scancode = (uint)inputBind.Index
                };
            }

            if (inputBind.EventType == InputEventType.InputEventJoypadButton)
            {
                return new InputEventJoypadButton
                {
                    Device = inputBind.Device,
                    ButtonIndex = inputBind.Index
                };
            }

            if (inputBind.EventType == InputEventType.InputEventJoypadMotion)
            {
                return new InputEventJoypadMotion
                {
                    Device = inputBind.Device,
                    Axis = inputBind.Axis,
                    AxisValue = inputBind.AxisValue
                };
            }

            return null;
        }
    }
}
