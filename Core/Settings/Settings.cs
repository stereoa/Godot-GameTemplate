using Godot;
using System.Collections.Generic;
using static GodotGameTemplate.Core.SettingsEnums;

namespace GodotGameTemplate.Core
{
    public static class Settings
    {
        public static bool Html5 = false;

        public static bool Fullscreen { get; set; }

        public static bool Borderless { get; set; }
        public static Viewport View { get; set; }
        public static Rect2 ViewRect2 { get; set; }
        public static Vector2 GameResolution { get; set; }
        public static Vector2 WindowResolution { get; set; } 
        public static Vector2 ScreenResolution { get; set; } 
        public static float ScreenAspectRatio { get; set; }

        public static int Scale { get; set; }
        public static int MaxScale { get; set; }

        public static float VolumeMaster { get; set; }
        public static float VolumeMusic { get; set; }
        public static float VolumeSfx { get; set; }
        public static float VolumeRange { get; set; }


        //CONTROLS
        public static Dictionary<ActionType, List<InputBind>> ActionControls = new Dictionary<ActionType, List<InputBind>>();

        //LOCALIZATION
        public static string Language { get; set; }
        public static Dictionary<Language, string> Languages = new Dictionary<Language, string> {
        {SettingsEnums.Language.EN, "en"},
        {SettingsEnums.Language.DE, "de"},
        {SettingsEnums.Language.ES, "es"},
        {SettingsEnums.Language.FR, "fr"},
        {SettingsEnums.Language.BR, "pt_BR"},
        {SettingsEnums.Language.LV, "lv"},
        {SettingsEnums.Language.IT, "it"}
    };

    }
}
