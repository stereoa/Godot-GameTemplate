namespace GodotGameTemplate.Core
{
    public class SettingsEnums
    {
        public enum VolumeType
        {
            Master,
            Music,
            SFX,
            Range
        }

        public enum Language
        {
            EN,
            DE,
            ES,
            FR,
            BR,
            LV,
            IT
        }

        public enum ActionType
        {
            Up,
            Down,
            Left,
            Right,
            Jump,
            Crouch,
            Prone,
            PrimaryFire,
            SecondaryFire,
            Use
        }

        public enum InputEventType
        {
            InputEventKey,
            InputEventJoypadButton,
            InputEventJoypadMotion
        }
    }
}
