using UniRx;

namespace Platformer.Game.Services
{
	public interface IPlayerAmmoService
	{
		IReadOnlyReactiveProperty<int> AmountAmmo { get; }
		bool TryTakeAmmo();
		void AddAmmo(int value);
		void Reset();
	}
}