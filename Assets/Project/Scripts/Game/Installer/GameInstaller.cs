using Platformer.Game.Services;
using UnityEngine;
using Zenject;

namespace Platformer.Game.Installer
{
	public class GameInstaller : MonoInstaller
	{
		[SerializeField] private PlayerController _playerController = null!;
		[SerializeField] private EnemySpawnController _enemySpawnController = null!;

		public override void InstallBindings()
		{
			var projectContext = ProjectContext.Instance.Container;
			projectContext.BindInstance(_playerController);
			projectContext.BindInterfacesAndSelfTo<EnemySpawnService>()
			              .AsSingle()
			              .WithArguments(_enemySpawnController, _playerController.transform)
			              .NonLazy();
		}
	}
}