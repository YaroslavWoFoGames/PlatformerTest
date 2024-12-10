using Platformer.Core;
using Platformer.Game.Services;

namespace Platformer.Game.States
{
	public class LevelPlayState : GameState
	{
		private IEnemySpawnService _enemySpawnService => DiContainer.Resolve<IEnemySpawnService>();
		private IPlayerAmmoService _playerAmmoService => DiContainer.Resolve<IPlayerAmmoService>();
		protected override void OnEnter()
		{
			_enemySpawnService.DespawnAll();
			_enemySpawnService.StartSpawning();
			_playerAmmoService.Reset();
		}

		protected override void OnExit()
		{
			_enemySpawnService.StopSpawning();
		}
	}
}