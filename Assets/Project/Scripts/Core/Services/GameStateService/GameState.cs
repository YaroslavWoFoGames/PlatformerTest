using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Platformer.Core
{
	public interface IGameState : IDisposable
	{
		void SetStateMachine(GameStateMachine stateMachine, DiContainer diContainer);
		void Enter();
		void Exit();
		void Tick();
		void Reset();
		bool ShouldRemoveFromCache { get; set; }
	}

	public interface IGameState<in T> : IGameState where T : IGameStateContext
	{
		void SetContext(T context);
	}

	public abstract class GameState<T> : GameState, IGameState<T> where T : IGameStateContext
	{
		public T Context { get; private set; } = default!;

		public void SetContext(T context)
		{
			Context = context;
		}
	}

	public abstract class GameState : IGameState
	{
		public bool ShouldRemoveFromCache { get; set; }

		protected IGameStateService GameStateService => DiContainer.Resolve<IGameStateService>();
		protected IUIService UIService => DiContainer.Resolve<IUIService>();
		protected DiContainer DiContainer { get; private set; }

		protected CompositeDisposable DisposableExit { get; private set; } = new();
		protected GameStateMachine StateMachine { get; private set; } = null!;
		protected GameState? Substate { get; private set; }

		private GameState? _owner;

		public void SetStateMachine(GameStateMachine stateMachine, DiContainer container)
		{
			StateMachine = stateMachine;
			DiContainer = container;
		}

		public void Enter()
		{
			Debug.Log($"[{nameof(GameState)}] Enter state: {GetType().Name}");
			OnEnter();
		}

		public virtual void Exit()
		{
			Debug.Log($"[{nameof(GameState)}] Exit state: {GetType().Name}");
			DisposableExit.Clear();
			ExitCurrentSubstate();
			OnExit();
			_owner?.DisposeSubstate(this);
		}

		public void Tick()
		{
			Substate?.Tick();
			OnTick();
		}

		public void Dispose()
		{
			DisposableExit.Dispose();
			OnDispose();
		}

		public void Reset() { }

		protected abstract void OnEnter();
		protected abstract void OnExit();
		protected virtual void OnDispose() { }
		protected virtual void OnTick() { }

		protected void SetSubstate(GameState? substate)
		{
			ExitCurrentSubstate();
			if (substate == null)
			{
				return;
			}

			Substate = substate;
			substate._owner = this;
			substate.Enter();
		}

		protected void ExitCurrentSubstate()
		{
			Substate?.Exit();
			DisposeSubstate(Substate);
		}

		private void DisposeSubstate(GameState? gameState)
		{
			if (Substate != gameState)
			{
				return;
			}

			Substate = null;
		}
	}
}