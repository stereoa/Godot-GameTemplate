using Godot;
namespace GodotGameTemplate.Gui
{
    public class FadeLayer : CanvasLayer
    {
        private float _percent;
        public float Percent
        {
            get
            {
                return _percent;
            }
            set
            {
                _percent = Mathf.Clamp(value, 0, 1);
                //Fade logic
                var colorRect = GetNode<ColorRect>("ColorRect");
                colorRect.Modulate = new Color(0, 0, 0, _percent);
            }
        }

        public override void _Ready()
        {
            Percent = 0;
        }
    }
}