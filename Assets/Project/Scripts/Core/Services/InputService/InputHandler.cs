using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.Core.InputService
{
	public class InputHandler : Inputs.IPlayerInGameActions
	{
		private readonly CompositeDisposable _disposable = new();
		private readonly Dictionary<InputActionType, Subject<InputActionPhase>> _boolActionMap = new();
		private readonly Dictionary<InputActionType, Subject<Vector2>> _vector2ActionMap = new();

		public InputHandler()
		{
			//PlayerInGame
			InitializeVector2Action(InputActionType.Move);
			InitializeBoolProperty(InputActionType.LeftAction);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}

		public IDisposable? SubscribeBoolAction(InputActionType actionType, Action callback)
		{
			if (_boolActionMap.TryGetValue(actionType, out var property))
			{
				return property.Subscribe(_ => callback()).AddTo(_disposable);
			}

			return null;
		}

		public IDisposable? SubscribeBoolAction(InputActionType actionType, Action<InputActionPhase> callback)
		{
			if (_boolActionMap.TryGetValue(actionType, out var property))
			{
				return property.Subscribe(callback).AddTo(_disposable);
			}

			return null;
		}

		public IDisposable? SubscribeVector2Action(InputActionType actionType, Action<Vector2> callback)
		{
			if (_vector2ActionMap.TryGetValue(actionType, out var property))
			{
				return property.Subscribe(callback).AddTo(_disposable);
			}

			return null;
		}

		public IDisposable? SubscribeVector2Action(InputActionType actionType, Action callback)
		{
			if (_vector2ActionMap.TryGetValue(actionType, out var property))
			{
				return property.Subscribe(_ => callback()).AddTo(_disposable);
			}

			return null;
		}

		private void InitializeVector2Action(InputActionType inputActionType)
		{
			var property = new Subject<Vector2>().AddTo(_disposable);
			_vector2ActionMap[inputActionType] = property;
		}

		private void InitializeBoolProperty(InputActionType inputActionType)
		{
			var property = new Subject<InputActionPhase>().AddTo(_disposable);
			_boolActionMap[inputActionType] = property;
		}

		public void HardHandleInput(InputActionType inputGameAction, InputActionPhase phase)
		{
			if (_boolActionMap.TryGetValue(inputGameAction, out var property))
			{
				property.OnNext(phase);
			}
		}

		public void HandleVector2Action(InputActionType actionType, InputAction.CallbackContext context)
		{
			if (_vector2ActionMap.TryGetValue(actionType, out var property))
			{
				property.OnNext(context.ReadValue<Vector2>());
			}
		}

		private void HandleInput(InputActionType gameAction, InputAction.CallbackContext context)
		{
			if (_boolActionMap.TryGetValue(gameAction, out var property))
			{
				property.OnNext(context.phase);
			}
		}

		#region InputActions.Inputs.IPlayerInGameActions

		public void OnMove(InputAction.CallbackContext context)
		{
			HandleVector2Action(InputActionType.Move, context);
		}

		public void OnActionLeft(InputAction.CallbackContext context)
		{
			HandleInput(InputActionType.LeftAction, context);
		}

		#endregion
	}
}