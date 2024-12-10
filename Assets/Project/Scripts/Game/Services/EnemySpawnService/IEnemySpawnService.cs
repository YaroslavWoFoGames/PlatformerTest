namespace Platformer.Game.Services
{
	public interface IEnemySpawnService
	{
		void StartSpawning();
		void StopSpawning();
		void DespawnAll();
	}
}