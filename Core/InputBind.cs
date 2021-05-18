using static GodotGameTemplate.Core.SettingsEnums;

namespace GodotGameTemplate.Core
{
    public class InputBind
    {
		//Scan code / Button index
		public int Index { get; set; }
		public int Device { get; set; }
		public int Axis { get; set; }
		public float AxisValue { get; set; }
		public InputEventType EventType { get; set; }
	}
}
