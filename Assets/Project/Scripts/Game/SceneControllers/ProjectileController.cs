using Platformer.Core.SceneControllers;
using UnityEngine;

namespace Platformer.Game
{
	public class ProjectileController : SceneController
	{
		[SerializeField] protected Rigidbody2D _rigidbody2D = null!;
	}
}