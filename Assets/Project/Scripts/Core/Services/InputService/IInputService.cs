using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.Core.InputService
{
	public interface IInputService
	{
		void EnableInputMap(InputMap map);
		void DisableInputMap(InputMap map);
		IDisposable? SubscribeBool(InputActionType gameAction, Action callback);
		IDisposable? SubscribeBool(InputActionType gameAction, Action<InputActionPhase>? callback);
		IDisposable? SubscribeVector2(InputActionType gameAction, Action<Vector2>? callback);
		void HardHandleInput(InputActionType inputGameAction, InputActionPhase inputActionPhase);
		bool TryGetInputAction(InputActionType gameAction, out InputAction inputAction);
		bool TryGetInputAction(string actionName, out InputAction inputAction);
	}
}