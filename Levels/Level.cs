using Godot;
using GodotGameTemplate.Core;

namespace GodotGameTemplate.Levels
{
    public class Level : Node2D
    {
        [Export(PropertyHint.File, "*.tscn")]
        public string NextScene;
        private EventManager _eventManager;


        public void OnButtonPressed()
        {
            _eventManager = GetNode<EventManager>("EventManager");
            _eventManager.EmitSignal("ChangeScene", NextScene);
        }
    }
}