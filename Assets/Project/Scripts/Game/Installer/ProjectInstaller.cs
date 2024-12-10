using Platformer.Core;
using Platformer.Core.InputService;
using Platformer.Game.Services;
using UnityEngine;
using Zenject;

namespace Platformer.Game.Installer
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] private GameConfiguration _gameConfiguration = null!;
		[SerializeField] private Canvas _rootCanvas = null;
		[SerializeField] private AudioSource _audioSource = null;

		public override void InstallBindings()
		{
			Container.BindInstance(_gameConfiguration).AsSingle();
			Container.BindInstance(_rootCanvas).AsSingle();
			Container.BindInstance(_audioSource).AsSingle();

			Container.BindInterfacesAndSelfTo<GameConfigurationService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AudioService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<UIService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<SceneLoadingService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<GameStateService>().AsCached().NonLazy();
			Container.BindInterfacesAndSelfTo<PlayerAmmoService>().AsCached().NonLazy();
		}
	}
}