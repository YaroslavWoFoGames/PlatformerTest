using Platformer.Core.SceneControllers;

namespace Platformer.Game
{
	public abstract class CreatureController : SceneController, IMovable, IAttack, IMortal, IDamagable
	{
		public virtual void Move(float deltaTime) { }

		public virtual void Attack() { }

		public virtual void Die() { }

		public virtual void TakeDamage(float damage) { }
	}
}