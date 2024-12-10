using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Platformer.Core;
using Platformer.Game.Services;
using Platformer.Game.Views;
using Zenject;

namespace Platformer.Game.States
{
	public class LaunchState : GameState
	{
		private ISceneLoadingService _sceneLoadingService => DiContainer.Resolve<ISceneLoadingService>();
		private IGameConfigurationService _gameConfigurationService => DiContainer.Resolve<IGameConfigurationService>();
		private IPlayerAmmoService _playerAmmoService => DiContainer.Resolve<IPlayerAmmoService>();

		protected override async void OnEnter()
		{
			//TODO: симуляция загрузки - можно спокойно убрать
			await UniTask.Delay(500);
			var gameScene = _gameConfigurationService.GameConfiguration.SceneConfig.GameSceneName;
			await _sceneLoadingService.LoadSceneAsync(gameScene);

			var _statViewArgs = new List<StatViewArgs>();
			_statViewArgs.Add(new StatViewArgs(_playerAmmoService.AmountAmmo));
			var args = new CharacterStatsViewArgs(_statViewArgs);
			UIService.CreateView<CharacterStatsView, CharacterStatsViewArgs>(args);
			GameStateService.SwitchState<LevelPlayState>();
		}

		protected override void OnExit() { }
	}
}