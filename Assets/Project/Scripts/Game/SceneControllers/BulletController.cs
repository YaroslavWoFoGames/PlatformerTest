using Lean.Pool;
using UnityEngine;

namespace Platformer.Game
{
	public class BulletController : ProjectileController
	{
		[SerializeField] private float _speed;

		private float _damage;

		public void Initialize(float damage, Vector2 direction)
		{
			_damage = damage;
			_rigidbody2D.AddForce(direction * _speed);
			LeanPool.Despawn(gameObject, 5);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out IDamagable damagable))
			{
				damagable.TakeDamage(_damage);
				LeanPool.Despawn(gameObject);
			}
		}
	}
}