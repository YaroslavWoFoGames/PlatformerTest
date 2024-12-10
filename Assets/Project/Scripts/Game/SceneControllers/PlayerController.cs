using Lean.Pool;
using Platformer.Core;
using Platformer.Core.InputService;
using Platformer.Game.Services;
using Platformer.Game.States;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using InputActionType = Platformer.Core.InputService.InputActionType;

namespace Platformer.Game
{
	public class PlayerController : CreatureController
	{
		[SerializeField] private Transform _shootingPoint;
		[SerializeField] private PlayerAnimationController _playerAnimationController = null!;

		private PlayerConfig _playerConfig;
		private IPlayerAmmoService _playerAmmoService;
		private IGameStateService _gameStateService;
		private IAudioService _audioService;

		private Vector2 _facingDirection = Vector2.right;
		private float _horizontalInput;
		private bool _isShoot;
		private float _timeAttack;
		private bool _isMoving;
		private bool _isDie;

		public override void TakeDamage(float damage)
		{
			Die();
		}

		public override void Attack()
		{
			Shoot();
		}

		public override void Die()
		{
			_gameStateService.SwitchState<DiedState>();
			_audioService.PlayOneShotById(AudioOneShotType.Died);
			_isDie = true;
		}

		public override void Move(float deltaTime)
		{
			var movement = _horizontalInput * _playerConfig.Speed * deltaTime;
			var position = transform.position;
			var newPositionX = position.x + movement;

			newPositionX = Mathf.Clamp(newPositionX, _playerConfig.BoundMovementLeft, _playerConfig.BoundMovementRight);

			position = new Vector3(newPositionX, position.y, position.z);
			transform.position = position;

			_isMoving = Mathf.Abs(_horizontalInput) > Mathf.Epsilon;

			if (_isMoving)
			{
				_playerAnimationController.PlayRun();
			}
			else if (!_isShoot)
			{
				_playerAnimationController.PlayIdle();
			}

			if (_horizontalInput > 0)
			{
				_facingDirection = Vector2.right;
				Flip(false);
			}
			else if (_horizontalInput < 0)
			{
				_facingDirection = Vector2.left;
				Flip(true);
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			_timeAttack += deltaTime;
			Move(deltaTime);
			if (_isShoot && _timeAttack >= _playerConfig.AttackSpeed && !_isMoving && _playerAmmoService.TryTakeAmmo())
			{
				Shoot();
				_timeAttack = 0;
			}
		}

		private void Shoot()
		{
			var bullet = LeanPool.Spawn(_playerConfig.Prefab, _shootingPoint.position, Quaternion.identity);
			bullet.Initialize(_playerConfig.Damage, _facingDirection);
			_playerAnimationController.PlayFire();
		}

		private void Flip(bool facingLeft)
		{
			var scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x) * (facingLeft ? -1 : 1);
			transform.localScale = scale;
		}

		[Inject]
		private void Construct(IGameConfigurationService gameConfigurationService, IInputService inputService,
		                       IPlayerAmmoService playerAmmoService, IGameStateService gameStateService,
		                       IAudioService audioService)
		{
			_audioService = audioService;
			_playerAmmoService = playerAmmoService;
			_gameStateService = gameStateService;
			_playerConfig = gameConfigurationService.GameConfiguration.PlayerConfig;
			inputService.SubscribeVector2(InputActionType.Move, HandleChangeVector2)
			            .AddTo(CompositeDisposableOnDeactivate);
			inputService.SubscribeBool(InputActionType.LeftAction, HandleShoot)
			            .AddTo(CompositeDisposableOnDeactivate);
			_playerAnimationController.SetFireSpeed(1 / _playerConfig.AttackSpeed);
			_playerAmmoService.AmountAmmo.Where(x => x <= 0).Subscribe((x) => Die())
			                  .AddTo(CompositeDisposableOnDeactivate);
		}

		private void HandleChangeVector2(Vector2 obj)
		{
			_horizontalInput = obj.x;
		}

		private void HandleShoot(InputActionPhase inputActionPhase)
		{
			_isShoot = inputActionPhase == InputActionPhase.Performed;
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.TryGetComponent(out IPickap loot))
			{
				_playerAmmoService.AddAmmo(loot.Value);
				loot.Pickup();
				_audioService.PlayOneShotById(AudioOneShotType.Pickup);
			}
		}
	}
}