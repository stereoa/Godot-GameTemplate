using Godot;
using GodotGameTemplate.Core;
using GodotGameTemplate.Gui;

namespace GodotGameTemplate
{
	public class Game : Node2D
	{
		[Signal]
		public delegate void SceneIsLoaded();
		
		PackedScene _currentScene;
		Node _currentSceneInstance;

		FadeState _fadeState = FadeState.Idle;
		FadeLayer _fadeLayer;
		Tween _fadeTween;

		private GuiBrain _guiBrain;
		private EventManager _eventManager;

		public override void _Ready()
		{
			_eventManager = GetNode<EventManager>("/root/EventManager");
			_guiBrain = GetNode<GuiBrain>("/root/GuiBrain");

			_eventManager.Connect("Options", this, "OnOptions");
			_eventManager.Connect("Exit", this, "OnExit");
			_eventManager.Connect("ChangeScene", this, "OnChangeScene");
			_eventManager.Connect("Restart", this, "RestartScene");

			_fadeLayer = GetNode<FadeLayer>("FadeLayer");
			_fadeTween = _fadeLayer.GetNode<Tween>("FadeTween");

			_fadeTween.Connect("tween_completed", this, "OnFadeTweenComplete");

			_guiBrain.GuiCollectFocusGroup();
		}

		public void OnChangeScene()
		{
			if (_fadeState != FadeState.Idle)
			{
				return;
			}

			_fadeState = FadeState.FadeOut;
			
			_fadeTween.InterpolateProperty(_fadeLayer, "Percent", 0.0, 1.0, 0.5F, Tween.TransitionType.Linear, Tween.EaseType.In, 0.0F);
			_fadeTween.Start();
		}

		public void OnExit()
		{
			if (_fadeState != FadeState.Idle)
			{
				return;
			}
			GetTree().Quit();
		}

		public async void ChangeScene(string scene)
		{ 
			await ToSignal(GetTree(), "idle_frame");
			_currentSceneInstance.Free();
			_currentScene = ResourceLoader.Load<PackedScene>(scene);
			_currentSceneInstance = _currentScene.Instance();
			GetNode("CurrentScene").AddChild(_currentSceneInstance);
			EmitSignal("SceneLoaded");
		}

		public async void RestartScene()
		{
			if (_fadeState != FadeState.Idle)
			{
				return;
			}
			await ToSignal(GetTree(), "idle_frame");
			_currentSceneInstance.Free();
			_currentSceneInstance = _currentScene.Instance();
			GetNode("Levels").AddChild(_currentSceneInstance);
		}

		public void OnFadeTweenComplete()
		{
			switch (_fadeState)
			{
				case FadeState.Idle:
					break;
				case FadeState.FadeOut:
					ChangeScene();
					_fadeState = FadeState.FadeIn;
					var fadeLayer = GetNode<FadeLayer>("FadeLayer");
					var fadeTween = fadeLayer.GetNode<Tween>("FadeTween");
					fadeTween.InterpolateProperty(fadeLayer, "Percent", 1.0, 0.0, 0.5F, Tween.TransitionType.Linear, Tween.EaseType.In, 0.0F);
					fadeTween.Start();
					break;
				case FadeState.FadeIn:
					_fadeState = FadeState.Idle;
					break;
			}
		}
	}
}
