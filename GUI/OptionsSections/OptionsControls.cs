using Godot;
using Godot.Collections;
using GodotGameTemplate.Core;
using static GodotGameTemplate.Core.SettingsEnums;

namespace GodotGameTemplate.Gui
{
	public class OptionsControls : VBoxContainer
	{

		private VBoxContainer _actionList;
		private Popup _popup;
		private PackedScene _actionBind = (PackedScene)ResourceLoader.Load("res://GUI/ReBindSection/ActionBind.tscn");
		private PackedScene _controlBind = (PackedScene)ResourceLoader.Load("res://GUI/ReBindSection/ControlBind.tscn");
		private string _actionNamePath = "Name";
		private string _actionAddPath = "AddAction";
		private string _controlNamePath = "Name";
		private string _controlRemovePath = "RemoveAction";
		private EventManager _eventManager;
		private SettingsManager _settingsManager;
		private GuiBrain _guiBrain;
		private System.Collections.Generic.Dictionary<ActionType, VBoxContainer> _actionNodes = new System.Collections.Generic.Dictionary<ActionType, VBoxContainer>();

		public override void _Ready()
		{
			_popup = (Popup)FindNode("Popup");
			_actionList = (VBoxContainer)FindNode("ActionList");

			_eventManager = GetNode<EventManager>("/root/EventManager");
			_settingsManager = GetNode<SettingsManager>("/root/SettingsManager");
			_guiBrain = GetNode<GuiBrain>("/root/GuiBrain");

			SetActionList();

			_eventManager.Connect("Controls", this, "ShowControls");
			_settingsManager.Connect("ReTranslate", this, "Retranslate");

			Retranslate();

		}
		public void ShowControls(bool visible)
		{
			Visible = visible;
		}

		public void SetActionList()
		{
			_actionNodes.Clear();

			var actions = Settings.ActionControls.Keys;


			foreach (var action in actions)
			{

				var node = (VBoxContainer)_actionBind.Instance();

				_actionList.AddChild(node);
				_actionNodes[action] = node;

				var addButton = (Button)node.FindNode("AddAction");
				addButton.Connect("pressed", this, "AddControl", new Array { action });

				var actionLabel = (Label)node.FindNode("Name");
				actionLabel.Text = action.ToString();

				_guiBrain.EmitSignal("NewScrollContainerButton", node);

				SetControlList(action);
			}
		}
		public void SetControlList(ActionType action)
		{
			var binds = Settings.ActionControls[action];

			foreach (var bind in binds)
			{
				NewBind(action, bind);

			}
		}

		public void NewBind(ActionType action, InputBind bind)
		{
			var eventNode = (HBoxContainer)_controlBind.Instance();

			var parent = _actionNodes[action];

			parent.AddChild(eventNode);

			var bindName = (Label)eventNode.FindNode("Name");

			var remove = (Button)eventNode.FindNode("RemoveAction");

			bindName.Text = GetInputEventName(bind);
			remove.Connect("pressed", this, "RemoveControl", new Array { action, bind, eventNode });
			_guiBrain.EmitSignal("NewScrollContainerButton", eventNode);
		}

		public string GetInputEventName(InputBind bind)
		{
			var text = "";
			if (bind.EventType == InputEventType.InputEventKey)
			{
				text = "Keyboard: " + OS.GetScancodeString((uint)bind.Index);
			}
			else if (bind.EventType == InputEventType.InputEventJoypadButton)
			{
				text = "Gamepad: ";

				if (Input.IsJoyKnown(bind.Device))
				{
					text += Input.GetJoyButtonString(bind.Index);
				}
				else
				{
					text += $"Btn. {bind.Index.ToString()}";
				}
			}
			else if (bind.EventType == InputEventType.InputEventJoypadMotion)
			{
				text = "Gamepad: ";
				if (Input.IsJoyKnown(bind.Device))
				{
					text += $"{Input.GetJoyAxisString(bind.Axis)} ";
				}
				else
				{
					text += $"Axis: {bind.Axis} ";
				}
				text += Mathf.Round(bind.AxisValue);
			}
			return text;
		}
		public async void AddControl(ActionType action)
		{
			_popup.Popup_();

			await ToSignal(_popup, "NewControl");

			if (_popup.NewEvent == null)
			{
				return;
			}

			var inputEvent = _popup.NewEvent;
			var bind = inputEvent.ToInputBind();
			Settings.ActionControls[action].Add(bind);

			InputMap.ActionAddEvent(action.ToString(), inputEvent);
			NewBind(action, bind);
		}
		public void RemoveControl(Array binds)
		{
			var action = (ActionType)binds[0];
			var inputEvent = (InputEvent)binds[1];
			var node = (HBoxContainer)binds[2];

			var type = inputEvent.ToInputBind().EventType;

			Settings.ActionControls[action].RemoveAll(x => x.EventType == type);
			InputMap.ActionEraseEvent(action.ToString(), inputEvent);

			node.QueueFree();
		}
		public void OnDefaultPressed()
		{
			_settingsManager.DefaultControls();

			foreach (var node in _actionNodes.Values)
			{

				node.QueueFree();
			}
			SetActionList();
		}

		public void OnBackPressed()
		{
			_eventManager.ControlsShown = false;
		}

		//Localization
		public void Retranslate()
		{
			var defaultButton = (Button)FindNode("Default");
			defaultButton.Text = Tr("DEFAULT");

			var backButton = (Button)FindNode("Back");
			backButton.Text = Tr("BACK");

			var actionsLabel = (Label)FindNode("Actions");
			actionsLabel.Text = Tr("ACTIONS");

			foreach (var actionNode in _actionNodes)
			{
				var nameLabel = (Label)actionNode.Value.FindNode("Name");
				nameLabel.Text = Tr(actionNode.Key.ToString());
			}
		}
	}
}
