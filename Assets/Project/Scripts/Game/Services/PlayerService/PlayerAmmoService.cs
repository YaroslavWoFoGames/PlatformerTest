using UniRx;
using UnityEngine;

namespace Platformer.Game.Services
{
	public class PlayerAmmoService : IPlayerAmmoService
	{
		public IReadOnlyReactiveProperty<int> AmountAmmo => _amountAmmo;
		private readonly ReactiveProperty<int> _amountAmmo;
		private readonly PlayerConfig _playerConfig;

		public PlayerAmmoService(IGameConfigurationService gameConfigurationService)
		{
			_playerConfig = gameConfigurationService.GameConfiguration.PlayerConfig;
			_amountAmmo = new ReactiveProperty<int>(_playerConfig.StartAmmoAmount);
		}

		public bool TryTakeAmmo()
		{
			if (_amountAmmo.Value - 1 < 0)
			{
				return false;
			}
			else
			{
				_amountAmmo.Value--;
				return true;
			}
		}

		public void AddAmmo(int value)
		{
			if (value < 0)
			{
				Debug.LogError("add negative ammo");
				return;
			}

			_amountAmmo.Value += value;
		}

		public void Reset()
		{
			_amountAmmo.Value = _playerConfig.StartAmmoAmount;
		}
	}
}