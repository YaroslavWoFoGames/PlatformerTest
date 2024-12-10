using Lean.Pool;
using Platformer.Core;
using Platformer.Game.Services;
using Platformer.Game.Views;
using UniRx;
using UnityEngine;

namespace Platformer.Game
{
	public class EnemyController : CreatureController
	{
		[SerializeField] private HealthBarView _bar;
		private IAudioService _audioService;
		private EnemyRemoteDto _config;
		private Transform _transformTarget;
		private LootController _prefab;
		private ReactiveProperty<float> _currentHealthPercent = new();
		private float _currentHealth;
		private float _maxHealth;
		private int _rewardLootDieValue;

		public void Initialize(EnemyRemoteDto data, Transform target, LootController lootController,
		                       int rewardLootDieValue, IAudioService audioService)
		{
			_audioService = audioService;
			_prefab = lootController;
			_config = data;
			_transformTarget = target;
			_currentHealth = _config.Health;
			_maxHealth = _config.Health;
			_currentHealthPercent = new ReactiveProperty<float>(1);
			_rewardLootDieValue = rewardLootDieValue;
			_bar.Setup(new HealthBarViewArgs(_currentHealthPercent));
		}

		public override void TakeDamage(float damage)
		{
			if (damage < 0)
			{
				Debug.LogWarning("Damage cannot be negative.");
				return;
			}

			_currentHealth -= damage;
			if (_currentHealth <= 0)
			{
				Die();
			}

			_currentHealthPercent.Value = _currentHealth / _maxHealth;
		}

		public override void Die()
		{
			LeanPool.Despawn(gameObject);
			var position = transform.position;
			var loot = LeanPool.Spawn(_prefab, new Vector2(position.x, position.y + 1),
			                          Quaternion.identity);
			loot.Initialize(_rewardLootDieValue);
			_audioService.PlayOneShotById(AudioOneShotType.DiedEnemy);
		}

		public override void Move(float deltaTime)
		{
			if (_transformTarget == null)
			{
				Debug.LogWarning("Target is not assigned.");
				return;
			}

			Vector2 direction = (_transformTarget.position - transform.position).normalized;

			var movement = direction * (_config.Speed * deltaTime);
			transform.Translate(movement);

			if (direction.x > 0)
			{
				Flip(false);
			}
			else if (direction.x < 0)
			{
				Flip(true);
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			Move(deltaTime);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out IDamagable damagable))
			{
				damagable.TakeDamage(_config.Damage);
			}
		}

		private void Flip(bool facingLeft)
		{
			var scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x) * (facingLeft ? -1 : 1);
			transform.localScale = scale;
		}
	}
}