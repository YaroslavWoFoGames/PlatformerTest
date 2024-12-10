using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using Platformer.Core;
using UnityEngine;

namespace Platformer.Game.Services
{
	public class EnemySpawnService : IEnemySpawnService
	{
		private readonly EnemySpawnController _spawnController;
		private readonly EnemyConfig _enemyConfig;
		private readonly Transform _enemyTarget;
		private readonly LootController _lootController;
		private readonly IAudioService _audioService;
		private bool _isSpawning;
		private float _time;
		private List<EnemyController> _enemy = new();

		public EnemySpawnService(EnemySpawnController spawnController, Transform target,
		                         IGameConfigurationService _gameConfigurationService, IAudioService audioService)
		{
			_audioService = audioService;
			_enemyTarget = target;
			_spawnController = spawnController;
			_enemyConfig = _gameConfigurationService.GameConfiguration.EnemyConfig;
			_lootController = _enemyConfig.Prefab;
		}

		public void StartSpawning()
		{
			_isSpawning = true;
			SpawnLoop().Forget();
		}

		public void StopSpawning()
		{
			_isSpawning = false;
		}

		private async UniTaskVoid SpawnLoop()
		{
			while (_isSpawning)
			{
				var delay = Random.Range(_enemyConfig.PerSpawnTimeMin, _enemyConfig.PerSpawnTimeMax) * 1000;
				await UniTask.Delay((int)delay);

				var enemyInfo = _enemyConfig.GetRandomEnemy();
				var position = _spawnController.GetRandomPosition();

				var enemy = LeanPool.Spawn(enemyInfo.Prefab, position, Quaternion.identity, _spawnController.transform);
				var randomValueLoot = Random.Range(_enemyConfig.MinLootAmount, _enemyConfig.MaxLootAmount);
				enemy.Initialize(enemyInfo.Data, _enemyTarget, _lootController, randomValueLoot
				               , _audioService);
				_enemy.Add(enemy);
			}
		}

		public void DespawnAll()
		{
			foreach (var enemy in _enemy)
			{
				LeanPool.Despawn(enemy);
			}
		}
	}
}