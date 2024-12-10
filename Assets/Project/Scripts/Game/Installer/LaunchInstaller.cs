using Zenject;

namespace Platformer.Game.Installer
{
	public class LaunchInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<EntryPoint>().AsSingle().NonLazy();
		}
	}
}