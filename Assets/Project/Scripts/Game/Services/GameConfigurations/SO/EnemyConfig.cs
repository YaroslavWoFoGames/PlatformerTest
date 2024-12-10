using System;
using System.Collections.Generic;
using Platformer.Core.SO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Game.Services
{
	[Serializable]
	public class EnemyRemoteDto
	{
		[field: SerializeField] public float Damage { get; private set; }
		[field: SerializeField] public float Speed { get; private set; }
		[field: SerializeField] public float Health { get; private set; }
	}

	[Serializable]
	public class EnemyConfigInfo
	{
		[field: SerializeField] public string Id { get; private set; }
		[field: SerializeField] public EnemyController Prefab { get; private set; }
		[field: SerializeField] public EnemyRemoteDto Data { get; private set; }
	}

	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(EnemyConfig),
		                fileName = nameof(EnemyConfig))]
	public class EnemyConfig : Config
	{
		[field: SerializeField] public float PerSpawnTimeMin { get; private set; } = 1;
		[field: SerializeField] public float PerSpawnTimeMax { get; private set; } = 10;
		[field: SerializeField] public LootController Prefab = null!;

		[field: SerializeField] public int MinLootAmount { get; private set; } = 10;
		[field: SerializeField] public int MaxLootAmount { get; private set; } = 20;
		
		[SerializeField] private List<EnemyConfigInfo> _enemyConfigRepository = new();
		
		public EnemyConfigInfo GetRandomEnemy()
		{
			return _enemyConfigRepository[Random.Range(0, _enemyConfigRepository.Count)];
		}
	}
}