using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Platformer.Core.InputService
{
	[Flags]
	public enum InputMap
	{
		None = 0,
		PlayerInGame = 1 << 0
	}

	public enum InputActionType
	{
		// PlayerInGame
		Move = 0,
		LeftAction
	}

	public static class InputActionExtensions
	{
		public static string GetDisplayName(this InputAction inputAction)
		{
			return inputAction.controls.FirstOrDefault()?.displayName ?? string.Empty;
		}

		public static string GetShortDisplayName(this InputAction inputAction)
		{
			var control = inputAction.controls.FirstOrDefault();
			var name = control?.shortDisplayName;
			if (string.IsNullOrEmpty(name))
			{
				name = control?.displayName;
			}

			return name ?? string.Empty;
		}

		public static bool HasHoldInteraction(this InputAction inputAction)
		{
			return inputAction.interactions.Contains("Hold");
		}
	}

	public static class InputGameActionExtensions
	{
		public static string GetLocalizedKey(this InputActionType inputActionType)
		{
			return $"input_action_{inputActionType}".ToLower();
		}

		public static string GetInputActionName(this InputActionType inputActionType)
		{
			return inputActionType.ToString();
		}
	}

	public class InputService : IInputService, IDisposable, IInitializable
	{
		private readonly Inputs _inputs;
		private readonly InputHandler _inputHandler;
		private readonly Dictionary<InputMap, InputActionMap> _inputMaps = new();
		private InputMap _inputMap = InputMap.None;

		public InputService()
		{
			_inputs = new Inputs();
			_inputHandler = new InputHandler();
		}

		public void Initialize()
		{
			_inputMaps.Clear();
			foreach (var action in _inputs)
			{
				var map = action.actionMap;
				var mapName = map.name;

				if (!Enum.TryParse(mapName, true, out InputMap inputMap))
				{
					Debug.LogError(
					               $"[{nameof(InputService)}] Not implemented {nameof(InputMap)} for map name: {mapName}");
					continue;
				}

				_inputMaps.TryAdd(inputMap, map);
			}

			EnableInputMap(InputMap.PlayerInGame);
			UpdateInputMap();
			_inputs.PlayerInGame.SetCallbacks(_inputHandler);
			_inputs.Enable();
		}

		public void Dispose()
		{
			_inputs.Disable();
			_inputMaps.Clear();
			_inputHandler.Dispose();
		}

		public void EnableInputMap(InputMap map)
		{
			_inputMap = _inputMap | map;
			UpdateInputMap();
		}

		public void DisableInputMap(InputMap map)
		{
			_inputMap = _inputMap & ~map;
			UpdateInputMap();
		}

		public IDisposable? SubscribeBool(InputActionType gameAction, Action callback)
		{
			return _inputHandler.SubscribeBoolAction(gameAction, callback);
		}

		public IDisposable? SubscribeBool(InputActionType gameAction, Action<InputActionPhase>? callback)
		{
			return _inputHandler.SubscribeBoolAction(gameAction, callback);
		}

		public IDisposable? SubscribeVector2(InputActionType gameAction, Action<Vector2>? callback)
		{
			return _inputHandler.SubscribeVector2Action(gameAction, callback);
		}

		public void HardHandleInput(InputActionType inputGameAction, InputActionPhase inputActionPhase)
		{
			_inputHandler.HardHandleInput(inputGameAction, inputActionPhase);
		}

		public bool TryGetInputAction(InputActionType gameAction, out InputAction inputAction)
		{
			return TryGetInputAction(gameAction.GetInputActionName(), out inputAction);
		}

		public bool TryGetInputAction(string actionName, out InputAction inputAction)
		{
			inputAction = _inputs.FindAction(actionName);
			return inputAction != null;
		}

		private void UpdateInputMap()
		{
			foreach (var (type, map) in _inputMaps)
			{
				if (_inputMap.HasFlag(type))
				{
					map.Enable();
				}
				else
				{
					map.Disable();
				}
			}

			Debug.Log($"[{nameof(InputService)}] Set input map: {_inputMap}");
		}
	}
}