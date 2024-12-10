using Platformer.Core.SO;
using UnityEngine;

namespace Platformer.Game.Services
{
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(PlayerConfig),
		                fileName = nameof(PlayerConfig))]
	public class PlayerConfig : Config
	{
		[field: SerializeField] public float Speed { get; private set; }
		[field: SerializeField] public float AttackSpeed { get; private set; } = 0.1f;
		[field: SerializeField] public float Damage { get; private set; }
		[field: SerializeField] public int StartAmmoAmount { get; private set; } = 99;

		[field: SerializeField] public float BoundMovementLeft { get; private set; } = 0;
		[field: SerializeField] public float BoundMovementRight { get; private set; } = 0;

		[field: SerializeField] public BulletController Prefab { get; private set; } = null!;
	}
}