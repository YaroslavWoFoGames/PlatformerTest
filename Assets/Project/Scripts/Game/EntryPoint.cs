using Platformer.Core;
using Platformer.Game.States;
using Zenject;

namespace Platformer.Game
{
	public class EntryPoint : IInitializable
	{
		private readonly GameStateService _gameStateService;

		public EntryPoint(GameStateService gameStateService)
		{
			_gameStateService = gameStateService;
			Initialize();
		}

		public void Initialize()
		{
			_gameStateService.SwitchState<LaunchState>();
		}
	}
}