using Godot;

namespace GodotGameTemplate.Levels
{
    public class Player : Sprite
    {
        private int _speed = 2 * 60;

        public override void _Process(float delta)
        {
            var dir = Vector2.Zero;

            dir.x = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");

            dir.y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

            Translate(dir * _speed * delta);
        }
    }
}