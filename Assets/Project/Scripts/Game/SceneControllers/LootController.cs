using Lean.Pool;
using Platformer.Core.SceneControllers;

namespace Platformer.Game
{
	public class LootController : SceneController, IPickap
	{
		public int Value { get; private set; }

		public void Initialize(int value)
		{
			Value = value;
		}

		public void Pickup()
		{
			LeanPool.Despawn(gameObject);
		}
	}
}