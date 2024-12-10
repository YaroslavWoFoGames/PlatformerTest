using System.Collections.Generic;
using Platformer.Core.SceneControllers;
using UnityEngine;

namespace Platformer.Game
{
	public class EnemySpawnController : SceneController
	{
		[SerializeField] private List<Transform> _spawnPoints = new();

		public Vector3 GetRandomPosition()
		{
			return _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
		}
	}
}